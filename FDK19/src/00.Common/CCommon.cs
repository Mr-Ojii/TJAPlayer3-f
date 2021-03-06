﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace FDK
{
	public class CCommon
	{
		// 解放

		public static void tDispose<T>( ref T obj )
		{
			if( obj == null )
				return;

			var d = obj as IDisposable;

			if( d != null )
			{
				d.Dispose();
				obj = default( T );
			}
		}
		public static void tDispose<T>( T obj )
		{
			if( obj == null )
				return;

			var d = obj as IDisposable;

			if( d != null )
				d.Dispose();
		}

		public static void tRunCompleteGC()
		{
			GC.Collect();					// アクセス不可能なオブジェクトを除去し、ファイナライぜーション実施。
			GC.WaitForPendingFinalizers();	// ファイナライゼーションが終わるまでスレッドを待機。
			GC.Collect();					// ファイナライズされたばかりのオブジェクトに関連するメモリを開放。

			// 出展: http://msdn.microsoft.com/ja-jp/library/ms998547.aspx#scalenetchapt05_topic10
		}
	}	
}
