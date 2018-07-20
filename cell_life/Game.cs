using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cell_life
{
    class Game
    {
        private int w;
        private int h;

        private bool[,] grid;
        private bool[,] gridBc;

        private int generationID;
        private int cellCount;

        private int[] cellCountHistory;
        private int graphPos;
        //private int generationsLimit;

        private Random rnd;

        public Game(int w = 64, int h = 64)
        {
            rnd = new Random();

            this.w = w;
            this.h = h;
        }

        public void createNewGrid(int graph_width)
        {
            gridBc = new bool[w, h];
            cellCountHistory = new int[graph_width];

            resetGrid();
        }

        public void resetGrid()
        {
            grid = new bool[w, h];
            cellCount = 0;
            generationID = 0;

            //clear graph history
            for (int i = 0; i < cellCountHistory.Length; i++)
            {
                cellCountHistory[i] = 0;
            }
            graphPos = 0;

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
        /// <returns>Ammount of changes</returns>
        public int nextStep()
        {
            int changes = 0;

            //create temporary grid
            bool[,] tmp = new bool[w, h];
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    bool alive = grid[x, y];

                    int n = countNeighbours(x, y);
                    if (alive)
                    {
                        if (n < 2 || n > 3)
                        {
                            //die
                            alive = false;
                        }
                    }
                    else //dead
                    {
                        if (n == 3)
                        {
                            //ressurect
                            alive = true;
                        }
                    }

                    tmp[x, y] = alive;
                    if (alive != grid[x, y])
                    {
                        changes++;
                    }
                }
            }

            //update original grid
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    set(x, y, tmp[x, y]);
                }
            }

            //save current cell count
            cellCountHistory[graphPos] = cellCount;

            //increase or cycle (start over at 0) graph position
            graphPos++;
            if (graphPos >= cellCountHistory.Length)
            {
                graphPos = 0;
            }

            if (changes > 0)
            {
                generationID++;
            }

            return changes;
        }

        private int countNeighbours(int x, int y)
        {
            int n = 0;

            if (get(x + 1, y))
                n++;
            if (get(x - 1, y))
                n++;
            if (get(x, y + 1))
                n++;
            if (get(x, y - 1))
                n++;

            if (get(x + 1, y + 1))
                n++;
            if (get(x + 1, y - 1))
                n++;
            if (get(x - 1, y + 1))
                n++;
            if (get(x - 1, y - 1))
                n++;

            return n;
        }

        public bool get(int x, int y)
        {
            /*if (x < 0 || y < 0 || x >= w || y >= h)
            {
                return false;
            }*/
            while (x < 0)
            {
                x += w;
            }
            while (x >= w)
            {
                x -= w;
            }
            while (y < 0)
            {
                y += h;
            }
            while (y >= h)
            {
                y -= h;
            }

            return grid[x, y];
        }

        private bool set(int x, int y, bool value)
        {
            /*if (x < 0 || y < 0 || x >= w || y >= h)
            {
                return false;
            }*/
            while (x < 0)
            {
                x += w;
            }
            while (x >= w)
            {
                x -= w;
            }
            while (y < 0)
            {
                y += h;
            }
            while (y >= h)
            {
                y -= h;
            }

            if (grid[x, y] != value)
            {
                grid[x, y] = value;
                cellCount += value ? 1 : -1;
            }

            return true;
        }

        public void generateRandomShape(int x, int y)
        {
            int[] dx = new int[] { 1, 0, -1, 0, 1, 1, -1, -1 };
            int[] dy = new int[] { 0, 1, 0, -1, 1, -1, 1, -1 };

            for (int i = 0; i < dx.Length; i++)
            {
                int z = 1;
                while (rnd.NextDouble() < 0.5)
                {
                    set(x + z * dx[i], y + z * dy[i], true);
                }
            }
        }

        public void backupGrid()
        {
            //backup grid
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    gridBc[x, y] = grid[x, y];
                }
            }

            generationID = 1;
        }

        public void restoreGrid()
        {
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    set(x, y, gridBc[x, y]);
                }
            }
        }

        public void invertField(int x, int y)
        {
            set(x, y, !get(x, y));
        }

        public bool load(string filename, ref string error)
        {
            try
            {
                StreamReader sr = new StreamReader(filename);
                w = int.Parse(sr.ReadLine());
                h = int.Parse(sr.ReadLine());
                grid = new bool[w, h];
                while (!sr.EndOfStream)
                {
                    string[] line = sr.ReadLine().Split('#');
                    int x = int.Parse(line[0]);
                    int y = int.Parse(line[1]);

                    if (!set(x, y, true))
                    {
                        error = string.Format("Can't assign value to field: {0} x {1}", x, y);
                        return false;
                    }
                }
                sr.Close();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool save(string fileName, ref string error)
        {
            try
            {
                StreamWriter sw = new StreamWriter(fileName);

                //write grid size
                sw.WriteLine("" + w);
                sw.WriteLine("" + h);

                //write current grid
                for (int x = 0; x < w; x++)
                {
                    for (int y = 0; y < h; y++)
                    {
                        if (get(x, y))
                        {
                            sw.WriteLine(string.Format("{0}#{1}", x, y));
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

        public int GenerationID { get { return generationID; } }
        public int CellCount { get { return cellCount; } }
        public int[] CellCountHistory { get { return cellCountHistory; } }
        public int GraphPosition { get { return graphPos; } }

        public int W { get { return w; } }
        public int H { get { return h; } }
    }
}
