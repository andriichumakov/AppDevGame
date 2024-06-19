using Microsoft.Xna.Framework;
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
            string contentPath = Path.Combine("Content", _subfolder);
            MainApp.Log("Loading textures from: " + contentPath);
            string[] files = Directory.GetFiles(contentPath);
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

        public void LoadSpecificResource(string path, string key)
        {
            try
            {
                string fullPath = Path.Combine("Content", path);
                if (!File.Exists(fullPath))
                {
                    throw new FileNotFoundException($"File not found: {fullPath}");
                }

                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    _textures[key] = Texture2D.FromStream(_graphics, stream);
                    MainApp.Log($"Loaded texture: {key}");
                }
            }
            catch (Exception ex)
            {
                MainApp.Log($"Error loading texture from {path}: {ex.Message}");
                throw; // Re-throw the exception after logging it
            }
        }
    }
}