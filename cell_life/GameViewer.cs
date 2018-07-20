using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

/// <summary>
/// Cell Life game, created by LogicLimit @ 2018
/// </summary>
namespace cell_life
{
    /*
     * TODO: Prompt save-changes before deleted / creating new canvas or closing application
     */

    public partial class GameViewer : Form
    {
        // Graphics related related objects
        private Bitmap canvasBmp;
        private Graphics canvasG;

        private Bitmap graphBmp;
        private Graphics graphG;

        /// <summary>
        /// Color themes for drawing game elements
        /// </summary>
        private enum Theme
        {
            Dark1,
            Dark2,
            Light1,
            Light2
        }
        private Theme currentTheme = Theme.Dark1;

        // Color definitions
        private Color col_bgLight = Color.WhiteSmoke;
        private Color col_bgDark = Color.Black;
        private Color col_grid = Color.Black;
        private Color col_field = Color.Yellow;

        // Constants
        private const int CELL_SIZE = 6;
        private const string FILE_FILTER = "CLX Files (*.clx)|*.clx";

        // Current game object
        private Game game;

        public GameViewer()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Setup new game and grid
            game = new Game();
            game.createNewGrid(pbHistoryGraph.Width);

            // Initialize bitmaps and graphics objects
            graphBmp = new Bitmap(pbHistoryGraph.Width, pbHistoryGraph.Height);
            graphG = Graphics.FromImage(graphBmp);

            canvasBmp = new Bitmap(game.W * CELL_SIZE + 1, game.H * CELL_SIZE + 1);
            canvasG = Graphics.FromImage(canvasBmp);

            // Make game´s picture the same size as its bitmap
            pbGame.Size = canvasBmp.Size;

            // Update themes combobox
            cmbTheme.Items.AddRange(Enum.GetNames(typeof(Theme)));
            cmbTheme.SelectedIndex = 0;
        }

        /// <summary>
        /// Draws filled circle on game bitmap
        /// </summary>
        private void drawCircle(Brush backgroundBrush, int x, int y, int radius)
        {
            canvasG.FillEllipse(backgroundBrush, x - radius / 2, y - radius / 2, radius, radius);
        }

