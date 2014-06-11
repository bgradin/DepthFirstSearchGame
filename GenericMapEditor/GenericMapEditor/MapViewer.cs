using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using GameClassLibrary;

namespace GenericMapEditor
{
    class MapViewer : GraphicsDeviceControl
    {
        SpriteBatch sb;
        
        Vector2 selectorPos;
        Vector2 _offset;
        
        Texture2D whiteText;
        Rectangle drawArea;

        SortedList<string, Tile>[] dlayers;

		public int TileSize { get; set; }

        bool animate = false;
        System.Windows.Forms.Timer animTimer;

        public Vector2 SelectorPosition
        {
            get { return this.selectorPos; }
            set { this.selectorPos = value; }
        }
        public Vector2 Offset
        {
            get { return this._offset; }
            set { this._offset = value; }
        }
        public List<Texture2D> DrawImages { get; set; }
        public Texture2D SelectorText { get; set; }
        public Texture2D GridText { get; set; }
        public Rectangle Area
        {
            get { return this.drawArea; }
        }
        public bool ShowGrid { get; set; }
        public bool LayerTile { get; set; }
        public bool LayerSpecial { get; set; }
        public bool LayerNPC { get; set; }
        public PlaceState State { get; set; }

        double zf;
        public double ZoomFactor
        {
            get { return this.zf; }
            set
            {
                drawArea.Width = (int)(drawArea.Width / zf);
                drawArea.Height = (int)(drawArea.Height / zf);
                this.zf = value;
                drawArea.Width = (int)(drawArea.Width * zf);
                drawArea.Height = (int)(drawArea.Height * zf);
            }
        }

        public void SetMapData(SortedList<string, Tile>[] layerData, int width, int height)
        {
            dlayers = layerData;
            drawArea = new Rectangle(0, 0, (int)(width * TileSize * ZoomFactor), (int)(height * TileSize * ZoomFactor));
        }

        public void ClearMapData()
        {
            dlayers = null;
            drawArea = new Rectangle();
        }
        
        protected override void Initialize()
        {
            selectorPos = new Vector2(0, 0);
            _offset = new Vector2(0, 0);
            sb = new SpriteBatch(GraphicsDevice);
            ZoomFactor = 1;

            dlayers = null;
            DrawImages = null;
            
            whiteText = new Texture2D(GraphicsDevice, 1, 1);
            whiteText.SetData<Color>(new Color[] { Color.White });

            LayerTile = true;
            LayerSpecial = true;

            animTimer = new System.Windows.Forms.Timer();
            animTimer.Enabled = true;
            animTimer.Tick += new EventHandler(animTimer_Tick);
            animTimer.Interval = 500;
        }

