using System;
using System.IO;

namespace LogLim.EasyCellLife
{
    internal class Game
    {
        // Private
        private readonly Random _rnd;
        private bool[,] _grid;
        private bool[,] _gridBc;

        /// <summary>
        /// Creates a new instance of Game class, with desired width and height
        /// </summary>
        /// <param name="width">Horizontal resolution of grid</param>
        /// <param name="height">Vertical resolution of grid</param>
        public Game(int width = 64, int height = 64)
        {
            _rnd = new Random();

            Width = width;
            Height = height;
        }

        /// <summary>
        /// Creates a new grid with desired history length
        /// </summary>
        /// <param name="historyLength">The width of history graph</param>
        public void CreateNewGrid(int historyLength)
        {
            _gridBc = new bool[Width, Height];
            CellCountHistory = new int[historyLength];

            ResetGrid();
        }

        /// <summary>
        /// Clear current grid
        /// </summary>
        public void ResetGrid()
        {
            _grid = new bool[Width, Height];
            CellCount = 0;
            GenerationId = 0;

            // Clear graph history
            for (var i = 0; i < CellCountHistory.Length; i++)
            {
                CellCountHistory[i] = 0;
            }
            GraphPosition = 0;

            //textBox1.Text = string.Empty;

            //osc #1
            /*grid[3, 3] = true;
            grid[3, 4] = true;
            grid[3, 5] = true;*/

            //official
            _grid[1, 2] = true;
            _grid[1, 3] = true;
            _grid[1, 4] = true;
            _grid[3, 6] = true;
            _grid[4, 6] = true;
            _grid[5, 6] = true;
            _grid[6, 5] = true;
            _grid[6, 4] = true;
            _grid[6, 3] = true;
            _grid[4, 1] = true;
            _grid[3, 1] = true;
            _grid[2, 1] = true;

            //cool
            /*grid[2, 3] = true;
            grid[2, 4] = true;
            grid[3, 5] = true;
            grid[3, 6] = true;
            grid[4, 4] = true;
            grid[4, 3] = true;
            grid[3, 2] = true;
            grid[3, 1] = true;*/
        }

        /// <summary>
        /// Calculates next step of simulation
        /// </summary>
        /// <returns>Amount of changes</returns>
        public int NextStep()
        {
            var changes = 0;

            // Create temporary grid
            var tmp = new bool[Width, Height];
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    var alive = _grid[x, y];

                    var n = CountNeighbors(x, y);
                    if (alive)
                    {
                        if (n < 2 || n > 3)
                        {
                            // Kill
                            alive = false;
                        }
                    }
                    else // Dead
                    {
                        if (n == 3)
                        {
                            // Resurrect
                            alive = true;
                        }
                    }

                    tmp[x, y] = alive;
                    if (alive != _grid[x, y])
                    {
                        changes++;
                    }
                }
            }

            // Update original grid
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    Set(x, y, tmp[x, y]);
                }
            }

            // Save current cell count
            CellCountHistory[GraphPosition] = CellCount;

            // Increase or cycle (start over at 0) graph position
            GraphPosition++;
            if (GraphPosition >= CellCountHistory.Length)
            {
                GraphPosition = 0;
            }

            if (changes > 0)
            {
                GenerationId++;
            }

            return changes;
        }

        private int CountNeighbors(int x, int y)
        {
            var n = 0;

            if (Get(x + 1, y))
                n++;
            if (Get(x - 1, y))
                n++;
            if (Get(x, y + 1))
                n++;
            if (Get(x, y - 1))
                n++;

            if (Get(x + 1, y + 1))
                n++;
            if (Get(x + 1, y - 1))
                n++;
            if (Get(x - 1, y + 1))
                n++;
            if (Get(x - 1, y - 1))
                n++;

            return n;
        }

        /// <summary>
        /// Returns element at target position, overflows once in each direction
        /// </summary>
        /// <param name="x">Target position x</param>
        /// <param name="y">Target position y</param>
        /// <returns></returns>
        public bool Get(int x, int y)
        {
            /*if (x < 0 || y < 0 || x >= width || y >= height)
            {
                return false;
            }*/
            while (x < 0)
            {
                x += Width;
            }
            while (x >= Width)
            {
                x -= Width;
            }
            while (y < 0)
            {
                y += Height;
            }
            while (y >= Height)
            {
                y -= Height;
            }

            return _grid[x, y];
        }

        private bool Set(int x, int y, bool value)
        {
            /*if (x < 0 || y < 0 || x >= width || y >= height)
            {
                return false;
            }*/
            while (x < 0)
            {
                x += Width;
            }
            while (x >= Width)
            {
                x -= Width;
            }
            while (y < 0)
            {
                y += Height;
            }
            while (y >= Height)
            {
                y -= Height;
            }

            if (_grid[x, y] == value) return true;

            _grid[x, y] = value;
            CellCount += value ? 1 : -1;

            return true;
        }

        /// <summary>
        /// Generates a random group of cell originating at target position
        /// </summary>
        /// <param name="x">Target position x</param>
        /// <param name="y">Target position y</param>
        public void GenerateRandomShape(int x, int y)
        {
            int[] dx = { 1, 0, -1, 0, 1, 1, -1, -1 };
            int[] dy = { 0, 1, 0, -1, 1, -1, 1, -1 };

            for (var i = 0; i < dx.Length; i++)
            {
                var z = 1;
                while (_rnd.NextDouble() < 0.5)
                {
                    Set(x + z * dx[i], y + z * dy[i], true);
                }
            }
        }

        /// <summary>
        /// Stores a backup copy of current grid
        /// </summary>
        public void BackupGrid()
        {
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    _gridBc[x, y] = _grid[x, y];
                }
            }

            GenerationId = 1;
        }

        /// <summary>
        /// Overwrites current grid from backed-up copy
        /// </summary>
        public void RestoreGrid()
        {
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    Set(x, y, _gridBc[x, y]);
                }
            }
        }

        /// <summary>
        /// Inverts value in target position field
        /// </summary>
        /// <param name="x">Target position x</param>
        /// <param name="y">Target position y</param>
        public void InvertField(int x, int y)
        {
            Set(x, y, !Get(x, y));
        }

        /// <summary>
        /// Loads grid from provided file, returns encountered error message
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Load(string filename, ref string error)
        {
            try
            {
                var sr = new StreamReader(filename);
                Width = int.Parse(sr.ReadLine() ?? throw new InvalidOperationException());
                Height = int.Parse(sr.ReadLine() ?? throw new InvalidOperationException());
                _grid = new bool[Width, Height];
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine()?.Split('#');
                    if (line == null) continue;

                    var x = int.Parse(line[0]);
                    var y = int.Parse(line[1]);

                    if (Set(x, y, true)) continue;

                    error = $"Can't assign value to field: {x} x {y}";
                    return false;
                }
                sr.Close();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Saves grid to provided file, returns encountered error message
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Save(string fileName, ref string error)
        {
            try
            {
                var sw = new StreamWriter(fileName);

                // Write grid size
                sw.WriteLine("" + Width);
                sw.WriteLine("" + Height);

                // Write current grid
                for (var x = 0; x < Width; x++)
                {
                    for (var y = 0; y < Height; y++)
                    {
                        if (Get(x, y))
                        {
                            sw.WriteLine($"{x}#{y}");
                        }
                    }
                }
                sw.Close();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public int GenerationId { get; private set; }
        public int CellCount { get; private set; }
        public int[] CellCountHistory { get; private set; }
        public int GraphPosition { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
    }
}
