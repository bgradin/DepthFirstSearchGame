﻿using System;
using GameClassLibrary;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;
using System.Threading;

namespace XNAControls
{
	internal class NativeMethods
	{
		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		public static extern IntPtr LoadKeyboardLayout(
			  string pwszKLID,  // input locale identifier
			  uint Flags       // input locale identifier options
			  );

		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		public static extern int GetKeyboardLayoutName(
			  System.Text.StringBuilder pwszKLID  //[out] string that receives the name of the locale identifier
			  );

		[DllImport("Imm32.dll", CharSet = CharSet.Unicode)]
		public static extern IntPtr ImmGetContext(IntPtr hWnd);

		[DllImport("Imm32.dll", CharSet = CharSet.Unicode)]
		public static extern IntPtr ImmAssociateContext(IntPtr hWnd, IntPtr hIMC);

		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		public static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
	}
	public class KeyboardLayout
	{
		const uint KLF_ACTIVATE = 1; //activate the layout
		const int KL_NAMELENGTH = 9; // length of the keyboard buffer
		const string LANG_EN_US = "00000409";
		const string LANG_HE_IL = "0001101A";

		public static string getName()
		{
			System.Text.StringBuilder name = new System.Text.StringBuilder(KL_NAMELENGTH);
			NativeMethods.GetKeyboardLayoutName(name);
			return name.ToString();
		}
	}

	public class CharacterEventArgs : EventArgs
	{
		private readonly char character;
		private readonly int lParam;

		public CharacterEventArgs(char character, int lParam)
		{
			this.character = character;
			this.lParam = lParam;
		}

		public char Character
		{
			get { return character; }
		}

		public int Param
		{
			get { return lParam; }
		}

		public int RepeatCount
		{
			get { return lParam & 0xffff; }
		}

		public bool ExtendedKey
		{
			get { return (lParam & (1 << 24)) > 0; }
		}

		public bool AltPressed
		{
			get { return (lParam & (1 << 29)) > 0; }
		}

		public bool PreviousState
		{
			get { return (lParam & (1 << 30)) > 0; }
		}

		public bool TransitionState
		{
			get { return (lParam & (1 << 31)) > 0; }
		}
	}

	public class XNAKeyEventArgs : EventArgs
	{
		private Keys keyCode;

		public XNAKeyEventArgs(Keys keyCode)
		{
			this.keyCode = keyCode;
		}

		public Keys KeyCode
		{
			get { return keyCode; }
		}
	}

	public delegate void CharEnteredHandler(object sender, CharacterEventArgs e);
	public delegate void KeyEventHandler(object sender, XNAKeyEventArgs e);

	public static class EventInput
	{
		/// <summary>
		/// Event raised when a character has been entered.
		/// </summary>
		public static event CharEnteredHandler CharEntered;

		/// <summary>
		/// Event raised when a key has been pressed down. May fire multiple times due to keyboard repeat.
		/// </summary>
		public static event KeyEventHandler KeyDown;

		/// <summary>
		/// Event raised when a key has been released.
		/// </summary>
		public static event KeyEventHandler KeyUp;

		delegate IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

		static bool initialized;
		static IntPtr prevWndProc;
		static WndProc hookProcDelegate;
		static IntPtr hIMC;

		//various Win32 constants that we need
		const int GWL_WNDPROC = -4;
		const int WM_KEYDOWN = 0x100;
		const int WM_KEYUP = 0x101;
		const int WM_CHAR = 0x102;
		const int WM_IME_SETCONTEXT = 0x0281;
		const int WM_INPUTLANGCHANGE = 0x51;
		const int WM_GETDLGCODE = 0x87;
		const int WM_IME_COMPOSITION = 0x10f;
		const int DLGC_WANTALLKEYS = 4;
		
		/// <summary>
		/// Initialize the TextInput with the given GameWindow.
		/// </summary>
		/// <param name="window">The XNA window to which text input should be linked.</param>
		public static void Initialize(GameWindow window)
		{
			if (initialized)
				throw new InvalidOperationException("TextInput.Initialize can only be called once!");

			hookProcDelegate = new WndProc(HookProc);
			prevWndProc = (IntPtr)NativeMethods.SetWindowLong(window.Handle, GWL_WNDPROC,
				(int)Marshal.GetFunctionPointerForDelegate(hookProcDelegate));

			hIMC = NativeMethods.ImmGetContext(window.Handle);
			initialized = true;
		}

		static IntPtr HookProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
		{
			IntPtr returnCode = NativeMethods.CallWindowProc(prevWndProc, hWnd, msg, wParam, lParam);

			switch (msg)
			{
				case WM_GETDLGCODE:
					returnCode = (IntPtr)(returnCode.ToInt32() | DLGC_WANTALLKEYS);
					break;

				case WM_KEYDOWN:
					if (KeyDown != null)
						KeyDown(null, new XNAKeyEventArgs((Keys)wParam));
					break;

				case WM_KEYUP:
					if (KeyUp != null)
						KeyUp(null, new XNAKeyEventArgs((Keys)wParam));
					break;

				case WM_CHAR:
					if (CharEntered != null)
						CharEntered(null, new CharacterEventArgs((char)wParam, lParam.ToInt32()));
					break;

				case WM_IME_SETCONTEXT:
					if (wParam.ToInt32() == 1)
						NativeMethods.ImmAssociateContext(hWnd, hIMC);
					break;

				case WM_INPUTLANGCHANGE:
					NativeMethods.ImmAssociateContext(hWnd, hIMC);
					returnCode = (IntPtr)1;
					break;
			}

			return returnCode;
		}
	}
	public interface IKeyboardSubscriber
	{
		void RecieveTextInput(char inputChar);
		void RecieveTextInput(string text);
		void RecieveCommandInput(char command);
		void RecieveSpecialInput(Keys key);

