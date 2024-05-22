using AppDevGame;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace AppDevGame
{
    public class MainApp: Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private WindowManager _windowManager;
        private ImageLoader _imageLoader;

        public MainApp() 
        {
            this._graphics = new GraphicsDeviceManager(this);
            this._windowManager = new WindowManager();
            this._spriteBatch = new SpriteBatch(this.GraphicsDevice);
            this._imageLoader = new ImageLoader(this.GraphicsDevice, "backup.png");
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
        
            base.Initialize();
        }

        protected override void LoadContent()
        {
            this._imageLoader.LoadContent();
        }

        protected Texture2D GetTexture(string key) {
            return this._imageLoader.GetResource(key);
        }
    }

    public class WindowManager 
    {
        private BaseWindow _currentWindow;
        private GraphicsDeviceManager _graphicsManager;
        private GraphicsDevice _graphics;

        public WindowManager(BaseWindow currentWindow) 
        {
            this._currentWindow = currentWindow;
        }
        public WindowManager() 
        {
            this._currentWindow = new MenuWindow();
        }

        public void ClearWindow() 
        {
            this._currentWindow.Clear();
            this._currentWindow = null;
        }

        public void LoadWindow(BaseWindow window) 
        {
            this.ClearWindow();
            this._currentWindow = window;
        }
        
        public void Draw(SpriteBatch spriteBatch) {
            // delegate the responsibility to the class inside the window manager
            if (this._currentWindow != null) {
                this._currentWindow.Draw(spriteBatch);
            }
        }
        public void Update(GameTime gameTime) {}

    }

    public interface IOnScreenObject 
    {
        void Draw(SpriteBatch spriteBatch);
        void Update(GameTime gameTime);
    }

    public interface Clickable {
        public ICommand GetOnClick();
    }

    public abstract class BaseButton : IOnScreenObject, Clickable
    {
        protected Rectangle _bounds {get; }
        protected ICommand _onClick {get; }

        public BaseButton(Rectangle bounds, ICommand onClick) {
            this._bounds = bounds;
            this._onClick = onClick;
        }

        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void Update(GameTime gameTime);

        public Rectangle getBounds() {
            return this._bounds;
        }

        public ICommand getOnClick() {
            return this._onClick;
        }
        
    }

    public class TextButton : BaseButton 
    {
        private string _text {get; }
        private Color _backgroundColor {get; }
        private Color _textColor {get; }
    }

    public class IconButton : BaseButton
    {
        private Texture2D _icon { get; }

        public IconButton(Rectangle bounds, ICommand onClick, Texture2D icon)
            : base(bounds, onClick)
        {
            this._icon = icon;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_icon, _bounds, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            
        }
    }

    

    public class MenuWindow: BaseWindow 
    {
    }

    public class LevelWindow: BaseWindow 
    {
    }

    public interface ICommand 
    {
        // represents a command that can be executed
        public void Execute();
    }

    public class QuitCommand: ICommand 
    {
        public void Execute() {
            Console.WriteLine("quitting");
            Environment.Exit(0);
        }
    }

    public class LoadWindowCommand: ICommand 
    {
        private WindowManager _windowManager;
        private BaseWindow _targetWindow;
        
        public LoadWindowCommand(WindowManager windowManager, BaseWindow targetWindow) {
            this._windowManager = windowManager;
            this._targetWindow = targetWindow;
        }
            
        public void Execute() {
            this._windowManager.LoadWindow(this._targetWindow);
        }
    }

    public abstract class ResourceLoader 
    {
        // responsible for loading and keeping track of the resources
        protected static readonly string _contentPath = "Content";
        protected static List<string> _modPaths = new List<string>();
        protected List<string> _allowedFileTypes;
        protected string _loaderSubFolder; // the subfolder for this particular resource loader
        protected string _backupResourcePath; // this resource will be returned if we can't find the resource by its id

        public ResourceLoader(List<string> allowedFileTypes, string subfolder, string backupResourcePath) 
        {
            this._allowedFileTypes = allowedFileTypes;
            this._loaderSubFolder = subfolder;
            this._backupResourcePath = backupResourcePath;
        }

        public static void AddModPath(string modPath) 
        {
            // add a mod path to the list of the mod paths if it exists
            if (Directory.Exists(modPath)) {
                _modPaths.Add(modPath);
            }
            else {
                Console.WriteLine("Could not load the mod path: " + modPath);
            }
        }

        public bool DoesSubfolderExist(string mainPath) 
        {
            return Directory.Exists(Path.Combine(mainPath, _loaderSubFolder));
        }

        public void LoadContent() 
        {
            // combine all the paths into one list and add subfolders to it
            List<string> pathList = new List<string>(); // an array of all the paths we want to load from
            
            // add the content path to the list
            if (Directory.Exists(_contentPath)) {
                pathList.Add(Path.Combine(_contentPath, _loaderSubFolder));
            }

            // add all the mod paths to the list            
            foreach (string modPath in _modPaths) {
                if (DoesSubfolderExist(modPath)) {
                    pathList.Add(Path.Combine(modPath, _loaderSubFolder));
                }
            }

            // load all the files in every directory
            foreach (string path in pathList) {
                string[] files = Directory.GetFiles(path);
                foreach (string file in files) {
                    // check if the file type matches what we expect
                    string extension = Path.GetExtension(file);
                    if (this._allowedFileTypes.Contains(extension))
                    {
                        LoadFile(file);
                    }
                    else {
                        Console.WriteLine("Warning: unexpected file type in " + _loaderSubFolder + ": " + file + extension);
                    }
                }
            }
        }
        // load all the files in every directory

        public abstract void LoadFile(string filePath);
        // load a specific file from the directory

        public virtual object GetResource(string key) {
            return null;
        }
    }

    public class ImageLoader : ResourceLoader 
    {
        protected GraphicsDevice _graphics;
        protected Dictionary<string, Texture2D> _textures;

        public ImageLoader(GraphicsDevice graphics, string backupResourcePath) : base(new List<string> {".png", ".jpg", ".jpeg"}, "gfx", backupResourcePath) {
            this._graphics = graphics;
        }

        public override void LoadFile(string filePath) {
            using (FileStream stream = new FileStream(filePath, FileMode.Open))
            {
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                _textures[fileName] = Texture2D.FromStream(this._graphics, stream);
            }
        }

        public Texture2D GetResource(string key) {
            if (_textures.ContainsKey(key)) {
                return _textures[key];
            }
            // TODO: add failsafe in case we couldn't find the backup resource (this may cause a crash otherwise)
            else {
                return GetResource(_backupResourcePath);
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
            this._content = content;
            this._fonts = new Dictionary<string, SpriteFont>();
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