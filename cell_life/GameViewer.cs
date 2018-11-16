using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
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

            // Update combo boxes
            foreach (var theme in Theme.Themes)
            {
                ThemeComboBox.Items.Add(theme.Name);
            }
            foreach (var entry in Enum.GetValues(typeof(DrawQuality)))
            {
                QualityComboBox.Items.Add(entry);
            }
            ThemeComboBox.SelectedIndex = QualityComboBox.SelectedIndex = 0;
        }

        private void GameViewer_Load(object sender, EventArgs e)
        {
            // Setup new game and grid
            _game = new Game();
            _game.CreateNewGrid(pbHistoryGraph.Width);

            // Setup game view
            _gameView = new GameView(_game, pbHistoryGraph.Width, pbHistoryGraph.Height);
            _gameView.SetTheme(Theme.Themes[0]);

            Draw();
        }

        private void Draw()
        {
            // Update pictures
            GamePictureBox.Image = _gameView.GetGrid();
            pbHistoryGraph.Image = _gameView.GetGraph();

            // Make game´s picture the same size as its bitmap
            GamePictureBox.Size = GamePictureBox.Image.Size;

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

            Draw();
        }

        private void SetControlButtons(bool start, bool reset, bool pause)
        {
            StartButton.Enabled = start;
            ResetButton.Enabled = reset;
            PauseButton.Enabled = pause;
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

        private void NewFile()
        {
            if (_game.CellCount > 0)
            {
                // TODO: Check unsaved changes + prompt save
            }

            _game.CreateNewGrid(pbHistoryGraph.Width);
            SetControlButtons(true, false, false);
            Draw();
        }

        private void SpeedSlider_Scroll(object sender, EventArgs e)
        {
            var fps = SpeedSlider.Value;

            var speed = (int)Math.Round(1000f / fps);
            SpeedLabel.Text = $"{fps} fps";
            SetStatus(string.Format(Strings.SpeedSetTo, fps));
            timer1.Interval = speed;
        }

        private void SetStatus(string message)
        {
            StatusLabel.Text = message;
        }

        private void ThemeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ThemeComboBox.SelectedIndex == -1 || _gameView == null) return;

            var theme = Theme.Themes[ThemeComboBox.SelectedIndex];
            _gameView?.SetTheme(theme);
            SetStatus(string.Format(Strings.ThemeSetTo, theme.Name));
            Draw();
        }

        private void QualityComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (QualityComboBox.SelectedIndex == -1 || _gameView == null) return;

            var quality = (DrawQuality) QualityComboBox.SelectedIndex;
            _gameView?.SetQuality(quality);
            SetStatus(string.Format(Strings.QualitySetTo, quality));
            Draw();
        }

        private void StopSimulation()
        {
            timer1.Stop();
            PauseButton.Text = Strings.Pause;
        }

        #region ButtonsClickAction
        private void BtnStart_Click(object sender, EventArgs e)
        {
            _game.BackupGrid();
            timer1.Start();
            SetControlButtons(false, true, true);
            SetStatus(Strings.SimulationStarted);
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            StopSimulation();
            _game.RestoreGrid();
            Draw();
            SetControlButtons(true, false, false);
            SetStatus(Strings.SimulationReset);
        }

        private void BtnPause_Click(object sender, EventArgs e)
        {
            timer1.Enabled = !timer1.Enabled;
            PauseButton.Text = timer1.Enabled ? Strings.Pause : Strings.Resume;
            SetStatus(timer1.Enabled ? Strings.SimulationResumed : Strings.SimulationPaused);
        }

        private void NewButton_Click(object sender, EventArgs e)
        {
            NewFile();
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
        #endregion ButtonsClickAction
    }
}
