using System;

namespace HillClimbing
{
    internal class FunctionOptimumFinder
    {
        static readonly int X_bias = 0;
        static readonly int Y_bias = 0;
        static readonly double A = 0.01;
        static readonly double B = -0.3;
        static readonly double C = -0.4;
        static void Main()
        {
            Console.WriteLine($"Goal to find: [{X_bias}, {Y_bias}] (fitness: {double.Round(f([X_bias, Y_bias]), 10)})");

            double[] best = HillClimbingSteepestAscentRetry(tryCount: 1000, epsilon: 0.0001);

            Console.WriteLine($"Absolute best [X,Y] is: {double.Round(best[0], 4)}, {double.Round(best[1], 4)} (fitness: {double.Round(f(best), 10)})");
        }

        private static double[] HillClimbingSteepestAscentRetry(int tryCount, double epsilon)
        {
            double[] best = [Util.GetDouble(), Util.GetDouble()];
            Console.WriteLine($"Starting [X,Y] is: {double.Round(best[0], 4)}, {double.Round(best[1], 4)} (fitness: {double.Round(f(best), 10)})");
            for (int i = 0; i < tryCount; i++)
            {
                double[] curr = HillClimbingSteepestAscent(epsilon);
                if (f(curr) <= f(best))
                {
                    best = curr;
                    Console.WriteLine($"Better [X,Y] is: {double.Round(best[0], 4)}, {double.Round(best[1], 4)} (fitness: {double.Round(f(best), 10)})");
                }
            }
            return best;
        }

        private static double f(double[] point)
        {
            return (A * (Math.Pow(point[0] - X_bias, 2) + Math.Pow(point[1] - Y_bias, 2)) + B * Math.Cos(point[0]) + C * Math.Cos(point[1])) - (B + C);
        }

        private static double[] HillClimbingSteepestAscent(double epsilon)
        {
            double[] best = GetRandomPos();

            bool stuck = false;
            while (!stuck)
            {
                double[] next = SelectMinimum(best, epsilon);

                if (f(next) < f(best))
                {
                    best = next;
                }
                else
                {
                    stuck = true;
                }
            }
            return best;
        }

        private static double[] GetRandomPos()
        {
            double[] pos = [0, 0];

            if (Util.GetNumber() % 2 == 0)
                pos[0] = Util.GetDouble();
            else
                pos[0] = -Util.GetDouble();

            if (Util.GetNumber() % 2 == 0)
                pos[1] = Util.GetDouble();
            else
                pos[1] = -Util.GetDouble();

            return pos;
        }

        private static double[] SelectMinimum(double[] current, double epsilon)
        {
            //8 ways decider
            double[] minimal = current;

            if (f([current[0] + epsilon, current[1] + epsilon]) < f(minimal))   // fel, jobbra
                minimal = [current[0] + epsilon, current[1] + epsilon];

            if (f([current[0], current[1] + epsilon]) < f(minimal))             // fel
                minimal = [current[0] + epsilon, current[1] + epsilon];

            if (f([current[0] - epsilon, current[1] + epsilon]) < f(minimal))   //fel, ballra
                minimal = [current[0] + epsilon, current[1] + epsilon];

            if (f([current[0] - epsilon, current[1]]) < f(minimal))             //ballra
                minimal = [current[0] + epsilon, current[1] + epsilon];

            if (f([current[0] - epsilon, current[1] - epsilon]) < f(minimal))   //le, ballra
                minimal = [current[0] + epsilon, current[1] + epsilon];

            if (f([current[0], current[1] - epsilon]) < f(minimal))             //le
                minimal = [current[0] + epsilon, current[1] + epsilon];

            if (f([current[0] + epsilon, current[1] - epsilon]) < f(minimal))   //le, jobbra
                minimal = [current[0] + epsilon, current[1] + epsilon];

            if (f([current[0] + epsilon, current[1]]) < f(minimal))             //jobbra
                minimal = [current[0] + epsilon, current[1] + epsilon];

            return minimal;
        }
    }
    class Util
    {
        static readonly int RANGE = 10;
        static readonly Random RND = new();
        public static double GetDouble() => RND.NextDouble() * RANGE;
        public static int GetNumber() => RND.Next();
    }
}
