using GameClassLibrary;

namespace GenericMapEditor
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
			this.mnuMain = new System.Windows.Forms.MenuStrip();
			this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuNew = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuOpen = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuFileRecent = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuSave = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSaveAs = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuClose = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuExit = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuEdit = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuUndo = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRedo = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuMap = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuClearLayer = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuSpawnData = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuProperties = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSep1 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuTestMap = new System.Windows.Forms.ToolStripMenuItem();
			this.tilesetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.recentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
			this.propertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuView = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuLayers = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuTileLayer = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSpecialLayer = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuGrid = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuZoomIn = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuZoomOut = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuHelp = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuAbout = new System.Windows.Forms.ToolStripMenuItem();
			this.hmnuHidden = new System.Windows.Forms.ToolStripMenuItem();
			this.hmnuSelector = new System.Windows.Forms.ToolStripMenuItem();
			this.hmnuEraser = new System.Windows.Forms.ToolStripMenuItem();
			this.hmnuPan = new System.Windows.Forms.ToolStripMenuItem();
			this.ssMain = new System.Windows.Forms.StatusStrip();
			this.tslblFileName = new System.Windows.Forms.ToolStripStatusLabel();
			this.tslblZoomLevel = new System.Windows.Forms.ToolStripStatusLabel();
			this.tsLblSelX = new System.Windows.Forms.ToolStripStatusLabel();
			this.tsLblSelY = new System.Windows.Forms.ToolStripStatusLabel();
			this.tsLblNumTiles = new System.Windows.Forms.ToolStripStatusLabel();
			this.tsTools = new System.Windows.Forms.ToolStrip();
			this.tsNew = new System.Windows.Forms.ToolStripButton();
			this.tsOpen = new System.Windows.Forms.ToolStripButton();
			this.tsSave = new System.Windows.Forms.ToolStripButton();
			this.tsSep = new System.Windows.Forms.ToolStripSeparator();
			this.tsArrow = new System.Windows.Forms.ToolStripButton();
			this.tsMArrow = new System.Windows.Forms.ToolStripButton();
			this.tsErase = new System.Windows.Forms.ToolStripButton();
			this.tsPan = new System.Windows.Forms.ToolStripButton();
			this.tabsGround = new System.Windows.Forms.TabPage();
			this.lvTiles = new System.Windows.Forms.ListView();
			this.tabs = new System.Windows.Forms.TabControl();
			this.tabsSpecial = new System.Windows.Forms.TabPage();
			this.lvTiles2 = new System.Windows.Forms.ListView();
			this.grpWarpPlacement = new System.Windows.Forms.GroupBox();
			this.cmbWarpAnim = new System.Windows.Forms.ComboBox();
			this.lblWarpAnimation = new System.Windows.Forms.Label();
			this.lblDestWarpMap = new System.Windows.Forms.Label();
			this.txtDestWarpMap = new System.Windows.Forms.TextBox();
			this.lblDestWarpY = new System.Windows.Forms.Label();
			this.txtDestWarpY = new System.Windows.Forms.TextBox();
			this.lblDestWarpX = new System.Windows.Forms.Label();
			this.txtDestWarpX = new System.Windows.Forms.TextBox();
			this.lblSpecialTileType = new System.Windows.Forms.Label();
			this.cmbSpecialType = new System.Windows.Forms.ComboBox();
			this.tabsNPCs = new System.Windows.Forms.TabPage();
			this.grpNPCInfo = new System.Windows.Forms.GroupBox();
			this.label4 = new System.Windows.Forms.Label();
			this.chkNPCMoves = new System.Windows.Forms.CheckBox();
			this.txtNPCMoveSpeed = new System.Windows.Forms.TextBox();
			this.lblNPCMoveSpeed = new System.Windows.Forms.Label();
			this.txtNPCId = new System.Windows.Forms.TextBox();
			this.lblNPCid = new System.Windows.Forms.Label();
			this.tabsItems = new System.Windows.Forms.TabPage();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtItemRespawn = new System.Windows.Forms.TextBox();
			this.lblItemRespawn = new System.Windows.Forms.Label();
			this.chkItemHidden = new System.Windows.Forms.CheckBox();
			this.txtItemID = new System.Windows.Forms.TextBox();
			this.lblItemID = new System.Windows.Forms.Label();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.txtDensity = new System.Windows.Forms.TextBox();
			this.pbTilePreview = new GenericMapEditor.InterpolatedPictureBox();
			this.mapViewer1 = new GenericMapEditor.MapViewer();
			this.mnuMain.SuspendLayout();
			this.ssMain.SuspendLayout();
			this.tsTools.SuspendLayout();
			this.tabsGround.SuspendLayout();
			this.tabs.SuspendLayout();
			this.tabsSpecial.SuspendLayout();
			this.grpWarpPlacement.SuspendLayout();
			this.tabsNPCs.SuspendLayout();
			this.grpNPCInfo.SuspendLayout();
			this.tabsItems.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbTilePreview)).BeginInit();
			this.SuspendLayout();
			// 
			// mnuMain
			// 
			this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.mnuEdit,
            this.mnuMap,
            this.tilesetToolStripMenuItem,
            this.mnuView,
            this.mnuHelp,
            this.hmnuHidden});
			this.mnuMain.Location = new System.Drawing.Point(0, 0);
			this.mnuMain.Name = "mnuMain";
			this.mnuMain.Size = new System.Drawing.Size(765, 24);
			this.mnuMain.TabIndex = 0;
			this.mnuMain.Text = "mnuMain";
			// 
			// mnuFile
			// 
			this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuNew,
            this.mnuOpen,
            this.mnuFileRecent,
            this.toolStripSeparator2,
            this.mnuSave,
            this.mnuSaveAs,
            this.toolStripSeparator1,
            this.mnuClose,
            this.mnuExit});
			this.mnuFile.Name = "mnuFile";
			this.mnuFile.Size = new System.Drawing.Size(37, 20);
			this.mnuFile.Text = "&File";
			// 
			// mnuNew
			// 
			this.mnuNew.Image = ((System.Drawing.Image)(resources.GetObject("mnuNew.Image")));
			this.mnuNew.Name = "mnuNew";
			this.mnuNew.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
			this.mnuNew.Size = new System.Drawing.Size(195, 22);
			this.mnuNew.Text = "&New";
			this.mnuNew.Click += new System.EventHandler(this.mnuNew_Click);
			// 
			// mnuOpen
			// 
			this.mnuOpen.Image = ((System.Drawing.Image)(resources.GetObject("mnuOpen.Image")));
			this.mnuOpen.Name = "mnuOpen";
			this.mnuOpen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.mnuOpen.Size = new System.Drawing.Size(195, 22);
			this.mnuOpen.Text = "&Open";
			this.mnuOpen.Click += new System.EventHandler(this.mnuOpen_Click);
			// 
			// mnuFileRecent
			// 
			this.mnuFileRecent.Enabled = false;
			this.mnuFileRecent.Name = "mnuFileRecent";
			this.mnuFileRecent.Size = new System.Drawing.Size(195, 22);
			this.mnuFileRecent.Text = "&Recent";
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(192, 6);
			// 
			// mnuSave
			// 
			this.mnuSave.Image = ((System.Drawing.Image)(resources.GetObject("mnuSave.Image")));
			this.mnuSave.Name = "mnuSave";
			this.mnuSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.mnuSave.Size = new System.Drawing.Size(195, 22);
			this.mnuSave.Text = "&Save";
			this.mnuSave.Click += new System.EventHandler(this.mnuSave_Click);
			// 
			// mnuSaveAs
			// 
			this.mnuSaveAs.Name = "mnuSaveAs";
			this.mnuSaveAs.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
			this.mnuSaveAs.Size = new System.Drawing.Size(195, 22);
			this.mnuSaveAs.Text = "Save &As...";
			this.mnuSaveAs.Click += new System.EventHandler(this.mnuSaveAs_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(192, 6);
			// 
			// mnuClose
			// 
			this.mnuClose.Name = "mnuClose";
			this.mnuClose.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
			this.mnuClose.Size = new System.Drawing.Size(195, 22);
			this.mnuClose.Text = "&Close";
			this.mnuClose.Click += new System.EventHandler(this.mnuClose_Click);
			// 
			// mnuExit
			// 
			this.mnuExit.Name = "mnuExit";
			this.mnuExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
			this.mnuExit.Size = new System.Drawing.Size(195, 22);
			this.mnuExit.Text = "&Exit";
			this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
			// 
			// mnuEdit
			// 
			this.mnuEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuUndo,
            this.mnuRedo});
			this.mnuEdit.Name = "mnuEdit";
			this.mnuEdit.Size = new System.Drawing.Size(39, 20);
			this.mnuEdit.Text = "&Edit";
			// 
			// mnuUndo
			// 
			this.mnuUndo.Enabled = false;
			this.mnuUndo.Name = "mnuUndo";
			this.mnuUndo.ShortcutKeyDisplayString = "";
			this.mnuUndo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
			this.mnuUndo.Size = new System.Drawing.Size(144, 22);
			this.mnuUndo.Text = "&Undo";
			this.mnuUndo.Click += new System.EventHandler(this.mnuUndo_Click);
			// 
			// mnuRedo
			// 
			this.mnuRedo.Enabled = false;
			this.mnuRedo.Name = "mnuRedo";
			this.mnuRedo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
			this.mnuRedo.Size = new System.Drawing.Size(144, 22);
			this.mnuRedo.Text = "&Redo";
			this.mnuRedo.Click += new System.EventHandler(this.mnuRedo_Click);
			// 
			// mnuMap
			// 
			this.mnuMap.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuClearLayer,
            this.toolStripSeparator4,
            this.mnuSpawnData,
            this.mnuProperties,
            this.mnuSep1,
            this.mnuTestMap});
			this.mnuMap.Name = "mnuMap";
			this.mnuMap.Size = new System.Drawing.Size(43, 20);
			this.mnuMap.Text = "&Map";
			// 
			// mnuClearLayer
			// 
			this.mnuClearLayer.Name = "mnuClearLayer";
			this.mnuClearLayer.Size = new System.Drawing.Size(145, 22);
			this.mnuClearLayer.Text = "&Clear Layer";
			this.mnuClearLayer.Click += new System.EventHandler(this.mnuClearLayer_Click);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(142, 6);
			// 
			// mnuSpawnData
			// 
			this.mnuSpawnData.Name = "mnuSpawnData";
			this.mnuSpawnData.Size = new System.Drawing.Size(145, 22);
			this.mnuSpawnData.Text = "&Spawn Data...";
			this.mnuSpawnData.Click += new System.EventHandler(this.mnuSpawnData_Click);
			// 
			// mnuProperties
			// 
			this.mnuProperties.Name = "mnuProperties";
			this.mnuProperties.Size = new System.Drawing.Size(145, 22);
			this.mnuProperties.Text = "&Properties...";
			this.mnuProperties.Click += new System.EventHandler(this.mnuProperties_Click);
			// 
			// mnuSep1
			// 
			this.mnuSep1.Name = "mnuSep1";
			this.mnuSep1.Size = new System.Drawing.Size(142, 6);
			// 
			// mnuTestMap
			// 
			this.mnuTestMap.Enabled = false;
			this.mnuTestMap.Name = "mnuTestMap";
			this.mnuTestMap.Size = new System.Drawing.Size(145, 22);
			this.mnuTestMap.Text = "&Test Map...";
			this.mnuTestMap.Click += new System.EventHandler(this.mnuTestMap_Click);
			// 
			// tilesetToolStripMenuItem
			// 
			this.tilesetToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.recentToolStripMenuItem,
            this.toolStripMenuItem2,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripMenuItem3,
            this.propertiesToolStripMenuItem});
			this.tilesetToolStripMenuItem.Name = "tilesetToolStripMenuItem";
			this.tilesetToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
			this.tilesetToolStripMenuItem.Text = "&Tileset";
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Image = global::GenericMapEditor.Properties.Resources.open;
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
			this.openToolStripMenuItem.Text = "&Open";
			this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
			// 
			// recentToolStripMenuItem
			// 
			this.recentToolStripMenuItem.Enabled = false;
			this.recentToolStripMenuItem.Name = "recentToolStripMenuItem";
			this.recentToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
			this.recentToolStripMenuItem.Text = "&Recent";
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(124, 6);
			// 
			// saveToolStripMenuItem
			// 
			this.saveToolStripMenuItem.Image = global::GenericMapEditor.Properties.Resources.save;
			this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			this.saveToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
			this.saveToolStripMenuItem.Text = "&Save";
			this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
			// 
			// saveAsToolStripMenuItem
			// 
			this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
			this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
			this.saveAsToolStripMenuItem.Text = "Save &As...";
			this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
			// 
			// toolStripMenuItem3
			// 
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			this.toolStripMenuItem3.Size = new System.Drawing.Size(124, 6);
			// 
			// propertiesToolStripMenuItem
			// 
			this.propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
			this.propertiesToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
			this.propertiesToolStripMenuItem.Text = "&Properties";
			this.propertiesToolStripMenuItem.Click += new System.EventHandler(this.propertiesToolStripMenuItem_Click);
			// 
			// mnuView
			// 
			this.mnuView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuLayers,
            this.mnuGrid,
            this.toolStripSeparator3,
            this.mnuZoomIn,
            this.mnuZoomOut});
			this.mnuView.Name = "mnuView";
			this.mnuView.Size = new System.Drawing.Size(44, 20);
			this.mnuView.Text = "&View";
			// 
			// mnuLayers
			// 
			this.mnuLayers.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuTileLayer,
            this.mnuSpecialLayer});
			this.mnuLayers.Name = "mnuLayers";
			this.mnuLayers.Size = new System.Drawing.Size(138, 22);
			this.mnuLayers.Text = "&Layers";
			// 
			// mnuTileLayer
			// 
			this.mnuTileLayer.Checked = true;
			this.mnuTileLayer.CheckOnClick = true;
			this.mnuTileLayer.CheckState = System.Windows.Forms.CheckState.Checked;
			this.mnuTileLayer.Name = "mnuTileLayer";
			this.mnuTileLayer.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D0)));
			this.mnuTileLayer.Size = new System.Drawing.Size(178, 22);
			this.mnuTileLayer.Text = "Tiles";
			this.mnuTileLayer.Click += new System.EventHandler(this.mnuTileLayer_Click);
			// 
			// mnuSpecialLayer
			// 
			this.mnuSpecialLayer.Checked = true;
			this.mnuSpecialLayer.CheckOnClick = true;
			this.mnuSpecialLayer.CheckState = System.Windows.Forms.CheckState.Checked;
			this.mnuSpecialLayer.Name = "mnuSpecialLayer";
			this.mnuSpecialLayer.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D1)));
			this.mnuSpecialLayer.Size = new System.Drawing.Size(178, 22);
			this.mnuSpecialLayer.Text = "Special Tiles";
			this.mnuSpecialLayer.Click += new System.EventHandler(this.mnuSpecialLayer_Click);
			// 
			// mnuGrid
			// 
			this.mnuGrid.CheckOnClick = true;
			this.mnuGrid.Name = "mnuGrid";
			this.mnuGrid.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
			this.mnuGrid.Size = new System.Drawing.Size(138, 22);
			this.mnuGrid.Text = "&Grid";
			this.mnuGrid.Click += new System.EventHandler(this.mnuGrid_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(135, 6);
			// 
			// mnuZoomIn
			// 
			this.mnuZoomIn.Name = "mnuZoomIn";
			this.mnuZoomIn.ShortcutKeyDisplayString = "";
			this.mnuZoomIn.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Oemplus)));
			this.mnuZoomIn.ShowShortcutKeys = false;
			this.mnuZoomIn.Size = new System.Drawing.Size(138, 22);
			this.mnuZoomIn.Text = "Zoom &In";
			this.mnuZoomIn.Click += new System.EventHandler(this.mnuZoomIn_Click);
			// 
			// mnuZoomOut
			// 
			this.mnuZoomOut.Name = "mnuZoomOut";
			this.mnuZoomOut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.OemMinus)));
			this.mnuZoomOut.ShowShortcutKeys = false;
			this.mnuZoomOut.Size = new System.Drawing.Size(138, 22);
			this.mnuZoomOut.Text = "Zoom &Out";
			this.mnuZoomOut.Click += new System.EventHandler(this.mnuZoomOut_Click);
			// 
			// mnuHelp
			// 
			this.mnuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuAbout});
			this.mnuHelp.Name = "mnuHelp";
			this.mnuHelp.Size = new System.Drawing.Size(44, 20);
			this.mnuHelp.Text = "&Help";
			// 
			// mnuAbout
			// 
			this.mnuAbout.Name = "mnuAbout";
			this.mnuAbout.Size = new System.Drawing.Size(107, 22);
			this.mnuAbout.Text = "&About";
			this.mnuAbout.Click += new System.EventHandler(this.mnuAbout_Click);
			// 
			// hmnuHidden
			// 
			this.hmnuHidden.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hmnuSelector,
            this.hmnuEraser,
            this.hmnuPan});
			this.hmnuHidden.Name = "hmnuHidden";
			this.hmnuHidden.Size = new System.Drawing.Size(64, 20);
			this.hmnuHidden.Text = "(hidden)";
			this.hmnuHidden.Visible = false;
			// 
			// hmnuSelector
			// 
			this.hmnuSelector.Name = "hmnuSelector";
			this.hmnuSelector.Size = new System.Drawing.Size(141, 22);
			// 
			// hmnuEraser
			// 
			this.hmnuEraser.Name = "hmnuEraser";
			this.hmnuEraser.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.E)));
			this.hmnuEraser.Size = new System.Drawing.Size(141, 22);
			this.hmnuEraser.Text = "Eraser";
			this.hmnuEraser.Click += new System.EventHandler(this.hmnuEraser_Click);
			// 
			// hmnuPan
			// 
			this.hmnuPan.Name = "hmnuPan";
			this.hmnuPan.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D)));
			this.hmnuPan.Size = new System.Drawing.Size(141, 22);
			this.hmnuPan.Text = "Pan";
			this.hmnuPan.Click += new System.EventHandler(this.hmnuPan_Click);
			// 
			// ssMain
			// 
			this.ssMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslblFileName,
            this.tslblZoomLevel,
            this.tsLblSelX,
            this.tsLblSelY,
            this.tsLblNumTiles});
			this.ssMain.Location = new System.Drawing.Point(0, 539);
			this.ssMain.Name = "ssMain";
			this.ssMain.Size = new System.Drawing.Size(765, 22);
			this.ssMain.TabIndex = 1;
			this.ssMain.Text = "ssMain";
			// 
			// tslblFileName
			// 
			this.tslblFileName.Name = "tslblFileName";
			this.tslblFileName.Size = new System.Drawing.Size(432, 17);
			this.tslblFileName.Spring = true;
			this.tslblFileName.Text = "[File Name]";
			this.tslblFileName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.tslblFileName.ToolTipText = "Currently open file name";
			this.tslblFileName.Visible = false;
			// 
			// tslblZoomLevel
			// 
			this.tslblZoomLevel.AutoSize = false;
			this.tslblZoomLevel.Name = "tslblZoomLevel";
			this.tslblZoomLevel.Size = new System.Drawing.Size(58, 17);
			this.tslblZoomLevel.Text = "100%";
			this.tslblZoomLevel.ToolTipText = "Current zoom level of map";
			this.tslblZoomLevel.Visible = false;
			// 
			// tsLblSelX
			// 
			this.tsLblSelX.AutoSize = false;
			this.tsLblSelX.Name = "tsLblSelX";
			this.tsLblSelX.Size = new System.Drawing.Size(40, 17);
			this.tsLblSelX.Text = "[x]";
			this.tsLblSelX.ToolTipText = "X coordinate of the cursor";
			this.tsLblSelX.Visible = false;
			// 
			// tsLblSelY
			// 
			this.tsLblSelY.AutoSize = false;
			this.tsLblSelY.Name = "tsLblSelY";
			this.tsLblSelY.Size = new System.Drawing.Size(40, 17);
			this.tsLblSelY.Text = "[y]";
			this.tsLblSelY.ToolTipText = "Y coordinate of the cursor";
			this.tsLblSelY.Visible = false;
			// 
			// tsLblNumTiles
			// 
			this.tsLblNumTiles.AutoSize = false;
			this.tsLblNumTiles.Name = "tsLblNumTiles";
			this.tsLblNumTiles.Size = new System.Drawing.Size(180, 17);
			this.tsLblNumTiles.Text = "[num]";
			this.tsLblNumTiles.ToolTipText = "Number of tiles in the map";
			this.tsLblNumTiles.Visible = false;
			// 
			// tsTools
			// 
			this.tsTools.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsNew,
            this.tsOpen,
            this.tsSave,
            this.tsSep,
            this.tsArrow,
            this.tsMArrow,
            this.tsErase,
            this.tsPan});
			this.tsTools.Location = new System.Drawing.Point(0, 24);
			this.tsTools.Name = "tsTools";
			this.tsTools.Size = new System.Drawing.Size(765, 25);
			this.tsTools.TabIndex = 3;
			this.tsTools.Text = "toolStrip1";
			// 
			// tsNew
			// 
			this.tsNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsNew.Image = ((System.Drawing.Image)(resources.GetObject("tsNew.Image")));
			this.tsNew.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsNew.Name = "tsNew";
			this.tsNew.Size = new System.Drawing.Size(23, 22);
			this.tsNew.Text = "New Map...";
			this.tsNew.Click += new System.EventHandler(this.tsNew_Click);
			// 
			// tsOpen
			// 
			this.tsOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsOpen.Image = ((System.Drawing.Image)(resources.GetObject("tsOpen.Image")));
			this.tsOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsOpen.Name = "tsOpen";
			this.tsOpen.Size = new System.Drawing.Size(23, 22);
			this.tsOpen.Text = "Open Map...";
			this.tsOpen.Click += new System.EventHandler(this.tsOpen_Click);
			// 
			// tsSave
			// 
			this.tsSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsSave.Enabled = false;
			this.tsSave.Image = ((System.Drawing.Image)(resources.GetObject("tsSave.Image")));
			this.tsSave.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsSave.Name = "tsSave";
			this.tsSave.Size = new System.Drawing.Size(23, 22);
			this.tsSave.Text = "Save Map";
			this.tsSave.Click += new System.EventHandler(this.tsSave_Click);
			// 
			// tsSep
			// 
			this.tsSep.Name = "tsSep";
			this.tsSep.Size = new System.Drawing.Size(6, 25);
			// 
			// tsArrow
			// 
			this.tsArrow.Checked = true;
			this.tsArrow.CheckOnClick = true;
			this.tsArrow.CheckState = System.Windows.Forms.CheckState.Checked;
			this.tsArrow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsArrow.Image = ((System.Drawing.Image)(resources.GetObject("tsArrow.Image")));
			this.tsArrow.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsArrow.Name = "tsArrow";
			this.tsArrow.Size = new System.Drawing.Size(23, 22);
			this.tsArrow.Text = "Single Fill (alt+s)";
			this.tsArrow.Click += new System.EventHandler(this.tsArrow_Click);
			// 
			// tsMArrow
			// 
			this.tsMArrow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsMArrow.Image = global::GenericMapEditor.Properties.Resources.multiSelector;
			this.tsMArrow.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsMArrow.Name = "tsMArrow";
			this.tsMArrow.Size = new System.Drawing.Size(23, 22);
			this.tsMArrow.Text = "Multi Fill ";
			this.tsMArrow.Click += new System.EventHandler(this.tsMArrow_Click);
			// 
			// tsErase
			// 
			this.tsErase.CheckOnClick = true;
			this.tsErase.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsErase.Image = ((System.Drawing.Image)(resources.GetObject("tsErase.Image")));
			this.tsErase.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsErase.Name = "tsErase";
			this.tsErase.Size = new System.Drawing.Size(23, 22);
			this.tsErase.Text = "Eraser (alt+e)";
			this.tsErase.Click += new System.EventHandler(this.tsErase_Click);
			// 
			// tsPan
			// 
			this.tsPan.CheckOnClick = true;
			this.tsPan.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsPan.Image = ((System.Drawing.Image)(resources.GetObject("tsPan.Image")));
			this.tsPan.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsPan.Name = "tsPan";
			this.tsPan.Size = new System.Drawing.Size(23, 22);
			this.tsPan.Text = "Pan Map (alt+d)";
			this.tsPan.Click += new System.EventHandler(this.tsPan_Click);
			// 
			// tabsGround
			// 
			this.tabsGround.Controls.Add(this.pbTilePreview);
			this.tabsGround.Controls.Add(this.lvTiles);
			this.tabsGround.Location = new System.Drawing.Point(4, 22);
			this.tabsGround.Name = "tabsGround";
			this.tabsGround.Padding = new System.Windows.Forms.Padding(3);
			this.tabsGround.Size = new System.Drawing.Size(202, 455);
			this.tabsGround.TabIndex = 0;
			this.tabsGround.Text = "Graphics";
			this.tabsGround.UseVisualStyleBackColor = true;
			// 
			// lvTiles
			// 
			this.lvTiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lvTiles.HideSelection = false;
			this.lvTiles.Location = new System.Drawing.Point(6, 6);
			this.lvTiles.MultiSelect = false;
			this.lvTiles.Name = "lvTiles";
			this.lvTiles.Size = new System.Drawing.Size(190, 309);
			this.lvTiles.TabIndex = 0;
			this.lvTiles.TileSize = new System.Drawing.Size(32, 32);
			this.lvTiles.UseCompatibleStateImageBehavior = false;
			this.lvTiles.View = System.Windows.Forms.View.Tile;
			this.lvTiles.SelectedIndexChanged += new System.EventHandler(this.lvTiles_SelectedIndexChanged);
			// 
			// tabs
			// 
			this.tabs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabs.Controls.Add(this.tabsGround);
			this.tabs.Controls.Add(this.tabsSpecial);
			this.tabs.Controls.Add(this.tabsNPCs);
			this.tabs.Controls.Add(this.tabsItems);
			this.tabs.Location = new System.Drawing.Point(544, 52);
			this.tabs.Name = "tabs";
			this.tabs.SelectedIndex = 0;
			this.tabs.Size = new System.Drawing.Size(210, 481);
			this.tabs.TabIndex = 0;
			// 
			// tabsSpecial
			// 
			this.tabsSpecial.Controls.Add(this.groupBox2);
			this.tabsSpecial.Controls.Add(this.lvTiles2);
			this.tabsSpecial.Controls.Add(this.grpWarpPlacement);
			this.tabsSpecial.Controls.Add(this.lblSpecialTileType);
			this.tabsSpecial.Controls.Add(this.cmbSpecialType);
			this.tabsSpecial.Location = new System.Drawing.Point(4, 22);
			this.tabsSpecial.Name = "tabsSpecial";
			this.tabsSpecial.Padding = new System.Windows.Forms.Padding(3);
			this.tabsSpecial.Size = new System.Drawing.Size(202, 455);
			this.tabsSpecial.TabIndex = 1;
			this.tabsSpecial.Text = "Special";
			this.tabsSpecial.UseVisualStyleBackColor = true;
			// 
			// lvTiles2
			// 
			this.lvTiles2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lvTiles2.HideSelection = false;
			this.lvTiles2.Location = new System.Drawing.Point(6, 33);
			this.lvTiles2.MultiSelect = false;
			this.lvTiles2.Name = "lvTiles2";
			this.lvTiles2.Size = new System.Drawing.Size(190, 232);
			this.lvTiles2.TabIndex = 4;
			this.lvTiles2.TileSize = new System.Drawing.Size(32, 32);
			this.lvTiles2.UseCompatibleStateImageBehavior = false;
			this.lvTiles2.View = System.Windows.Forms.View.Tile;
			// 
			// grpWarpPlacement
			// 
			this.grpWarpPlacement.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grpWarpPlacement.Controls.Add(this.cmbWarpAnim);
			this.grpWarpPlacement.Controls.Add(this.lblWarpAnimation);
			this.grpWarpPlacement.Controls.Add(this.lblDestWarpMap);
			this.grpWarpPlacement.Controls.Add(this.txtDestWarpMap);
			this.grpWarpPlacement.Controls.Add(this.lblDestWarpY);
			this.grpWarpPlacement.Controls.Add(this.txtDestWarpY);
			this.grpWarpPlacement.Controls.Add(this.lblDestWarpX);
			this.grpWarpPlacement.Controls.Add(this.txtDestWarpX);
			this.grpWarpPlacement.Enabled = false;
			this.grpWarpPlacement.Location = new System.Drawing.Point(3, 326);
			this.grpWarpPlacement.Name = "grpWarpPlacement";
			this.grpWarpPlacement.Size = new System.Drawing.Size(196, 126);
			this.grpWarpPlacement.TabIndex = 3;
			this.grpWarpPlacement.TabStop = false;
			this.grpWarpPlacement.Text = "Warp Placement";
			// 
			// cmbWarpAnim
			// 
			this.cmbWarpAnim.FormattingEnabled = true;
			this.cmbWarpAnim.Location = new System.Drawing.Point(76, 97);
			this.cmbWarpAnim.Name = "cmbWarpAnim";
			this.cmbWarpAnim.Size = new System.Drawing.Size(108, 21);
			this.cmbWarpAnim.TabIndex = 1;
			// 
			// lblWarpAnimation
			// 
			this.lblWarpAnimation.AutoSize = true;
			this.lblWarpAnimation.Location = new System.Drawing.Point(17, 100);
			this.lblWarpAnimation.Name = "lblWarpAnimation";
			this.lblWarpAnimation.Size = new System.Drawing.Size(53, 13);
			this.lblWarpAnimation.TabIndex = 4;
			this.lblWarpAnimation.Text = "Animation";
			// 
			// lblDestWarpMap
			// 
			this.lblDestWarpMap.AutoSize = true;
			this.lblDestWarpMap.Location = new System.Drawing.Point(17, 74);
			this.lblDestWarpMap.Name = "lblDestWarpMap";
			this.lblDestWarpMap.Size = new System.Drawing.Size(98, 13);
			this.lblDestWarpMap.TabIndex = 4;
			this.lblDestWarpMap.Text = "Destination Map ID";
			// 
			// txtDestWarpMap
			// 
			this.txtDestWarpMap.Location = new System.Drawing.Point(124, 71);
			this.txtDestWarpMap.Name = "txtDestWarpMap";
			this.txtDestWarpMap.Size = new System.Drawing.Size(50, 20);
			this.txtDestWarpMap.TabIndex = 5;
			// 
			// lblDestWarpY
			// 
			this.lblDestWarpY.AutoSize = true;
			this.lblDestWarpY.Location = new System.Drawing.Point(17, 48);
			this.lblDestWarpY.Name = "lblDestWarpY";
			this.lblDestWarpY.Size = new System.Drawing.Size(101, 13);
			this.lblDestWarpY.TabIndex = 2;
			this.lblDestWarpY.Text = "Destination Y Coord";
			// 
			// txtDestWarpY
			// 
			this.txtDestWarpY.Location = new System.Drawing.Point(124, 45);
			this.txtDestWarpY.Name = "txtDestWarpY";
			this.txtDestWarpY.Size = new System.Drawing.Size(50, 20);
			this.txtDestWarpY.TabIndex = 3;
			// 
			// lblDestWarpX
			// 
			this.lblDestWarpX.AutoSize = true;
			this.lblDestWarpX.Location = new System.Drawing.Point(17, 22);
			this.lblDestWarpX.Name = "lblDestWarpX";
			this.lblDestWarpX.Size = new System.Drawing.Size(101, 13);
			this.lblDestWarpX.TabIndex = 0;
			this.lblDestWarpX.Text = "Destination X Coord";
			// 
			// txtDestWarpX
			// 
			this.txtDestWarpX.Location = new System.Drawing.Point(124, 19);
			this.txtDestWarpX.Name = "txtDestWarpX";
			this.txtDestWarpX.Size = new System.Drawing.Size(50, 20);
			this.txtDestWarpX.TabIndex = 1;
			// 
			// lblSpecialTileType
			// 
			this.lblSpecialTileType.AutoSize = true;
			this.lblSpecialTileType.Location = new System.Drawing.Point(6, 9);
			this.lblSpecialTileType.Name = "lblSpecialTileType";
			this.lblSpecialTileType.Size = new System.Drawing.Size(51, 13);
			this.lblSpecialTileType.TabIndex = 0;
			this.lblSpecialTileType.Text = "Tile Type";
			// 
			// cmbSpecialType
			// 
			this.cmbSpecialType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cmbSpecialType.Enabled = false;
			this.cmbSpecialType.FormattingEnabled = true;
			this.cmbSpecialType.Location = new System.Drawing.Point(63, 6);
			this.cmbSpecialType.Name = "cmbSpecialType";
			this.cmbSpecialType.Size = new System.Drawing.Size(133, 21);
			this.cmbSpecialType.TabIndex = 1;
			this.cmbSpecialType.SelectedIndexChanged += new System.EventHandler(this.cmbSpecialType_SelectedIndexChanged);
			// 
			// tabsNPCs
			// 
			this.tabsNPCs.Controls.Add(this.grpNPCInfo);
			this.tabsNPCs.Controls.Add(this.chkNPCMoves);
			this.tabsNPCs.Controls.Add(this.txtNPCMoveSpeed);
			this.tabsNPCs.Controls.Add(this.lblNPCMoveSpeed);
			this.tabsNPCs.Controls.Add(this.txtNPCId);
			this.tabsNPCs.Controls.Add(this.lblNPCid);
			this.tabsNPCs.Location = new System.Drawing.Point(4, 22);
			this.tabsNPCs.Name = "tabsNPCs";
			this.tabsNPCs.Padding = new System.Windows.Forms.Padding(3);
			this.tabsNPCs.Size = new System.Drawing.Size(202, 455);
			this.tabsNPCs.TabIndex = 2;
			this.tabsNPCs.Text = "NPCs";
			this.tabsNPCs.UseVisualStyleBackColor = true;
			// 
			// grpNPCInfo
			// 
			this.grpNPCInfo.Controls.Add(this.label4);
			this.grpNPCInfo.Location = new System.Drawing.Point(6, 98);
			this.grpNPCInfo.Name = "grpNPCInfo";
			this.grpNPCInfo.Size = new System.Drawing.Size(190, 189);
			this.grpNPCInfo.TabIndex = 6;
			this.grpNPCInfo.TabStop = false;
			this.grpNPCInfo.Text = "NPC Info";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(6, 87);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(177, 13);
			this.label4.TabIndex = 4;
			this.label4.Text = "THIS FEATURE MUST BE ADDED";
			// 
			// chkNPCMoves
			// 
			this.chkNPCMoves.AutoSize = true;
			this.chkNPCMoves.Location = new System.Drawing.Point(9, 60);
			this.chkNPCMoves.Name = "chkNPCMoves";
			this.chkNPCMoves.Size = new System.Drawing.Size(58, 17);
			this.chkNPCMoves.TabIndex = 2;
			this.chkNPCMoves.Text = "Moves";
			this.chkNPCMoves.UseVisualStyleBackColor = true;
			// 
			// txtNPCMoveSpeed
			// 
			this.txtNPCMoveSpeed.Location = new System.Drawing.Point(80, 32);
			this.txtNPCMoveSpeed.Name = "txtNPCMoveSpeed";
			this.txtNPCMoveSpeed.Size = new System.Drawing.Size(49, 20);
			this.txtNPCMoveSpeed.TabIndex = 1;
			// 
			// lblNPCMoveSpeed
			// 
			this.lblNPCMoveSpeed.AutoSize = true;
			this.lblNPCMoveSpeed.Location = new System.Drawing.Point(6, 35);
			this.lblNPCMoveSpeed.Name = "lblNPCMoveSpeed";
			this.lblNPCMoveSpeed.Size = new System.Drawing.Size(71, 13);
			this.lblNPCMoveSpeed.TabIndex = 0;
			this.lblNPCMoveSpeed.Text = "Move Speed:";
			// 
			// txtNPCId
			// 
			this.txtNPCId.Location = new System.Drawing.Point(80, 6);
			this.txtNPCId.Name = "txtNPCId";
			this.txtNPCId.Size = new System.Drawing.Size(49, 20);
			this.txtNPCId.TabIndex = 1;
			// 
			// lblNPCid
			// 
			this.lblNPCid.AutoSize = true;
			this.lblNPCid.Location = new System.Drawing.Point(6, 9);
			this.lblNPCid.Name = "lblNPCid";
			this.lblNPCid.Size = new System.Drawing.Size(46, 13);
			this.lblNPCid.TabIndex = 0;
			this.lblNPCid.Text = "NPC ID:";
			// 
			// tabsItems
			// 
			this.tabsItems.Controls.Add(this.groupBox1);
			this.tabsItems.Controls.Add(this.txtItemRespawn);
			this.tabsItems.Controls.Add(this.lblItemRespawn);
			this.tabsItems.Controls.Add(this.chkItemHidden);
			this.tabsItems.Controls.Add(this.txtItemID);
			this.tabsItems.Controls.Add(this.lblItemID);
			this.tabsItems.Location = new System.Drawing.Point(4, 22);
			this.tabsItems.Name = "tabsItems";
			this.tabsItems.Padding = new System.Windows.Forms.Padding(3);
			this.tabsItems.Size = new System.Drawing.Size(202, 455);
			this.tabsItems.TabIndex = 3;
			this.tabsItems.Text = "Items";
			this.tabsItems.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Location = new System.Drawing.Point(6, 98);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(190, 189);
			this.groupBox1.TabIndex = 5;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Item Info";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(6, 87);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(177, 13);
			this.label3.TabIndex = 4;
			this.label3.Text = "THIS FEATURE MUST BE ADDED";
			// 
			// txtItemRespawn
			// 
			this.txtItemRespawn.Location = new System.Drawing.Point(80, 32);
			this.txtItemRespawn.Name = "txtItemRespawn";
			this.txtItemRespawn.Size = new System.Drawing.Size(49, 20);
			this.txtItemRespawn.TabIndex = 4;
			// 
			// lblItemRespawn
			// 
			this.lblItemRespawn.AutoSize = true;
			this.lblItemRespawn.Location = new System.Drawing.Point(6, 35);
			this.lblItemRespawn.Name = "lblItemRespawn";
			this.lblItemRespawn.Size = new System.Drawing.Size(69, 13);
			this.lblItemRespawn.TabIndex = 3;
			this.lblItemRespawn.Text = "Respawn (s):";
			// 
			// chkItemHidden
			// 
			this.chkItemHidden.AutoSize = true;
			this.chkItemHidden.Location = new System.Drawing.Point(9, 60);
			this.chkItemHidden.Name = "chkItemHidden";
			this.chkItemHidden.Size = new System.Drawing.Size(60, 17);
			this.chkItemHidden.TabIndex = 2;
			this.chkItemHidden.Text = "Hidden";
			this.chkItemHidden.UseVisualStyleBackColor = true;
			// 
			// txtItemID
			// 
			this.txtItemID.Location = new System.Drawing.Point(80, 6);
			this.txtItemID.Name = "txtItemID";
			this.txtItemID.Size = new System.Drawing.Size(49, 20);
			this.txtItemID.TabIndex = 1;
			// 
			// lblItemID
			// 
			this.lblItemID.AutoSize = true;
			this.lblItemID.Location = new System.Drawing.Point(6, 9);
			this.lblItemID.Name = "lblItemID";
			this.lblItemID.Size = new System.Drawing.Size(44, 13);
			this.lblItemID.TabIndex = 0;
			this.lblItemID.Text = "Item ID:";
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(32, 19);
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.txtDensity);
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Location = new System.Drawing.Point(7, 271);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(189, 49);
			this.groupBox2.TabIndex = 5;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Solid Properties";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(17, 22);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(42, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Density";
			// 
			// txtDensity
			// 
			this.txtDensity.Location = new System.Drawing.Point(65, 19);
			this.txtDensity.Name = "txtDensity";
			this.txtDensity.Size = new System.Drawing.Size(105, 20);
			this.txtDensity.TabIndex = 1;
			this.txtDensity.Text = "0";
			this.txtDensity.TextChanged += new System.EventHandler(this.txtDensity_TextChanged);
			// 
			// pbTilePreview
			// 
			this.pbTilePreview.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pbTilePreview.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
			this.pbTilePreview.Location = new System.Drawing.Point(36, 321);
			this.pbTilePreview.Name = "pbTilePreview";
			this.pbTilePreview.Size = new System.Drawing.Size(128, 128);
			this.pbTilePreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pbTilePreview.TabIndex = 1;
			this.pbTilePreview.TabStop = false;
			// 
			// mapViewer1
			// 
			this.mapViewer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.mapViewer1.DrawImages = null;
			this.mapViewer1.GridText = null;
			this.mapViewer1.LayerNPC = false;
			this.mapViewer1.LayerSpecial = false;
			this.mapViewer1.LayerTile = false;
			this.mapViewer1.Location = new System.Drawing.Point(2, 52);
			this.mapViewer1.Name = "mapViewer1";
			this.mapViewer1.Offset = new Microsoft.Xna.Framework.Vector2(float.NaN, float.NaN);
			this.mapViewer1.SelectorPosition = new Microsoft.Xna.Framework.Vector2(0F, 0F);
			this.mapViewer1.SelectorText = null;
			this.mapViewer1.ShowGrid = false;
			this.mapViewer1.Size = new System.Drawing.Size(537, 481);
			this.mapViewer1.State = GenericMapEditor.PlaceState.SINGLEFILL;
			this.mapViewer1.TabIndex = 2;
			this.mapViewer1.Text = "mapViewer1";
			this.mapViewer1.TileSize = 0;
			this.mapViewer1.ZoomFactor = 0D;
			this.mapViewer1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.UpdateMapViewer_MouseMove);
			this.mapViewer1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.UpdateMapViewer_MouseMove);
			this.mapViewer1.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.UpdateMapViewer_MouseWheel);
			// 
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(765, 561);
			this.Controls.Add(this.tabs);
			this.Controls.Add(this.tsTools);
			this.Controls.Add(this.mapViewer1);
			this.Controls.Add(this.ssMain);
			this.Controls.Add(this.mnuMain);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.MainMenuStrip = this.mnuMain;
			this.MinimumSize = new System.Drawing.Size(781, 500);
			this.Name = "frmMain";
			this.Text = "Generic Map Editor";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
			this.Load += new System.EventHandler(this.frmMain_Load);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmMain_KeyDown);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmMain_KeyUp);
			this.mnuMain.ResumeLayout(false);
			this.mnuMain.PerformLayout();
			this.ssMain.ResumeLayout(false);
			this.ssMain.PerformLayout();
			this.tsTools.ResumeLayout(false);
			this.tsTools.PerformLayout();
			this.tabsGround.ResumeLayout(false);
			this.tabs.ResumeLayout(false);
			this.tabsSpecial.ResumeLayout(false);
			this.tabsSpecial.PerformLayout();
			this.grpWarpPlacement.ResumeLayout(false);
			this.grpWarpPlacement.PerformLayout();
			this.tabsNPCs.ResumeLayout(false);
			this.tabsNPCs.PerformLayout();
			this.grpNPCInfo.ResumeLayout(false);
			this.grpNPCInfo.PerformLayout();
			this.tabsItems.ResumeLayout(false);
			this.tabsItems.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbTilePreview)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mnuMain;
        private System.Windows.Forms.ToolStripMenuItem mnuFile;
        private System.Windows.Forms.ToolStripMenuItem mnuHelp;
        private System.Windows.Forms.StatusStrip ssMain;
        private System.Windows.Forms.ToolStripStatusLabel tslblFileName;
        private System.Windows.Forms.ToolStripMenuItem mnuView;
        private MapViewer mapViewer1;
        private System.Windows.Forms.ToolStripMenuItem mnuNew;
        private System.Windows.Forms.ToolStripMenuItem mnuOpen;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem mnuSave;
        private System.Windows.Forms.ToolStripMenuItem mnuSaveAs;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mnuClose;
        private System.Windows.Forms.ToolStripMenuItem mnuExit;
        private System.Windows.Forms.ToolStripMenuItem mnuEdit;
        private System.Windows.Forms.ToolStripMenuItem mnuUndo;
        private System.Windows.Forms.ToolStripMenuItem mnuRedo;
        private System.Windows.Forms.ToolStripMenuItem mnuLayers;
        private System.Windows.Forms.ToolStripMenuItem mnuGrid;
        private System.Windows.Forms.ToolStripMenuItem mnuZoomIn;
        private System.Windows.Forms.ToolStripMenuItem mnuAbout;
		private System.Windows.Forms.ToolStripMenuItem mnuMap;
        private System.Windows.Forms.ToolStripMenuItem mnuSpawnData;
        private System.Windows.Forms.ToolStripMenuItem mnuProperties;
        private System.Windows.Forms.ToolStripMenuItem mnuTileLayer;
        private System.Windows.Forms.ToolStripMenuItem mnuSpecialLayer;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem mnuZoomOut;
        private System.Windows.Forms.ToolStripStatusLabel tsLblSelX;
        private System.Windows.Forms.ToolStripStatusLabel tsLblSelY;
        private System.Windows.Forms.ToolStrip tsTools;
        private System.Windows.Forms.ToolStripButton tsNew;
        private System.Windows.Forms.ToolStripButton tsOpen;
        private System.Windows.Forms.ToolStripButton tsSave;
        private System.Windows.Forms.ToolStripSeparator tsSep;
        private System.Windows.Forms.ToolStripButton tsArrow;
        private System.Windows.Forms.ToolStripButton tsErase;
        private System.Windows.Forms.ToolStripButton tsPan;
        private System.Windows.Forms.ToolStripSeparator mnuSep1;
        private System.Windows.Forms.ToolStripMenuItem mnuTestMap;
        private System.Windows.Forms.TabPage tabsGround;
		private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.ListView lvTiles;
        private System.Windows.Forms.ToolStripStatusLabel tsLblNumTiles;
        private System.Windows.Forms.ToolStripMenuItem mnuClearLayer;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem hmnuHidden;
        private System.Windows.Forms.ToolStripMenuItem hmnuSelector;
        private System.Windows.Forms.ToolStripMenuItem hmnuEraser;
        private System.Windows.Forms.ToolStripMenuItem hmnuPan;
        private System.Windows.Forms.ToolStripStatusLabel tslblZoomLevel;
		private System.Windows.Forms.TabPage tabsSpecial;
        private System.Windows.Forms.Label lblSpecialTileType;
        private System.Windows.Forms.ComboBox cmbSpecialType;
        private System.Windows.Forms.GroupBox grpWarpPlacement;
        private System.Windows.Forms.Label lblDestWarpMap;
        private System.Windows.Forms.TextBox txtDestWarpMap;
        private System.Windows.Forms.Label lblDestWarpY;
        private System.Windows.Forms.TextBox txtDestWarpY;
        private System.Windows.Forms.Label lblDestWarpX;
        private System.Windows.Forms.TextBox txtDestWarpX;
        private System.Windows.Forms.ToolStripMenuItem mnuFileRecent;
        private System.Windows.Forms.ComboBox cmbWarpAnim;
        private System.Windows.Forms.Label lblWarpAnimation;
        private System.Windows.Forms.TabPage tabsNPCs;
        private System.Windows.Forms.Label lblNPCid;
        private System.Windows.Forms.CheckBox chkNPCMoves;
        private System.Windows.Forms.TextBox txtNPCMoveSpeed;
        private System.Windows.Forms.Label lblNPCMoveSpeed;
        private System.Windows.Forms.TabPage tabsItems;
        private System.Windows.Forms.GroupBox grpNPCInfo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtItemRespawn;
        private System.Windows.Forms.Label lblItemRespawn;
        private System.Windows.Forms.CheckBox chkItemHidden;
        private System.Windows.Forms.TextBox txtItemID;
		private System.Windows.Forms.Label lblItemID;
        private System.Windows.Forms.TextBox txtNPCId;
		private System.Windows.Forms.ToolStripButton tsMArrow;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem tilesetToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem recentToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
		private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem;
		private System.Windows.Forms.ListView lvTiles2;
		private InterpolatedPictureBox pbTilePreview;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.TextBox txtDensity;
		private System.Windows.Forms.Label label1;
    }
}

