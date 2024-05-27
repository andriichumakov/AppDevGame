using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;

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
            string path = Path.Combine("Content", _subfolder);
            MainApp.Log("Loading textures from: " + path);
            
            if (!Directory.Exists(path))
            {
                MainApp.Log("Texture directory not found: " + path);
                return;
            }

            string[] files = Directory.GetFiles(path);
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