using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DBSCAN
{
    internal class ClickingPositionClusterer
    {
        static readonly int SCREEN_WIDTH = 1920;
        static readonly int SCREEN_HEIGHT = 1080;
        static void Main()
        {
            string logfile = "clicks.txt";
            Point[] clicks = File.ReadAllLines(logfile).Select(line => new Point { X = int.Parse(line.Split(',')[0]), Y = int.Parse(line.Split(',')[1]) }).ToArray();
            int minPts = 3;
            double eps = 50;
            List<HashSet<Point>> clusters = DBSCAN(clicks, minPts, eps);
            Console.WriteLine($"{clusters.Count} clusters found!");
            foreach (var cluster in clusters)
            {
                Console.WriteLine($"Cluster position average X: {cluster.Average(p => p.X)} Y: {cluster.Average(p => p.Y)}");
            }
            DrawIllustration(clusters);
        }

        private static void DrawIllustration(List<HashSet<Point>> clusters)
        {
            int minimized_x = 64 * 2; //multiply to compensate for chars being not equally tall as wide on console
            int minimized_y = 36;
            foreach (var cluster in clusters)
            {
                var pos_x = cluster.Average(p => p.X) / SCREEN_WIDTH * minimized_x
                            + 1; //for drawing border
                var pos_y = cluster.Average(p => p.Y) / SCREEN_HEIGHT * minimized_y
                            + clusters.Count + 2; //writes out info of clusters and then illustrates
                Console.SetCursorPosition((int)pos_x, (int)pos_y);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write('X');
                Console.ResetColor();
            }

            for (int i = 1; i < minimized_y; i++)
            {
                Console.SetCursorPosition(0, clusters.Count + 2 + i);
                Console.Write('|');
                Console.SetCursorPosition(minimized_x, clusters.Count + 2 + i);
                Console.Write('|');
            }

            for (int i = 1; i < minimized_x; i++)
            {
                Console.SetCursorPosition(i, clusters.Count + 2);
                Console.Write('-');
                Console.SetCursorPosition(i, clusters.Count + 2 + minimized_y);
                Console.Write('-');
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
