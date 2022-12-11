using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace DungeonGame
{
    class Player : Entity
    {
        public Player(int health, int damage, Vector2 position) 
            : base(health, damage, m_Icon, position)
        {
            m_Rng = new Random();
        }

        public Player(int health, int damage)
            : this(health, damage, new Vector2(0, 0)) 
        { }

        private void InteractWithChest() 
        {
            const int prychance = 7;
            Console.WriteLine("You try to pry the treasure chest!");
            Console.WriteLine($"Enter a num between {1} and {prychance - 1}");
            int n = int.Parse(Console.ReadLine());
                
            int r = m_Rng.Next(1, prychance);
            if (r == n || m_Pry == 3)
            {
                int chanceToGetHealth = m_Rng.Next(0, 2);
                if (chanceToGetHealth == 0)
                    Health += 30;
                else
                    Damage += 10;

                Console.WriteLine("Success! You pried the chest open! Press any key to continue!");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("The chest didn't budge! Good luck next time!");
            }
        }

        private void InteractWithDoor(GameManager gameManager) 
        {
            gameManager.GenerateNewMap();
        }

        private void InteractWithEnemy() 
        {
            int armorvalue = m_Helmet + m_Chestplate + m_Pants;
            armorvalue = armorvalue * 3;
            int dmgtaken = m_Rng.Next(10, 40);
            dmgtaken = dmgtaken - armorvalue;
            Health = Health - dmgtaken;
            Console.WriteLine($"Ouch! You took {dmgtaken} from battle!");
            int item1 = m_Rng.Next(1, 4);
            int gol = m_Rng.Next(10, 40);
            m_Gold += gol;
            if (item1 == 1)
            {
                int value = m_Rng.Next(m_Helmet, 4);
                m_Helmet = value;
                //Console.WriteLine($"Congrats! You got {helmetItems[helmet]}!");
               // helmett = helmetItems[helmet];
            }
            if (item1 == 2)
            {
                int value = m_Rng.Next(m_Chestplate, 4);
                m_Chestplate = value;
                //Console.WriteLine($"Congrats! You got {chestItems[chest]}!");
                //chestt = chestItems[chest];
            }
            if (item1 == 3)
            {
                int value = m_Rng.Next(m_Pants, 4);
                m_Pants = value;
                //Console.WriteLine($"Congrats! You got {pantsItems[pants]}!");
                //pantss = pantsItems[pants];
            }
            Console.WriteLine("Press any key to continue!");
            Console.ReadLine();
        }

        public void Move(Tile[,] map, GameManager gameManager)
        {
            int mapHeight = map.GetUpperBound(0);
            int mapWidth = map.GetUpperBound(1);
            int playerX = (int)Position.X, playerY = (int)Position.Y;

            int directionX = 0, directionY = 0;

            ConsoleKey key = Console.ReadKey().Key;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write("");

            switch (key)
            {
                case ConsoleKey.W:
                {
                    directionY--;
                    break;
                }
                case ConsoleKey.S:
                {
                    directionY++;
                    break;
                }
                case ConsoleKey.A:
                {
                    directionX--;
                    break;
                }
                case ConsoleKey.D:
                {
                    directionX++;
                    break;
                }
            }

            if (playerY + directionY < 0 || playerX + directionX < 0 || playerY + directionY > mapHeight || playerX + directionX > mapWidth) return;

            Tile tileToInteractWith = map[playerY + directionY, playerX + directionX];
            if (tileToInteractWith is Chest)
                InteractWithChest();
            else if (tileToInteractWith is Door)
            {
                InteractWithDoor(gameManager);
                return;
            }
            else if (tileToInteractWith is Enemy)
                InteractWithEnemy();

            map[playerY, playerX] = new Tile(new Vector2(playerX, playerY), '░', ConsoleColor.Green);
            map[playerY + directionY, playerX + directionX] = this;

            Position.X += directionX;
            Position.Y += directionY;
        }

        public void DisplayStats() 
        {
            Console.WriteLine();
            Console.WriteLine("hp - " + Health);
            Console.WriteLine("dmg - " + Damage);
            //Console.WriteLine("levels completed - " + levels);
            //Console.WriteLine("gear:");
            //Console.WriteLine("pry - " + pryy);
            //Console.WriteLine("helmet - " + helmett);
            //Console.WriteLine("chest - " + chestt);
            //Console.WriteLine("pants - " + pantss);
            //Console.WriteLine("gold - " + gold);
        }

        private const char m_Icon = '█';
        private int m_Pry = 0;
        private Random m_Rng;
        private int m_Helmet = 0, m_Chestplate = 0, m_Pants = 0;
        private int m_Gold = 20;
    }
}
