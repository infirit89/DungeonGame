using System.Numerics;

namespace DungeonGame
{
    class Chest : Tile
    {
        public Chest(Vector2 position)
            : base(position, '$')
        {

        }
        
        public int HealthAdder = 30;
        public int DamageAdder = 10;
    }
}
