using System;

namespace AppDevGame.Entities.Weapons
{
    public class WeaponFactory
    {
        public IWeapon CreateWeapon(string weaponType)
        {
            switch (weaponType.ToLower())
            {
                case "sword":
                    return new Sword();
                case "bow":
                    return new Bow();
                default:
                    throw new ArgumentException("Invalid weapon type");
            }
        }
    }
}
