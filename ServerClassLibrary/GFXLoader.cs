using GameClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ServerClassLibrary
{
    public enum PGFXDir
    {
        GraphicTiles,
        SpecialTiles,
        NPCTiles,
        InteractiveTiles,
        PlayerSprite,
        //add more kinds of GFX
        NUM_VALS
    }

    public class PGFXCollection
    {
        public const string CONTENT_ROOT_DIR = "GFX";
        private const string GRAPHIC_TILE_DIRECTORY = CONTENT_ROOT_DIR + @"\GraphicTile\";
        private const string SPECIAL_TILE_DIRECTORY = CONTENT_ROOT_DIR + @"\SpecialTile\";
        private const string NPC_TILE_DIRECTORY = CONTENT_ROOT_DIR + @"\NPCTile\";
        private const string INTERACTIVE_TILE_DIRECTORY = CONTENT_ROOT_DIR + @"\InteractiveTile\";

        private ContentManager manager;
        private Dictionary<string, int> nameMapper;
        private List<Texture2D> textures;
        private List<Bitmap> bmpTextures;
        private List<int> animatedTiles;

        public PGFXCollection(ContentManager content)
        {
            if (content == null)
                throw new ArgumentNullException(@"'content' must not be null.");
            manager = content;
            nameMapper = new Dictionary<string, int>();
            textures = new List<Texture2D>();
            animatedTiles = new List<int>();
            bmpTextures = new List<Bitmap>();
        }

        public bool Load(PGFXDir dir)
        {
            string directory;
            switch (dir)
            {
                case PGFXDir.GraphicTiles: directory = GRAPHIC_TILE_DIRECTORY; break;
                case PGFXDir.SpecialTiles: directory = SPECIAL_TILE_DIRECTORY; break;
                case PGFXDir.NPCTiles: directory = NPC_TILE_DIRECTORY; break;
                case PGFXDir.InteractiveTiles: directory = INTERACTIVE_TILE_DIRECTORY; break;
                default: return false;
            }

            if (!Directory.Exists(directory))
                return false;

            string[] filenames = Directory.GetFiles(directory, "*.xnb", SearchOption.AllDirectories);
            if (filenames == null || filenames.Length == 0)
            {
                return false;
            }

            foreach (string name in filenames)
            {
                string contentName;
                int slashindex = name.IndexOf(CONTENT_ROOT_DIR + @"\"); //strip off the root directory if present
                if (slashindex < 0)
                    contentName = name;
                else
                    contentName = name.Substring(slashindex + CONTENT_ROOT_DIR.Length + 1);
                contentName = contentName.Replace(".xnb", "");
                textures.Add(manager.Load<Texture2D>(contentName));
                textures[textures.Count - 1].Name = contentName.Contains('\\') ? contentName.Substring(contentName.IndexOf('\\') + 1) : contentName;
                nameMapper.Add(textures[textures.Count - 1].Name, textures.Count - 1);
                bmpTextures.Add(hlpConvertToBitmap(textures.Count - 1));
                if (textures[textures.Count - 1].Width != Const.TILE_SIZE)
                {
                    animatedTiles.Add(textures.Count - 1);
                }
            }

            return true;
        }

        private Bitmap hlpConvertToBitmap(int index)
        {            
            using (MemoryStream mem = new MemoryStream())
            {
                Texture2D source = textures[index];

                if (source.Width == Const.TILE_SIZE)
                {
                    source.SaveAsPng(mem, source.Width, source.Height);
                }
                else
                {
                    Microsoft.Xna.Framework.Color[] data = new Microsoft.Xna.Framework.Color[Const.TILE_SIZE * Const.TILE_SIZE];
                    source.GetData<Microsoft.Xna.Framework.Color>(0, new Microsoft.Xna.Framework.Rectangle(0, 0, Const.TILE_SIZE, Const.TILE_SIZE), data, 0, Const.TILE_SIZE * Const.TILE_SIZE);
                    Texture2D tempText = new Texture2D(source.GraphicsDevice, Const.TILE_SIZE, Const.TILE_SIZE);
                    tempText.SetData<Microsoft.Xna.Framework.Color>(data);
                    tempText.SaveAsPng(mem, Const.TILE_SIZE, Const.TILE_SIZE);
                }

                return new Bitmap(mem);
            }
        }

        public Image ElementAsImage(string elemName)
        {
            return ElementAsImage(nameMapper[elemName]);
        }

        public Image ElementAsImage(int index)
        {
            if (index < 0 || index >= bmpTextures.Count)
                return null;

            return bmpTextures[index];
        }

        public bool IsAnimatedByIndex(int index)
        {
            return animatedTiles.Contains(index);
        }

        public List<Texture2D> GetTextureCollection()
        {
            return textures;
        }

        public Texture2D this[int index]
        {
            get
            {
                if (index < 0 || index >= textures.Count)
                    return null;
                return textures[index];
            }
        }

        public Texture2D this[string elemName]
        {
            get
            {
                if (!nameMapper.ContainsKey(elemName))
                    return null;
                return textures[nameMapper[elemName]];
            }
        }

        public int NumTextures
        {
            get { return this.textures.Count; }
        }
    }
}
