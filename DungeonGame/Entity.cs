using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace DungeonGame
{
    class Entity : Tile
    {
        public Entity(int health, int damage, char icon, Vector2 position)
            : base(position, icon)
        {
            Health = health;
            Damage = damage;
            Icon = icon;
            Position = position;
        }

        public int Health;
        public int Damage;
    }
}