        /// <summary>
        /// Draws filled rectangle on game bitmap
        /// </summary>
        private void drawRect(Color color, int centerX, int centerY, int radius)
        {
            //Var. A) Single color 
            canvasG.FillRectangle(Brushes.Yellow, centerX - radius, centerY - radius, 2 * radius, 2 * radius);

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
        private void draw()
        {
            Color backcolor = currentTheme == Theme.Dark1 || currentTheme == Theme.Dark2 ? col_bgDark : col_bgLight;
            Color forecolor = currentTheme == Theme.Dark1 || currentTheme == Theme.Dark2 ? col_bgLight : col_bgDark;
            canvasG.Clear(backcolor);

            Pen p1 = new Pen(col_grid, 1f);
            int offset = 1;
            for (int x = 0; x < game.W; x++)
            {
                for (int y = 0; y < game.H; y++)
                {
                    // Draw grid
                    canvasG.DrawRectangle(p1, x * CELL_SIZE, y * CELL_SIZE, CELL_SIZE, CELL_SIZE);

                    // Draw field
                    if(game.get(x, y))
                    {
                        switch (currentTheme)
                        {
                            case Theme.Dark1:
                                {
                                    drawRect(col_field, (int)((x + 0.5) * CELL_SIZE), (int)((y + 0.5) * CELL_SIZE), CELL_SIZE / 2);
                                    break;
                                }
                            case Theme.Dark2:
                                {
                                    // Outlined circles
                                    drawCircle(Brushes.LightBlue, x * CELL_SIZE + CELL_SIZE / 2, y * CELL_SIZE + CELL_SIZE / 2, CELL_SIZE);
                                    drawCircle(Brushes.Blue, x * CELL_SIZE + CELL_SIZE / 2, y * CELL_SIZE + CELL_SIZE / 2, (int)(CELL_SIZE / 1.3));
                                    drawCircle(Brushes.DarkBlue, x * CELL_SIZE + CELL_SIZE / 2, y * CELL_SIZE + CELL_SIZE / 2, (int)(CELL_SIZE / 1.5));
                                    break;
                                }
                            case Theme.Light1:
                                {
                                    // Unnamed, but interesting
                                    canvasG.FillEllipse(Brushes.LightBlue, x * CELL_SIZE + offset, y * CELL_SIZE + offset, CELL_SIZE - offset, CELL_SIZE - offset);
                                    canvasG.FillEllipse(Brushes.Blue, x * CELL_SIZE + offset, y * CELL_SIZE + offset, CELL_SIZE - offset, CELL_SIZE - offset);
                                    canvasG.FillEllipse(Brushes.DarkBlue, x * CELL_SIZE + offset, y * CELL_SIZE + offset, CELL_SIZE - offset, CELL_SIZE - offset);
                                    break;
                                }
                            case Theme.Light2:
                                {
                                    // Simple filled rectangles
                                    canvasG.FillRectangle(Brushes.Blue, x * CELL_SIZE + offset, y * CELL_SIZE + offset, CELL_SIZE - offset, CELL_SIZE - offset);
                                    break;
                                }
                        }
                    }
                }
            }

            // Show changes
            pbGame.Image = canvasBmp;

            // Draw and show population graph
            graphG.Clear(this.BackColor);

            int max = game.CellCountHistory.Max();
            if(max == 0)
            {
                max = 1;
            }

            int padding = 8;
            double mult = (graphBmp.Height - 2 * padding) / max;

            Pen p2 = new Pen(Color.Red, 1f);
            for (int i = 0; i < game.CellCountHistory.Length; i++)
            {
                int p = game.GraphPosition - i;
                if(p < 0)
                {
                    p += game.CellCountHistory.Length;
                }
                graphG.DrawLine(p2, i, graphBmp.Height - padding, i, graphBmp.Height - (int)(game.CellCountHistory[p] * mult) - padding);

                if(game.CellCountHistory[i] == 0)
                {
                    break;
                }
            }
            //graphG.DrawLine(p1, generations.Length - 1, graph.Height - padding, generations.Length - 1, graph.Height - (int)((double)generations[generations.Length - 1] * mult) - padding);

            Font f = new Font(FontFamily.GenericSansSerif, 12f);

            // Draw y axis with labels
            graphG.DrawLine(p1, 0, padding, 32, padding);
            graphG.DrawLine(p1, 0, graphBmp.Height - padding, 32, graphBmp.Height - padding);
            graphG.DrawString(max.ToString(), f, Brushes.Black, 0, padding);
            graphG.DrawString("0", f, Brushes.Black, 0, graphBmp.Height - padding - 20);

            pbHistoryGraph.Image = graphBmp;

            lblGeneration.Text = string.Format("Generation #{0}\nCells: {1}", game.GenerationID, game.CellCount);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int changes = game.nextStep();

            if (changes == 0)
            {
                timer1.Stop();
                lblGeneration.Text += "\n\nDead population!";
            }

            draw();
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            int x = e.X / CELL_SIZE;
            int y = e.Y / CELL_SIZE;

            if(e.Button == MouseButtons.Right)
            {
                game.generateRandomShape(x, y);
            }
            game.invertField(x, y);

            //textBox1.Text += string.Format("{0} x {1}\n", x, y);//("grid[{0}, {1}] = true;\n", x, y);

            draw();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            game.backupGrid();
            
            btnStart.Enabled = false;
            btnDelete.Enabled = true;
            btnReset.Enabled = true;
            btnPause.Enabled = true;

            timer1.Start();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            btnStart.Enabled = true;
            btnDelete.Enabled = true;
            btnReset.Enabled = false;
            btnPause.Enabled = false;

            game.restoreGrid();
            draw();
        }

        private void deleteCanvas()
        {
            btnStart.Enabled = true;
            btnDelete.Enabled = false;
            btnReset.Enabled = false;
            btnPause.Enabled = false;

            timer1.Stop();
            game.resetGrid();
            draw();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            deleteCanvas();
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            timer1.Enabled = !timer1.Enabled;
            btnPause.Text = timer1.Enabled ? "Pause" : "Resume";
        }

        private void sldSpeed_Scroll(object sender, EventArgs e)
        {
            int speed = (sldSpeed.Maximum - sldSpeed.Value) * 10;
            if(speed < 1)
            {
                speed = 1;
            }

            lblSpeed.Text = string.Format("{0}ms", speed);
            timer1.Interval = speed;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            deleteCanvas();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = FILE_FILTER;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string error = string.Empty;
                if (game.save(sfd.FileName, ref error))
                {
                    MessageBox.Show("File succesfully saved!");
                }
                else
                {
                    MessageBox.Show("Warning", "An error occured while trying to save the game!");
                }
            }
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = FILE_FILTER;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string error = string.Empty;
                    if (game.load(ofd.FileName, ref error))
                    {
                        draw();
                        MessageBox.Show("File succesfully loaded!");
                    }
                    else
                    {
                        MessageBox.Show("Error", "An error occured while trying to load specified file\n\n" + error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occured while trying to load specified file:\n\n" + ex.Message);
                }
            }
        }

        private void exportAsImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Bitamp files (*.bmp)|*.bmp";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                pbGame.Image.Save(sfd.FileName);
                MessageBox.Show("Image saved as\n" + sfd.FileName);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"Created by Logic Limit (LogLim) @ 2018\n\nFor more software visit https://www.loglim.cz/");
        }

        private void cmbTheme_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbTheme.SelectedIndex != -1)
            {
                currentTheme = (Theme)cmbTheme.SelectedIndex;
                draw();
            }
        }
    }
}
