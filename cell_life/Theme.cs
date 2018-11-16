using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using LogLim.EasyCellLife.Properties;

namespace LogLim.EasyCellLife
{ 

    public class Theme
    {
        // Constants
        public const string ThemeFile = "themes.xml";

        // Public
        public string Name { get; }
        public Color Background { get; }
        public Color Foreground { get; }
        public Color GridColor { get; }
        public string CellShape { get; }
        public string CellStyle { get; }
        public int CellInset { get; }
        public bool UseGrid { get; }

        public static Theme[] Themes { get; } = ThemeLoader.LoadFromFile(ThemeFile);

        public Theme(string name, Color background, Color foreground, Color gridColor, string cellShape, string cellStyle, int cellInset, bool useGrid)
        {
            Name = name;
            Background = background;
            Foreground = foreground;
            GridColor = gridColor;
            CellShape = cellShape;
            CellStyle = cellStyle;
            CellInset = cellInset;
            UseGrid = useGrid;
        }
    }

    public class ThemeLoader
    {
        public static Theme[] LoadFromFile(string path)
        {
            var foundThemes = new List<Theme>();

            // Create default theme
            var defaultTheme = new Theme("Default", Color.White, Color.Black, Color.Gray, "Square", "Filled", 1, true);
            foundThemes.Add(defaultTheme);
            if (!File.Exists(path)) return foundThemes.ToArray();

            var doc = new XmlDocument();
            try
            {
                doc.Load(path);
            }
            catch (XmlException e)
            {
                MessageBox.Show(string.Format(Strings.ThemesLoadingError, Theme.ThemeFile, e.Message), Strings.Error);
                throw;
            }
            
            var themeNodes = doc.DocumentElement?.SelectNodes("theme");
            if (themeNodes == null) return foundThemes.ToArray();

            foreach (XmlNode node in themeNodes)
            {
                try
                {
                    var name = node.SelectSingleNode("name")?.InnerText;
                    var background = Color.FromName(node.SelectSingleNode("background")?.InnerText ?? throw new InvalidOperationException());
                    var foreground = Color.FromName(node.SelectSingleNode("foreground")?.InnerText ?? throw new InvalidOperationException());
                    var gridColor = Color.FromName(node.SelectSingleNode("gridcolor")?.InnerText ?? throw new InvalidOperationException());
                    var cellShape = node.SelectSingleNode("cellshape")?.InnerText ?? throw new InvalidOperationException();
                    var cellStyle = node.SelectSingleNode("cellstyle")?.InnerText ?? throw new InvalidOperationException();
                    var cellInset = int.Parse(node.SelectSingleNode("cellinset")?.InnerText ?? throw new InvalidOperationException());
                    var showGrid = bool.Parse(node.SelectSingleNode("showgrid")?.InnerText ?? throw new InvalidOperationException());

                    foundThemes.Add(new Theme(name, background, foreground, gridColor, cellShape, cellStyle, cellInset, showGrid));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            return foundThemes.ToArray();
        }
    }
}