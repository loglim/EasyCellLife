using System;
using System.Reflection;
using System.Windows.Forms;
using LogLim.EasyCellLife.Properties;

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

        // Private
        private Game _game;
        private GameView _gameView;

        public GameViewer()
        {
            InitializeComponent();

            // Show info
            VersionLabel.Text = string.Format(Strings.Version, Assembly.GetExecutingAssembly().GetName().Version);
            SetStatus(Strings.Welcome);

            // Update theme options
            foreach (var theme in Enum.GetValues(typeof(Theme)))
            {
                ThemeComboBox.Items.Add(theme);
            }
            ThemeComboBox.SelectedIndex = 0;
        }

        private void GameViewer_Load(object sender, EventArgs e)
        {
            // Setup new game and grid
            _game = new Game();
            _game.CreateNewGrid(pbHistoryGraph.Width);

            // Make game´s picture the same size as its bitmap
            //GamePictureBox.Size = _canvasBmp.Size;

            _gameView = new GameView(_game, pbHistoryGraph.Width, pbHistoryGraph.Height);
            Draw();
        }

        private void Draw()
        {
            // Update pictures
            GamePictureBox.Image = _gameView.GetGrid();
            pbHistoryGraph.Image = _gameView.GetGraph();

            // Update info
            lblGeneration.Text = string.Format(Strings.GenerationCells, _game.GenerationId, _game.CellCount);
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            var changes = _game.NextStep();

            if (changes == 0)
            {
                timer1.Stop();
                lblGeneration.Text += $"\n\n{Strings.DeadPopulation}";
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
            SetStatus(Strings.SimulationStarted);
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            _game.RestoreGrid();
            Draw();
            SetControlButtons(true, true, false, false);
            SetStatus(Strings.SimulationReset);
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
            PauseButton.Text = timer1.Enabled ? Strings.Pause : Strings.Resume;
        }

        private void Export()
        {
            var sfd = new SaveFileDialog { Filter = Strings.BitmapFileFilter };
            if (sfd.ShowDialog() != DialogResult.OK) return;

            GamePictureBox.Image.Save(sfd.FileName);
            MessageBox.Show(string.Format(Strings.ImageSavedAs, sfd.FileName));
        }

        private void SaveFile()
        {
            var sfd = new SaveFileDialog { Filter = FileFilter };
            if (sfd.ShowDialog() != DialogResult.OK) return;

            var error = string.Empty;
            if (_game.Save(sfd.FileName, ref error))
            {
                MessageBox.Show(Strings.FileSavedOk);
            }
            else
            {
                MessageBox.Show(Strings.Warning, Strings.SaveError);
            }
        }

        private void LoadFile()
        {
            var ofd = new OpenFileDialog { Filter = FileFilter };
            if (ofd.ShowDialog() != DialogResult.OK) return;

            try
            {
                var error = string.Empty;
                if (_game.Load(ofd.FileName, ref error))
                {
                    Draw();
                    MessageBox.Show(Strings.FileLoadOk);
                }
                else
                {
                    MessageBox.Show(Strings.Error, $"{Strings.LoadFileError}\n\n{error}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{Strings.LoadFileError}\n\n{ex.Message}");
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

        private void SetStatus(string message)
        {
            StatusLabel.Text = message;
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            LoadFile();
        }

        private void SaveAsButton_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void ExportButton_Click(object sender, EventArgs e)
        {
            Export();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AboutButton_Click(object sender, EventArgs e)
        {
            new About().ShowDialog();
        }

        private void ThemeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ThemeComboBox.SelectedIndex == -1 || _gameView == null) return;

            _gameView?.SetTheme((Theme)ThemeComboBox.SelectedIndex);
            Draw();
        }
    }
}