		bool Selected { get; set; } //or Focused
	}

	public class KeyboardDispatcher
	{
		public KeyboardDispatcher(GameWindow window)
		{
			EventInput.Initialize(window);
			EventInput.CharEntered += new CharEnteredHandler(EventInput_CharEntered);
			EventInput.KeyDown += new KeyEventHandler(EventInput_KeyDown);
		}

		void EventInput_KeyDown(object sender, XNAKeyEventArgs e)
		{
			if (_subscriber == null)
				return;

			_subscriber.RecieveSpecialInput(e.KeyCode);
		}

		void EventInput_CharEntered(object sender, CharacterEventArgs e)
		{
			if (_subscriber == null)
				return;
			if (char.IsControl(e.Character))
			{
				//ctrl-v
				if (e.Character == 0x16)
				{
					//XNA runs in Multiple Thread Apartment state, which cannot recieve clipboard
					Thread thread = new Thread(PasteThread);
					thread.SetApartmentState(ApartmentState.STA);
					thread.Start();
					thread.Join();
					_subscriber.RecieveTextInput(_pasteResult);
				}
				else
				{
					_subscriber.RecieveCommandInput(e.Character);
				}
			}
			else
			{
				_subscriber.RecieveTextInput(e.Character);
			}
		}

		IKeyboardSubscriber _subscriber;
		public IKeyboardSubscriber Subscriber
		{
			get { return _subscriber; }
			set
			{
				if (_subscriber != null)
					_subscriber.Selected = false;
				_subscriber = value;
				if (value != null)
					value.Selected = true;
			}
		}

		//Thread has to be in Single Thread Apartment state in order to receive clipboard
		string _pasteResult = "";
		[STAThread]
		void PasteThread()
		{
			if (System.Windows.Forms.Clipboard.ContainsText())
			{
				_pasteResult = System.Windows.Forms.Clipboard.GetText();
			}
			else
			{
				_pasteResult = "";
			}
		}
	}

	public delegate void TextBoxEvent(object sender, EventArgs e = null);

	public class XNATextBox : XNAControl, IKeyboardSubscriber
	{
		Texture2D _textBoxBG;
		Texture2D _textBoxLeft;
		Texture2D _textBoxRight;
		Texture2D _caretTexture;

		System.Drawing.Font _font;
		System.Windows.Media.GlyphTypeface _glyphs;
		Texture2D _textTexture;
		Texture2D _defaultTextTexture;

		public int MaxChars { get; set; }

		public bool Highlighted { get; set; }

		public bool PasswordBox { get; set; }

		public event TextBoxEvent Clicked;

		string _text = "";
		public String Text
		{
			get
			{
				return _text;
			}
			set
			{
				if (value.Length > MaxChars)
					return;

				_text = value;
				if (_text == null)
					_text = "";

				if (_text != "")
				{
					//if you attempt to display a character that is not in your font
					//you will get an exception, so we filter the characters
					String filtered = "";
					foreach (char c in value)
					{
						ushort bla;
						if (_glyphs != null && _glyphs.CharacterToGlyphMap.TryGetValue((int)Convert.ToUInt16(c), out bla))
							filtered += c;
					}

					_text = filtered;
				}

				_textTexture = EncapsulatingGame.DrawText(PasswordBox ? new string('*', _text.Length) : _text, _font, TextColor);
			}
		}

		System.Drawing.Color textColor = System.Drawing.Color.Black;
		public System.Drawing.Color TextColor
		{
			get { return textColor; }
			set { textColor = value; }
		}

		string _defaultText = "";
		public string DefaultText
		{
			get
			{
				return _defaultText;
			}
			set
			{
				if (value.Length > MaxChars)
					return;

				_defaultText = value;
				if (_defaultText == null)
					_defaultText = "";

				if (_defaultText != "")
				{
					//if you attempt to display a character that is not in your font
					//you will get an exception, so we filter the characters
					String filtered = "";
					foreach (char c in value)
					{
						ushort bla;
						if (_glyphs != null && _glyphs.CharacterToGlyphMap.TryGetValue((int)Convert.ToUInt16(c), out bla))
							filtered += c;
					}

					_defaultText = filtered;
				}

				_defaultTextTexture = EncapsulatingGame.DrawText(_defaultText, _font, System.Drawing.Color.FromArgb(80, 80, 80));
			}
		}

