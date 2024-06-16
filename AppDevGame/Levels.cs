using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace AppDevGame
{
    public class Level1 : TopDownLevel
    {
        private Portal _portal;
        private int _maxHearts = 3;
        private int _currentHeartCount = 0;
        private int _totalLanterns = 0;
        private int _litLanterns = 0;
        private bool _bossSpawned = false;
        private SpriteFont _font;
        private BossEnemy _boss;

        public Level1(int frameWidth, int frameHeight, int actualWidth, int actualHeight, Texture2D background = null)
            : base(frameWidth, frameHeight, actualWidth, actualHeight, background)
        {
        }

        public override void Setup()
        {
            base.Setup();
            // Initialize entities and background specific to Level1

            // Example of adding entities to the level
            Texture2D entityTexture = MainApp.GetInstance()._imageLoader.GetResource("Frog");
            Texture2D entityTexture2 = MainApp.GetInstance()._imageLoader.GetResource("Ghost");
            Texture2D playerTexture = MainApp.GetInstance()._imageLoader.GetResource("character");
            Texture2D activePortalTexture = MainApp.GetInstance()._imageLoader.GetResource("PortalActive");
            Texture2D inactivePortalTexture = MainApp.GetInstance()._imageLoader.GetResource("PortalInactive");
            Texture2D heartTexture = MainApp.GetInstance()._imageLoader.GetResource("Heart");
            Texture2D coinTexture = MainApp.GetInstance()._imageLoader.GetResource("Coin");
            Texture2D litLanternTexture = MainApp.GetInstance()._imageLoader.GetResource("LanternLit");
            Texture2D unlitLanternTexture = MainApp.GetInstance()._imageLoader.GetResource("LanternUnlit");
            Texture2D bossTexture = MainApp.GetInstance()._imageLoader.GetResource("plantbeast"); // Use the plantbeast texture
            _font = MainApp.GetInstance()._fontLoader.GetResource("Default");

            if (entityTexture != null)
            {
                // Add entities at specified positions
                AddEntity(new MeleeAttackEnemy(this, entityTexture, new Vector2(850, 1100), maxHealth: 100, damage: 1, scale: 2.0f));
                AddEntity(new MeleeAttackEnemy(this, entityTexture, new Vector2(700, 1300), maxHealth: 100, damage: 1, scale: 2.0f));
                AddEntity(new MeleeAttackEnemy(this, entityTexture, new Vector2(1200, 450), maxHealth: 100, damage: 1, scale: 2.0f));
                AddEntity(new MeleeAttackEnemy(this, entityTexture2, new Vector2(600, 800), maxHealth: 100, damage: 1, scale: 2.0f, selfDestruct: true));
                AddEntity(new MeleeAttackEnemy(this, entityTexture2, new Vector2(800, 500), maxHealth: 100, damage: 1, scale: 2.0f, selfDestruct: true));
                AddEntity(new MeleeAttackEnemy(this, entityTexture2, new Vector2(1000, 450), maxHealth: 100, damage: 1, scale: 2.0f, selfDestruct: true));
            }

            if (playerTexture != null)
            {
                // Ensure the player is set only once
                if (Player == null)
                {
                    MainApp.Log("Adding player to the level.");
                    SetPlayer(new Player(this, playerTexture, new Vector2(700, 500)));
                }
                else
                {
                    MainApp.Log("Player is already set in the level.");
                }
            }

            if (activePortalTexture != null && inactivePortalTexture != null)
            {
                // Add inactive portal at the desired position (update the coordinates accordingly)
                _portal = new Portal(this, activePortalTexture, inactivePortalTexture, new Vector2(1225, 65), scale: 5.0f, isActive: false);
                AddEntity(_portal);
            }

            if (heartTexture != null)
            {
                // Add hearts at specified positions
                AddHeart(heartTexture, new Vector2(1200, 500));
                AddHeart(heartTexture, new Vector2(900, 400));
                AddHeart(heartTexture, new Vector2(700, 600));
            }

            if (coinTexture != null)
            {
                // Add coins at specified positions
                AddCoin(coinTexture, new Vector2(1200, 200));
                AddCoin(coinTexture, new Vector2(700, 500));
                AddCoin(coinTexture, new Vector2(800, 700));
            }

            if (litLanternTexture != null && unlitLanternTexture != null)
            {
                // Add lanterns at specified positions
                AddLantern(unlitLanternTexture, litLanternTexture, new Vector2(800, 550));
                AddLantern(unlitLanternTexture, litLanternTexture, new Vector2(1350, 600));
                AddLantern(unlitLanternTexture, litLanternTexture, new Vector2(1500, 800));
            }

            if (bossTexture != null)
            {
                // Initialize the boss but do not add it yet
                _boss = new PlantBeast(this, bossTexture, Vector2.Zero, maxHealth: 300, damage: 5, speed: 120f, scale: 2.5f);
            }
        }

        private void AddHeart(Texture2D heartTexture, Vector2 position)
        {
            if (_currentHeartCount < _maxHearts)
            {
                AddEntity(new Heart(this, heartTexture, position, scale: 2.0f)); // Ensure the scale value is provided
                _currentHeartCount++;
            }
        }

        private void AddCoin(Texture2D coinTexture, Vector2 position)
        {
            AddEntity(new Coin(this, coinTexture, position, scale: 2.0f)); // Ensure the scale value is provided
        }

        private void AddLantern(Texture2D unlitTexture, Texture2D litTexture, Vector2 position)
        {
            AddEntity(new Lantern(this, litTexture, unlitTexture, position));
            _totalLanterns++;
        }

        public void IncrementLitLanterns()
        {
            _litLanterns++;
            CheckPortalActivation();
        }

        public void DecrementHeartCount()
        {
            if (_currentHeartCount > 0)
            {
                _currentHeartCount--;
            }
        }

        private void CheckPortalActivation()
        {
            if (_entities.OfType<Enemy>().All(e => e.IsDead()) && _litLanterns >= _totalLanterns)
            {
                if (!_bossSpawned && _boss != null)
                {
                    // Spawn the boss near the portal
                    _boss.SpawnNearPortal(_portal.Position);
                    AddEntity(_boss);
                    _bossSpawned = true;
                }
                else if (_boss != null && _boss.IsDead())
                {
                    // Activate the portal if the boss is dead
                    _portal.IsActive = true;
                }
            }
        }

        private int GetRemainingEnemies()
        {
            return _entities.OfType<Enemy>().Count(e => !e.IsDead());
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Check if the portal should be activated
            CheckPortalActivation();

            // Update Level1 specific logic here
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            // Draw Level1 specific elements here

            // Draw the UI for remaining lanterns
            string lanternText = $"Lanterns: {_litLanterns} / {_totalLanterns}";
            if (_font != null)
            {
                spriteBatch.DrawString(_font, lanternText, new Vector2(10, 40), Color.Yellow);
            }

            // Draw the UI for remaining enemies
            string enemyText = $"Enemies: {GetRemainingEnemies()}";
            if (_font != null)
            {
                spriteBatch.DrawString(_font, enemyText, new Vector2(10, 70), Color.Red);
            }

            // Draw boss health bar if the boss is present
            if (_boss != null && !_boss.IsDead())
            {
                string bossHealthText = $"{_boss.Name} Health: {_boss.CurrentHealth} / {_boss.MaxHealth}";
                spriteBatch.DrawString(_font, bossHealthText, new Vector2(10, 100), Color.Green);
            }
        }
    }
}