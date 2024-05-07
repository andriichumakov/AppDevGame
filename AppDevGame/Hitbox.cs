using Microsoft.Xna.Framework;

using AppDevGame;

namespace AppDevGame 
{

    public class Hitbox
    {
        private Vector2 coord1; // refers to the coord closest to origin, in this case, the top left one
        private int width;
        private int height;

        public Hitbox(Vector2 coord1, int width, int height) 
        {
            this.coord1 = coord1;
            this.width = width;
            this.height = height;
        }

        public Vector2 getCoord1() {
            return coord1;
        }

        public Vector2 getCoord2() {
            return new Vector2(this.coord1.X + width, this.coord1.Y + height);
        }

        public bool doesXCollideWith(Hitbox other) {
            float thisX1 = this.getCoord1().X;
            float thisX2 = this.getCoord2().X;
            float otherX1 = other.getCoord1().X;
            float otherX2 = other.getCoord2().X;

            return (thisX1 < otherX2 && otherX1 < thisX2) || (otherX1 < thisX2 && thisX1 < otherX2);
        }

        public bool doesYCollideWith(Hitbox other)
        {
            float thisY1 = this.getCoord1().Y;
            float thisY2 = this.getCoord2().Y;
            float otherY1 = other.getCoord1().Y;
            float otherY2 = other.getCoord2().Y;

            return (thisY1 < otherY2 && otherY1 < thisY2) || (otherY1 < thisY2 && thisY1 < otherY2);
        }

        public bool DoesCollideWith(Hitbox other)
        {
            return doesXCollideWith(other) && doesYCollideWith(other);
        }

        public void move(Vector2 moveVector, Vector2 levelEdgeVector)
        {
            // Calculate the new position after applying the moveVector
            Vector2 newPosition = coord1 + moveVector;

            // Clamp X coordinate to valid range
            if (newPosition.X < 0)
            {
                coord1.X = 0;
            }
            else if (newPosition.X + width > levelEdgeVector.X)
            {
                coord1.X = levelEdgeVector.X - width;
            }
            else
            {
                coord1.X = newPosition.X;
            }

            // Clamp Y coordinate to valid range
            if (newPosition.Y < 0)
            {
                coord1.Y = 0;
            }
            else if (newPosition.Y + height > levelEdgeVector.Y)
            {
                coord1.Y = levelEdgeVector.Y - height;
            }
            else
            {
                coord1.Y = newPosition.Y;
            }
        }

    }
}