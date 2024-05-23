namespace AppDevGame
{
    public class WindowManager
    {
        private static WindowManager _instance;
        private BaseWindow _window;

        private WindowManager() 
        {
            _window = null;
        }

        public static WindowManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new WindowManager();
            }
            return _instance;
        }

        public void LoadWindow(BaseWindow window) 
        {
            this.Clear();
            _window = window;
            _window.Setup();
        }

        public void Clear() 
        {
            _window?.Clear();
            _window = null;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _window?.Draw(spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            _window?.Update(gameTime);
        }
    }
}
