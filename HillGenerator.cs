using System;
using System.Collections.Generic;
using System.Linq;
using ECSL;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tanks.Components;

namespace Tanks
{
    public class HillGenerator
    {

        private static HillGenerator instance;

        private Random random;
        private Texture2D hillsTexture;
        private Color[,] colors;
        private List<Point> positions;

        private HillGenerator()
        {
            random = new Random();
            positions = new List<Point>();
        }

        public static HillGenerator GetInstance()
        {
            if (instance == null)
                instance = new HillGenerator();
            return instance;
        }

        public Texture2D GenerateHills(GraphicsDevice graphicsDevice, Int32 minHeight, Int32 maxHeight,
            Int32 terrainComplexity, Int32 width, Int32 height)
        {
            Int32[] noise = GenerateNoise(minHeight, maxHeight, -5, 5, width);
            for (int i = 0; i < terrainComplexity; i++)
                noise = Smoother(noise);

            Color[,] pixels = new Color[width, height];
            for (int i = 0; i < width; i++)
            {
                Int32 pixelHeight = noise[i];
                for (int j = 0; j < 50; j++)
                {
                    pixelHeight += 1;
                    pixels[i, pixelHeight] = Color.Green;
                }
                for (int j = pixelHeight; j < 1080; j++)
                {
                    pixels[i, j] = new Color(117, 80, 0);
                }
            }

            Texture2D texture = new Texture2D(graphicsDevice, width, height);
            texture.SetData(FormatPixels(pixels));

            hillsTexture = texture;
            colors = pixels;

            SetPositions();

            return texture;
        }

        private Boolean IsAtTop(Point point)
        {
            if (colors[point.X, point.Y].A == 0)
                return false;

            foreach (var neighbor in GetNeighbors(point))
            {
                if (colors[neighbor.X, neighbor.Y].A == 0)
                    return true;
            }
            return false;
        }

        private Point[] GetNeighbors(Point point)
        {
            List<Point> points = new List<Point>();

            if (point.X != 0)
                points.Add(new Point(point.X - 1, point.Y));
            if (point.X != 1919)
                points.Add(new Point(point.X + 1, point.Y));
            if (point.Y != 599)
                points.Add(new Point(point.X, point.Y - 1));
            if (point.Y != 1079)
                points.Add(new Point(point.X, point.Y + 1));

            return points.ToArray();
        }

        private Point GetNearest(Point start)
        {
            Queue<Point> frontier = new Queue<Point>();
            List<Point> visited = positions.ToList();
            frontier.Enqueue(start);

            while (frontier.Any())
            {
                Point current = frontier.Dequeue();
                visited.Add(current);

                if (current != start && IsAtTop(current))
                    return current;

                foreach (var neighbor in GetNeighbors(current))
                {
                    if (!visited.Contains(neighbor))
                        frontier.Enqueue(neighbor);
                }
            }
            return new Point(-1);
        }

        private Point GetFirst()
        {
            for (int i = 0; i < 1080; i++)
            {
                if (colors[0, i].A == 255)
                    return new Point(0, i);
            }
            throw new Exception("First position not found!");
        }

        private void SetPositions()
        {
            positions.Clear();

            Point point = GetFirst();
            Point negative = new Point(-1);
            positions.Add(point);

            while ((point = GetNearest(point)) != negative)
            {
                positions.Add(point);
                if (point.X == 1918)
                    return;
            }
        }

        public Int32 GetSpawnPosition(Boolean left)
        {
            Point[] points;
            if (left)
                points = positions.Where(p => p.X <= 960).ToArray();
            else
                points = positions.Where(p => p.X >= 960).ToArray();

            var quarter = points.Length / 4;
            Point point = points[random.Next(quarter, 2 * quarter)];
            return positions.IndexOf(point);
        }

        public Boolean Collide(Position position, out Int32 index)
        {
            for (int i = 0; i < positions.Count; i++)
            {
                Rectangle rectangle = new Rectangle((Int32)position.X - 10, (Int32)position.Y - 10, 20, 20);
                if (rectangle.Contains(positions[i]))
                {
                    index = i;
                    return true;
                }
            }
            index = 0;
            return false;
        }

        private Position origin;
        private Position position1;
        private Position position2;
        private Int32 playerIndex;

