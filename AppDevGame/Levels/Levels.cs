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
            MainApp.Log("Setting up Level1...");
            base.Setup();
            // Clear existing entities before setting up the level again
            _entities.Clear();
            _entitiesToAdd.Clear();
            _entitiesToRemove.Clear();
            _currentHeartCount = 0;
            _totalLanterns = 0;
            _litLanterns = 0;
            _player = null;  // Reset the player
            _bossSpawned = false; // Reset boss spawned state

            // Initialize entities and background specific to Level1

            // Example of adding entities to the level
            Texture2D frogTexture = MainApp.GetInstance()._imageLoader.GetResource("frog_full_jumping");
            Texture2D ghostTexture = MainApp.GetInstance()._imageLoader.GetResource("Ghost");
            Texture2D playerRunTexture = MainApp.GetInstance()._imageLoader.GetResource("Gunner_Blue_Run");
            Texture2D playerIdleTexture = MainApp.GetInstance()._imageLoader.GetResource("Gunner_Blue_Idle");
            Texture2D activePortalTexture = MainApp.GetInstance()._imageLoader.GetResource("PortalActive");
            Texture2D inactivePortalTexture = MainApp.GetInstance()._imageLoader.GetResource("PortalInactive");
            Texture2D heartTexture = MainApp.GetInstance()._imageLoader.GetResource("Heart");
            Texture2D coinTexture = MainApp.GetInstance()._imageLoader.GetResource("coin1");
            Texture2D litLanternTexture = MainApp.GetInstance()._imageLoader.GetResource("LanternLit");
            Texture2D unlitLanternTexture = MainApp.GetInstance()._imageLoader.GetResource("LanternUnlit");
            Texture2D bossTexture = MainApp.GetInstance()._imageLoader.GetResource("PlantBeast");
            _font = MainApp.GetInstance()._fontLoader.GetResource("Default");

            if (frogTexture != null)
            {
                // Add entities at specified positions
                AddEntity(new Frog(this, frogTexture, new Vector2(850, 1100), maxHealth: 100, damage: 1, scale: 2.0f));
                AddEntity(new Frog(this, frogTexture, new Vector2(700, 1300), maxHealth: 100, damage: 1, scale: 2.0f));
                AddEntity(new Frog(this, frogTexture, new Vector2(1200, 450), maxHealth: 100, damage: 1, scale: 2.0f));
            }

            if (ghostTexture != null)
            {
                // Add ghosts at specified positions
                AddEntity(new Ghost(this, ghostTexture, new Vector2(600, 800), maxHealth: 100, damage: 1, scale: 2.0f));
                AddEntity(new Ghost(this, ghostTexture, new Vector2(800, 500), maxHealth: 100, damage: 1, scale: 2.0f));
                AddEntity(new Ghost(this, ghostTexture, new Vector2(1000, 450), maxHealth: 100, damage: 1, scale: 2.0f));
            }

            if (playerRunTexture != null && playerIdleTexture != null)
            {
                SetPlayer(new Player(this, playerRunTexture, playerIdleTexture, new Vector2(700, 500), MainApp.GetInstance().BackgroundTexture, 200f, 100));
            }

            if (activePortalTexture != null && inactivePortalTexture != null)
            {
                // Add inactive portal at the desired position (update the coordinates accordingly)
                _portal = new Portal(this, activePortalTexture, inactivePortalTexture, new Vector2(1385, 85), scale: 5.0f, isActive: false);
                AddEntity(_portal);
            }

            if (heartTexture != null)
            {
                // Add hearts at specified positions
                AddHeart(heartTexture, new Vector2(890, 500));
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
                _boss = new PlantBeast(this, bossTexture, Vector2.Zero, maxHealth: 300, damage: 10, speed: 100f, scale: 3.0f);
            }

            // hidden obstacles

            // Maze walls
            AddEntity(new OnMapBounds(this, new Rectangle(645, 370, 10, 356))); // wall
            AddEntity(new OnMapBounds(this, new Rectangle(648, 360, 480, 10))); // wall
            AddEntity(new OnMapBounds(this, new Rectangle(1130, 165, 10, 200))); // wall
            AddEntity(new OnMapBounds(this, new Rectangle(1130, 165, 100, 10)));
            AddEntity(new OnMapBounds(this, new Rectangle(1230, 65, 10, 100)));
            AddEntity(new OnMapBounds(this, new Rectangle(1230, 65, 390, 10)));
            AddEntity(new OnMapBounds(this, new Rectangle(1595, 165, 100, 10)));
            AddEntity(new OnMapBounds(this, new Rectangle(1595, 65, 10, 100)));
            AddEntity(new OnMapBounds(this, new Rectangle(1695, 165, 10, 200)));
            AddEntity(new OnMapBounds(this, new Rectangle(1705, 365, 200, 10)));
            AddEntity(new OnMapBounds(this, new Rectangle(1905, 365, 10, 300)));
            AddEntity(new OnMapBounds(this, new Rectangle(1905, 665, 210, 10)));
            AddEntity(new OnMapBounds(this, new Rectangle(2100, 665, 10, 300)));

            // room NW
            AddEntity(new OnMapBounds(this, new Rectangle(1810, 955, 390, 10)));
            AddEntity(new OnMapBounds(this, new Rectangle(1810, 955, 10, 500)));
            AddEntity(new OnMapBounds(this, new Rectangle(2190, 955, 10, 600)));

            AddEntity(new OnMapBounds(this, new Rectangle(2190, 1455, 400, 10)));
            AddEntity(new OnMapBounds(this, new Rectangle(2590, 1455, 10, 700)));

            AddEntity(new OnMapBounds(this, new Rectangle(2190, 1555, 200, 10)));
            AddEntity(new OnMapBounds(this, new Rectangle(2390, 1555, 10, 480)));

            AddEntity(new OnMapBounds(this, new Rectangle(2200, 2155, 390, 10)));
            AddEntity(new OnMapBounds(this, new Rectangle(2200, 1755, 10, 900)));
            AddEntity(new OnMapBounds(this, new Rectangle(2000, 1955, 10, 700)));
            AddEntity(new OnMapBounds(this, new Rectangle(2200, 2535, 200, 10)));
            AddEntity(new OnMapBounds(this, new Rectangle(2400, 2535, 10, 500)));
            AddEntity(new OnMapBounds(this, new Rectangle(2000, 2655, 200, 10)));

            AddEntity(new OnMapBounds(this, new Rectangle(2000, 3035, 400, 10)));
            AddEntity(new OnMapBounds(this, new Rectangle(2000, 3035, 200, 10)));
            
            AddEntity(new OnMapBounds(this, new Rectangle(1250, 3200, 750, 10)));
            AddEntity(new OnMapBounds(this, new Rectangle(1250, 3020, 10, 200)));
            AddEntity(new OnMapBounds(this, new Rectangle(2000, 3020, 10, 200)));
            AddEntity(new OnMapBounds(this, new Rectangle(50, 3000, 1200, 10)));
            AddEntity(new OnMapBounds(this, new Rectangle(70, 2500, 10, 500)));

            AddEntity(new OnMapBounds(this, new Rectangle(1810, 1445, 200, 190)));

            // Stone wall around the staircase
            AddEntity(new OnMapBounds(this, new Rectangle(1505, 365, 200, 190)));
            AddEntity(new OnMapBounds(this, new Rectangle(1130, 365, 200, 190)));

            AddEntity(new OnMapBounds(this, new Rectangle(335, 750, 550, 10)));
            AddEntity(new OnMapBounds(this, new Rectangle(335, 750, 10, 300)));
            AddEntity(new OnMapBounds(this, new Rectangle(920, 750, 10, 300)));

            AddEntity(new OnMapBounds(this, new Rectangle(335, 1060, 200, 200)));
            AddEntity(new OnMapBounds(this, new Rectangle(735, 1060, 200, 200)));

            AddEntity(new OnMapBounds(this, new Rectangle(250, 950, 100, 10)));
            AddEntity(new OnMapBounds(this, new Rectangle(250, 950, 10, 1700)));

            AddEntity(new OnMapBounds(this, new Rectangle(80, 2545, 100, 10)));
            AddEntity(new OnMapBounds(this, new Rectangle(370, 2620, 10, 150)));
            AddEntity(new OnMapBounds(this, new Rectangle(540, 2620, 10, 150)));
            AddEntity(new OnMapBounds(this, new Rectangle(550, 2620, 1180, 10)));
            AddEntity(new OnMapBounds(this, new Rectangle(1710, 2440, 10, 150)));
            AddEntity(new OnMapBounds(this, new Rectangle(630, 2440, 1100, 10)));
            AddEntity(new OnMapBounds(this, new Rectangle(1840, 1755, 10, 500)));
            AddEntity(new OnMapBounds(this, new Rectangle(1850, 1730, 180, 10)));
            AddEntity(new OnMapBounds(this, new Rectangle(1600, 970, 10, 500)));
            AddEntity(new OnMapBounds(this, new Rectangle(1330, 1470, 270, 10)));
            AddEntity(new OnMapBounds(this, new Rectangle(810, 1450, 10, 270)));
            AddEntity(new OnMapBounds(this, new Rectangle(810, 1450, 270, 10)));
            AddEntity(new OnMapBounds(this, new Rectangle(1100, 1450, 10, 300)));
            AddEntity(new OnMapBounds(this, new Rectangle(1300, 1450, 10, 300)));
            AddEntity(new OnMapBounds(this, new Rectangle(425, 1650, 10, 600)));
            AddEntity(new OnMapBounds(this, new Rectangle(1100, 1450, 10, 300)));
            AddEntity(new OnMapBounds(this, new Rectangle(425, 1650, 350, 10)));
            AddEntity(new OnMapBounds(this, new Rectangle(640, 2226, 10, 200)));
            AddEntity(new OnMapBounds(this, new Rectangle(1100, 1450, 10, 300)));
            AddEntity(new OnMapBounds(this, new Rectangle(440, 2220, 150, 10)));
            AddEntity(new OnMapBounds(this, new Rectangle(260, 2640, 70, 10)));

            AddEntity(new OnMapBounds(this, new Rectangle(930, 975, 650, 10)));


            MainApp.GetInstance().PlayLevelMusic(); // Add this line
            MainApp.Log("Level1 setup complete.");
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
            MainApp.Log($"Coin added at position: {position}");
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

            // Draw the boss health bar if the boss is present
            if (_boss != null && !_boss.IsDead())
            {
                string bossHealthText = $"Boss: {_boss.Name} Health: {_boss.CurrentHealth} / {_boss.MaxHealth}";
                spriteBatch.DrawString(_font, bossHealthText, new Vector2(10, 100), Color.Green);
            }
        }
    }
}
