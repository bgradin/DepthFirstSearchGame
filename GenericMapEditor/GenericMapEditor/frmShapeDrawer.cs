using GameClassLibrary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace GenericMapEditor
{
	public partial class frmShapeDrawer : Form
	{
		Image _layer1, _layer2, _layer3, _buffer;
		List<Shape> _newShapes;
		Point _adjustedStartPoint, _adjustedEndPoint;

		Rectangle _imageRectangle;
		int _imageSize, _imageMultiplier, _imageX, _imageY;
		bool _dragging = false, _firstTime = true;

		Graphics _image1Graphics, _image2Graphics, _image3Graphics, _pictureBox1Graphics, _bufferGraphics;

		Color _drawColor = Color.Red;

		public EventHandler<UpdatingEventArgs> Updating;

		public frmShapeDrawer(Image image, List<Shape> shapes)
		{
			_layer1 = new Bitmap(image);
			_image1Graphics = Graphics.FromImage(_layer1);

			if (shapes != null)
				_newShapes = new List<Shape>(shapes);
			else
				_newShapes = new List<Shape>();

			InitializeComponent();

			DoSize();

			pictureBox1.MouseDown += pictureBox1_MouseDown;
			pictureBox1.MouseUp += pictureBox1_MouseUp;
			pictureBox1.MouseMove += pictureBox1_MouseMove;
			
			ResizeEnd += frmShapeDrawer_ResizeEnd;

			pictureBox1.Paint += pictureBox1_Paint;
		}

		void pictureBox1_Paint(object sender, PaintEventArgs e)
		{
			if (_firstTime)
			{
				_firstTime = false;
				DoLinesPaint();
				PaintAll(e.Graphics);
			}
		}

		void DoSize()
		{
			_pictureBox1Graphics = pictureBox1.CreateGraphics();
			_pictureBox1Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
			_pictureBox1Graphics.PixelOffsetMode = PixelOffsetMode.Half;

			_imageMultiplier = (int)Math.Floor((double)pictureBox1.Width / _layer1.Width);
			_imageSize = _layer1.Width * _imageMultiplier;
			_imageX = pictureBox1.Width / 2 - _imageSize / 2 - 1;
			_imageY = pictureBox1.Height / 2 - _imageSize / 2 - 1;
			_imageRectangle = new Rectangle(_imageX, _imageY, _imageSize, _imageSize);

			_layer2 = new Bitmap(_imageRectangle.Width, _imageRectangle.Height);
			_image2Graphics = Graphics.FromImage(_layer2);
			_layer3 = new Bitmap(_imageRectangle.Width, _imageRectangle.Height);
			_image3Graphics = Graphics.FromImage(_layer3);
			_buffer = new Bitmap(_imageRectangle.Width, _imageRectangle.Height);
			_bufferGraphics = Graphics.FromImage(_buffer);
			_bufferGraphics.InterpolationMode = InterpolationMode.NearestNeighbor;
			_bufferGraphics.PixelOffsetMode = PixelOffsetMode.Half;
		}

		Size _formerSize = new Size(0, 0);
		void frmShapeDrawer_ResizeEnd(object sender, EventArgs e)
		{
			if (_formerSize != Size)
			{
				Height = Width + 48;
				Refresh();

				DoSize();
				_pictureBox1Graphics.Clear(Color.FromArgb(200, 200, 200));
				DoLinesPaint();
				PaintAll();
			}
		}

		void PaintAll(Graphics g = null)
		{
			if (g == null)
				g = _pictureBox1Graphics;

			Rectangle rect = new Rectangle(0, 0, _buffer.Width, _buffer.Height);
			_bufferGraphics.DrawImage(_layer1, rect);
			_bufferGraphics.DrawImage(_layer2, rect);
			_bufferGraphics.DrawImage(_layer3, rect);

			g.DrawImage(_buffer, _imageRectangle);
		}

		void DrawLine(Point start, Point finish, Graphics g = null)
		{
			Func<int, int> translate = (o) => { return o * _imageMultiplier + _imageMultiplier / 2; };

			if (g == null)
				g = _image3Graphics;

			using (SolidBrush brush = new SolidBrush(_drawColor))
			{
				using (Pen pen = new Pen(brush, 2f))
				{
					g.DrawLine(pen,
						new Point(translate(start.X), translate(start.Y)),
						new Point(translate(finish.X), translate(finish.Y)));
				}
			}
		}

		void pictureBox1_MouseMove(object sender, MouseEventArgs e)
		{
			if (!_imageRectangle.ContainsPoint(_imageX + _adjustedStartPoint.X * _imageMultiplier, _imageY + _adjustedStartPoint.Y * _imageMultiplier)
				|| !_imageRectangle.ContainsPoint(_imageX + (e.X / _imageMultiplier) * _imageMultiplier + 1, _imageY + (e.Y / _imageMultiplier) * _imageMultiplier + 1))
				return;

			int gap = pictureBox1.Width - _imageSize;
			Point endPoint = new Point((e.X - gap / 2) / _imageMultiplier, (e.Y - gap / 2) / _imageMultiplier);

			if (_dragging && endPoint != _adjustedEndPoint)
			{
				_adjustedEndPoint = endPoint;

				DrawLine(_adjustedStartPoint, _adjustedEndPoint);
				PaintAll();
				_image3Graphics.Clear(Color.Transparent);
			}
		}

		void pictureBox1_MouseUp(object sender, MouseEventArgs e)
		{
			_dragging = false;

			if (!_imageRectangle.ContainsPoint(_imageX + _adjustedStartPoint.X * _imageMultiplier, _imageY + _adjustedStartPoint.Y * _imageMultiplier)
				|| !_imageRectangle.ContainsPoint(_imageX + (e.X / _imageMultiplier) * _imageMultiplier + 1, _imageY + (e.Y / _imageMultiplier) * _imageMultiplier + 1))
			{
				PaintAll();
				return;
			}

			Func<Line, Point, bool> lineEndsInPoint = (line, point) =>
				{
					return line.P1.Equals(point) || line.P2.Equals(point);
				};

			Shape[] shapesWithStartPoint = _newShapes.Where(i => i.Lines.Where(j => lineEndsInPoint(j, _adjustedStartPoint)).ToArray().Length > 0).ToArray();
			Shape[] shapesWithEndPoint = _newShapes.Where(i => i.Lines.Where(j => lineEndsInPoint(j, _adjustedEndPoint)).ToArray().Length > 0).ToArray();

			if (shapesWithStartPoint.Length > 1 || shapesWithEndPoint.Length > 1)
			{
				PaintAll();
				return;
			}

			PointDouble pdStartPoint = new PointDouble(_adjustedStartPoint.X, _adjustedStartPoint.Y);
			PointDouble pdEndPoint = new PointDouble(_adjustedEndPoint.X, _adjustedEndPoint.Y);

			Shape newShape = new Shape(pdStartPoint, pdEndPoint);

			if (shapesWithStartPoint.Length == 0 && shapesWithEndPoint.Length == 0)
			{
				_newShapes.Add(newShape);
				DoLinesPaint();
				PaintAll();
				return;
			}

			if (shapesWithStartPoint.Length > 0 && shapesWithEndPoint.Length > 0 && shapesWithStartPoint[0] == shapesWithEndPoint[0])
			{
				if (newShape.JoinWith(shapesWithStartPoint[0]))
					_newShapes.Remove(shapesWithStartPoint[0]);

				_newShapes.Add(newShape);
				DoLinesPaint();
				PaintAll();
				return;
			}

			if (shapesWithStartPoint.Length > 0)
			{
				if (newShape.JoinWith(shapesWithStartPoint[0]))
					_newShapes.Remove(shapesWithStartPoint[0]);

				_newShapes.Add(newShape);
			}

			if (shapesWithEndPoint.Length > 0)
			{
				if (newShape.JoinWith(shapesWithEndPoint[0]))
					_newShapes.Remove(shapesWithEndPoint[0]);

				_newShapes.Add(newShape);
			}

			DoLinesPaint();
			PaintAll();
		}

		void pictureBox1_MouseDown(object sender, MouseEventArgs e)
		{
			int gap = pictureBox1.Width - _imageSize;
			_adjustedStartPoint = new Point((e.X - gap / 2) / _imageMultiplier, (e.Y - gap /2) / _imageMultiplier);
			_dragging = true;
		}

		void DoLinesPaint()
		{
			_image2Graphics.Clear(Color.Transparent);

			if (_newShapes != null && _newShapes.Count != 0)
			{
				foreach (Shape shape in _newShapes)
				{
					foreach (Line line in shape.Lines)
						DrawLine(new Point((int)Math.Floor(line.P1.X), (int)Math.Floor(line.P1.Y)), new Point((int)Math.Floor(line.P2.X), (int)Math.Floor(line.P2.Y)), _image2Graphics);
				}
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			if (Updating != null)
				Updating(this, new UpdatingEventArgs(_newShapes));

			Close();
		}

		private void btnReset_Click(object sender, EventArgs e)
		{
			_newShapes = new List<Shape>();
			DoLinesPaint();
			PaintAll();
		}
	}

	public class UpdatingEventArgs : EventArgs
	{
		public List<Shape> Shapes { get; private set; }

		public UpdatingEventArgs(List<Shape> shapes)
		{
			Shapes = shapes;
		}
	}
}
