namespace AppDevGame.Entities.Weapons
{
    public class Sword : IWeapon
    {
        public void Attack()
        {
            MainApp.Log("Swinging a sword!");
        }
    }
}
