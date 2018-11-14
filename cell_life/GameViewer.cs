using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LogLim.EasyCellLife
{
    /// <summary>
    /// Cell Life game, created by LogicLimit @ 2018
    /// </summary>
    public partial class GameViewer : Form
    {
        // Constants
        private const int CellSize = 6;
        private const string FileFilter = "CLX Files (*.clx)|*.clx";
        protected internal const int GraphPadding = 8;
        private readonly Color _colBgLight = Color.WhiteSmoke;
        private readonly Color _colBgDark = Color.Black;
        private readonly Color _colGrid = Color.Black;
        private readonly Color _colField = Color.Yellow;

        // Private
        private Theme CurrentTheme = Theme.Dark1;
        private Bitmap _canvasBmp;
        private Bitmap _graphBmp;
        private Graphics _canvasG;
        private Graphics _graphG;

        // Current game object
        private Game _game;

        public GameViewer()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Setup new game and grid
            _game = new Game();
            _game.CreateNewGrid(pbHistoryGraph.Width);

            // Initialize bitmaps and graphics objects
            _graphBmp = new Bitmap(pbHistoryGraph.Width, pbHistoryGraph.Height);
            _graphG = Graphics.FromImage(_graphBmp);

            _canvasBmp = new Bitmap(_game.W * CellSize + 1, _game.H * CellSize + 1);
            _canvasG = Graphics.FromImage(_canvasBmp);

            // Make game´s picture the same size as its bitmap
            GamePictureBox.Size = _canvasBmp.Size;

            Draw();
        }

        /// <summary>
        /// Draws filled circle on game bitmap
        /// </summary>
        private void DrawCircle(Brush backgroundBrush, int x, int y, int radius)
        {
            _canvasG.FillEllipse(backgroundBrush, x - radius / 2, y - radius / 2, radius, radius);
        }

        /// <summary>
        /// Draws filled rectangle on game bitmap
        /// </summary>
        private void DrawRect(int centerX, int centerY, int radius)
        {
            //Var. A) Single color 
            _canvasG.FillRectangle(Brushes.Yellow, centerX - radius, centerY - radius, 2 * radius, 2 * radius);

            //Var. B) gradient version
            /*for (int i = 1; i <= r; i++)
            {
                double a = ((double)(r - i) / r * 255);
                canvasG.DrawRectangle(new Pen(new SolidBrush(Color.FromArgb((int)a, col)), 1), cx - i, cy - i, 2 * i, 2 * i);
            }*/
        }

        /// <summary>
        /// Draws all game elements onto game bitmap
        /// </summary>
        private void Draw()
        {
            var backColor = CurrentTheme == Theme.Dark1 || CurrentTheme == Theme.Dark2 ? _colBgDark : _colBgLight;
            var foreColor = CurrentTheme == Theme.Dark1 || CurrentTheme == Theme.Dark2 ? _colBgLight : _colBgDark;
            _canvasG.Clear(backColor);

            var p1 = new Pen(_colGrid, 1f);
            const int offset = 1;
            for (var x = 0; x < _game.W; x++)
            {
                for (var y = 0; y < _game.H; y++)
                {
                    // Draw grid
                    _canvasG.DrawRectangle(p1, x * CellSize, y * CellSize, CellSize, CellSize);

                    // Draw field
                    if (!_game.Get(x, y)) continue;

                    switch (CurrentTheme)
                    {
                        case Theme.Dark1:
                            {
                                DrawRect((int)((x + 0.5) * CellSize), (int)((y + 0.5) * CellSize), CellSize / 2);
                                break;
                            }
                        case Theme.Dark2:
                            {
                                // Outlined circles
                                DrawCircle(Brushes.LightBlue, x * CellSize + CellSize / 2, y * CellSize + CellSize / 2, CellSize);
                                DrawCircle(Brushes.Blue, x * CellSize + CellSize / 2, y * CellSize + CellSize / 2, (int)(CellSize / 1.3));
                                DrawCircle(Brushes.DarkBlue, x * CellSize + CellSize / 2, y * CellSize + CellSize / 2, (int)(CellSize / 1.5));
                                break;
                            }
                        case Theme.Light1:
                            {
                                // Unnamed, but interesting
                                _canvasG.FillEllipse(Brushes.LightBlue, x * CellSize + offset, y * CellSize + offset, CellSize - offset, CellSize - offset);
                                _canvasG.FillEllipse(Brushes.Blue, x * CellSize + offset, y * CellSize + offset, CellSize - offset, CellSize - offset);
                                _canvasG.FillEllipse(Brushes.DarkBlue, x * CellSize + offset, y * CellSize + offset, CellSize - offset, CellSize - offset);
                                break;
                            }
                        case Theme.Light2:
                            {
                                // Simple filled rectangles
                                _canvasG.FillRectangle(Brushes.Blue, x * CellSize + offset, y * CellSize + offset, CellSize - offset, CellSize - offset);
                                break;
                            }
                    }
                }
            }

            // Show changes
            GamePictureBox.Image = _canvasBmp;

            // Draw and show population graph
            _graphG.Clear(BackColor);

            var max = _game.CellCountHistory.Max();
            if (max == 0)
            {
                max = 1;
            }

            var multiplier = (double)(_graphBmp.Height - 2 * GraphPadding) / max;
            var p2 = new Pen(Color.Red, 1f);
            for (var i = 0; i < _game.CellCountHistory.Length; i++)
            {
                var p = _game.GraphPosition - i;
                if (p < 0)
                {
                    p += _game.CellCountHistory.Length;
                }
                _graphG.DrawLine(p2, i, _graphBmp.Height - GraphPadding, i, _graphBmp.Height - (int)(_game.CellCountHistory[p] * multiplier) - GraphPadding);

                if (_game.CellCountHistory[i] == 0)
                {
                    break;
                }
            }
            //graphG.DrawLine(p1, generations.Length - 1, graph.Height - padding, generations.Length - 1, graph.Height - (int)((double)generations[generations.Length - 1] * mult) - padding);

            var f = new Font(FontFamily.GenericSansSerif, 12f);

            // Draw y axis with labels
            _graphG.DrawLine(p1, 0, GraphPadding, 32, GraphPadding);
            _graphG.DrawLine(p1, 0, _graphBmp.Height - GraphPadding, 32, _graphBmp.Height - GraphPadding);
            _graphG.DrawString(max.ToString(), f, Brushes.Black, 0, GraphPadding);
            _graphG.DrawString("0", f, Brushes.Black, 0, _graphBmp.Height - GraphPadding - 20);

            pbHistoryGraph.Image = _graphBmp;

            lblGeneration.Text = $"Generation #{_game.GenerationId}\nCells: {_game.CellCount}";
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            var changes = _game.NextStep();

            if (changes == 0)
            {
                timer1.Stop();
                lblGeneration.Text += "\n\nDead population!";
            }

            Draw();
        }

        private void PictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            var x = e.X / CellSize;
            var y = e.Y / CellSize;

            if (e.Button == MouseButtons.Right)
            {
                _game.GenerateRandomShape(x, y);
            }
            _game.InvertField(x, y);

            //textBox1.Text += string.Format("{0} x {1}\n", x, y);//("grid[{0}, {1}] = true;\n", x, y);

            Draw();
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            _game.BackupGrid();
            timer1.Start();
            SetControlButtons(false, true, true, true);
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            _game.RestoreGrid();
            Draw();
            SetControlButtons(true, true, false, false);
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            _game.ResetGrid();
            Draw();
            SetControlButtons(true, false, false, false);
        }

        private void SetControlButtons(bool start, bool delete, bool reset, bool pause)
        {
            StartButton.Enabled = start;
            DeleteButton.Enabled = delete;
            ResetButton.Enabled = reset;
            PauseButton.Enabled = pause;
        }

        private void BtnPause_Click(object sender, EventArgs e)
        {
            timer1.Enabled = !timer1.Enabled;
            PauseButton.Text = timer1.Enabled ? "Pause" : "Resume";
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog { Filter = "Bitamp files (*.bmp)|*.bmp" };
            if (sfd.ShowDialog() != DialogResult.OK) return;

            GamePictureBox.Image.Save(sfd.FileName);
            MessageBox.Show($"Image saved as\n{sfd.FileName}");
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog { Filter = FileFilter };
            if (sfd.ShowDialog() != DialogResult.OK) return;

            var error = string.Empty;
            if (_game.Save(sfd.FileName, ref error))
            {
                MessageBox.Show("File succesfully saved!");
            }
            else
            {
                MessageBox.Show("Warning", "An error occured while trying to save the game!");
            }
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog { Filter = FileFilter };
            if (ofd.ShowDialog() != DialogResult.OK) return;

            try
            {
                var error = string.Empty;
                if (_game.Load(ofd.FileName, ref error))
                {
                    Draw();
                    MessageBox.Show("File succesfully loaded!");
                }
                else
                {
                    MessageBox.Show("Error", $"An error occured while trying to load specified file\n\n{error}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occured while trying to load specified file:\n\n{ex.Message}");
            }
        }

        private void SpeedSlider_Scroll(object sender, EventArgs e)
        {
            var speed = (SpeedSlider.Maximum - SpeedSlider.Value) * 10;
            if (speed < 1)
            {
                speed = 1;
            }

            SpeedLabel.Text = $"{speed}ms";
            timer1.Interval = speed;
        }
    }
}
