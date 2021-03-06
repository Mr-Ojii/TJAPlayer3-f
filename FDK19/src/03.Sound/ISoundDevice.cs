﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace FDK
{
	internal interface ISoundDevice : IDisposable
	{
		ESoundDeviceType eOutputDevice { get; }
		int nMasterVolume { get; set; }
		long nOutPutDelayms { get; }
		long nBufferSizems { get; }
		long nElapsedTimems { get; }
		long SystemTimemsWhenUpdatingElapsedTime  { get; }
		CTimer tmSystemTimer { get; }
		float CPUUsage { get; }

		CSound tCreateSound( string strFilename, ESoundGroup soundGroup );
		void tCreateSound( string strFilename, CSound sound );
		void tCreateSound( byte[] byArrWAVファイルイメージ, CSound sound );
	}
}
