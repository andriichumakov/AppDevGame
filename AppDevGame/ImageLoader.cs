using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;

using AppDevGame;

namespace AppDevGame
{
    public class ImageLoader : ResourceLoader
    {
        private GraphicsDevice _graphics;
        private Dictionary<string, Texture2D> _textures;

        public ImageLoader(GraphicsDevice graphics) 
            : base("gfx", new List<string> { ".png", ".jpg", ".jpeg" })
        {
            _graphics = graphics;
            _textures = new Dictionary<string, Texture2D>();
        }

        public override void LoadContent(ContentManager content)
        {
            string[] files = Directory.GetFiles(Path.Combine(content.RootDirectory, _subfolder));
            foreach (string file in files)
            {
                string extension = Path.GetExtension(file);
                if (_allowedTypes.Contains(extension))
                {
                    using (FileStream stream = new FileStream(file, FileMode.Open))
                    {
                        string fileName = Path.GetFileNameWithoutExtension(file);
                        _textures[fileName] = Texture2D.FromStream(_graphics, stream);
                    }
                }
            }
        }

        public Texture2D GetTexture(string key)
        {
            if (_textures.ContainsKey(key))
            {
                return _textures[key];
            }
            return null;
        }
    }
}