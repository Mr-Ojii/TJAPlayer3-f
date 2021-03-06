﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;

namespace FDK
{
	/// <summary>
	/// 高速描画版のCPrivateFontクラス。
	/// といっても、一度レンダリングした結果をキャッシュして使いまわしているだけ。
	/// </summary>
	public class CPrivateFastFont : CPrivateFont
	{
		#region [ コンストラクタ ]
		public CPrivateFastFont( string fontpath, int pt, SixLabors.Fonts.FontStyle style )
		{
			Initialize( fontpath, pt, style );
		}
		public CPrivateFastFont( string fontpath, int pt )
		{
			Initialize( fontpath, pt, SixLabors.Fonts.FontStyle.Regular );
		}
		public CPrivateFastFont()
		{
			throw new ArgumentException("CPrivateFastFont: 引数があるコンストラクタを使用してください。");
		}
		#endregion
		#region [ コンストラクタから呼ばれる初期化処理 ]
		protected new void Initialize( string fontpath, int pt, SixLabors.Fonts.FontStyle style )
		{
			this.bDisposed_CPrivateFastFont = false;
			this.listFontCache = new List<FontCache>();
			base.Initialize( fontpath, pt, style );
		}
		#endregion


		#region [ DrawPrivateFontのオーバーロード群 ]
		/// <summary>
		/// 文字列を描画したテクスチャを返す
		/// </summary>
		/// <param name="drawstr">描画文字列</param>
		/// <param name="fontColor">描画色</param>
		/// <returns>描画済テクスチャ</returns>
		public new SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.Rgba32> DrawPrivateFont( string drawstr, Color fontColor )
		{
			return DrawPrivateFont( drawstr, DrawMode.Normal, fontColor, Color.White, Color.White, Color.White, 0 );
		}

		/// <summary>
		/// 文字列を描画したテクスチャを返す
		/// </summary>
		/// <param name="drawstr">描画文字列</param>
		/// <param name="fontColor">描画色</param>
		/// <param name="edgeColor">縁取色</param>
		/// <returns>描画済テクスチャ</returns>
		public new SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.Rgba32> DrawPrivateFont( string drawstr, Color fontColor, Color edgeColor, int edge_Ratio)
		{
			return DrawPrivateFont( drawstr, DrawMode.Edge, fontColor, edgeColor, Color.White, Color.White, edge_Ratio );
		}

		/// <summary>
		/// 文字列を描画したテクスチャを返す
		/// </summary>
		/// <param name="drawstr">描画文字列</param>
		/// <param name="fontColor">描画色</param>
		/// <param name="edgeColor">縁取色</param>
		/// <returns>描画済テクスチャ</returns>
		public SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.Rgba32> DrawPrivateFont( string drawstr, Color fontColor, Color edgeColor, DrawMode dMode, int edge_Ratio)
		{
			return DrawPrivateFont( drawstr, dMode, fontColor, edgeColor, Color.White, Color.White, edge_Ratio );
		}

		/// <summary>
		/// 文字列を描画したテクスチャを返す
		/// </summary>
		/// <param name="drawstr">描画文字列</param>
		/// <param name="fontColor">描画色</param>
		/// <param name="edgeColor">縁取色</param>
		/// <param name="gradationTopColor">グラデーション 上側の色</param>
		/// <param name="gradationBottomColor">グラデーション 下側の色</param>
		/// <returns>描画済テクスチャ</returns>
		public new SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.Rgba32> DrawPrivateFont( string drawstr, Color fontColor, Color edgeColor, Color gradationTopColor, Color gradataionBottomColor, int edge_Ratio )
		{
			return DrawPrivateFont( drawstr, DrawMode.Edge | DrawMode.Gradation, fontColor, edgeColor, gradationTopColor, gradataionBottomColor, edge_Ratio );
		}

		/// <summary>
		/// 文字列を描画したテクスチャを返す
		/// </summary>
		/// <param name="drawstr">描画文字列</param>
		/// <param name="fontColor">描画色</param>
		/// <param name="edgeColor">縁取色</param>
		/// <param name="gradationTopColor">グラデーション 上側の色</param>
		/// <param name="gradationBottomColor">グラデーション 下側の色</param>
		/// <returns>描画済テクスチャ</returns>
		public SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.Rgba32> DrawPrivateFont_V( string drawstr, Color fontColor, Color edgeColor, int edge_Ratio )
		{
			return DrawPrivateFont_V(drawstr, DrawMode.Edge, fontColor, edgeColor, edge_Ratio);
		}

		#endregion

