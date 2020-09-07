﻿using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFmpeg.AutoGen;
using SharpDX.Direct3D9;
using System.Diagnostics.Eventing.Reader;

namespace FDK
{
	unsafe class CVideoDecoder : IDisposable
	{
		public CVideoDecoder(string filename) {

			if (!File.Exists(filename))
				throw new FileNotFoundException(filename + " not found...");

			format_context = ffmpeg.avformat_alloc_context();
			received_frame = ffmpeg.av_frame_alloc();
			fixed (AVFormatContext** format_contexttmp = &format_context)
			{
				if (ffmpeg.avformat_open_input(format_contexttmp, filename, null, null) != 0)
					throw new FileLoadException("avformat_open_input failed\n");

				if (ffmpeg.avformat_find_stream_info(*format_contexttmp, null) < 0)
					throw new FileLoadException("avformat_find_stream_info failed\n");

				// find audio stream
				for (int i = 0; i < (int)format_context->nb_streams; i++)
				{
					if (format_context->streams[i]->codecpar->codec_type == AVMediaType.AVMEDIA_TYPE_VIDEO)
					{
						video_stream = format_context->streams[i];
						break;
					}
				}
				if (video_stream == null)
					throw new FileLoadException("No video stream ...\n");

				// find decoder
				codec = ffmpeg.avcodec_find_decoder(video_stream->codecpar->codec_id);
				if (codec == null)
					throw new NotSupportedException("No supported decoder ...\n");

				codec_context = ffmpeg.avcodec_alloc_context3(codec);

				if (ffmpeg.avcodec_parameters_to_context(codec_context, video_stream->codecpar) < 0)
					Trace.WriteLine("avcodec_parameters_to_context failed\n");
				
				if (ffmpeg.avcodec_open2(codec_context, codec, null) != 0)
					Trace.WriteLine("avcodec_open2 failed\n");

				FrameSize = new Size(codec_context->width, codec_context->height);
				pixelFormat = codec_context->pix_fmt;
				framecount = codec_context->frame_number;

				frame = ffmpeg.av_frame_alloc();
				packet = ffmpeg.av_packet_alloc();

				convert_context = ffmpeg.sws_getContext(FrameSize.Width, FrameSize.Height, pixelFormat,
				FrameSize.Width,
				FrameSize.Height, 
				AVPixelFormat.AV_PIX_FMT_BGR24,
				ffmpeg.SWS_FAST_BILINEAR, null, null, null);
				if (convert_context == null) throw new ApplicationException("Could not initialize the conversion context.");
				_dstData = new byte_ptrArray4();
				_dstLinesize = new int_array4();
				decodedframes = new Queue<CDecodedFrame>();
			}
		}

		public void Dispose()
		{
			ffmpeg.sws_freeContext(convert_context);
			ffmpeg.av_frame_unref(frame);
			ffmpeg.av_free(frame);

			ffmpeg.av_packet_unref(packet);
			ffmpeg.av_free(packet); 

			ffmpeg.avcodec_close(codec_context);
			fixed (AVFormatContext** format_contexttmp = &format_context)
			{
				ffmpeg.avformat_close_input(format_contexttmp);
			}
			if (lastTexture != null)
				lastTexture.Dispose();
			if (nextTexture != null)
				nextTexture.Dispose();
		}

		public void Start()
		{
			CTimer.tリセット();
			CTimer.t再開();
			this.bPlaying = true;
		}

		public void Stop() 
		{
			CTimer.t一時停止();
			this.bPlaying = false;
		}