        public Point Booom(Int32 index, Position position1, Position position2, out Int32 playerIndex)
        {
            Position origin = positions[index];
            origin.X -= 32;
            origin.Y -= 64;

            this.origin = origin;
            this.position1 = position1;
            this.position2 = position2;
            this.playerIndex = -1;

            Int32 radius = 64;
            Int32 x0 = (Int32)origin.X + radius;
            Int32 y0 = (Int32)origin.Y + radius;
            Int32 x = radius - 1;
            Int32 y = 0;
            Int32 dx = 1;
            Int32 dy = 1;
            Int32 err = dx - (radius << 1);

            while(x >= y)
            {
                ClearLine(y0 + y, x0 + x, x0 - x);
                ClearLine(y0 + x, x0 + y, x0 - y);
                ClearLine(y0 - y, x0 - x, x0 + x);
                ClearLine(y0 - x, x0 - y, x0 + y);

                if (err <= 0)
                {
                    y++;
                    err += dy;
                    dy += 2;
                }

                if (err > 0)
                {
                    x--;
                    dx += 2;
                    err += dx - (radius << 1);
                }
            }
            playerIndex = this.playerIndex;
            hillsTexture.SetData(FormatPixels(colors));
            SetPositions();
            return new Point(AdjustPosition(this.position1), AdjustPosition(this.position2));
        }

        private Int32 AdjustPosition(Position position)
        {
            for (int i = 0; i < positions.Count; i++)
            {
                if (positions[i].ToVector2() == position.ToVector2())
                    return i;
            }

            for (int i = 0; i < positions.Count; i++)
            {
                if (positions[i].X == position.X)
                    return i;
            }
            throw new Exception("Position index not found!");
        }

        private void ClearLine(Int32 y, Int32 x1, Int32 x2)
        {
            Int32 startX = Math.Min(x1, x2);
            Int32 endX = Math.Max(x1, x2);
            for (int i = startX; i <= endX; i++)
            {
                ClearPixel(i, y);
            }
        }

        private void ClearPixel(Int32 x, Int32 y)
        {
            Int32 rX = x ;
            Int32 rY = y ;
            if (rX >= 0 && rX < 1920 && rY >= 0 && rY < 1080)
            {
                colors[rX, rY] = new Color(0, 0, 0, 0);

                if ((Int32)position1.X == rX && (Int32)position1.Y == rY)
                    playerIndex = 0;
                if ((Int32)position2.X == rX && (Int32)position2.Y == rY)
                    playerIndex = 1;
            }
        }

        private Int32[] GenerateNoise(Int32 minHeight, Int32 maxHeight, Int32 minAdd, Int32 maxAdd, Int32 lenght)
        {
            Int32[] noise = new Int32[lenght];
            noise[0] = random.Next(minHeight, maxHeight + 1);
            for (Int32 i = 1; i < 1920; i++)
            {
                noise[i] = noise[i - 1] + random.Next(minAdd, maxAdd + 1);
                if (noise[i] > maxHeight)
                    noise[i] = maxHeight;
                else if (noise[i] < minHeight)
                    noise[i] = minHeight;
            }
            return noise;
        }

        private Int32[] Smoother(Int32[] noise)
        {
            Int32[] output = new Int32[noise.Length];
            for (int i = 0; i < noise.Length - 1; i++)
            {
                output[i] = (Int32)(0.5 * (noise[i] + noise[i + 1]));
            }
            output[output.Length - 1] = noise[noise.Length - 2];
            return output;
        }

        private Color[] FormatPixels(Color[,] colors)
        {
            Color[] formated = new Color[colors.GetLength(0) * colors.GetLength(1)];
            Int32 y = 0;
            for (int i = 0; i < colors.GetLength(1); i++)
            {
                for (int j = 0; j < colors.GetLength(0); j++)
                {
                    formated[y++] = colors[j, i];
                }
            }
            return formated;
        }

        public Position GetPosition(HillPosition hillPosition)
        {
            Normalize(hillPosition);

            return positions[(Int32)hillPosition.Position];
        }

        private void Normalize(HillPosition hillPosition)
        {
            if (hillPosition.Position >= positions.Count)
                hillPosition.Position = positions.Count - 1;
            else if (hillPosition.Position < 0)
                hillPosition.Position = 0;
        }

    }
}
