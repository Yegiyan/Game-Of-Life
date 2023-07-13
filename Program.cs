﻿using static Raylib_cs.Raylib;
using Raylib_cs;

namespace GameOfLife
{
    class Program
    {
        enum State { ACTIVE, INACTIVE }

        static void Main(string[] args)
        {
            InitWindow(800, 600, "Conway's Game of Life");
            State state = State.INACTIVE;

            double simulationSpeed = 10.0;
            double accumulator = 0.0;

            int gridWidth = 160;
            int gridHeight = 120;

            Cell[,] grid = new Cell[gridWidth, gridHeight];

            int cellWidth = GetScreenWidth() / gridWidth;
            int cellHeight = GetScreenHeight() / gridHeight;

            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    grid[x, y] = new Cell(x, y, cellWidth, cellHeight, 0, false);
                    grid[x, y].X = x * cellWidth;
                    grid[x, y].Y = y * cellHeight;
                    grid[x, y].Width = cellWidth;
                    grid[x, y].Height = cellHeight;
                }
            }

            while (!WindowShouldClose())
            {
                double timeStep = 1.0 / simulationSpeed;
                double deltaTime = GetFrameTime();
                accumulator += deltaTime;

                if (IsMouseButtonDown(MouseButton.MOUSE_LEFT_BUTTON))
                {
                    int cellX = GetMouseX() / cellWidth;
                    int cellY = GetMouseY() / cellHeight;

                    if (cellX >= 0 && cellX < gridWidth && cellY >= 0 && cellY < gridHeight)
                    {
                        Cell cell = grid[cellX, cellY];
                        cell.IsAlive = true;
                    }
                }

                if (IsMouseButtonDown(MouseButton.MOUSE_RIGHT_BUTTON))
                {
                    int cellX = GetMouseX() / cellWidth;
                    int cellY = GetMouseY() / cellHeight;

                    if (cellX >= 0 && cellX < gridWidth && cellY >= 0 && cellY < gridHeight)
                    {
                        Cell cell = grid[cellX, cellY];
                        cell.IsAlive = false;
                    }
                }

                if (IsKeyPressed(KeyboardKey.KEY_SPACE) && state == State.INACTIVE)
                    state = State.ACTIVE;

                else if (IsKeyPressed(KeyboardKey.KEY_SPACE) && state == State.ACTIVE)
                    state = State.INACTIVE;

                if (IsKeyPressed(KeyboardKey.KEY_RIGHT))
                    simulationSpeed += 5.0;

                if (IsKeyPressed(KeyboardKey.KEY_LEFT) && simulationSpeed > 0)
                    simulationSpeed -= 5.0;

                if (IsKeyPressed(KeyboardKey.KEY_C))
                    for (int x = 0; x < gridWidth; x++)
                    {
                        for (int y = 0; y < gridHeight; y++)
                        {
                            grid[x, y] = new Cell(x, y, cellWidth, cellHeight, 0, false);
                            grid[x, y].X = x * cellWidth;
                            grid[x, y].Y = y * cellHeight;
                            grid[x, y].Width = cellWidth;
                            grid[x, y].Height = cellHeight;
                        }
                    }

                while (accumulator >= timeStep)
                {
                    if (state == State.ACTIVE)
                    {
                        Cell[,] newGrid = new Cell[gridWidth, gridHeight];

                        for (int x = 0; x < gridWidth; x++)
                        {
                            for (int y = 0; y < gridHeight; y++)
                            {
                                newGrid[x, y] = new Cell(x, y, cellWidth, cellHeight, 0, false);
                                newGrid[x, y].X = x * cellWidth;
                                newGrid[x, y].Y = y * cellHeight;
                                newGrid[x, y].Width = cellWidth;
                                newGrid[x, y].Height = cellHeight;

                                int liveNeighbors = 0;

                                for (int dx = -1; dx <= 1; dx++)
                                {
                                    for (int dy = -1; dy <= 1; dy++)
                                    {
                                        if (dx == 0 && dy == 0)
                                            continue;

                                        int neighborX = x + dx;
                                        int neighborY = y + dy;

                                        if (neighborX >= 0 && neighborX < gridWidth && neighborY >= 0 && neighborY < gridHeight)
                                            if (grid[neighborX, neighborY].IsAlive)
                                                liveNeighbors++;
                                    }
                                }

                                if (liveNeighbors < 2 && grid[x, y].IsAlive)
                                    newGrid[x, y].IsAlive = false;

                                else if (liveNeighbors >= 2 && liveNeighbors <= 3 && grid[x, y].IsAlive)
                                    newGrid[x, y].IsAlive = true;

                                else if (liveNeighbors > 3 && grid[x, y].IsAlive)
                                    newGrid[x, y].IsAlive = false;

                                else if (liveNeighbors == 3 && !grid[x, y].IsAlive)
                                    newGrid[x, y].IsAlive = true;
                            }
                        }

                        grid = newGrid;
                    }

                    accumulator -= timeStep;
                }

                BeginDrawing();
                ClearBackground(Color.BLACK);

                for (int x = 0; x < gridWidth; x++)
                {
                    for (int y = 0; y < gridHeight; y++)
                    {
                        if (grid[x, y].IsAlive)
                            DrawRectangle(grid[x, y].X, grid[x, y].Y, grid[x, y].Width, grid[x, y].Height, Color.WHITE);
                        else
                            DrawRectangle(grid[x, y].X, grid[x, y].Y, grid[x, y].Width, grid[x, y].Height, Color.BLACK);
                    }
                }

                DrawText("Simulation:", 15, 15, 20, Color.LIGHTGRAY);
                if (state == State.INACTIVE) DrawText("INACTIVE", 125, 15, 20, Color.RED);
                if (state == State.ACTIVE) DrawText("ACTIVE", 125, 15, 20, Color.GREEN);

                DrawText("Speed:", 15, 40, 20, Color.LIGHTGRAY);
                DrawText("" + simulationSpeed, 90, 40, 20, Color.LIGHTGRAY);

                DrawText("C to Clear", 15, 65, 20, Color.LIGHTGRAY);

                EndDrawing();
            }
            CloseWindow();
        }
    }
}