using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using GameClassLibrary;

namespace GenericMapEditor
{
	public partial class frmMapProperties : Form
	{
		private Map openMap;

		public Map Map
		{
			get { return this.openMap; }
		}

		public frmMapProperties()
		{
			InitializeComponent();
			MessageBox.Show("Error: map is null.");
			this.Close();
		}

		public frmMapProperties(Map map)
		{
			InitializeComponent();
			openMap = map;
		}

		private void frmMapProperties_Load(object sender, EventArgs e)
		{
			string[] types = Enum.GetNames(typeof(MapType));
			cmbMapType.Items.AddRange(types);
			cmbMapType.SelectedIndex = 0;

			txtWidth.Text = openMap.Width.ToString();
			txtHeight.Text = openMap.Height.ToString();
			txtSpawnX.Text = openMap.PlayerSpawn.X.ToString();
			txtSpawnY.Text = openMap.PlayerSpawn.Y.ToString();
			txtWarpID.Text = openMap.Warp.ToString();
			txtMapName.Text = openMap.MapName;
			cmbMapType.SelectedIndex = (int)openMap.Type;
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			int newW, newH, newSpawnX, newSpawnY, newWarp;
			if (!int.TryParse(txtWidth.Text, out newW) || !int.TryParse(txtHeight.Text, out newH) || !int.TryParse(txtWarpID.Text, out newWarp)
				|| !int.TryParse(txtSpawnX.Text, out newSpawnX) || !int.TryParse(txtSpawnY.Text, out newSpawnY))
			{
				MessageBox.Show("Please enter integer values for width, height, SpawnX, SpawnY, and warp ID.");
				return;
			}

			if (newSpawnX > newW || newSpawnY > newH)
			{
				MessageBox.Show("Spawn X and Spawn Y must be in range of the map's size.");
				return;
			}

			if (newW < openMap.Width || newH < openMap.Height)
			{
				DialogResult dr = MessageBox.Show("Width or height is less than original; some clipping will occur.", "Continue?", MessageBoxButtons.OKCancel);

				if (dr == System.Windows.Forms.DialogResult.Cancel)
					return;
			}
			
			openMap.Resize(newW, newH);
			openMap.PlayerSpawn = new Microsoft.Xna.Framework.Point(newSpawnX, newSpawnY);
			openMap.Warp = newWarp;
			if (cmbMapType.SelectedIndex >= 0)
				openMap.Type = (MapType)cmbMapType.SelectedIndex;
			openMap.Rename(txtMapName.Text);
		}
	}
}
