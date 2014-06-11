using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GenericMapEditor
{
	/// <summary>
	/// Inherits from PictureBox; adds Interpolation Mode Setting
	/// </summary>
	public class InterpolatedPictureBox : PictureBox
	{
		public InterpolationMode InterpolationMode { get; set; }

		protected override void OnPaint(PaintEventArgs paintEventArgs)
		{
			paintEventArgs.Graphics.InterpolationMode = InterpolationMode;
			paintEventArgs.Graphics.PixelOffsetMode = PixelOffsetMode.Half;
			base.OnPaint(paintEventArgs);
		}
	}
}