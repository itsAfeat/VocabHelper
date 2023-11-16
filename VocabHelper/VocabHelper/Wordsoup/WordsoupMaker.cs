using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;

namespace VocabHelper.Wordsoup
{
    enum DIRECTION
    {
        UP = 0,
        DOWN = 1,
        LEFT = 2,
        RIGHT = 3
    };

    public class WordsoupMaker
    {
        private readonly int sizeX, sizeY;
        private readonly CSVFile file;
        private readonly StackPanel localLabelPanel;

        private readonly Dictionary<Point, char> usedPointsDict = new();
        private readonly Dictionary<Point[], (string, string)> foreignWordsDict = new();
        private readonly Dictionary<TextBlock, TextBlock> blockPairs = new();

        private readonly SolidColorBrush pickedColor = new(Color.FromArgb(127, 153, 255, 187));
        private readonly LinearGradientBrush foundColor = new(Color.FromArgb(127, 0, 230, 77), Color.FromArgb(127, 153, 255, 187), 45.0);
        private readonly List<TextBlock> affectedBlocks = new();
        private readonly List<TextBlock> foundWordBlocks = new();
        private readonly Grid soupGrid;

        private TextBlock? StartBlock = null;

        public WordsoupMaker(Grid soupGrid, int sizeX, int sizeY, CSVFile file, StackPanel localLabelPanel)
        {
            this.soupGrid = soupGrid;
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            this.file = file;
            this.localLabelPanel = localLabelPanel;
        }

        public Grid CreateSoup()
        {
            Random r = new();

            for (int i = 0; i < sizeX; i++)
            { soupGrid.ColumnDefinitions.Add(new() { Width = new(1, GridUnitType.Star) }); }
            for (int i = 0; i < sizeY; i++)
            { soupGrid.RowDefinitions.Add(new() { Height = new(1, GridUnitType.Star) }); }

            for (int y = 0; y < sizeY; y++)
            {
                for (int x = 0; x < sizeX; x++)
                {
                    TextBlock tBlock = new()
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        TextAlignment = TextAlignment.Center,
                        Text = $"{Convert.ToChar(r.Next(65, 91))}" //$"{x}/{y}"
                    };

                    TextBlock bBlock = new()
                    {
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch,
                        Background = null
                    };

                    blockPairs.Add(bBlock, tBlock);
                    bBlock.MouseDown += Block_MouseDown;
                    bBlock.MouseUp += Block_MouseUp;

                    Grid.SetColumn(bBlock, x);
                    Grid.SetColumn(tBlock, x);
                    Grid.SetRow(bBlock, y);
                    Grid.SetRow(tBlock, y);

                    soupGrid.Children.Add(tBlock);
                    soupGrid.Children.Add(bBlock);
                }
            }

            return soupGrid;
        }

