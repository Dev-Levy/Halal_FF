namespace HillClimbing
{
    internal class FunctionOptimumFinder
    {
        static readonly double MAX = 10.0;
        static readonly double MIN = 0.0;
        static void Main(string[] args)
        {
            double best = 1.0;
            Console.WriteLine($"Starting X is: {double.Round(best, 4)} (fitness: {double.Round(f(best), 8)})");
            for (int i = 0; i < 1000; i++)
            {
                double curr = HillClimbingSteepestAscent(0.0001);
                if (f(curr) <= f(best))
                {
                    best = curr;
                    Console.WriteLine($"Better X is: {double.Round(best, 4)} (fitness: {double.Round(f(best), 8)})");
                }
            }
            Console.WriteLine($"Absolute best X is: {double.Round(best, 4)} (fitness: {double.Round(f(best), 8)})");
        }

        private static double f(double x)
        {
            return Math.Sin(x) + 0.5 * Math.Sin(2 * x) + 0.25 * Math.Sin(3 * x) + 0.5 * Math.Cos(Math.PI * x);
        }

        private static double HillClimbingSteepestAscent(double epsilon)
        {
            double best = Random.Shared.NextDouble() * 10;
            bool stuck = false;
            while (!stuck)
            {
                double next = SelectMinimum(best, epsilon);

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

        private static double SelectMinimum(double current, double epsilon)
        {
            if (f(current + epsilon) < f(current - epsilon))
            {
                return Math.Min(current + epsilon, MAX);
            }
            else
            {
                return Math.Max(current - epsilon, MIN);
            }
        }
    }
}