		protected new SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.Rgba32> DrawPrivateFont( string drawstr, DrawMode drawmode, Color fontColor, Color edgeColor, Color gradationTopColor, Color gradationBottomColor, int edge_Ratio )
		{
			#region [ 以前レンダリングしたことのある文字列/フォントか? (キャッシュにヒットするか?) ]
			int index = listFontCache.FindIndex(
				delegate( FontCache fontcache )
				{
					return (
						drawstr == fontcache.drawstr &&
						drawmode == fontcache.drawmode &&
						fontColor == fontcache.fontColor &&
						edgeColor == fontcache.edgeColor &&
						gradationTopColor == fontcache.gradationTopColor &&
						gradationBottomColor == fontcache.gradationBottomColor
						// _font == fontcache.font
					);
				}
			);
			#endregion
			if ( index < 0 )
			{
				// キャッシュにヒットせず。
				#region [ レンダリングして、キャッシュに登録 ]
				FontCache fc = new FontCache();
				fc.bmp = base.DrawPrivateFont( drawstr, drawmode, fontColor, edgeColor, gradationTopColor, gradationBottomColor, edge_Ratio);
				fc.drawstr = drawstr;
				fc.drawmode = drawmode;
				fc.fontColor = fontColor;
				fc.edgeColor = edgeColor;
				fc.gradationTopColor = gradationTopColor;
				fc.gradationBottomColor = gradationBottomColor;
				fc.rectStrings = RectStrings;
				fc.ptOrigin = PtOrigin;
				listFontCache.Add( fc );
				Debug.WriteLine( drawstr + ": Cacheにヒットせず。(cachesize=" + listFontCache.Count + ")" );
				#endregion
				#region [ もしキャッシュがあふれたら、最も古いキャッシュを破棄する ]
				if ( listFontCache.Count > MAXCACHESIZE )
				{
					Debug.WriteLine( "Cache溢れ。" + listFontCache[ 0 ].drawstr + " を解放します。" );
					if ( listFontCache[ 0 ].bmp != null )
					{
						listFontCache[ 0 ].bmp.Dispose();
					}
					listFontCache.RemoveAt( 0 );
				}
				#endregion

				// 呼び出し元のDispose()でキャッシュもDispose()されないように、Clone()で返す。
				return (SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.Rgba32>)listFontCache[ listFontCache.Count - 1 ].bmp.Clone();
			}
			else
			{
				Debug.WriteLine( drawstr + ": Cacheにヒット!! index=" + index );
				#region [ キャッシュにヒット。レンダリングは行わず、キャッシュ内のデータを返して終了。]
				RectStrings = listFontCache[ index ].rectStrings;
				PtOrigin = listFontCache[ index ].ptOrigin;
				// 呼び出し元のDispose()でキャッシュもDispose()されないように、Clone()で返す。
				return (SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.Rgba32>) listFontCache[ index ].bmp.Clone();
				#endregion
			}
		}

		protected new SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.Rgba32> DrawPrivateFont_V(string drawstr, DrawMode drawMode, Color fontColor, Color edgeColor,int edge_Ratio)
		{
			#region [ 以前レンダリングしたことのある文字列/フォントか? (キャッシュにヒットするか?) ]
			int index = listFontCache.FindIndex(
				delegate (FontCache fontcache)
				{
					return (
						drawstr == fontcache.drawstr &&
						fontColor == fontcache.fontColor &&
						edgeColor == fontcache.edgeColor 
					// _font == fontcache.font
					);
				}
			);
			#endregion
			if ( index < 0 )
			{
				// キャッシュにヒットせず。
				#region [ レンダリングして、キャッシュに登録 ]
				FontCache fc = new FontCache();
				fc.bmp = base.DrawPrivateFont_V(drawstr, drawMode, fontColor, edgeColor, edge_Ratio);
				fc.drawstr = drawstr;
				fc.fontColor = fontColor;
				fc.edgeColor = edgeColor;
				fc.rectStrings = RectStrings;
				fc.ptOrigin = PtOrigin;
				listFontCache.Add( fc );
				Debug.WriteLine( drawstr + ": Cacheにヒットせず。(cachesize=" + listFontCache.Count + ")" );
				#endregion
				#region [ もしキャッシュがあふれたら、最も古いキャッシュを破棄する ]
				if ( listFontCache.Count > MAXCACHESIZE )
				{
					Debug.WriteLine( "Cache溢れ。" + listFontCache[ 0 ].drawstr + " を解放します。" );
					if ( listFontCache[ 0 ].bmp != null )
					{
						listFontCache[ 0 ].bmp.Dispose();
					}
					listFontCache.RemoveAt( 0 );
				}
				#endregion

				// 呼び出し元のDispose()でキャッシュもDispose()されないように、Clone()で返す。
				return (SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.Rgba32>)listFontCache[ listFontCache.Count - 1 ].bmp.Clone();
			}
			else
			{
				Debug.WriteLine( drawstr + ": Cacheにヒット!! index=" + index );
				#region [ キャッシュにヒット。レンダリングは行わず、キャッシュ内のデータを返して終了。]
				RectStrings = listFontCache[ index ].rectStrings;
				PtOrigin = listFontCache[ index ].ptOrigin;
				// 呼び出し元のDispose()でキャッシュもDispose()されないように、Clone()で返す。
				return (SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.Rgba32>) listFontCache[ index ].bmp.Clone();
				#endregion
			}
		}

		#region [ IDisposable 実装 ]
		//-----------------
		public new void Dispose()
		{
			if (!this.bDisposed_CPrivateFastFont)
			{
				if (listFontCache != null)
				{
					//Debug.WriteLine( "Disposing CPrivateFastFont()" );
					#region [ キャッシュしている画像を破棄する ]
					foreach (FontCache bc in listFontCache)
					{
						if (bc.bmp != null)
						{
							bc.bmp.Dispose();
						}
					}
					#endregion
					listFontCache.Clear();
					listFontCache = null;
				}
				this.bDisposed_CPrivateFastFont = true;
			}
			base.Dispose();
		}
		//-----------------
		#endregion

		#region [ private ]
		//-----------------
		/// <summary>
		/// キャッシュ容量
		/// </summary>
		private const int MAXCACHESIZE = 256;

		private struct FontCache
		{
			// public Font font;
			public string drawstr;
			public DrawMode drawmode;
			public Color fontColor;
			public Color edgeColor;
			public Color gradationTopColor;
			public Color gradationBottomColor;
			public SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.Rgba32> bmp;
			public Rectangle rectStrings;
			public Point ptOrigin;
		}
		private List<FontCache> listFontCache;

		protected bool bDisposed_CPrivateFastFont;
		//-----------------
		#endregion
	}
}
