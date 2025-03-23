
namespace ParticleSwarmOptimization
{
    internal class WareHousePlaceFinder
    {
        static readonly int MAP_SIZE = 100;
        static readonly int POPULATION_SIZE = 20;
        static readonly double INERTIA_WEIGHT = 0.5;
        static readonly double ÖNFEJŰSÉG_WEIGHT = 0.5;
        static readonly double KONVERGÁLÁS_WEIGHT = 0.5;

        static void Main()
        {
            //problem: where to put warehouse
            //away from residentals, close to stores
        }

        static double f(double[] x)
        {
            return 0;
            //TODO
        }

        static void ParticeSwarmOptimization(int mapSize)
        {
            double[] globalOpt = [0.0, 0.0];
            var P = InitPopulation(mapSize);
            Evaluation(P, globalOpt);
            while (true)
            {
                CalculateVelocity(P, globalOpt);
                foreach (var p in P)
                {
                    for (int i = 0; i < p.Position.Length; i++)
                    {
                        p.Position[i] += p.Velocity[i];
                    }
                }
                Evaluation(P, globalOpt);
            }
        }
        static Entity[] InitPopulation(int mapSize)
        {
            Entity[] population = new Entity[POPULATION_SIZE];
            for (int i = 0; i < population.Length; i++)
            {
                population[i] = new()
                {
                    Position = [Random.Shared.NextDouble() * MAP_SIZE, Random.Shared.NextDouble() * MAP_SIZE],
                    Optimum = [Random.Shared.NextDouble() * MAP_SIZE, Random.Shared.NextDouble() * MAP_SIZE],
                    Velocity = [Random.Shared.NextDouble() * MAP_SIZE / 10, Random.Shared.NextDouble() * MAP_SIZE / 10]
                };
            }
            return population;
        }

        static void Evaluation(Entity[] Population, double[] globalOpt)
        {
            foreach (var p in Population)
            {
                if (f(p.Position) < f(p.Optimum))
                {
                    p.Optimum = p.Position;
                    if (f(p.Optimum) < f(globalOpt))
                    {
                        globalOpt = p.Optimum;
                    }
                }
            }
        }

        private static void CalculateVelocity(Entity[] P, double[] globalOpt)
        {
            foreach (var p in P)
            {
                double Rp = Random.Shared.NextDouble();
                double Rg = Random.Shared.NextDouble();
                for (int i = 0; i < p.Velocity.Length; i++)
                {
                    p.Velocity[i] = INERTIA_WEIGHT * p.Velocity[i]
                                  + ÖNFEJŰSÉG_WEIGHT * Rp * (p.Optimum[i] - p.Velocity[i])
                                  + KONVERGÁLÁS_WEIGHT * Rg * (globalOpt[i] - p.Velocity[i]);
                }
            }
        }
    }

    internal class Entity
    {
        public required double[] Optimum { get; set; }
        public required double[] Position { get; set; }
        public required double[] Velocity { get; set; }
    }
}
