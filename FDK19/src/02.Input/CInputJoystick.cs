using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using OpenTK.Input;

namespace FDK
{
	public class CInputJoystick : IInputDevice, IDisposable
	{
		// コンストラクタ

		public CInputJoystick(int joystickindex)
		{
			this.eInputDeviceType = EInputDeviceType.Joystick;
			this.ID = joystickindex;
			this.GUID = Joystick.GetGuid(ID).ToString();

			for (int i = 0; i < this.bButtonState.Length; i++)
				this.bButtonState[i] = false;

			this.listInputEvents = new List<STInputEvent>(32);
		}


		// メソッド

		#region [ IInputDevice 実装 ]
		//-----------------
		public EInputDeviceType eInputDeviceType
		{
			get;
			private set;
		}
		public string GUID
		{
			get;
			private set;
		}
		public int ID
		{
			get;
			private set;
		}
		public List<STInputEvent> listInputEvents
		{
			get;
			private set;
		}

		public void tPolling(bool bIsWindowActive, bool bEnableBufferInput)
		{
			#region [ bButtonフラグ初期化 ]
			for (int i = 0; i < 256; i++)
			{
				this.bButtonPushDown[i] = false;
				this.bButtonPullUp[i] = false;
			}
			#endregion

			if (bIsWindowActive)
			{
				this.listInputEvents.Clear();                        // #xxxxx 2012.6.11 yyagi; To optimize, I removed new();


				#region [ 入力 ]
				//-----------------------------
				JoystickState ButtonState = Joystick.GetState(ID);
				if (ButtonState.IsConnected)
				{
					#region [ X軸－ ]
					//-----------------------------
					if (ButtonState.GetAxis(0) < -0.5)
					{
						if (this.bButtonState[0] == false)
						{
							STInputEvent ev = new STInputEvent()
							{
								nKey = 0,
								bPressed = true,
								nTimeStamp = CSoundManager.rc演奏用タイマ.nシステム時刻ms, // 演奏用タイマと同じタイマを使うことで、BGMと譜面、入力ずれを防ぐ。
							};
							this.listInputEvents.Add(ev);

							this.bButtonState[0] = true;
							this.bButtonPushDown[0] = true;
						}
					}
					else
					{
						if (this.bButtonState[0] == true)
						{
							STInputEvent ev = new STInputEvent()
							{
								nKey = 0,
								bPressed = false,
								nTimeStamp = CSoundManager.rc演奏用タイマ.nシステム時刻ms, // 演奏用タイマと同じタイマを使うことで、BGMと譜面、入力ずれを防ぐ。
							};
							this.listInputEvents.Add(ev);

							this.bButtonState[0] = false;
							this.bButtonPullUp[0] = true;
						}
					}
					//-----------------------------
					#endregion
					#region [ X軸＋ ]
					//-----------------------------
					if (ButtonState.GetAxis(0) > 0.5)
					{
						if (this.bButtonState[1] == false)
						{
							STInputEvent ev = new STInputEvent()
							{
								nKey = 1,
								bPressed = true,
								nTimeStamp = CSoundManager.rc演奏用タイマ.nシステム時刻ms, // 演奏用タイマと同じタイマを使うことで、BGMと譜面、入力ずれを防ぐ。
							};
							this.listInputEvents.Add(ev);

							this.bButtonState[1] = true;
							this.bButtonPushDown[1] = true;
						}
					}
					else
					{
						if (this.bButtonState[1] == true)
						{
							STInputEvent event7 = new STInputEvent()
							{
								nKey = 1,
								bPressed = false,
								nTimeStamp = CSoundManager.rc演奏用タイマ.nシステム時刻ms, // 演奏用タイマと同じタイマを使うことで、BGMと譜面、入力ずれを防ぐ。
							};
							this.listInputEvents.Add(event7);

							this.bButtonState[1] = false;
							this.bButtonPullUp[1] = true;
						}
					}
					//-----------------------------
					#endregion
					#region [ Y軸－ ]
					//-----------------------------
					if (ButtonState.GetAxis(1) < -0.5)
					{
						if (this.bButtonState[2] == false)
						{
							STInputEvent ev = new STInputEvent()
							{
								nKey = 2,
								bPressed = true,
								nTimeStamp = CSoundManager.rc演奏用タイマ.nシステム時刻ms, // 演奏用タイマと同じタイマを使うことで、BGMと譜面、入力ずれを防ぐ。
							};
							this.listInputEvents.Add(ev);

							this.bButtonState[2] = true;
							this.bButtonPushDown[2] = true;
						}
					}
					else
					{
						if (this.bButtonState[2] == true)
						{
							STInputEvent ev = new STInputEvent()
							{
								nKey = 2,
								bPressed = false,
								nTimeStamp = CSoundManager.rc演奏用タイマ.nシステム時刻ms, // 演奏用タイマと同じタイマを使うことで、BGMと譜面、入力ずれを防ぐ。
							};
							this.listInputEvents.Add(ev);

							this.bButtonState[2] = false;
							this.bButtonPullUp[2] = true;
						}
					}
					//-----------------------------
					#endregion
					#region [ Y軸＋ ]
					//-----------------------------
					if (ButtonState.GetAxis(1) > 0.5)
					{
						if (this.bButtonState[3] == false)
						{
							STInputEvent ev = new STInputEvent()
							{
								nKey = 3,
								bPressed = true,
								nTimeStamp = CSoundManager.rc演奏用タイマ.nシステム時刻ms, // 演奏用タイマと同じタイマを使うことで、BGMと譜面、入力ずれを防ぐ。
							};
							this.listInputEvents.Add(ev);

							this.bButtonState[3] = true;
							this.bButtonPushDown[3] = true;
						}
					}
					else
					{
						if (this.bButtonState[3] == true)
						{
							STInputEvent ev = new STInputEvent()
							{
								nKey = 3,
								bPressed = false,
								nTimeStamp = CSoundManager.rc演奏用タイマ.nシステム時刻ms, // 演奏用タイマと同じタイマを使うことで、BGMと譜面、入力ずれを防ぐ。
							};
							this.listInputEvents.Add(ev);

							this.bButtonState[3] = false;
							this.bButtonPullUp[3] = true;
						}
					}
					//-----------------------------
					#endregion
					#region [ Z軸－ ]
					//-----------------------------
					if (ButtonState.GetAxis(2) < -0.5)
					{
						if (this.bButtonState[4] == false)
						{
							STInputEvent ev = new STInputEvent()
							{
								nKey = 4,
								bPressed = true,
								nTimeStamp = CSoundManager.rc演奏用タイマ.nシステム時刻ms, // 演奏用タイマと同じタイマを使うことで、BGMと譜面、入力ずれを防ぐ。
							};
							this.listInputEvents.Add(ev);

							this.bButtonState[4] = true;
							this.bButtonPushDown[4] = true;
						}
					}
					else
					{
						if (this.bButtonState[4] == true)
						{
							STInputEvent ev = new STInputEvent()
							{
								nKey = 4,
								bPressed = false,
								nTimeStamp = CSoundManager.rc演奏用タイマ.nシステム時刻ms, // 演奏用タイマと同じタイマを使うことで、BGMと譜面、入力ずれを防ぐ。
							};
							this.listInputEvents.Add(ev);

							this.bButtonState[4] = false;
							this.bButtonPullUp[4] = true;
						}
					}
					//-----------------------------
					#endregion
					#region [ Z軸＋ ]
					//-----------------------------
					if (ButtonState.GetAxis(2) > 0.5)
					{
						if (this.bButtonState[5] == false)
						{
							STInputEvent ev = new STInputEvent()
							{
								nKey = 5,
								bPressed = true,
								nTimeStamp = CSoundManager.rc演奏用タイマ.nシステム時刻ms, // 演奏用タイマと同じタイマを使うことで、BGMと譜面、入力ずれを防ぐ。
							};
							this.listInputEvents.Add(ev);

							this.bButtonState[5] = true;
							this.bButtonPushDown[5] = true;
						}
					}
					else
					{
						if (this.bButtonState[5] == true)
						{
							STInputEvent event15 = new STInputEvent()
							{
								nKey = 5,
								bPressed = false,
								nTimeStamp = CSoundManager.rc演奏用タイマ.nシステム時刻ms, // 演奏用タイマと同じタイマを使うことで、BGMと譜面、入力ずれを防ぐ。
							};
							this.listInputEvents.Add(event15);

							this.bButtonState[5] = false;
							this.bButtonPullUp[5] = true;
						}
					}
					//-----------------------------
					#endregion
					#region [ Z軸回転－ ]
					//-----------------------------
					if (ButtonState.GetAxis(3) < -0.5)
					{
						if (this.bButtonState[6] == false)
						{
							STInputEvent ev = new STInputEvent()
							{
								nKey = 6,
								bPressed = true,
								nTimeStamp = CSoundManager.rc演奏用タイマ.nシステム時刻ms, // 演奏用タイマと同じタイマを使うことで、BGMと譜面、入力ずれを防ぐ。
							};
							this.listInputEvents.Add(ev);

							this.bButtonState[6] = true;
							this.bButtonPushDown[6] = true;
						}
					}
					else
					{
						if (this.bButtonState[4] == true)
						{
							STInputEvent ev = new STInputEvent()
							{
								nKey = 6,
								bPressed = false,
								nTimeStamp = CSoundManager.rc演奏用タイマ.nシステム時刻ms, // 演奏用タイマと同じタイマを使うことで、BGMと譜面、入力ずれを防ぐ。
							};
							this.listInputEvents.Add(ev);

							this.bButtonState[6] = false;
							this.bButtonPullUp[6] = true;
						}
					}
					//-----------------------------
					#endregion
					#region [ Z軸回転＋ ]
					//-----------------------------
					if (ButtonState.GetAxis(3) > 0.5)
					{
						if (this.bButtonState[7] == false)
						{
							STInputEvent ev = new STInputEvent()
							{
								nKey = 7,
								bPressed = true,
								nTimeStamp = CSoundManager.rc演奏用タイマ.nシステム時刻ms, // 演奏用タイマと同じタイマを使うことで、BGMと譜面、入力ずれを防ぐ。
							};
							this.listInputEvents.Add(ev);

							this.bButtonState[7] = true;
							this.bButtonPushDown[7] = true;
						}
					}
					else
					{
						if (this.bButtonState[7] == true)
						{
							STInputEvent event15 = new STInputEvent()
							{
								nKey = 7,
								bPressed = false,
								nTimeStamp = CSoundManager.rc演奏用タイマ.nシステム時刻ms, // 演奏用タイマと同じタイマを使うことで、BGMと譜面、入力ずれを防ぐ。
							};
							this.listInputEvents.Add(event15);

							this.bButtonState[7] = false;
							this.bButtonPullUp[7] = true;
						}
					}
					//-----------------------------
					#endregion
					#region [ Button ]
					//-----------------------------
					bool bIsButtonPressedReleased = false;
					for (int j = 0; j < 128; j++)
					{
						if (this.bButtonState[8 + j] == false && ButtonState.IsButtonDown(j))
						{
							STInputEvent item = new STInputEvent()
							{
								nKey = 8 + j,
								bPressed = true,
								nTimeStamp = CSoundManager.rc演奏用タイマ.nシステム時刻ms, // 演奏用タイマと同じタイマを使うことで、BGMと譜面、入力ずれを防ぐ。
							};
							this.listInputEvents.Add(item);

							this.bButtonState[8 + j] = true;
							this.bButtonPushDown[8 + j] = true;
							bIsButtonPressedReleased = true;
						}
						else if (this.bButtonState[8 + j] == true && !ButtonState.IsButtonDown(j))
						{
							STInputEvent item = new STInputEvent()
							{
								nKey = 8 + j,
								bPressed = false,
								nTimeStamp = CSoundManager.rc演奏用タイマ.nシステム時刻ms, // 演奏用タイマと同じタイマを使うことで、BGMと譜面、入力ずれを防ぐ。
							};
							this.listInputEvents.Add(item);

							this.bButtonState[8 + j] = false;
							this.bButtonPullUp[8 + j] = true;
							bIsButtonPressedReleased = true;
						}
					}
					//-----------------------------
					#endregion
					// #24341 2011.3.12 yyagi: POV support
					#region [ POV HAT 4/8way (only single POV switch is supported)]
					JoystickHatState hatState = ButtonState.GetHat(JoystickHat.Hat0);

					for (int nWay = 0; nWay < 8; nWay++)
					{
						if (hatState.Position == (OpenTK.Input.HatPosition)nWay + 1)
						{
							if (this.bButtonState[8 + 128 + nWay] == false)
							{
								STInputEvent stevent = new STInputEvent()
								{
									nKey = 8 + 128 + nWay,
									bPressed = true,
									nTimeStamp = CSoundManager.rc演奏用タイマ.nシステム時刻ms, // 演奏用タイマと同じタイマを使うことで、BGMと譜面、入力ずれを防ぐ。
								};
								this.listInputEvents.Add(stevent);

								this.bButtonState[stevent.nKey] = true;
								this.bButtonPushDown[stevent.nKey] = true;
							}
							bIsButtonPressedReleased = true;
						}
					}
					if (bIsButtonPressedReleased == false) // #xxxxx 2011.12.3 yyagi 他のボタンが何も押され/離されてない＝POVが離された
					{
						int nWay = 0;
						for (int i = 8 + 0x80; i < 8 + 0x80 + 8; i++)
						{                                           // 離されたボタンを調べるために、元々押されていたボタンを探す。
							if (this.bButtonState[i] == true)   // DirectInputを直接いじるならこんなことしなくて良いのに、あぁ面倒。
							{                                       // この処理が必要なために、POVを1個しかサポートできない。無念。
								nWay = i;
								break;
							}
						}
						if (nWay != 0)
						{
							STInputEvent stevent = new STInputEvent()
							{
								nKey = nWay,
								bPressed = false,
								nTimeStamp = CSoundManager.rc演奏用タイマ.nシステム時刻ms, // 演奏用タイマと同じタイマを使うことで、BGMと譜面、入力ずれを防ぐ。
							};
							this.listInputEvents.Add(stevent);

							this.bButtonState[nWay] = false;
							this.bButtonPullUp[nWay] = true;
						}
					}
					#endregion
					//-----------------------------
					#endregion
				}
			}
		}

		public bool bIsKeyPressed(int nButton)
		{
			return this.bButtonPushDown[nButton];
		}
		public bool bIsKeyDown(int nButton)
		{
			return this.bButtonState[nButton];
		}
		public bool bIsKeyReleased(int nButton)
		{
			return this.bButtonPullUp[nButton];
		}
		public bool bIsKeyUp(int nButton)
		{
			return !this.bButtonState[nButton];
		}
		//-----------------
		#endregion

		#region [ IDisposable 実装 ]
		//-----------------
		public void Dispose()
		{
			if (!this.bDisposed)
			{
				if (this.listInputEvents != null)
				{
					this.listInputEvents = null;
				}
				this.bDisposed = true;
			}
		}
		//-----------------
		#endregion


		// その他

		#region [ private ]
		//-----------------
		private bool[] bButtonPullUp = new bool[0x100];
		private bool[] bButtonPushDown = new bool[0x100];
		private bool[] bButtonState = new bool[0x100];      // 0-5: XYZ, 6 - 0x128+5: buttons, 0x128+6 - 0x128+6+8: POV/HAT
		private bool bDisposed;
		//-----------------
		#endregion
	}
}
