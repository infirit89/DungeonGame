using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonGame.Source
{
	static class Input
	{
		public static bool IsKeyPressed(ConsoleKey key) 
		{
			if (Console.KeyAvailable) 
				return key == Console.ReadKey(true).Key;

			return false;
		}
	}
}
