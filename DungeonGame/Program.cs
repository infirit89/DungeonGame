using System;
using System.Collections.Generic;
using System.Numerics;
using System.Diagnostics;

namespace DungeonGame
{
    class Program
    {
        static void Main(string[] args)
        {
            
            GameManager gameManager = new GameManager();
            /*int levels = 0;
            int pry = 1;
            int helmet = 0;
            int chest = 0;
            int pants = 0;
            int gold = 20;*/

            /*while (true)
            {
                
            }
            while (true)
            {
                    Console.WriteLine();
                    Console.WriteLine("You have not eaten since yesterday. You are hungry");
                    string[,] board = Generate();
                    List<int> stats = Display(board, health, damage, levels, pry, helmet, chest, pants, gold);
                    health = stats[0];
                    damage = stats[1];
                    levels = stats[2];
                    pry = stats[3];
                    helmet = stats[4];
                    chest = stats[5];
                    pants = stats[6];
                    gold = stats[7];
            }*/
            while (true)
            {
                gameManager.Display();
            }
        }

        static List<int> Display(string[,] board, int health, int damage, int levels, int pry, int helmet, int chest, int pants, int gold)
        {
            List<int> stats = new List<int>();
            int uBound0 = board.GetUpperBound(0);
            int uBound1 = board.GetUpperBound(1);
            int y = 0;
            int x = 0;
            string[] pryItems = { "dirty shoe", "golden shoe", "iron pry bar", "golden pry bar" };
            string[] helmetItems = { "dirty hat", "fedora", "iron helmet", "golden helmet" };
            string[] chestItems = { "dirty shirt", "elegant shirt", "iron chestplate", "golden chestplate" };
            string[] pantsItems = { "dirty pants", "elegant pants", "iron pants", "golden pants" };
            string pryy = pryItems[pry];
            string helmett = pryItems[helmet];
            string chestt = pryItems[chest];
            string pantss = pryItems[pants];
            int prychance = 0;
            if (pry==0)
            {
                prychance = 11;
            }
            else if (pry==1)
            {
                prychance = 7;
            }
            else if (pry==2)
            {
                prychance = 4;
            }
            else if (pry == 3)
            {
                prychance = 2;
            }
            while (true)
            {
                if (health<=0)
                {
                     health = 100;
                     damage = 20;
                     levels = 0;
                     pry = 0;
                     helmet = 0;
                     chest = 0;
                     pants = 0;
                    if (gold<100)
                    {
                        Console.WriteLine($"You are dead! You won {gold} gold! Just enough for a slice of bread!");
                    }
                    else if (gold>100&&gold<200)
                    {
                        Console.WriteLine($"You are dead! You won {gold} gold! Just enough for a burger!");
                    }
                    else if (gold > 200 && gold < 300)
                    {
                        Console.WriteLine($"You are dead! You won {gold} gold! Just enough for a pizza!");
                    }
                    else if (gold > 300 && gold < 400)
                    {
                        Console.WriteLine($"You are dead! You won {gold} gold! Just enough for a sushi!");
                    }

                    stats.Add(health);
                    stats.Add(damage);
                    stats.Add(levels);
                    stats.Add(pry);
                    stats.Add(helmet);
                    stats.Add(chest);
                    stats.Add(pants);
                    stats.Add(gold);
                    return stats;
                }
                Random rng = new Random();
                for (int i = 0; i <= uBound0; i++)
                {
                    for (int j = 0; j <= uBound1; j++)
                    {
                        if (board[i, j] == "@")
                        {
                            y = i;
                            x = j;
                        }
                    }
                }
                Console.Clear();
                for (int i = 0; i <= uBound0; i++)
                {
                    for (int j = 0; j <= uBound1; j++)
                    {
                        Console.Write(board[i, j]);
                    }
                    Console.WriteLine();
                }
                Console.WriteLine("hp - " + health);
                Console.WriteLine("dmg - " + damage);
                Console.WriteLine("levels completed - " + levels);
                Console.WriteLine("gear:");
                Console.WriteLine("pry - " + pryy);
                Console.WriteLine("helmet - " + helmett);
                Console.WriteLine("chest - " + chestt);
                Console.WriteLine("pants - " + pantss);
                Console.WriteLine("gold - " + gold);
                Console.WriteLine("You have not eaten since yesterday. You are hungry");
                ConsoleKeyInfo inp = Console.ReadKey();
                if (inp.Key == ConsoleKey.W)
                {
                    Console.WriteLine();
                    for (int i = 0; i <= uBound0; i++)
                    {
                        for (int j = 0; j <= uBound1; j++)
                        {
                            if (board[i, j] == "$")
                            {
                                int rand = rng.Next(1, 3);
                                if (i == y - 1 && j == x)
                                {
                                    Console.WriteLine("You try to pry the treasure chest!");
                                    if (pry==0||pry==1||pry==2)
                                    {
                                        Console.WriteLine($"Enter a num between {1} and {prychance-1}");
                                        int n = int.Parse(Console.ReadLine());
                                        int r = rng.Next(1, prychance);
                                        if (r==n||pry==3)
                                        {
                                            if (rand == 1)
                                            {
                                                health += 30;
                                            }
                                            else
                                            {
                                                damage += 10;
                                            }
                                            Console.WriteLine("Success! You pried the chest open! Press any key to continue!");
                                            Console.ReadLine();
                                        }

                                    }
                                    else
                                    {
                                        Console.WriteLine("The chest didn't budge! Good luck next time!");
                                    }
                                }
                            }

                            if (board[i, j] == "^")
                            {
                                if (i == y - 1 && j == x)
                                {
                                    stats.Add(health);
                                    stats.Add(damage);
                                    levels += 1;
                                    stats.Add(levels);
                                    stats.Add(pry);
                                    stats.Add(helmet);
                                    stats.Add(chest);
                                    stats.Add(pants);
                                    stats.Add(gold);
                                    return stats;
                                }
                            }

                            if (board[i, j] == "x")
                            {
                                if (i == y - 1 && j == x)
                                {
                                    int armorvalue = helmet + chest + pants;
                                    armorvalue = armorvalue * 3;
                                    int dmgtaken = rng.Next(10, 40);
                                    dmgtaken = dmgtaken - armorvalue;
                                    health = health - dmgtaken;
                                    Console.WriteLine($"Ouch! You took {dmgtaken} from battle!");
                                    int item1 = rng.Next(1, 5);
                                    int gol = rng.Next(10, 40);
                                    gold += gol;
                                    if (item1 == 1)
                                    {
                                        int value = rng.Next(helmet, 4);
                                        helmet = value;
                                        Console.WriteLine($"Congrats! You got {helmetItems[helmet]}!");
                                        helmett = helmetItems[helmet];
                                    }
                                    if (item1 == 2)
                                    {
                                        int value = rng.Next(chest, 4);
                                        chest = value;
                                        Console.WriteLine($"Congrats! You got {chestItems[chest]}!");
                                        chestt = chestItems[chest];
                                    }
                                    if (item1 == 3)
                                    {
                                        int value = rng.Next(pants, 4);
                                        pants = value;
                                        Console.WriteLine($"Congrats! You got {pantsItems[pants]}!");
                                        pantss = pantsItems[pants];
                                    }
                                    if (item1 == 4)
                                    {
                                        int value = rng.Next(pants, 4);
                                        pry = value;
                                        Console.WriteLine($"Congrats! You got {pryItems[pry]}!");
                                        pryy = pryItems[pry];
                                    }
                                    Console.WriteLine("Press any key to continue!");
                                    Console.ReadLine();
                                }
                            }
                            
                            if (y==i&&x==j)
                            {

                                board[y-1,x] = "@";
                                board[y, x] = "+";
                            }
                        }
                        Console.WriteLine();
                    }
                }

                else if (inp.Key == ConsoleKey.S)
                {
                        Console.WriteLine();
                        for (int i = 0; i <= uBound0; i++)
                        {
                            for (int j = 0; j <= uBound1; j++)
                            {
                            if (board[i, j] == "$")
                            {
                                int rand = rng.Next(1, 3);
                                if (i == y + 1 && j == x)
                                {
                                    Console.WriteLine("You try to pry the treasure chest!");
                                    if (pry == 0 || pry == 1 || pry == 2)
                                    {
                                        Console.WriteLine($"Enter a num between {1} and {prychance - 1}");
                                        int n = int.Parse(Console.ReadLine());
                                        int r = rng.Next(1, prychance);
                                        if (r == n || pry == 3)
                                        {
                                            if (rand == 1)
                                            {
                                                health += 30;
                                            }
                                            else
                                            {
                                                damage += 10;
                                            }
                                            Console.WriteLine("Success! You pried the chest open! Press any key to continue!");
                                            Console.ReadLine();
                                        }

                                    }
                                    else
                                    {
                                        Console.WriteLine("The chest didn't budge! Good luck next time!");
                                    }
                                }
                            }
                            if (board[i, j] == "x")
                            {
                                if (i == y + 1 && j == x)
                                {
                                    int armorvalue = helmet + chest + pants;
                                    armorvalue = armorvalue * 3;
                                    int dmgtaken = rng.Next(10, 40);
                                    health = dmgtaken - armorvalue;
                                    Console.WriteLine($"Ouch! You took {dmgtaken} from battle!");
                                    int item1 = rng.Next(1, 5);
                                    int gol = rng.Next(10, 40);
                                    gold += gol;
                                    if (item1 == 1)
                                    {
                                        int value = rng.Next(helmet, 4);
                                        helmet = value;
                                        Console.WriteLine($"Congrats! You got {helmetItems[helmet]}!");
                                        helmett = helmetItems[helmet];
                                    }
                                    if (item1 == 2)
                                    {
                                        int value = rng.Next(chest, 4);
                                        chest = value;
                                        Console.WriteLine($"Congrats! You got {chestItems[chest]}!");
                                        chestt = chestItems[chest];
                                    }
                                    if (item1 == 3)
                                    {
                                        int value = rng.Next(pants, 4);
                                        pants = value;
                                        Console.WriteLine($"Congrats! You got {pantsItems[pants]}!");
                                        pantss = pantsItems[pants];
                                    }
                                    if (item1 == 4)
                                    {
                                        int value = rng.Next(pants, 4);
                                        pry = value;
                                        Console.WriteLine($"Congrats! You got {pryItems[pry]}!");
                                        pryy = pryItems[pry];
                                    }
                                    Console.WriteLine("Press any key to continue!");
                                    Console.ReadLine();
                                }
                            }
                            if (y == i && x == j)
                                {
                                    board[y + 1, x] = "@";
                                    board[y, x] = "+";
                            }
                            }
                            Console.WriteLine();
                        }
                }

                else if (inp.Key == ConsoleKey.D)
                {
                    Console.WriteLine();
                    for (int i = 0; i <= uBound0; i++)
                    {
                        for (int j = 0; j <= uBound1; j++)
                        {
                            if (board[i, j] == "$")
                            {
                                int rand = rng.Next(1, 3);
                                if (i == y && j == x+2)
                                {
                                    Console.WriteLine("You try to pry the treasure chest!");
                                    if (pry == 0 || pry == 1 || pry == 2)
                                    {
                                        Console.WriteLine($"Enter a num between {1} and {prychance - 1}");
                                        int n = int.Parse(Console.ReadLine());
                                        int r = rng.Next(1, prychance);
                                        if (r == n || pry == 3)
                                        {
                                            if (rand == 1)
                                            {
                                                health += 30;
                                            }
                                            else
                                            {
                                                damage += 10;
                                            }
                                            Console.WriteLine("Success! You pried the chest open! Press any key to continue!");
                                            Console.ReadLine();
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("The chest didn't budge! Good luck next time!");
                                    }
                                }
                            }
                            if (board[i, j] == "x")
                            {
                                if (i == y && j == x+2)
                                {
                                    int armorvalue = helmet + chest + pants;
                                    armorvalue = armorvalue * 3;
                                    int dmgtaken = rng.Next(10, 40);
                                    health = dmgtaken - armorvalue;
                                    Console.WriteLine($"Ouch! You took {dmgtaken} from battle!");
                                    int item1 = rng.Next(1, 5);
                                    int gol = rng.Next(10, 40);
                                    gold += gol;
                                    if (item1 == 1)
                                    {
                                        int value = rng.Next(helmet, 4);
                                        helmet = value;
                                        Console.WriteLine($"Congrats! You got {helmetItems[helmet]}!");
                                        helmett = helmetItems[helmet];
                                    }
                                    if (item1 == 2)
                                    {
                                        int value = rng.Next(chest, 4);
                                        chest = value;
                                        Console.WriteLine($"Congrats! You got {chestItems[chest]}!");
                                        chestt = chestItems[chest];
                                    }
                                    if (item1 == 3)
                                    {
                                        int value = rng.Next(pants, 4);
                                        pants = value;
                                        Console.WriteLine($"Congrats! You got {pantsItems[pants]}!");
                                        pantss = pantsItems[pants];
                                    }
                                    if (item1 == 4)
                                    {
                                        int value = rng.Next(pants, 4);
                                        pry = value;
                                        Console.WriteLine($"Congrats! You got {pryItems[pry]}!");
                                        pryy = pryItems[pry];
                                    }
                                    Console.WriteLine("Press any key to continue!");
                                    Console.ReadLine();
                                }
                            }

                            if (y == i && x == j)
                            {
                                board[y,x+1] = "@";
                                board[y, x] = "+";
                            }
                        }
                        Console.WriteLine();
                    }
                }

                else if (inp.Key == ConsoleKey.A)
                {
                    Console.WriteLine();
                    for (int i = 0; i <= uBound0; i++)
                    {
                        for (int j = 0; j <= uBound1; j++)
                        {
                            if (board[i, j] == "$")
                            {
                                int rand = rng.Next(1, 3);
                                if (i == y && j == x-1)
                                {
                                    Console.WriteLine("You try to pry the treasure chest!");
                                    if (pry == 0 || pry == 1 || pry == 2)
                                    {
                                        Console.WriteLine($"Enter a num between {1} and {prychance - 1}");
                                        int n = int.Parse(Console.ReadLine());
                                        int r = rng.Next(1, prychance);
                                        if (r == n || pry == 3)
                                        {
                                            if (rand == 1)
                                            {
                                                health += 30;
                                            }
                                            else
                                            {
                                                damage += 10;
                                            }
                                            Console.WriteLine("Success! You pried the chest open! Press any key to continue!");
                                            Console.ReadLine();
                                        }

                                    }
                                    else
                                    {
                                        Console.WriteLine("The chest didn't budge! Good luck next time!");
                                    }
                                }
                            }
                            if (y == i && x == j)
                            {
                                board[y, x -1] = "@";
                                board[y, x] = "+";
                            }
                        }
                        Console.WriteLine();
                    }
                }
            }
        }
    }
}
