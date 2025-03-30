using System;

namespace HillClimbing
{
    internal class FunctionOptimumFinder
    {
        static readonly int X_bias = 0;
        static readonly int Y_bias = 0;
        static void Main()
        {
            Console.WriteLine($"Goal to find: [{X_bias}, {Y_bias}] {f([X_bias, Y_bias])}");
            double[] best = [Util.GetDouble(), Util.GetDouble()];
            Console.WriteLine($"Starting [X,Y] is: {double.Round(best[0], 4)}, {double.Round(best[1], 4)} (fitness: {double.Round(f(best), 8)})");
            for (int i = 0; i < 1000; i++)
            {
                double[] curr = HillClimbingSteepestAscent(0.001);
                if (f(curr) <= f(best))
                {
                    best = curr;
                    Console.WriteLine($"Better [X,Y] is: {double.Round(best[0], 4)}, {double.Round(best[1], 4)} (fitness: {double.Round(f(best), 8)})");
                }
            }
            Console.WriteLine($"Absolute best [X,Y] is: {double.Round(best[0], 4)}, {double.Round(best[1], 4)} (fitness: {double.Round(f(best), 8)})");
        }

        private static double f(double[] point)
        {
            return 0.01 * (Math.Pow(point[0] - X_bias, 2) + Math.Pow(point[1] - Y_bias, 2)) + -0.3 * Math.Cos(point[0]) + -0.4 * Math.Cos(point[1]);
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

            if (Random.Shared.Next() % 2 == 0)
                pos[0] = Util.GetDouble();
            else
                pos[0] = -Util.GetDouble();

            if (Random.Shared.Next() % 2 == 0)
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
    }
}
