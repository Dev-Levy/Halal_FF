using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DBSCAN
{
    internal class ClickingPositionClusterer
    {
        static readonly int SCREEN_WIDTH = 2560;
        static readonly int SCREEN_HEIGHT = 1440;
        static void Main()
        {
            string logfile = "clicks.txt";
            Point[] clicks = File.ReadAllLines(logfile).Select(line => new Point { X = int.Parse(line.Split(',')[0]), Y = int.Parse(line.Split(',')[1]) }).ToArray();
            int minPts = 3;
            double eps = 100;
            List<HashSet<Point>> clusters = DBSCAN(clicks, minPts, eps);
            Console.WriteLine($"{clusters.Count} clusters found!");
            foreach (var cluster in clusters)
            {
                Console.WriteLine($"Cluster average position:  [{Math.Round(cluster.Average(p => p.X), 4)}, {Math.Round(cluster.Average(p => p.Y), 4)}]");
            }
            DrawIllustration(clusters);
            Console.ReadLine();
        }

        private static void DrawIllustration(List<HashSet<Point>> clusters)
        {
            int minimized_width = 64 * 2; //multiply to compensate for chars not being equally tall as wide on console
            int minimized_heigth = 36;
            for (int i = 1; i < minimized_heigth; i++)
            {
                Console.SetCursorPosition(0, clusters.Count + 2 + i);
                Console.Write('|');
                Console.SetCursorPosition(minimized_width, clusters.Count + 2 + i);
                Console.Write('|');
            }

            for (int i = 1; i < minimized_width; i++)
            {
                Console.SetCursorPosition(i, clusters.Count + 2);
                Console.Write('-');
                Console.SetCursorPosition(i, clusters.Count + 2 + minimized_heigth);
                Console.Write('-');
            }

            foreach (var cluster in clusters)
            {
                int pos_x = (int)(cluster.Average(p => p.X) / SCREEN_WIDTH * minimized_width + 1); // + for drawing border
                int pos_y = (int)(cluster.Average(p => p.Y) / SCREEN_HEIGHT * minimized_heigth + clusters.Count + 2);
                Console.SetCursorPosition(pos_x, pos_y);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write('X');
                Console.ResetColor();
            }
        }

        public static List<HashSet<Point>> DBSCAN(Point[] clicks, int minPts, double eps)
        {
            List<HashSet<Point>> clusters = [];
            HashSet<Point> processed = [];

            foreach (Point p in clicks)
            {
                if (!processed.Contains(p))
                {
                    HashSet<Point> neighbours = clicks.Where(other => Ds(p, other) <= eps).ToHashSet();
                    if (neighbours.Count >= minPts)
                    {
                        HashSet<Point> cluster = [];
                        Queue<Point> queue = new(neighbours);

                        while (queue.Count > 0)
                        {
                            Point q = queue.Dequeue();
                            if (!processed.Contains(q))
                            {
                                processed.Add(q);
                                cluster.Add(q);
                                HashSet<Point> newNeighbours = clicks.Where(other => Ds(q, other) <= eps).ToHashSet();
                                if (newNeighbours.Count >= minPts)
                                {
                                    foreach (Point n in newNeighbours)
                                    {
                                        if (!processed.Contains(n))
                                        {
                                            queue.Enqueue(n);
                                        }
                                    }
                                }
                            }
                        }
                        clusters.Add(cluster);
                    }
                }
            }
            return clusters;
        }

        private static double Ds(Point p, Point other) => Math.Sqrt(Math.Pow(p.X - other.X, 2) + Math.Pow(p.Y - other.Y, 2));
    }

    class Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is Point point
                    && point.X == X
                    && point.Y == Y;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }
    }
}
