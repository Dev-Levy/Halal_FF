using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DBSCAN
{
    internal class ClickingPositionClusterer
    {
        static void Main()
        {
            string logfile = "MouseClicks.txt";
            int[][] clicks = File.ReadAllLines(logfile).Select(line => new int[] { int.Parse(line.Split(';')[0]), int.Parse(line.Split(';')[1]) }).ToArray();
            int minPts = 3;
            double eps = 1.0;
            List<HashSet<int[]>> clusters = DBSCAN(clicks, minPts, eps);

            foreach (var cluster in clusters)
            {
                Console.Write(cluster.Average(point => point[0]));
                Console.WriteLine(cluster.Average(point => point[1]));
            }
        }

        public static List<HashSet<int[]>> DBSCAN(int[][] clicks, int minPts, double eps)
        {
            List<HashSet<int[]>> clusters = [];
            HashSet<int[]> Processed = [];

            foreach (int[] p in clicks)
            {
                if (!Processed.Contains(p))
                {
                    HashSet<int[]> Neighbours = clicks.Where(other => Ds(p, other) <= eps).ToHashSet();
                    if (Neighbours.Count >= minPts)
                    {
                        HashSet<int[]> Reachable = [];
                        foreach (int[] q in Neighbours)
                        {
                            Processed.Add(q);
                            Reachable.Add(q);
                            HashSet<int[]> ClosePoints = clicks.Where(other => Ds(q, other) <= eps).ToHashSet();
                            if (ClosePoints.Count >= minPts)
                            {
                                Neighbours.UnionWith(ClosePoints); // ez problem
                            }
                        }
                        clusters.Add(Reachable);
                    }
                }
            }
            return clusters;
        }

        private static double Ds(int[] p, int[] other) => Math.Sqrt(Math.Pow(p[0] - other[0], 2) + Math.Pow(p[1] - other[1], 2));
    }
}
