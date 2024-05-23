using AppDevGame;
using System.Collections.Generic;
using System.IO;

namespace AppDevGame
{
    public abstract class ResourceLoader 
    {
        protected static readonly string _contentPath = "Content";
        protected static List<string> _modPaths = new List<string>();
        protected List<string> _allowedFileTypes;
        protected string _loaderSubFolder;
        protected string _backupResourcePath;

        public ResourceLoader(List<string> allowedFileTypes, string subfolder, string backupResourcePath) 
        {
            _allowedFileTypes = allowedFileTypes;
            _loaderSubFolder = subfolder;
            _backupResourcePath = backupResourcePath;
        }

        public static void AddModPath(string modPath) 
        {
            if (Directory.Exists(modPath)) {
                _modPaths.Add(modPath);
            } else {
                Console.WriteLine("Could not load the mod path: " + modPath);
            }
        }

        public bool DoesSubfolderExist(string mainPath) 
        {
            return Directory.Exists(Path.Combine(mainPath, _loaderSubFolder));
        }

        public void LoadContent() 
        {
            List<string> pathList = new List<string>();

            if (Directory.Exists(_contentPath)) {
                pathList.Add(Path.Combine(_contentPath, _loaderSubFolder));
            }

            foreach (string modPath in _modPaths) {
                if (DoesSubfolderExist(modPath)) {
                    pathList.Add(Path.Combine(modPath, _loaderSubFolder));
                }
            }

            foreach (string path in pathList) {
                string[] files = Directory.GetFiles(path);
                foreach (string file in files) {
                    string extension = Path.GetExtension(file);
                    if (_allowedFileTypes.Contains(extension)) {
                        LoadFile(file);
                    } else {
                        Console.WriteLine("Warning: unexpected file type in " + _loaderSubFolder + ": " + file + extension);
                    }
                }
            }
        }

        public abstract void LoadFile(string filePath);
        public virtual object GetResource(string key) {
            return null;
        }
    }

    public class ImageLoader : ResourceLoader 
    {
        private GraphicsDevice _graphics;
        private Dictionary<string, Texture2D> _textures;

        public ImageLoader(GraphicsDevice graphics, string backupResourcePath) 
            : base(new List<string> {".png", ".jpg", ".jpeg"}, "gfx", backupResourcePath) 
        {
            _graphics = graphics;
            _textures = new Dictionary<string, Texture2D>();
        }

        public override void LoadFile(string filePath) 
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Open))
            {
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                _textures[fileName] = Texture2D.FromStream(_graphics, stream);
            }
        }

        public new Texture2D GetResource(string key) 
        {
            if (_textures.ContainsKey(key)) {
                return _textures[key];
            } else {
                return _textures.ContainsKey(_backupResourcePath) ? _textures[_backupResourcePath] : null;
            }
        }
    }

    public class FontLoader : ResourceLoader
    {
        private ContentManager _content;
        private Dictionary<string, SpriteFont> _fonts;

        public FontLoader(ContentManager content, string backupResourcePath)
            : base(new List<string> { ".spritefont" }, "fonts", backupResourcePath)
        {
            _content = content;
            _fonts = new Dictionary<string, SpriteFont>();
        }

        public override void LoadFile(string filePath)
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            try
            {
                SpriteFont font = _content.Load<SpriteFont>(Path.Combine(_loaderSubFolder, fileName));
                _fonts[fileName] = font;
            }
            catch (ContentLoadException)
            {
                Console.WriteLine($"Error loading font: {fileName}");
            }
        }

        public override object GetResource(string key)
        {
            if (_fonts.ContainsKey(key))
            {
                return _fonts[key];
            }
            else
            {
                Console.WriteLine($"Font {key} not found. Returning backup font.");
                return _content.Load<SpriteFont>(_backupResourcePath);
            }
        }
    }
}