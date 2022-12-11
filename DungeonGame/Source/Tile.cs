using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace DungeonGame
{
    class Tile
    {
        public Tile(Vector2 position, char icon, ConsoleColor color) 
        {
            Position = position;
            Icon = icon;
            Color = color;
        }

        public Tile(Vector2 position, char icon) 
            : this(position, icon, ConsoleColor.White)
        { }

        public void Draw() 
        {
            ScreenBuffer.Write(Icon, Color);
            //Console.ForegroundColor = Color;
            //Console.Write(Icon);
            //Console.ForegroundColor = ConsoleColor.White;
        }

        public ConsoleColor Color;
        public Vector2 Position;
        public char Icon;
    }
}
