using System;
using System.Collections.Generic;
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

        // Private
        private readonly Bitmap _canvasBmp;
        private readonly Bitmap _graphBmp;
        private readonly Graphics _canvasG;
        private readonly Graphics _graphG;
        private readonly Game _game;
        private Pen _gridPen;
        private Pen _foregroundPen;
        private Brush _foregroundBrush;
        private Brush _graphTextBrush;
        private Theme _theme;

        public GameView(Game game, int w, int h)
        {
            _game = game;

            // Initialize bitmaps and graphics objects
            _graphBmp = new Bitmap(w, h);
            _graphG = Graphics.FromImage(_graphBmp);

            _canvasBmp = new Bitmap(_game.Width * CellSize + 1, _game.Height * CellSize + 1);
            _canvasG = Graphics.FromImage(_canvasBmp);

            SetQuality(DrawQuality.Low);
        }

        public void SetTheme(Theme theme)
        {
            _theme = theme;

            _gridPen = new Pen(_theme.GridColor);
            _foregroundPen = new Pen(_theme.Foreground);
            _foregroundBrush = new SolidBrush(_theme.Foreground);
            _graphTextBrush = new SolidBrush(Utils.MaxContrastColor(_theme.Background, _theme.Foreground));
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
        private void DrawCircle(int x, int y, int offset, bool filled)
        {
            var rectangle = new Rectangle(x * CellSize + offset, y * CellSize + offset, CellSize - 2 * offset, CellSize - 2 * offset);
            if (filled)
            {
                _canvasG.FillEllipse(_foregroundBrush, rectangle);
            }
            else
            {
                _canvasG.DrawEllipse(_foregroundPen, rectangle);
            }
        }

        /// <summary>
        /// Draws filled rectangle on game bitmap
        /// </summary>
        private void DrawRectangle(int x, int y, int offset, bool filled)
        {
            var rectangle = new Rectangle(x * CellSize + offset, y * CellSize + offset, CellSize - 2 * offset, CellSize - 2 * offset);
            if (filled)
            {
                _canvasG.FillRectangle(_foregroundBrush, rectangle);
            }
            else
            {
                _canvasG.DrawRectangle(_foregroundPen, rectangle);
            }
        }

        /// <summary>
        /// Draws all game elements onto game bitmap
        /// </summary>
        private void DrawGrid()
        {
            _canvasG.Clear(_theme.Background);

            var offset = _theme.CellInset;
            for (var x = 0; x < _game.Width; x++)
            {
                for (var y = 0; y < _game.Height; y++)
                {
                    // Draw grid
                    if (_theme.UseGrid)
                    {
                        _canvasG.DrawRectangle(_gridPen, x * CellSize, y * CellSize, CellSize, CellSize);
                    }

                    // Draw field
                    if (!_game.Get(x, y)) continue;

                    if (_theme.CellShape.Equals("Square"))
                    {
                        DrawRectangle(x, y, offset, _theme.CellStyle.Equals("Filled"));
                    }
                    else
                    {
                        DrawCircle(x, y, offset, _theme.CellStyle.Equals("Filled"));
                    }
                }
            }
        }

        private void DrawGraph()
        {
            // Draw and show population graph
            _graphG.Clear(_theme.Background);

            var max = _game.CellCountHistory.Max();
            if (max == 0)
            {
                max = 1;
            }

            var multiplier = (double)(_graphBmp.Height - 2 * GraphPadding) / max;
            var p2 = _foregroundPen; //new Pen(Color.Red, 1f);
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
            _graphG.DrawLine(_foregroundPen, 0, GraphPadding, 32, GraphPadding);
            _graphG.DrawLine(_foregroundPen, 0, _graphBmp.Height - GraphPadding, 32, _graphBmp.Height - GraphPadding);
            _graphG.DrawString(max.ToString(), f, _graphTextBrush, 0, GraphPadding);
            _graphG.DrawString("0", f, _graphTextBrush, 0, _graphBmp.Height - GraphPadding - 20);
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
            _gridPen.Dispose();
            _foregroundPen.Dispose();
            _graphTextBrush.Dispose();
            _foregroundBrush.Dispose();
            _graphBmp.Dispose();
            _canvasBmp.Dispose();
        }
    }
}
