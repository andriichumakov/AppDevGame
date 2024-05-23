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

    /*
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
    */
/* 
    public interface IOnScreenObject 
    {
        void Draw(SpriteBatch spriteBatch);
        void Update(GameTime gameTime);
    }*/
    /* 
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
    */

    

}