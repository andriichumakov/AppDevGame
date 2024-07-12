using System;
using Microsoft.Xna.Framework;

namespace AppDevGame
{
    public class LiveEntity : Entity
    {
        // A subclass shared by Players, Enemies, and Passives 
        // Adds health, death, vision, attack functionalities

        protected int _maxHealth;
        protected int _currentHealth;

        protected TimeSpan _attackCooldown = TimeSpan.FromSeconds(1);
        protected TimeSpan _lastAttackTime;

        protected int _damage;
        protected float _speed;
        protected Projectile _attackProjectile = null;

        public LiveEntity(LevelWindow level, Vector2 pos, EntityType type, int maxHealth, int damage = 1, float speed = 50f) 
            : base(level, pos, type)
        {
            _maxHealth = maxHealth;
            _currentHealth = maxHealth;
            _damage = damage;
        }

        public int GetMaxHealth()
        {
            return _maxHealth;
        }

        public int GetCurrentHealth()
        {
            return _currentHealth;
        }

        public void RestoreHealth()
        {
            _currentHealth = _maxHealth;
        }

        public virtual void Heal(int hearts)
        {
            _currentHealth += hearts;
            if (_currentHealth > _maxHealth)
            {
                RestoreHealth();
            }
        }

        public bool CanAttack(GameTime gameTime)
        {
            return gameTime.TotalGameTime - _lastAttackTime >= _attackCooldown;
        }

        public virtual void Attack(GameTime gameTime, Entity other)
        {
            if (CanAttack(gameTime))
            {
                _lastAttackTime = gameTime.TotalGameTime;
                // Implement attack logic here
            }
        }

        public virtual void TakeDamage(int damage)
        {
            _currentHealth -= damage;
        }

        public bool IsDead()
        {
            return _currentHealth <= 0;
        }

        public override void Update(GameTime gameTime)
        {
            if (IsDead())
            {
                SelfDestruct();
            }
            base.Update(gameTime);
        }
    }
}