		//accepts array with following:
		//	length 4: background texture, leftEnd, rightEnd, caret
		/// <summary>
		/// Construct an XNATextBox UI control.
		/// </summary>
		/// <param name="encapsulatingGame">Game object "owning" the text box</param>
		/// <param name="area">The area of the screen in which the TextBox should be rendered (x, y)</param>
		/// <param name="textures">Array of four textures. 0=background, 1=leftEdge, 2=rightEdge, 3=caret</param>
		/// <param name="font">Font to use when rendering text</param>
		public XNATextBox(Game encapsulatingGame, Rectangle area, Texture2D[] textures, System.Drawing.Font font)
			: base(encapsulatingGame, new Vector2(area.X, area.Y), area)
		{
			if (textures.Length != 4)
				throw new ArgumentException("The textures array is invalid. Please pass in an array that contains 4 textures.");
			_textBoxBG = textures[0];
			_textBoxLeft = textures[1];
			_textBoxRight = textures[2];
			_caretTexture = textures[3];
			_font = font;

			// Get glyphs
			Typeface typeface = new Typeface(_font.OriginalFontName);
			typeface.TryGetGlyphTypeface(out _glyphs);

			drawArea.Height = _textBoxBG.Height;
		}

		public override void Update(GameTime gameTime)
		{
			if (!Visible || (ModalDialog != null && TopParent != ModalDialog))
				return;
			MouseState mouse = Microsoft.Xna.Framework.Input.Mouse.GetState();
			Point mousePoint = new Point(mouse.X, mouse.Y);

			if (DrawAreaWithOffset.Contains(mousePoint))
			{
				Highlighted = true;
				if (PreviousMouseState.LeftButton == ButtonState.Released && mouse.LeftButton == ButtonState.Pressed)
				{
					if (Clicked != null)
						Clicked(this);
				}
			}
			else
			{
				Highlighted = false;
			}

			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			if (!Visible)
				return;
			bool caretVisible = true;

			if ((gameTime.TotalGameTime.TotalMilliseconds % 1000) < 500)
				caretVisible = false;
			else
				caretVisible = true;

			String toDraw = Text;

			SpriteBatch.Begin();
						
			//draw bg tiled
			SpriteBatch.Draw(_textBoxBG, DrawAreaWithOffset, Microsoft.Xna.Framework.Color.White);
			
			//draw left side
			Rectangle leftDrawArea = new Rectangle(DrawArea.X, DrawArea.Y, _textBoxLeft.Width, DrawArea.Height);
			SpriteBatch.Draw(_textBoxLeft, leftDrawArea, Microsoft.Xna.Framework.Color.White);
			
			//draw right side
			Rectangle rightDrawArea = new Rectangle(DrawArea.X + DrawAreaWithOffset.Width - _textBoxRight.Width, DrawAreaWithOffset.Y, _textBoxRight.Width, DrawAreaWithOffset.Height);
			SpriteBatch.Draw(_textBoxRight, rightDrawArea, Microsoft.Xna.Framework.Color.White);

			Texture2D texture = (_textTexture == null || _text == "") ? _defaultTextTexture : _textTexture;

			if (texture != null)
			{
				Rectangle? rect = texture.Bounds.Width < DrawAreaWithOffset.Width ? texture.Bounds :
					new Rectangle(texture.Width - DrawArea.Width, 0, DrawArea.Width, texture.Height);

				if (caretVisible && Selected)
				{
					//draw caret
					int x = texture != _defaultTextTexture && rect != null && rect.Value.Width > 5 ? rect.Value.Width - 4 : 2;

					SpriteBatch.Draw(_caretTexture,
						new Vector2(DrawAreaWithOffset.X + x, DrawAreaWithOffset.Y + 4),
						Microsoft.Xna.Framework.Color.White);
				}

				SpriteBatch.Draw(texture,
					new Rectangle(DrawAreaWithOffset.X, DrawAreaWithOffset.Y + DrawArea.Height / 2 - texture.Height / 2, rect.Value.Width, rect.Value.Height),
					rect,
					Microsoft.Xna.Framework.Color.White);
			}

			SpriteBatch.End();

			base.Draw(gameTime);
		}

		public void RecieveTextInput(char inputChar)
		{
			Text = Text + inputChar;
		}
		public void RecieveTextInput(string text)
		{
			Text = Text + text;
		}
		public void RecieveCommandInput(char command)
		{
			//ignore command input (ie enter keypresses) when there is a modal dialog up
			//	and this text box is not a member of the modal dialog that is up
			if (XNAControl.ModalDialog != null && TopParent != XNAControl.ModalDialog)
				return;

			switch (command)
			{
				case '\b': //backspace
					if (Text.Length > 0)
						Text = Text.Substring(0, Text.Length - 1);
					break;
				case '\r': //return
					if (OnEnterPressed != null)
						OnEnterPressed(this);
					break;
				case '\t': //tab
					if (OnTabPressed != null)
						OnTabPressed(this);
					break;
				default:
					break;
			}
		}
		public void RecieveSpecialInput(Keys key)
		{

		}

		public event TextBoxEvent OnEnterPressed;
		public event TextBoxEvent OnTabPressed;

		public bool Selected
		{
			get;
			set;
		}
	}
}