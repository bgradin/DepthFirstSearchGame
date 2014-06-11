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
    public partial class frmSpawnEditor : Form
    {
        public List<Spawn> Spawns
        {
            get { return this.spawns; }
        }
        public bool SpawnsChanged { get; set; }

        private List<Spawn> unchangedSpawns;
        private List<Spawn> spawns;
        Spawn selected;
        public frmSpawnEditor(List<Spawn> data)
        {
            InitializeComponent();

            unchangedSpawns = new List<Spawn>();
            foreach (Spawn s in data) //deep copy of values in list
                unchangedSpawns.Add(new Spawn(s));

            spawns = data;

            foreach (Spawn spawn in spawns)
            {
                lbSpawnList.Items.Add(spawn.Name + " (" + spawn.SpawnID + ")");
            }

            if (lbSpawnList.Items.Count > 0)
            {
                lbSpawnList.SelectedIndex = 0;
                pnlData.Enabled = true;
                btnRemoveSpawn.Enabled = true;
            }
        }

        private void lbSpawnList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbSpawnList.SelectedIndex < 0)
            {
                pnlData.Enabled = false;
                btnRemoveSpawn.Enabled = false;
                return;
            }

            lbPairs.Items.Clear();

            selected = spawns[lbSpawnList.SelectedIndex];
            foreach (KeyValuePair<int, int> pair in selected.Spawns)
            {
                lbPairs.Items.Add(pair.Key + " : " + pair.Value);
            }

            txtDisplayName.Text = selected.Name;
            txtSpawnID.Text = selected.SpawnID.ToString();

            btnRemoveSpawn.Enabled = true;
            pnlData.Enabled = true;

            if (lbPairs.Items.Count > 0)
                lbPairs.SelectedIndex = 0;
            else
            {
                lbPairs.SelectedIndex = -1;
                lbPairs_SelectedIndexChanged(null, null);
            }
        }

        private void lbPairs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbPairs.SelectedIndex < 0)
            {
                btnRemovePair.Enabled = false;
                btnUpdatePair.Enabled = false;
                txtPID.Text = "";
                txtFreq.Text = "";
                return;
            }

            string dispString = lbPairs.Items[lbPairs.SelectedIndex].ToString();
            int indexOfColon = dispString.IndexOf(':'), Key;
            if (!int.TryParse(dispString.Substring(0, indexOfColon), out Key))
                return;
            txtPID.Text = Key.ToString();
            txtFreq.Text = selected.Spawns[Key].ToString();

            btnRemovePair.Enabled = true;
            btnUpdatePair.Enabled = true;
        }

        private void txtSpawnID_TextChanged(object sender, EventArgs e)
        {
            if (txtSpawnID.TextLength > 0)
                btnAddSpawn.Enabled = true;
            else
                btnAddSpawn.Enabled = false;
        }

        private void btnAddSpawn_Click(object sender, EventArgs e)
        {
            int newSpawnId;
            if (!int.TryParse(txtSpawnID.Text, out newSpawnId))
            {
                MessageBox.Show("Please enter an integer value for the new Spawn ID.");
                return;
            }

            if (newSpawnId < 0)
            {
                MessageBox.Show("Spawn ID should be a positive value.");
                return;
            }

            if (spawns.Find((Spawn other) => { return other.SpawnID == newSpawnId; }) != null)
            {
                MessageBox.Show("Spawn IDs must be unique.");
                return;
            }

            spawns.Add(new Spawn(newSpawnId, txtDisplayName.Text));
            lbSpawnList.Items.Add(txtDisplayName.Text + " (" + newSpawnId + ")");
            lbSpawnList.SelectedIndex = lbSpawnList.Items.Count - 1;
        }

        private void btnRemoveSpawn_Click(object sender, EventArgs e)
        {
            if (lbSpawnList.SelectedIndex < 0)
                return;

            spawns.RemoveAt(lbSpawnList.SelectedIndex);
            lbSpawnList.Items.RemoveAt(lbSpawnList.SelectedIndex);

            if (lbSpawnList.Items.Count == 0)
                lbSpawnList.SelectedIndex = -1;
            else
                lbSpawnList.SelectedIndex = lbSpawnList.Items.Count - 1;
        }

        private void txtPID_TextChanged(object sender, EventArgs e)
        {
            if (txtPID.TextLength > 0 && txtFreq.TextLength > 0)
                btnAddPair.Enabled = true;
            else
                btnAddPair.Enabled = false;
        }

        private void btnAddPair_Click(object sender, EventArgs e)
        {
            int newPID, newFreq;
            if (!int.TryParse(txtPID.Text, out newPID) || !int.TryParse(txtFreq.Text, out newFreq))
            {
                MessageBox.Show("Please enter integer values for both Pokemon ID and Frequency of spawn.");
                return;
            }

            if (selected.Spawns.ContainsKey(newPID))
            {
                MessageBox.Show("This spawn already contains that Pokemon ID. Pokemon IDs must be unique.");
                return;
            }

            if (!selected.AddSpawnPair(newPID, newFreq))
            {
                MessageBox.Show("Error adding! Frequencies must not exceed 100 and Pokemon IDs must be unique.");
                return;
            }

            lbPairs.Items.Add(newPID + " : " + newFreq);
            lbPairs.SelectedIndex = lbPairs.Items.Count - 1;
        }

        private void btnRemovePair_Click(object sender, EventArgs e)
        {
            if (lbPairs.SelectedIndex < 0)
                return;

            string str = lbPairs.Items[lbPairs.SelectedIndex].ToString();
            int iOfColon = str.IndexOf(':'), Key;
            if(!int.TryParse(str.Substring(0, iOfColon), out Key))
                return;
            selected.RemoveSpawnPair(Key);

            lbPairs.Items.RemoveAt(lbPairs.SelectedIndex);

            if (lbPairs.Items.Count == 0)
                lbPairs.SelectedIndex = -1;
            else
                lbPairs.SelectedIndex = lbPairs.Items.Count - 1;
        }

        private void btnUpdatePair_Click(object sender, EventArgs e)
        {
            if (lbPairs.SelectedIndex < 0)
                return;

            string str = lbPairs.Items[lbPairs.SelectedIndex].ToString();
            int iOfColon = str.IndexOf(':'), Key, Freq;
            if (!int.TryParse(str.Substring(0, iOfColon), out Key) || !int.TryParse(txtFreq.Text, out Freq))
                return;

            if (!selected.AddSpawnPair(Key, Freq, true))
            {
                MessageBox.Show("Error updating selected spawn! Pokemon ID must exist and frequencies must not exceed 100.");
                return;
            }

            lbPairs.Items[lbPairs.SelectedIndex] = Key + " : " + Freq;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
            SpawnsChanged = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            spawns = unchangedSpawns;
            SpawnsChanged = false;
            this.Close();
        }
    }
}
