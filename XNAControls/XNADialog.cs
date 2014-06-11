using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

/*
 * NOTE: This file needs serious updating to be more generic. So much of the logic depends on this being a component for Endless Online.
 *			Things that need to be decided include:
 *			  1) How/where to store the textures that will be loaded by the dialogs
 *			  2) Implementing a static method such as XNADialog.Show that displays the proper dialog
 *			  3) How much of the EO specific stuff to keep included
*/
namespace EOControls
{
	//a scroll bar is basically a wrapper for a scrolloffset and 3 buttons:
	//	up button, down button, and scroll thingie
	//	A scroll bar is ONLY verticle, text is wrapped by the dialog containing it.
	public class XNAScrollBar : XNAControls.XNAControl
	{
		//All EO specific - this can probably be removed
		public enum ScrollColors
		{
			LightOnDark, //bottom set of light
			LightOnLight, //top set of light
			LightOnMed, //middle set of light
			DarkOnDark //very bottom set
		}

		private Rectangle scrollArea; //area valid for scrolling: always 16 from top and 16 from bottom
		public int ScrollOffset { get; set; }

		XNAControls.XNAButton up, down, scroll; //buttons

		int textNumRows, displayNumRows;

		public XNAScrollBar(Game encapsulatingGame, XNAControls.XNADialog parent, Vector2 relativeLoc, Vector2 size, ScrollColors palette) 
			: base(encapsulatingGame, relativeLoc, new Rectangle((int)relativeLoc.X, (int)relativeLoc.Y, (int)size.X, (int)size.Y))
		{
			SetParent(parent);
			scrollArea = new Rectangle(0, 15, 0, (int)size.Y - 15);
			DrawLocation = relativeLoc;
			ScrollOffset = 0;

			//textNumRows = parent.GetTextNumRows();
			//displayNumRows = parent.GetDisplayNumRows();

			//The sprite sheet was loaded from a constant graphic in my other client.
			//Need to find a good replacement method for providing a constant scroll texture.
			//Texture2D scrollSpriteSheet = GameLogin.loginImages.ScrollBar;
			Rectangle[] upArrows = new Rectangle[2];
			Rectangle[] downArrows = new Rectangle[2];
			Rectangle scrollBox;
			int vertOff;
			switch(palette)
			{
					//eo specific offsets for the sprite sheet containing the buttons
				case ScrollColors.LightOnLight: vertOff = 0; break;
				case ScrollColors.LightOnMed: vertOff = 105; break;
				case ScrollColors.LightOnDark: vertOff = 180; break;
				case ScrollColors.DarkOnDark: vertOff = 255; break;
				default:
					throw new ArgumentOutOfRangeException("Unrecognized palette!");
			}

			//regions based on verticle offset (which is based on the chosen palette)
			//Numbers are all eo specific
			upArrows[0] = new Rectangle(0, vertOff + 15 * 3, 16, 15);
			upArrows[1] = new Rectangle(0, vertOff + 15 * 4, 16, 15);
			downArrows[0] = new Rectangle(0, vertOff + 15, 16, 15);
			downArrows[1] = new Rectangle(0, vertOff + 15 * 2, 16, 15);
			scrollBox = new Rectangle(0, vertOff, 16, 15);
			
			Texture2D[] upButton = new Texture2D[2];
			Texture2D[] downButton = new Texture2D[2];
			Texture2D[] scrollButton = new Texture2D[2];
			//all EO specific...
			//for (int i = 0; i < 2; ++i)
			//{
			//	upButton[i] = new Texture2D(scrollSpriteSheet.GraphicsDevice, upArrows[i].Width, upArrows[i].Height);
			//	Color[] upData = new Color[upArrows[i].Width * upArrows[i].Height];
			//	scrollSpriteSheet.GetData<Color>(0, upArrows[i], upData, 0, upData.Length);
			//	upButton[i].SetData<Color>(upData);

			//	downButton[i] = new Texture2D(scrollSpriteSheet.GraphicsDevice, downArrows[i].Width, downArrows[i].Height);
			//	Color[] downData = new Color[downArrows[i].Width * downArrows[i].Height];
			//	scrollSpriteSheet.GetData<Color>(0, downArrows[i], downData, 0, downData.Length);
			//	downButton[i].SetData<Color>(downData);

			//	//same texture for hover, AFAIK
			//	scrollButton[i] = new Texture2D(scrollSpriteSheet.GraphicsDevice, scrollBox.Width, scrollBox.Height);
			//	Color[] scrollData = new Color[scrollBox.Width * scrollBox.Height];
			//	scrollSpriteSheet.GetData<Color>(0, scrollBox, scrollData, 0, scrollData.Length);
			//	scrollButton[i].SetData<Color>(scrollData);
			//}

			up = new XNAControls.XNAButton(encapsulatingGame, upButton, new Vector2(0, 0));
			up.OnClick += arrowClicked;
			up.SetParent(this);
			down = new XNAControls.XNAButton(encapsulatingGame, downButton, new Vector2(0, size.Y - 15)); //update coordinates!!!!
			down.OnClick += arrowClicked;
			down.SetParent(this);
			scroll = new XNAControls.XNAButton(encapsulatingGame, scrollButton, new Vector2(0, 15)); //update coordinates!!!!
			scroll.OnClickDrag += scrollDragged;
			scroll.SetParent(this);
		}

