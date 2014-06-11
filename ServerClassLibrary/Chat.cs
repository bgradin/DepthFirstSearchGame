using System.Drawing;

namespace ServerClassLibrary
{
	public class ChatMessage
	{
		string username = "";
		public string Username
		{
			get { return username; }
			set { username = value; }
		}

		public string Message { get; set; }

		Color usernameColor = Color.FromArgb(255, 0, 0, 0);
		public Color UsernameColor
		{
			get { return usernameColor; }
			set { usernameColor = value; }
		}

		Color messageColor = Color.FromArgb(255, 255, 255, 255);
		public Color MessageColor
		{
			get { return messageColor; }
			set { messageColor = value; }
		}
	}
}
