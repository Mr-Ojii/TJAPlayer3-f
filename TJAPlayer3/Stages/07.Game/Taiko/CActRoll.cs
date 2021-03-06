﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.Runtime.InteropServices;
using FDK;

namespace TJAPlayer3
{
	internal class CActRoll : CActivity
	{


		public CActRoll()
		{
			base.b活性化してない = true;
		}

		public override void On活性化()
		{
			this.ct連打枠カウンター = new CCounter[ 4 ];
			this.ct連打アニメ = new CCounter[4];
			FadeOutCounter = new CCounter[4];
			for ( int i = 0; i < 4; i++ )
			{
				this.ct連打枠カウンター[ i ] = new CCounter();
				this.ct連打アニメ[i] = new CCounter();
				FadeOutCounter[i] = new CCounter();
			}
			this.b表示 = new bool[]{ false, false, false, false };
			this.n連打数 = new int[ 4 ];

			base.On活性化();
		}

		public override void On非活性化()
		{
			for (int i = 0; i < 4; i++)
			{
				ct連打枠カウンター[i] = null;
				ct連打アニメ[i] = null;
				FadeOutCounter[i] = null;
			}
			base.On非活性化();
		}

		public override void OnManagedリソースの作成()
		{
			base.OnManagedリソースの作成();
		}

		public override void OnManagedリソースの解放()
		{
			base.OnManagedリソースの解放();
		}

		public override int On進行描画( )
		{
			return base.On進行描画();
		}

		public int On進行描画( int n連打数, int player )
		{
			this.ct連打枠カウンター[ player ].t進行();
			this.ct連打アニメ[player].t進行();
			FadeOutCounter[player].t進行();
			//1PY:-3 2PY:514
			//仮置き
			int[] nRollBalloon = new int[] { -3, 514, 0, 0 };
			int[] nRollNumber = new int[] { 48, 559, 0, 0 };
			const int fadenum = 167;
			for( int i = 0; i < TJAPlayer3.ConfigIni.nPlayerCount; i++ )
			{
				//CDTXMania.act文字コンソール.tPrint(0, 0, C文字コンソール.EFontType.白, this.ct連打枠カウンター[player].n現在の値.ToString());
				if (this.ct連打枠カウンター[player].b終了値に達してない)
				{
					if (ct連打枠カウンター[player].n現在の値 > 1333 && !FadeOutCounter[player].b進行中)
					{
						// 後から変えれるようにする。大体10フレーム分。
						FadeOutCounter[player].t開始(0, fadenum - 1, 1, TJAPlayer3.Timer);
					}
					var opacity = (fadenum - FadeOutCounter[player].n現在の値) * 255 / fadenum;
					if (TJAPlayer3.Tx.Balloon_Roll != null)
						TJAPlayer3.Tx.Balloon_Roll.Opacity = opacity;
					if (TJAPlayer3.Tx.Balloon_Number_Roll != null)
						TJAPlayer3.Tx.Balloon_Number_Roll.Opacity = opacity;


					if (TJAPlayer3.Tx.Balloon_Roll != null)
						TJAPlayer3.Tx.Balloon_Roll.t2D描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.Game_Balloon_Roll_Frame_X[player], TJAPlayer3.Skin.Game_Balloon_Roll_Frame_Y[player]);
					this.t文字表示(TJAPlayer3.Skin.Game_Balloon_Roll_Number_X[player], TJAPlayer3.Skin.Game_Balloon_Roll_Number_Y[player], n連打数, player);
				}
			}

			return base.On進行描画();
		}

		public void t枠表示時間延長( int player )
		{
			this.ct連打枠カウンター[ player ] = new CCounter( 0, 1500, 1, TJAPlayer3.Timer );
			FadeOutCounter[player].n現在の値 = 0;
			FadeOutCounter[player].t停止();
		}


		public bool[] b表示;
		public int[] n連打数;
		public CCounter[] ct連打枠カウンター;
		public CCounter[] ct連打アニメ;
		private float[] RollScale = new float[]
		{
			0.000f,
			0.123f, // リピート
			0.164f,
			0.164f,
			0.164f,
			0.137f,
			0.110f,
			0.082f,
			0.055f,
			0.000f
		};
		private CCounter[] FadeOutCounter;

		private void t文字表示( int x, int y, int n連打, int nPlayer)
		{
			int n桁数 = n連打.ToString().Length;

			for (int index = n連打.ToString().Length - 1; index >= 0; index--)
			{
				int i = (int)(n連打 / Math.Pow(10, index) % 10);
				Rectangle rectangle = new Rectangle(TJAPlayer3.Skin.Game_Balloon_Number_Size[0] * i, 0, TJAPlayer3.Skin.Game_Balloon_Number_Size[0], TJAPlayer3.Skin.Game_Balloon_Number_Size[1]);

				if (TJAPlayer3.Tx.Balloon_Number_Roll != null)
				{
					TJAPlayer3.Tx.Balloon_Number_Roll.vcScaling.X = TJAPlayer3.Skin.Game_Balloon_Roll_Number_Scale;
					TJAPlayer3.Tx.Balloon_Number_Roll.vcScaling.Y = TJAPlayer3.Skin.Game_Balloon_Roll_Number_Scale + RollScale[this.ct連打アニメ[nPlayer].n現在の値];
					TJAPlayer3.Tx.Balloon_Number_Roll.t2D拡大率考慮描画(TJAPlayer3.app.Device, CTexture.RefPnt.DownLeft, x - (((TJAPlayer3.Skin.Game_Balloon_Number_Padding + 2) * n桁数) / 2), y, rectangle);
				}
				x += (TJAPlayer3.Skin.Game_Balloon_Number_Padding - (n桁数 > 2 ? n桁数 * 2 : 0));
			}
		}
	}
}