		public void UpdateText(int textNum, int dispNum)
		{
			textNumRows = textNum;
			displayNumRows = dispNum;
		}

		private void arrowClicked(object sender, EventArgs e)
		{
			int overFlow = textNumRows - displayNumRows; //number of overflow: ie for 12 rows of text and 7 rows displayed, this will be 5
			if (overFlow <= 0)
				return;

			if (sender == up)
			{
				if (ScrollOffset == 0)
					return;

				ScrollOffset--;
			}
			else if (sender == down)
			{
				if (ScrollOffset == overFlow - 1)
					return;

				ScrollOffset++;
			}
			else
				return; //no other buttons should send this event

			int y = (int)((ScrollOffset / (float)overFlow) * (scrollArea.Height - scroll.DrawArea.Height)) + up.DrawArea.Height;
			
			if (y > scrollArea.Height - scroll.DrawArea.Height)
				y = scrollArea.Height - scroll.DrawArea.Height;

			scroll.DrawLocation = new Vector2(0, y);
		}

		private void scrollDragged(object sender, EventArgs e)
		{
			int overFlow = textNumRows - displayNumRows;
			if (overFlow < 0)
				return;

			int y = Mouse.GetState().Y - DrawAreaWithOffset.Y;

			if (y < up.DrawArea.Height)
				y = up.DrawArea.Height + 1;
			else if (y  > scrollArea.Height - scroll.DrawArea.Height)
				y = scrollArea.Height - scroll.DrawArea.Height;
			
			scroll.DrawLocation = new Vector2(0, y);
			
			ScrollOffset = (int)Math.Round(overFlow * ((y - up.DrawArea.Height) / (float)(scrollArea.Height - scroll.DrawArea.Height)));
		}

		public override void Update(GameTime gt)
		{
			if ((parent != null && !parent.Visible) || !Visible ||  (ModalDialog != null && TopParent != ModalDialog))
				return;
			base.Update(gt);
		}

		public override void Draw(GameTime gt)
		{
			if ((parent != null && !parent.Visible) || !Visible)
				return;
			base.Update(gt);
		}
	}
}

namespace XNAControls
{
	public class XNADialog : XNAControl
	{
		public enum XNADialogButtons
		{
			Ok,
			OkCancel
		}

		/// <summary>
		/// XNADialog.XNADialogResult
		/// Returns the value of the clicked button (based on the button text)
		/// </summary>
		public enum XNADialogResult
		{
			OK,
			Cancel,
			Yes,
			No,
			Back,
			Next
		}
		
		public delegate void CustomClose(bool success);

		public CustomClose CloseAction { get; set; }

		private XNALabel caption;
		public string CaptionText
		{
			get { return caption.Text; }
			set { caption.Text = value; }
		}

		private XNALabel message;
		public string MessageText
		{
			get { return message.Text; }
			set { message.Text = value; }
		}
		
		Texture2D bgTexture;
		
		List<XNAButton> dlgButtons;

		TimeSpan? openTime;

