namespace AppDevGame.Entities.Weapons
{
    public class Bow : IWeapon
    {
        public void Attack()
        {
            MainApp.Log("Shooting an arrow!");
        }
    }
}
