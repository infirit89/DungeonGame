﻿using DungeonGame.Source;
using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace DungeonGame
{
	struct Stats
	{

        public delegate void OnStatIncreasedCallback();

        public Stats(uint statThreshold = 4, uint thresholdAdder = 2) 
        {
            m_StatThresholdAdder = thresholdAdder;
            Stat = 0;
            m_StatPoints = 0;
            StatTreshold = statThreshold;
            m_OnStatIncreasedCallback = null;
		}

        public static Stats operator ++(Stats other) 
        {   
            other.StatPoints++;
            return other;
        }

        public void SetOnStatIncreasedCallback(OnStatIncreasedCallback callback) => m_OnStatIncreasedCallback = callback;

		public uint Stat;
        public uint StatPoints 
        {
            get => m_StatPoints;
            set 
            {
                m_StatPoints++;
                if (m_StatPoints >= StatTreshold)
                {
                    m_StatPoints = 0;
                    StatTreshold += m_StatThresholdAdder;
                    Stat++;
                    Stat = Math.Clamp(Stat, 0, 8);

                    if(m_OnStatIncreasedCallback != null)
                    m_OnStatIncreasedCallback();
                }
            }
        }

        public uint StatTreshold;

        private uint m_StatPoints;
		private uint m_StatThresholdAdder;
        private OnStatIncreasedCallback m_OnStatIncreasedCallback;
	}

	class Player : Entity
    {
        public Player(int health, int damage, Vector2 position) 
            : base(health, damage, m_Icon, position)
        {
            m_Rng = new Random();
            m_Lockpicking = new Stats(2, 1);

            // NOTE: may change
            m_Defense = new Stats();
			m_Melee = new Stats();

            m_Defense.SetOnStatIncreasedCallback(() => Health += 10);
		}

        public Player(int health, int damage)
            : this(health, damage, new Vector2(0, 0)) 
        { }

        private void InteractWithChest() 
        {
            ScreenBuffer.WriteLine("You try to pry the treasure chest!");
            uint prychance = 10 - m_Lockpicking.Stat;
            ScreenBuffer.WriteLine($"Enter a num between 1 and {prychance}");
            int currentCursorTop = ScreenBuffer.CursorTop;
            ScreenBuffer.Flush();

            int number = 0;
            while (true)
            {
                Console.SetCursorPosition(0, currentCursorTop);

                Console.CursorVisible = true;
                Console.ForegroundColor = ConsoleColor.White;
                bool success = int.TryParse(Console.ReadLine(), out number);
                Console.CursorVisible = false;

                if (number < 1 || number > prychance) 
                    success = false;

                // TODO: color stack
                if (success) break;
                else 
                {
					ScreenBuffer.WriteLine("The input wasn't a valid numer!\nPress any key to continue!", ConsoleColor.Red);
					ScreenBuffer.Flush();
                    ScreenBuffer.ReadKey();
					ScreenBuffer.SetCursorPosition(0, currentCursorTop);
					ScreenBuffer.WriteLine(new string(' ', ScreenBuffer.Width - 1));
					ScreenBuffer.WriteLine(new string(' ', ScreenBuffer.Width - 1));
					ScreenBuffer.CursorTop = currentCursorTop;
					ScreenBuffer.Flush();
				}
            }


            m_Lockpicking++;
            string message = "";
            int r = m_Rng.Next(1, (int)prychance + 1);
            if (r == number || m_Pry == 3)
            {
                int chanceToGetHealth = m_Rng.Next(0, 2);
                if (chanceToGetHealth == 0)
                    Health += 30;
                else
                    Damage += 10;

				message = "Success!\nYou pried the chest open!\nPress any key to continue!";
            }
            else
            {
                message = "The chest didn't budge!!\nGood luck next time!\nPress any key to continue!";
            }

            ScreenBuffer.WriteLine(message);
            ScreenBuffer.Flush();

            ScreenBuffer.ReadKey();
        }

        private void InteractWithDoor(GameManager gameManager) 
        {
            gameManager.GenerateNewMap();
        }

        private void InteractWithEnemy() 
        {
            int armorvalue = m_Helmet + m_Chestplate + m_Pants;
            armorvalue = armorvalue * 3;
            int dmgtaken = m_Rng.Next(2, 5);
            dmgtaken = dmgtaken - armorvalue;
            Health = Health - dmgtaken;
            m_Defense++;
            ScreenBuffer.WriteLine($"Ouch! You took {dmgtaken} from battle!");
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
            ScreenBuffer.WriteLine("Press any key to continue!");
            ScreenBuffer.Flush();
            ScreenBuffer.ReadKey();
        }

        public void Move(Tile[,] map, GameManager gameManager)
        {
            int mapHeight = map.GetUpperBound(0);
            int mapWidth = map.GetUpperBound(1);
            int playerX = (int)Position.X, playerY = (int)Position.Y;

            int directionX = 0, directionY = 0;

            if (Console.KeyAvailable) 
            {
                var key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.W: directionY--; break;
					case ConsoleKey.S: directionY++; break;
					case ConsoleKey.A: directionX--; break;
					case ConsoleKey.D: directionX++; break;

				}
            }

            if (playerY + directionY < 0 || playerX + directionX < 0 || playerY + directionY > mapHeight || playerX + directionX > mapWidth)
				return;

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
            ScreenBuffer.WriteLine("");
            ScreenBuffer.WriteLine("hp - " + Health);
            ScreenBuffer.WriteLine("dmg - " + Damage);
        }

        private const char m_Icon = '█';
        private int m_Pry = 0;
        private Random m_Rng;
        private int m_Helmet = 0, m_Chestplate = 0, m_Pants = 0;
        private int m_Gold = 20;
		private Stats m_Lockpicking, m_Defense, m_Melee;

	}
}
