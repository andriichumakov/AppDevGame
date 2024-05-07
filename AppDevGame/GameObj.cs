using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using AppDevGame;

namespace AppDevGame
{
    public class GameObj {
        /*
         refers to any item, enemy, npc, or player 
         shows up on the map
         can be interacted with
        */
        private SpriteBatch _spriteBatch;
        private Texture2D _sprite;
        private Hitbox _hitbox;

        private GameObj(SpriteBatch spriteBatch, Texture2D sprite) 
        {
            this._spriteBatch = spriteBatch;
            this._sprite = sprite;
        }

        public GameObj(SpriteBatch spriteBatch, Texture2D sprite, Hitbox hitbox) : this(spriteBatch, sprite)
        {
            this._hitbox = hitbox;
        }

        public virtual void Draw(SpriteBatch spriteBatch) {}

    }
}