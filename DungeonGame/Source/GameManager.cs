using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using DungeonGame.Source;

namespace DungeonGame
{
    enum GameState 
    {
        Intro,
        Instructions,
        Game,
        End
    }

    class GameManager
    {
        public GameManager() 
        {
            m_State = GameState.Intro;
            m_Player = new Player(PLAYER_HEALTH, PLAYER_DAMAGE);

            m_MapWidth = 5;
			m_MapHeight = 5;
			GenerateNewMap();
        }

        public void Display() 
        {
            switch (m_State)
            {
                case GameState.Intro:
                {
                    DisplayIntro();
                    break;
                }
                case GameState.Instructions:
                {
                    DisplayInstructions();
                    break;
                }
                case GameState.Game:
                {
                    for (int y = 0; y < m_MapHeight; y++)
                    {
                        for (int x = 0; x < m_MapWidth; x++)
                            m_Map[y, x].Draw();

                            ScreenBuffer.WriteLine("");
                    }

                    m_Player.DisplayStats();

                    m_Player.Move(m_Map, this);

                    break;
                }
                case GameState.End:
                    break;
                default:
                    break;
            }
        }

        public void GenerateNewMap()
        {
            Random rng = new Random();
            //m_MapWidth = rng.Next(4, 10);
            //m_MapHeight = rng.Next(4, 10);
            m_Map = new Tile[m_MapHeight, m_MapWidth];
            int exitPosition = rng.Next(m_MapWidth - 3, m_MapWidth - 1);

            for (int y = 0; y < m_MapHeight; y++)
            {
                for (int x = 0; x < m_MapWidth; x++)
                {
                    m_Map[y, x] = new Tile(new Vector2(x, y), '░', ConsoleColor.Green);

                    int chanceToGenerateEntity = rng.Next(1, 10);
                    if (chanceToGenerateEntity == 2)
                        m_Map[y, x] = new Enemy(new Vector2(x, y));
                    if (chanceToGenerateEntity == 7)
                        m_Map[y, x] = new Chest(new Vector2(x, y));
                }
            }

            m_Map[0, exitPosition] = new Door(new Vector2(0, exitPosition));

            // TODO: maybe make x position random?
            m_Map[m_MapHeight - 1, exitPosition] = m_Player;
            m_Player.Position = new Vector2(exitPosition, m_MapHeight - 1);
        }

        
        private void DisplayIntro() 
        {
            ScreenBuffer.WriteLine("Dungeon Game");
            ScreenBuffer.WriteLine("Press \"s\" to start");

            if (Input.IsKeyPressed(ConsoleKey.S)) 
            {
                m_State = GameState.Instructions;
				ScreenBuffer.SetDimensions(120, 15);
			}
        }

        private void DisplayInstructions()
        {
            ScreenBuffer.WriteLine("Commands:");
            ScreenBuffer.WriteLine("w/a/s/d for moving");
            ScreenBuffer.WriteLine("You can move on \"+\" tiles.");
			ScreenBuffer.WriteLine("The \"$\" tiles have treasures which increase your health or damage. To get them you need to have a pry bar or a shoe");
            ScreenBuffer.WriteLine("The \"x\" tiles have enemies which give you gold and other items. The more gold you get, the more food you can eat.");
            ScreenBuffer.WriteLine("The player is the \"@\" tile.");
            ScreenBuffer.WriteLine("You can go to the next dungeon by going to the \"^\" tile.");
            ScreenBuffer.WriteLine("You can approach enemies from top or bottom from one tile distance or from right from two tile distance!");
            ScreenBuffer.WriteLine("You can interact with treasure chests from top, bottom or right from one tile distance or left from two tiles distance!");
            ScreenBuffer.WriteLine("You can interact with the exit only from one tile distance from top!");
            ScreenBuffer.WriteLine("As time goes, enemies get stronger. Good Luck!");
            ScreenBuffer.WriteLine("Press space to continue");

            if (Input.IsKeyPressed(ConsoleKey.Spacebar)) 
            {
                m_State = GameState.Game;
                ScreenBuffer.SetDimensions(40, 20);
            }
        }

        public GameState State => m_State;

        private GameState m_State;
        private Tile[,] m_Map;
        private Player m_Player;

        private int m_MapWidth, m_MapHeight;

        private const int PLAYER_HEALTH = 10;
        private const int PLAYER_DAMAGE = 20;
    }
}
