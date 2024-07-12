using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AppDevGame
{
    public class Portal : Entity
    {
        private float _scale;
        public bool IsActive { get; set; }
        private Texture2D _inactiveTexture = MainApp.GetInstance()._imageLoader.GetResource("PortalInactive");
        private Texture2D _activeTexture = MainApp.GetInstance()._imageLoader.GetResource("PortalActive");
        private bool _hasPlayedActivationSound;

        public Portal(LevelWindow level, Texture2D activeTexture, Texture2D inactiveTexture, Vector2 position, float scale = 1.5f, bool isActive = false)
            : base(level, position, EntityType.Obstacle)
        {
            _activeTexture = activeTexture;
            _inactiveTexture = inactiveTexture;
            _scale = scale;
            IsActive = isActive;
            _hasPlayedActivationSound = false;
            _hitbox = new Rectangle((int)position.X, (int)position.Y, (int)(inactiveTexture.Width * scale), (int)(inactiveTexture.Height * scale));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (IsActive)
            {
                // Update hitbox position
                _hitbox.Location = _position.ToPoint();

                // Play activation sound only once
                if (!_hasPlayedActivationSound)
                {
                    AudioManager.GetInstance(MainApp.GetInstance().Content).PlaySoundEffect("portal_activate");
                    _hasPlayedActivationSound = true;
                }

                // Check for collision with the player
                if (_hitbox.Intersects(_level.Player.GetHitbox()))
                {
                    // Teleport player to main menu
                    new LoadWindowCommand(WindowManager.GetInstance(), MainApp.GetInstance().MainMenu).Execute();
                }
            }
            else
            {
                
                _hasPlayedActivationSound = false; // Reset the flag if the portal becomes inactive
            }
        }
    }
}