		public CTexture GetNowFrame(Device device) 
		{
			if (nextframetime <= CTimer.n現在時刻ms)
			{
				if (decodedframes.Count != 0)
				{
					if (lastTexture != null)
						lastTexture.Dispose();
					if (nextTexture == null)	
						nextTexture = new CTexture(device, new Bitmap(1, 1), Format.A8R8G8B8);
				
					lastTexture = nextTexture;
					CDecodedFrame cdecodedframe = decodedframes.Dequeue();
					Bitmap nowbitmap = cdecodedframe.Bitmap;
					nextframetime = cdecodedframe.Time;
					EnqueueFrames();
					nextTexture = new CTexture(device, nowbitmap, Format.A8R8G8B8);
					return lastTexture;
				}
				else
                {
					if (nextTexture != null)
						lastTexture = nextTexture;
					if (lastTexture == null)
					{
						Bitmap nowbitmap = new Bitmap(1, 1);
						lastTexture = new CTexture(device, nowbitmap, Format.A8R8G8B8);
					}
					return lastTexture;
				}
			}
			else
			{
				if (lastTexture == null)
				{
					Bitmap nowbitmap = new Bitmap(1, 1);
					lastTexture = new CTexture(device, nowbitmap, Format.A8R8G8B8);
				}
				return lastTexture;
			}
		}

		private bool EnqueueFrames()
		{
			bool ret = false;

			while (!(ret = EnqueueOneFrame()) && decodedframes.Count < 10) ;

			return ret;
		}

		private bool EnqueueOneFrame() {
			AVFrame outframe;
			ffmpeg.av_frame_unref(frame);
			ffmpeg.av_frame_unref(received_frame);
			int error;
			do
			{
				try
				{
					do
					{
						error = ffmpeg.av_read_frame(format_context, packet);
						if (error == ffmpeg.AVERROR_EOF)
						{
							outframe = *frame;
							return false;
						}

						if (error != 0)
							Trace.TraceError("av_read_frame eof or error.\n");

					} while (packet->stream_index != video_stream->index);

					if (ffmpeg.avcodec_send_packet(codec_context, packet) < 0)
						Trace.TraceError("avcodec_send_packet error\n");
				}
				finally
				{
					ffmpeg.av_packet_unref(packet);
				}

				error = ffmpeg.avcodec_receive_frame(codec_context, frame);
			} while (error == ffmpeg.AVERROR(ffmpeg.EAGAIN));
			if (error != 0)
				Trace.TraceError("error.\n");
			if (codec_context->hw_device_ctx != null)
			{
				if (ffmpeg.av_hwframe_transfer_data(received_frame, frame, 0) < 0)
					Trace.TraceError("av_hwframe_transfer_data error.\n");
				outframe = *received_frame;
			}
			else
			{
				outframe = *frame;
			}
			ffmpeg.sws_scale(convert_context, outframe.data, outframe.linesize, 0, outframe.height, _dstData, _dstLinesize);

			var data = new byte_ptrArray8();
			data.UpdateFrom(_dstData);
			var linesize = new int_array8();
			linesize.UpdateFrom(_dstLinesize);

			outframe = new AVFrame
			{
				data = data,
				linesize = linesize,
				width = FrameSize.Width,
				height = FrameSize.Height
			};

			decodedframes.Enqueue(new CDecodedFrame { Time = (frame->best_effort_timestamp - video_stream->start_time) * (video_stream->time_base.num / video_stream->time_base.den) * 1000, Bitmap = new Bitmap(outframe.width, outframe.height, outframe.linesize[0], PixelFormat.Format24bppRgb, (IntPtr)outframe.data[0]) });

			ffmpeg.av_frame_unref(&outframe);
			ffmpeg.av_free(&outframe);
			return true;
		}

		//for read & decode
		private float fPlaySpeed;
		private static AVFormatContext* format_context;
		private AVStream* video_stream;
		private AVCodec* codec;
		private AVCodecContext* codec_context;
		private AVFrame* received_frame;
		private AVFrame* frame;
		private AVPacket* packet;
		private Queue<CDecodedFrame> decodedframes;
		private Size FrameSize;
		private AVPixelFormat pixelFormat;
		private int framecount;
		private CTexture lastTexture;
		private CTexture nextTexture;
		private double nextframetime;

		//for play
		private bool bPlaying = false;
		private CTimer CTimer;

		//for convert
		private SwsContext* convert_context;
		private byte_ptrArray4 _dstData;
		private int_array4 _dstLinesize;
	}
}
