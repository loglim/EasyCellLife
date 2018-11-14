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

        //private int generationsLimit;

        public Game(int w = 64, int h = 64)
        {
            _rnd = new Random();

            W = w;
            H = h;
        }

        public void CreateNewGrid(int graphWidth)
        {
            _gridBc = new bool[W, H];
            CellCountHistory = new int[graphWidth];

            ResetGrid();
        }

        public void ResetGrid()
        {
            _grid = new bool[W, H];
            CellCount = 0;
            GenerationId = 0;

            //clear graph history
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
            /*grid[1, 2] = true;
            grid[1, 3] = true;
            grid[1, 4] = true;
            grid[3, 6] = true;
            grid[4, 6] = true;
            grid[5, 6] = true;
            grid[6, 5] = true;
            grid[6, 4] = true;
            grid[6, 3] = true;
            grid[4, 1] = true;
            grid[3, 1] = true;
            grid[2, 1] = true;*/

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
            var tmp = new bool[W, H];
            for (var x = 0; x < W; x++)
            {
                for (var y = 0; y < H; y++)
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

            //update original grid
            for (var x = 0; x < W; x++)
            {
                for (var y = 0; y < H; y++)
                {
                    Set(x, y, tmp[x, y]);
                }
            }

            //save current cell count
            CellCountHistory[GraphPosition] = CellCount;

            //increase or cycle (start over at 0) graph position
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

        public bool Get(int x, int y)
        {
            /*if (x < 0 || y < 0 || x >= w || y >= h)
            {
                return false;
            }*/
            while (x < 0)
            {
                x += W;
            }
            while (x >= W)
            {
                x -= W;
            }
            while (y < 0)
            {
                y += H;
            }
            while (y >= H)
            {
                y -= H;
            }

            return _grid[x, y];
        }

        private bool Set(int x, int y, bool value)
        {
            /*if (x < 0 || y < 0 || x >= w || y >= h)
            {
                return false;
            }*/
            while (x < 0)
            {
                x += W;
            }
            while (x >= W)
            {
                x -= W;
            }
            while (y < 0)
            {
                y += H;
            }
            while (y >= H)
            {
                y -= H;
            }

            if (_grid[x, y] == value) return true;

            _grid[x, y] = value;
            CellCount += value ? 1 : -1;

            return true;
        }

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

        public void BackupGrid()
        {
            for (var x = 0; x < W; x++)
            {
                for (var y = 0; y < H; y++)
                {
                    _gridBc[x, y] = _grid[x, y];
                }
            }

            GenerationId = 1;
        }

        public void RestoreGrid()
        {
            for (var x = 0; x < W; x++)
            {
                for (var y = 0; y < H; y++)
                {
                    Set(x, y, _gridBc[x, y]);
                }
            }
        }

        public void InvertField(int x, int y)
        {
            Set(x, y, !Get(x, y));
        }

        public bool Load(string filename, ref string error)
        {
            try
            {
                var sr = new StreamReader(filename);
                W = int.Parse(sr.ReadLine());
                H = int.Parse(sr.ReadLine());
                _grid = new bool[W, H];
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split('#');
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

        public bool Save(string fileName, ref string error)
        {
            try
            {
                var sw = new StreamWriter(fileName);

                // Write grid size
                sw.WriteLine("" + W);
                sw.WriteLine("" + H);

                // Write current grid
                for (var x = 0; x < W; x++)
                {
                    for (var y = 0; y < H; y++)
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
        public int W { get; private set; }
        public int H { get; private set; }
    }
}
