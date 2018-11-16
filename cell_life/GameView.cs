using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace LogLim.EasyCellLife
{
    // TODO: Un-hard-code theme definitions into something more standalone (perhaps xml file)

    internal class GameView : IDisposable
    {
        // Constants
        private const int GraphPadding = 8;
        private const int CellSize = 6;
        private readonly Color _colBg = Color.Gray;
        private readonly Color _colBgLight = Color.WhiteSmoke;
        private readonly Color _colBgDark = Color.Black;
        private readonly Color _colGrid = Color.Black;
        private readonly Color _colField = Color.Yellow;

        // Private
        private readonly Bitmap _canvasBmp;
        private readonly Bitmap _graphBmp;
        private readonly Graphics _canvasG;
        private readonly Graphics _graphG;
        private readonly Game _game;
        private readonly Pen _p1;
        private Theme _theme = Theme.Dark1;

        public GameView(Game game, int w, int h)
        {
            _game = game;

            // Initialize bitmaps and graphics objects
            _graphBmp = new Bitmap(w, h);
            _graphG = Graphics.FromImage(_graphBmp);

            _canvasBmp = new Bitmap(_game.Width * CellSize + 1, _game.Height * CellSize + 1);
            _canvasG = Graphics.FromImage(_canvasBmp);

            _p1 = new Pen(_colGrid, 1f);

            SetQuality(DrawQuality.Low);
        }

        public void SetTheme(Theme theme)
        {
            _theme = theme;
        }

        public void SetQuality(DrawQuality drawQuality)
        {
            switch (drawQuality)
            {
                case DrawQuality.High:
                {

                    _canvasG.SmoothingMode = SmoothingMode.HighQuality;
                    _canvasG.CompositingQuality = CompositingQuality.HighQuality;
                        break;
                }
                case DrawQuality.Mid:
                {
                    _canvasG.SmoothingMode = SmoothingMode.AntiAlias;
                    _canvasG.CompositingQuality = CompositingQuality.AssumeLinear;
                    break;
                }
                default:
                {
                    _canvasG.SmoothingMode = SmoothingMode.HighSpeed;
                    _canvasG.CompositingQuality = CompositingQuality.HighSpeed;
                    break;
                }
            }
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
        private void DrawRect(int centerX, int centerY, int radius, Color color, bool useGradient)
        {
            if (useGradient)
            {
                // Gradient version
                for (var i = 1; i <= radius; i++)
                {
                    var a = ((double) (radius - i) / radius * 255);
                    _canvasG.DrawRectangle(new Pen(new SolidBrush(Color.FromArgb((int) a, color)), 1), centerX - i,
                        centerY - i, 2 * i, 2 * i);
                }
            }
            else
            {
                // Single color 
                _canvasG.FillRectangle(new SolidBrush(color), centerX - radius, centerY - radius, 2 * radius, 2 * radius);
            }
        }

        /// <summary>
        /// Draws all game elements onto game bitmap
        /// </summary>
        private void DrawGrid()
        {
            var backColor = _theme.ToString().Contains("Dark") ? _colBgDark : _colBgLight;
            //var foreColor = _theme == Theme.Dark1 || _theme == Theme.Dark2 ? _colBgLight : _colBgDark;
            _canvasG.Clear(backColor);

            const int offset = 1;
            for (var x = 0; x < _game.Width; x++)
            {
                for (var y = 0; y < _game.Height; y++)
                {
                    // Draw grid
                    if (!_theme.ToString().Contains("Organic"))
                    {
                        _canvasG.DrawRectangle(_p1, x * CellSize, y * CellSize, CellSize, CellSize);
                    }

                    // Draw field
                    if (!_game.Get(x, y)) continue;

                    switch (_theme)
                    {
                        case Theme.Dark1:
                        {
                            DrawRect((int) ((x + 0.5) * CellSize), (int) ((y + 0.5) * CellSize), CellSize / 2,
                                Color.OrangeRed, false);
                            break;
                        }
                        case Theme.Dark2:
                        {
                            DrawRect((int) ((x + 0.5) * CellSize), (int) ((y + 0.5) * CellSize), CellSize / 2,
                                Color.YellowGreen, true);
                            break;
                        }
                        case Theme.Dark3:
                        {
                            // Outlined circles
                            DrawCircle(Brushes.LightBlue, x * CellSize + CellSize / 2, y * CellSize + CellSize / 2,
                                CellSize);
                            DrawCircle(Brushes.Blue, x * CellSize + CellSize / 2, y * CellSize + CellSize / 2,
                                (int) (CellSize / 1.3));
                            DrawCircle(Brushes.DarkBlue, x * CellSize + CellSize / 2, y * CellSize + CellSize / 2,
                                (int) (CellSize / 1.5));
                            break;
                        }
                        case Theme.Light1:
                        {
                            // Unnamed, but interesting
                            _canvasG.FillEllipse(Brushes.LightBlue, x * CellSize + offset, y * CellSize + offset,
                                CellSize - offset, CellSize - offset);
                            _canvasG.FillEllipse(Brushes.Blue, x * CellSize + offset, y * CellSize + offset,
                                CellSize - offset, CellSize - offset);
                            _canvasG.FillEllipse(Brushes.DarkBlue, x * CellSize + offset, y * CellSize + offset,
                                CellSize - offset, CellSize - offset);
                            break;
                        }
                        case Theme.Light2:
                        {
                            // Simple filled rectangles
                            _canvasG.FillRectangle(Brushes.Blue, x * CellSize + offset, y * CellSize + offset,
                                CellSize - offset, CellSize - offset);
                            break;
                        }
                        case Theme.Light3:
                        {
                            DrawRect((int) ((x + 0.5) * CellSize), (int) ((y + 0.5) * CellSize), CellSize / 2,
                                Color.Red, false);
                            break;
                        }
                        case Theme.Organic1:
                        {
                            DrawCircle(Brushes.DarkGreen, (int) ((x + 0.5) * CellSize),
                                (int) ((y + 0.5) * CellSize),
                                (int) (CellSize * 1.75));
                            break;
                        }
                        case Theme.Organic2:
                        {
                            DrawCircle(Brushes.DarkOrange, (int)((x + 0.5) * CellSize),
                                (int)((y + 0.5) * CellSize),
                                (int)(CellSize * 1.75));
                            break;
                        }
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
        }

        private void DrawGraph()
        {
            // Draw and show population graph
            _graphG.Clear(_colBg);

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
            _graphG.DrawLine(_p1, 0, GraphPadding, 32, GraphPadding);
            _graphG.DrawLine(_p1, 0, _graphBmp.Height - GraphPadding, 32, _graphBmp.Height - GraphPadding);
            _graphG.DrawString(max.ToString(), f, Brushes.Black, 0, GraphPadding);
            _graphG.DrawString("0", f, Brushes.Black, 0, _graphBmp.Height - GraphPadding - 20);
        }

        public Bitmap GetGrid()
        {
            DrawGrid();
            return _canvasBmp;
        }

        public Bitmap GetGraph()
        {
            DrawGraph();
            return _graphBmp;
        }

        public void Dispose()
        {
            _p1.Dispose();
            _graphBmp.Dispose();
            _canvasBmp.Dispose();
        }
    }
}