		public XNADialog(Game encapsulatingGame, string msgText, string captionText = "", XNADialogButtons whichButtons = XNADialogButtons.Ok)
			: base(encapsulatingGame)
		{
			if (XNAControl.ModalDialog != null)
				throw new InvalidOperationException("You can only have one modal XNADialog running at one time.");

			//specify location of any buttons relative to where control is being drawn
			dlgButtons = new List<XNAButton>();
			Visible = true;

			System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
			using (System.IO.Stream s = assembly.GetManifestResourceStream(@"XNAControls.img.dlg.png"))
			{
				bgTexture = Texture2D.FromStream(encapsulatingGame.GraphicsDevice, s);
			}

			_setSize(bgTexture.Width, bgTexture.Height);

			XNAButton Ok, Cancel;
			Ok = new XNAButton(encapsulatingGame, new Vector2(196, 116));
			Ok.Text = "Ok";
			Ok.OnClick += (object x, EventArgs e) => { Close(); };
			Ok.SetParent(this);
			Cancel = new XNAButton(encapsulatingGame, new Vector2(196, 116));
			Cancel.Text = "Cancel";
			Cancel.OnClick += (object x, EventArgs e) => { Close(); };
			Cancel.SetParent(this);

			switch (whichButtons)
			{
				case XNADialogButtons.Ok:
					dlgButtons.Add(Ok);
					Cancel.Close();
					break;
				case XNADialogButtons.OkCancel:
					Ok.DrawLocation = new Vector2(106, 116);
					dlgButtons.Add(Ok);
					dlgButtons.Add(Cancel);
					break;
			}

			//top left of text: 15, 40
			message = new XNALabel(encapsulatingGame, new Rectangle(15, 40, this.DrawArea.Width - 30, this.DrawArea.Height - 80));
			message.Text = msgText;
			message.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			message.Font = new System.Drawing.Font("Arial", 12);
			message.ForeColor = System.Drawing.Color.Black;
			message.SetParent(this);
			message.TextWidth = 250;

			//top left of cap : 9, 11
			caption = new XNALabel(encapsulatingGame, new Rectangle(9, 11, this.DrawArea.Width - 18, this.DrawArea.Height - 22));
			caption.Text = captionText;
			caption.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			caption.Font = new System.Drawing.Font("Arial", 12);
			caption.ForeColor = System.Drawing.Color.Black;
			caption.SetParent(this);

			//center dialog based on txtSize of background texture
			Center(encapsulatingGame.GraphicsDevice);

			//draw dialog on top of everything - always!
			//child controls DrawOrder is set accordingly
			DrawOrder = 0;
			XNAControl.ModalDialog = this;


			EncapsulatingGame.Components.Add(this);
		}

		public void Center(GraphicsDevice device)
		{
			int viewWidth = device.Viewport.Width;
			int viewHeight = device.Viewport.Height;

			DrawLocation = new Vector2( (viewWidth / 2) - (bgTexture.Width / 2), (viewHeight / 2) - (bgTexture.Height / 2));
		}
		
		public override void Update(GameTime gt)
		{
			if (!Visible && XNAControl.ModalDialog != null)
				return;
			
			KeyboardState keyState = Keyboard.GetState();
			//give a time buffer of 50ms so that an enter keypress from a textbox that produces a dialog isn't picked up by the update method here
			if(keyState.IsKeyUp(Keys.Enter) && PreviousKeyState.IsKeyDown(Keys.Enter) && (gt.TotalGameTime - (openTime ?? (openTime = gt.TotalGameTime))).Value.Duration().Milliseconds > 50)
			{
				//tie enter key press to close the dialog
				//if we ever implement dialogresults this should constitute an "OK" response
				Close();
			}

			MouseState curState = Mouse.GetState();
			if(PreviousMouseState.LeftButton == ButtonState.Pressed && curState.LeftButton == ButtonState.Pressed 
				&& DrawAreaWithOffset.Contains(curState.X, curState.Y) && shouldClickDrag)
			{
				Rectangle gdm = EncapsulatingGame.Window.ClientBounds;

				Vector2 newDrawLoc = new Vector2(DrawAreaWithOffset.X + (curState.X - PreviousMouseState.X), DrawAreaWithOffset.Y + (curState.Y - PreviousMouseState.Y));
				if (newDrawLoc.X < 0) newDrawLoc.X = 0;
				else if (newDrawLoc.Y < 0) newDrawLoc.Y = 0;
				else if (newDrawLoc.X > gdm.Width - DrawAreaWithOffset.Width) newDrawLoc.X = gdm.Width - DrawAreaWithOffset.Width;
				else if (newDrawLoc.Y > gdm.Height - DrawAreaWithOffset.Height) newDrawLoc.Y = gdm.Height - DrawAreaWithOffset.Height;
				DrawLocation = newDrawLoc;
			}
			
			base.Update(gt);
		}

		public override void Draw(GameTime gt)
		{
			if (!Visible && XNAControl.ModalDialog != null)
				return;

			SpriteBatch.Begin();
			SpriteBatch.Draw(bgTexture, DrawAreaWithOffset, Color.White);
			SpriteBatch.End();

			base.Draw(gt);
		}

		public override void Close()
		{
			XNAControl.ModalDialog = null;
			if (CloseAction != null)
				CloseAction(true);

			//set the dialog results accordingly based on the button press here
			base.Close();
		}
	}
}
