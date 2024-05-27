using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
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

        public void LoadSpecificResource(string path, string key)
        {
            try
            {
                using (FileStream stream = new FileStream(path, FileMode.Open))
                {
                    MainApp.Log("Loaded specific texture: " + key);
                    _textures[key] = Texture2D.FromStream(_graphics, stream);
                }
            }
            catch (FileNotFoundException ex)
            {
                MainApp.Log($"Error loading texture {key}: {ex.Message}");
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