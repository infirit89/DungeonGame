using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

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
            m_Map = Generate();
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
                    ConsoleKey key = Console.ReadKey().Key;
                    if (key == ConsoleKey.R) 
                    {
                        Console.Clear();
                        m_Map = Generate();
                    }

                    Console.Clear();
                    for (int y = 0; y < m_MapHeight; y++)
                    {
                        for (int x = 0; x < m_MapWidth; x++)
                            Console.Write(m_Map[y, x].Icon);

                        Console.WriteLine();
                    }

                    MovePlayer();

                    break;
                }
                case GameState.End:
                    break;
                default:
                    break;
            }
        }

        public Tile[,] Generate()
        {
            Random rng = new Random();
            m_MapWidth = rng.Next(4, 10);
            m_MapHeight = rng.Next(4, 10);
            Tile[,] board = new Tile[m_MapHeight, m_MapWidth];
            int exitPosition = rng.Next(m_MapWidth - 3, m_MapWidth - 1);

            for (int y = 0; y < m_MapHeight; y++)
            {
                for (int x = 0; x < m_MapWidth; x++)
                {
                    board[y, x] = new Tile(new Vector2(x, y), '+');

                    int chanceToGenerateEntity = rng.Next(1, 10);
                    if (chanceToGenerateEntity == 2)
                        board[y, x] = new Tile(new Vector2(x, y), 'x');
                    if (chanceToGenerateEntity == 7)
                        board[y, x] = new Tile(new Vector2(x, y), '$');
                }
            }

            board[0, exitPosition] = new Tile(new Vector2(0, exitPosition), '^');

            // TODO: maybe make x position random?
            board[m_MapHeight - 1, exitPosition] = m_Player;
            m_Player.Position = new Vector2(exitPosition, m_MapHeight - 1);

            return board;
        }

        private void MovePlayer() 
        {
            ConsoleKey key = Console.ReadKey().Key;

            switch (key)
            {
                case ConsoleKey.W:
                {
                    int playerX = (int)m_Player.Position.X, playerY = (int)m_Player.Position.Y;
                    if (playerY == 0) return;
                    m_Map[playerY, playerX] = new Tile(new Vector2(playerX, playerY), '+');
                    m_Map[playerY - 1, playerX] = m_Player;
                    m_Player.Position.Y--;
                    break;
                }
                case ConsoleKey.S:
                {
                    int playerX = (int)m_Player.Position.X, playerY = (int)m_Player.Position.Y;
                    if (playerY == m_Player.Position.Y - 1) return;
                    m_Map[playerY, playerX] = new Tile(new Vector2(playerX, playerY), '+');
                    m_Map[playerY + 1, playerX] = m_Player;
                    m_Player.Position.Y++;
                    break;
                }
                case ConsoleKey.A:
                    {
                        int playerX = (int)m_Player.Position.X, playerY = (int)m_Player.Position.Y;
                        if (playerX == 0) return;
                        m_Map[playerY, playerX] = new Tile(new Vector2(playerX, playerY), '+');
                        m_Map[playerY, playerX-1] = m_Player;
                        m_Player.Position.X--;
                        break;
                    }
                case ConsoleKey.D:
                    {
                        int playerX = (int)m_Player.Position.X, playerY = (int)m_Player.Position.Y;
                        if (playerX == m_Player.Position.X - 1) return;
                        m_Map[playerY, playerX] = new Tile(new Vector2(playerX, playerY), '+');
                        m_Map[playerY, playerX+1] = m_Player;
                        m_Player.Position.X++;
                        break;
                    }
            }
        }

        private void DisplayIntro() 
        {
            Console.WriteLine("Dungeon Game");
            Console.WriteLine("Press \"s\" to start");
            ConsoleKeyInfo input = Console.ReadKey();

            if (input.Key == ConsoleKey.S) m_State = GameState.Instructions;
        }

        private void DisplayInstructions()
        {
            Console.Clear();
            Console.WriteLine("Commands:");
            Console.WriteLine("w/a/s/d for moving");
            Console.WriteLine("quit");
            Console.WriteLine("You can move on \"+\" tiles.");
            Console.WriteLine("The \"$\" tiles have treasures which increase your health or damage. To get them you need to have a pry bar or a shoe");
            Console.WriteLine("The \"x\" tiles have enemies which give you gold and other items. The more gold you get, the more food you can eat.");
            Console.WriteLine("The player is the \"@\" tile.");
            Console.WriteLine("You can go to the next dungeon by going to the \"^\" tile.");
            Console.WriteLine("You can approach enemies from top or bottom from one tile distance or from right from two tile distance!");
            Console.WriteLine("You can interact with treasure chests from top, bottom or right from one tile distance or left from two tiles distance!");
            Console.WriteLine("You can interact with the exit only from one tile distance from top!");
            Console.WriteLine("As time goes, enemies get stronger. Good Luck!");
            Console.WriteLine("Press space to continue");

            ConsoleKeyInfo input = Console.ReadKey();
            if (input.Key == ConsoleKey.Spacebar) m_State = GameState.Game;
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
