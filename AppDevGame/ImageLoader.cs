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
            : base("", new List<string> { ".png", ".jpg", ".jpeg" })
        {
            _graphics = graphics;
            _textures = new Dictionary<string, Texture2D>();
        }

        public override void LoadContent()
        {
            MainApp.Log("Loading textures from: " + Path.Combine("Content", _subfolder));
            string[] files = Directory.GetFiles(Path.Combine("Content", _subfolder));
            foreach (string file in files)
            {
                MainApp.Log(file);
                string extension = Path.GetExtension(file);
                if (_allowedTypes.Contains(extension))
                {
                    using (FileStream stream = new FileStream(file, FileMode.Open))
                    {
                        string fileName = Path.GetFileNameWithoutExtension(file);
                        MainApp.Log("Loaded texture: " + fileName);
                        _textures[fileName] = Texture2D.FromStream(_graphics, stream);
                    }
                }
            }
        }

        public Texture2D GetResource(string key)
        {
            if (_textures.ContainsKey(key))
            {
                return _textures[key];
            }
            return null;
        }
    }
}