        private void Block_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (StartBlock != null)
            {
                TextBlock block = (TextBlock)sender;
                if (block == StartBlock)
                {
                    foreach (TextBlock b in affectedBlocks)
                    { b.Background = null; }
                }
                else
                {
                    block.Background = pickedColor;
                    affectedBlocks.Add(block);

                    Point start = new(Grid.GetColumn(StartBlock), Grid.GetRow(StartBlock));
                    Point end = new(Grid.GetColumn(block), Grid.GetRow(block));

                    if (start.X == end.X)
                    {
                        if (start.Y > end.Y) { FillLine(end, start); }
                        else { FillLine(start, end); }
                    }
                    else if (start.Y == end.Y)
                    {
                        if (start.X > end.X) { FillLine(end, start); }
                        else { FillLine(start, end); }
                    }
                    else
                    {
                        foreach (TextBlock b in affectedBlocks)
                        { b.Background = null; }
                    }


                    StartBlock = null;
                    Point[] points = new Point[affectedBlocks.Count];
                    for (int i = 0; i < points.Length; i++)
                    {
                        int x = Grid.GetColumn(affectedBlocks[i]);
                        int y = Grid.GetRow(affectedBlocks[i]);

                        points[i] = new Point(x, y);
                    }
                    if (points[0].X == points[1].X)
                    { points = points.OrderBy(p => p.Y).ToArray(); }
                    else
                    { points = points.OrderBy(p => p.X).ToArray(); }


                    (string?, string?) result = MarkedCorrectWord(points);
                    if (result.Item1 != null)
                    {
                        foreach (var child in localLabelPanel.Children)
                        {
                            if (child is TextBlock childLbl)
                            {
                                if (childLbl.Text.ToString().ToLower() == result.Item1.ToLower())
                                { childLbl.TextDecorations = TextDecorations.Strikethrough; break; }
                            }
                        }
                    }
                }
            }
        }

        private void Block_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (affectedBlocks.Count > 0)
            {
                foreach (TextBlock b in affectedBlocks)
                { b.Background = null; }
            }
            affectedBlocks.Clear();

            TextBlock block = (TextBlock)sender;
            block.Background = pickedColor;

            affectedBlocks.Add(block);
            StartBlock = block;
        }

        public Grid FillGridWithWords()
        {
            Random r = new();
            string[] localList = file.GetLocalList();
            string[] foreignList = file.GetForeignList();

            int wordAmount = SoupInputBox.WordAmount != null ? (int)SoupInputBox.WordAmount : 0;
            int[] usedWords = new int[wordAmount];

            for (int i = 0; i < wordAmount; i++)
            {
                int wordIndex;

                do { wordIndex = r.Next(localList.Length - 1); }
                while (usedWords.Contains(wordIndex));

                string localWord = localList[wordIndex].ToUpper();
                string foreignWord = foreignList[wordIndex].ToUpper();
                usedWords[i] = wordIndex;

                TextBlock labelTb = new()
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center,
                    Text = $"{localWord[0]}{localWord[1..^0].ToLower()}",
                    Padding = new Thickness(10, 5, 0, 0),
                    FontSize = 13,
                    FontWeight = FontWeights.Regular
                };
                localLabelPanel.Children.Add(labelTb);

                DIRECTION dir = (DIRECTION)r.Next(4);
                int minX = 0; int maxX = sizeX - 1;
                int minY = 0; int maxY = sizeY - 1;

                if (dir == DIRECTION.UP)
                { minY += foreignWord.Length; }
                else if (dir == DIRECTION.DOWN)
                { maxY -= foreignWord.Length; }
                else if (dir == DIRECTION.LEFT)
                { minX += foreignWord.Length; }
                else if (dir == DIRECTION.RIGHT)
                { maxX -= foreignWord.Length; }

                bool foundPath;
                do
                {
                    foundPath = true;
                    int x = r.Next(minX, maxX + 1);
                    int y = r.Next(minY, maxY + 1);
                    Point[] checkPoints = new Point[foreignWord.Length];

                    for (int j = 0; j < foreignWord.Length; j++)
                    {
                        Point p = new(x, y);
                        checkPoints[j] = p;
                        if (usedPointsDict.TryGetValue(p, out char c))
                        {
                            if (char.ToUpper(c) != foreignWord[j])
                            { foundPath = false; break; }
                        }

                        switch (dir)
                        {
                            case DIRECTION.UP:
                                y--;
                                break;
                            case DIRECTION.DOWN:
                                y++;
                                break;
                            case DIRECTION.LEFT:
                                x--;
                                break;
                            case DIRECTION.RIGHT:
                                x++;
                                break;
                        }

                    }

                    if (foundPath)
                    {
                        Point[] wordPoints = new Point[foreignWord.Length];
                        for (int j = 0; j < foreignWord.Length; j++)
                        {
                            Point p = checkPoints[j];
                            char c = foreignWord[j];
                            wordPoints[j] = p;

                            if (!usedPointsDict.ContainsKey(p)) { usedPointsDict.Add(p, c); }
                            TextBlock tb = (TextBlock)GetGridElement(p);
                            tb.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                            tb.Text = $"{c}";
                        }

                        if (wordPoints[0].X == wordPoints[1].X)
                        { wordPoints = wordPoints.OrderBy(p => p.Y).ToArray(); }
                        else
                        { wordPoints = wordPoints.OrderBy(p => p.X).ToArray(); }
                        foreignWordsDict.Add(wordPoints, (localWord, foreignWord));
                    }
                } while (!foundPath);
            }

            return soupGrid;
        }

        private UIElement GetGridElement(int column, int row)
        { return soupGrid.Children.Cast<UIElement>().First(e => Grid.GetColumn(e) == column && Grid.GetRow(e) == row); }

        private UIElement GetGridElement(Point p)
        { return GetGridElement((int)p.X, (int)p.Y); }

        private TextBlock SwapBlock(TextBlock block)
        {
            if (blockPairs.ContainsKey(block))
            { return blockPairs[block]; }
            if (blockPairs.ContainsValue(block))
            { return blockPairs.Keys.Where(b => blockPairs[b] == block).First(); }
            return new TextBlock();
        }

        private void FillLine(Point from, Point to)
        {
            int x, y;

            if (from.X == to.X) // Move y
            {
                x = (int)from.X;
                for (y = (int)from.Y; y < to.Y; y++)
                {
                    TextBlock block = (TextBlock)GetGridElement(x, y);
                    block = SwapBlock(block);
                    block.Background = pickedColor;

                    if (!affectedBlocks.Contains(block))
                    { affectedBlocks.Add(block); }
                }
            }
            else if (from.Y == to.Y) // Move x
            {
                y = (int)from.Y;
                for (x = (int)from.X; x < to.X; x++)
                {
                    TextBlock block = (TextBlock)GetGridElement(x, y);
                    block = SwapBlock(block);
                    block.Background = pickedColor;

                    if (!affectedBlocks.Contains(block))
                    { affectedBlocks.Add(block); }
                }
            }
        }

        private void ClearAffectedBlocks()
        {
            foreach (TextBlock block in affectedBlocks)
            { block.Background = null; }
            affectedBlocks.Clear();
        }

        private (string?, string?) MarkedCorrectWord(Point[] points)
        {
            bool found = true;
            Point[] key = Array.Empty<Point>();
            foreach (Point[] keyPoints in foreignWordsDict.Keys)
            {
                found = true;
                for (int i = 0; i < keyPoints.Length; i++)
                {
                    if (keyPoints[i] != points[i])
                    { found = false; break; }
                }

                if (found)
                {
                    key = keyPoints;
                    foreach (TextBlock block in affectedBlocks)
                    { block.Background = foundColor; foundWordBlocks.Add(block); }
                    affectedBlocks.Clear();
                    break;
                }
            }

            if (!found) { ClearAffectedBlocks(); }
            return found ? foreignWordsDict[key] : (null, null);
        }
    }
}