        protected override void Draw() //only draw tiles in display pane - not outside of bounds
        {
            GraphicsDevice.Clear(Color.Gray);

            sb.Begin();
            if (dlayers != null)
            {
                Rectangle tempArea = new Rectangle((int)_offset.X, (int)_offset.Y, drawArea.Width, drawArea.Height);
                sb.Draw(whiteText, tempArea, Color.White); //whitespace for map area

				Rectangle renderArea = new Rectangle(0, 0, drawArea.Width / TileSize, drawArea.Height / TileSize);
                if (tempArea.X < 0) //top-left:x is to the left of the viewport (start part-way through)
					renderArea.X = (int)(Math.Abs(tempArea.X) / TileSize);
                if (tempArea.Y < 0) //top-left:y is above the top of the viewport
					renderArea.Y = (int)(Math.Abs(tempArea.Y) / TileSize);
                if (tempArea.Right > GraphicsDevice.Viewport.Width) //right of drawArea is out of viewport width
					renderArea.Width = (int)((GraphicsDevice.Viewport.Width + Math.Abs(tempArea.X)) / TileSize);
                if (tempArea.Bottom > GraphicsDevice.Viewport.Height) //bottom of drawArea is out of viewport height
					renderArea.Height = (int)((GraphicsDevice.Viewport.Height + Math.Abs(tempArea.Y)) / TileSize);
                
                if (dlayers[(int)LAYERS.Graphic] != null && LayerTile)
                    foreach (KeyValuePair<string, Tile> tile in dlayers[(int)LAYERS.Graphic])
                    {
                        if (tile.Value.X * zf < renderArea.X || tile.Value.X * zf > renderArea.Width
                            || tile.Value.Y * zf < renderArea.Y || tile.Value.Y * zf > renderArea.Height) //need to multiply comparison by zf otherwise culling will occur when I don't want it to
                            continue;

						Rectangle sourceRect = new Rectangle(0, 0, TileSize, TileSize);
                        if (tile.Value is AnimatedTile)
                        {
                            AnimatedTile at = tile.Value as AnimatedTile;
                            if (animate)
                                at.Animate();
							sourceRect = new Rectangle(at.Frame * TileSize, 0, TileSize, TileSize); //render the proper frame for animated tiles
                        }

                        sb.Draw(DrawImages[(tile.Value as GraphicTile).Graphic],
							new Vector2((int)(tile.Value.X * TileSize * ZoomFactor) + _offset.X, (int)(tile.Value.Y * TileSize * ZoomFactor) + _offset.Y), 
                            sourceRect, Color.White, 0, Vector2.Zero, (float)zf, SpriteEffects.None, 0);
                    }
                if(dlayers[(int)LAYERS.Special] != null && LayerSpecial)
                    foreach (KeyValuePair<string, Tile> tile in dlayers[(int)LAYERS.Special])
                    {
                        if (tile.Value.X * zf < renderArea.X || tile.Value.X * zf > renderArea.Width
                            || tile.Value.Y * zf < renderArea.Y || tile.Value.Y * zf > renderArea.Height || (tile.Value as SpecialTile).Type == SpecialTileSpec.NONE)
                            continue;

                        sb.Draw(DrawImages[(int)(tile.Value as SpecialTile).Graphic],
                            new Vector2((int)(tile.Value.X * TileSize * ZoomFactor) + _offset.X, (int)(tile.Value.Y * TileSize * ZoomFactor) + _offset.Y),
                            null, Color.White, 0, new Vector2(0), (float)zf, SpriteEffects.None, 0);
                    }
                if(dlayers[(int)LAYERS.NPC] != null && LayerNPC)
                    foreach (KeyValuePair<string, Tile> tile in dlayers[(int)LAYERS.NPC])
                    {
                        if (tile.Value.X * zf < renderArea.X || tile.Value.X * zf > renderArea.Width
                            || tile.Value.Y * zf < renderArea.Y || tile.Value.Y * zf > renderArea.Height)
                            continue;

                        //get indices of NPCs based on NPC ID and draw
                    }
                //To be added: add draw logic for other implemented layers

                if (ShowGrid) //draw grid on top of map tiles
                {
                    for (int i = 0; i < drawArea.Height; i += (int)(TileSize * ZoomFactor))
                    {
                        for (int j = 0; j < drawArea.Width; j += (int)(TileSize * ZoomFactor))
                        {
                            sb.Draw(GridText, new Vector2(j + (int)_offset.X, i + (int)_offset.Y), 
                                null, Color.White, 0, new Vector2(0), (float)zf, SpriteEffects.None, 0);
                        }
                    }
                }

                if (State == PlaceState.SINGLEFILL || State == PlaceState.MULTIFILL || State == PlaceState.ERASER)//draw selector on top of all
                {
                    //selectorPos is modified in the update method in the main form
                    sb.Draw(SelectorText, selectorPos, null, Color.White, 0, new Vector2(0), (float)zf * TileSize / SelectorText.Width, SpriteEffects.None, 0);
                }

                animate = false;
            }
            sb.End();
        }

        public void ResetAnim()
        {
            if (dlayers == null)
                return;

            int numRedone = 0;
            foreach (Tile t in dlayers[0].Values)
            {
                if (!(t is AnimatedTile))
                    continue;
                AnimatedTile tt = (AnimatedTile)t;
                tt.Reset();
                numRedone++;
            }

            if(numRedone >= 2) //redraw if we've redone 2 or more
                this.Refresh();
        }

        public void animTimer_Tick(object sender, EventArgs e)
        {
            this.animate = true;
            this.Refresh();
        }
    }
}
