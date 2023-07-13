using System;

namespace GameOfLife
{
    class Cell
    {
        public int X { get; set; }
        public int Y { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public int LiveNeighbors { get; set; }

        public bool IsAlive { get; set; }

        public Cell(int x, int y, int width, int height, int liveNeighbors, bool isAlive)
        {
            X = x;
            Y = y;

            Width = width;
            Height = height;

            LiveNeighbors = liveNeighbors;

            IsAlive = isAlive;
        }
    }
}