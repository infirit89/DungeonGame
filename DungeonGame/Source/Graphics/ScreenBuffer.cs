using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace DungeonGame
{
    class ScreenBuffer
    {
        [StructLayout(LayoutKind.Sequential)]
        struct ScreenBufferElement
        {
            public ConsoleColor Color;
            public char Character;
        }

        private static ScreenBufferElement[,] s_ScreenBufferArray;

        public static int Width, Height;
        public static int CursorTop, CursorLeft;

        public static void Init(int width, int height) 
        {
            s_ScreenBufferArray = new ScreenBufferElement[height, width];
            Width = s_ScreenBufferArray.GetLength(1); Height = s_ScreenBufferArray.GetLength(0);
            CursorTop = 0; CursorLeft = 0;
        }

        public static void Write(char c, ConsoleColor color) 
        {
            switch (c)
            {
                case '\n':
                    SetCursorPosition(0, ++CursorTop);
                    break;
                default:
                    s_ScreenBufferArray[CursorTop, CursorLeft] = new ScreenBufferElement() { Character = c, Color = color };
                    CursorLeft++;
                    break;
            }
        }

        public static void Write(string text, ConsoleColor color = ConsoleColor.White) 
        {
            foreach (char c in text)
                Write(c, color);
        }

        public static void WriteLine(string text, ConsoleColor color = ConsoleColor.White) 
        {
            text += '\n';
            Write(text, color);
        }

        public static void SetCursorPosition(int left, int top) 
        {
            CursorLeft = left;
            CursorTop = top;
        }

        public static void Clear()
        {
            CursorLeft = 0;
            CursorTop = 0;

            s_ScreenBufferArray = new ScreenBufferElement[Height, Width];

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                    s_ScreenBufferArray[y, x] = new ScreenBufferElement() { Character = ' ', Color = ConsoleColor.Black };
            }
        }

        public static ConsoleKey ReadKey(bool intercept = true) => Console.ReadKey(intercept).Key;

        public static void Flush() 
        {
            for (int y = 0; y < Console.WindowHeight; y++)
            {
                for (int x = 0; x < Console.WindowWidth; x++)
                {
                    Console.SetCursorPosition(x, y);
                    Console.ForegroundColor = s_ScreenBufferArray[y, x].Color;
                    Console.Write(s_ScreenBufferArray[y, x].Character);
                }
            }

            Console.ResetColor();
        }
    }
}
