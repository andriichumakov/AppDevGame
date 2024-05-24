using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;

namespace AppDevGame
{
    public class FontLoader : ResourceLoader
    {
        private ContentManager _content;
        private Dictionary<string, SpriteFont> _fonts;

        public FontLoader(ContentManager content)
            : base("", new List<string> { ".spritefont" }) // Ensure subfolder is set correctly
        {
            _content = content;
            _fonts = new Dictionary<string, SpriteFont>();
        }

        public override void LoadContent()
        {
            string path = Path.Combine(_content.RootDirectory, _subfolder);
            MainApp.Log("Loading fonts from: " + path);

            // Check if directory exists before trying to load
            if (!Directory.Exists(path))
            {
                MainApp.Log("Font directory not found: " + path);
                return;
            }

            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                MainApp.Log(file);
                string extension = Path.GetExtension(file);
                if (_allowedTypes.Contains(extension))
                {
                    string fileName = Path.GetFileNameWithoutExtension(file);
                    MainApp.Log("Loaded font: " + fileName);
                    _fonts[fileName] = _content.Load<SpriteFont>(Path.Combine(_subfolder, fileName));
                }
            }
        }

        public SpriteFont GetResource(string key)
        {
            if (_fonts.ContainsKey(key))
            {
                return _fonts[key];
            }
            return null;
        }
    }
}