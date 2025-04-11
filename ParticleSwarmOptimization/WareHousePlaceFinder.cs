
using System;
using System.Collections.Generic;
using System.Threading;

namespace ParticleSwarmOptimization
{
    internal class WareHousePlaceFinder
    {
        static readonly int POPULATION_SIZE = 20;
        static readonly int NUM_OF_BUILDINGS = 10;
        static readonly double INERTIA_WEIGHT = 0.7;
        static readonly double ÖNFEJŰSÉG_WEIGHT = 0.5;
        static readonly double KONVERGÁLÁS_WEIGHT = 0.5;
        static List<double[]> STORES = [];
        static List<double[]> RESIDENTIALS = [];

        static void Main()
        {
            STORES = SetCoordinates();
            RESIDENTIALS = SetCoordinates();

            double[] bestPlace = ParticeSwarmOptimization(20);
            Console.WriteLine();
            Console.WriteLine($"The best place to put the warehouse is: [{Math.Round(bestPlace[0], 4)}, {Math.Round(bestPlace[1], 4)}]");
        }

        private static List<double[]> SetCoordinates()
        {
            List<double[]> doubles = [];
            for (int i = 0; i < NUM_OF_BUILDINGS / 2; i++)
            {
                doubles.Add([Random.Shared.NextDouble(), Random.Shared.NextDouble()]);
            }
            return doubles;
        }

        static double f(double[] particle)
        {
            double ClosestResidental = 1; //we want this to be maximal
            double FurthestStore = 0; //we want this to be minimal
            foreach (double[] store in STORES)
            {
                double distance = Math.Sqrt(Math.Pow((store[0] - particle[0]), 2) + Math.Pow((store[1] - particle[1]), 2));
                if (distance > FurthestStore)
                {
                    FurthestStore = distance;
                }
            }
            foreach (double[] res in RESIDENTIALS)
            {
                double distance = Math.Sqrt(Math.Pow((res[0] - particle[0]), 2) + Math.Pow((res[1] - particle[1]), 2));
                if (distance < ClosestResidental)
                {
                    ClosestResidental = distance;
                }
            }

            return ClosestResidental - FurthestStore; //here the bigger, the better
        }

        static double[] ParticeSwarmOptimization(int iterCount)
        {
            double[] globalOpt = [0.5, 0.5];
            var P = InitPopulation();

            DrawMap(P);

            Evaluation(P, ref globalOpt);
            for (int i = 0; i < iterCount; i++)
            {
                DrawMap(P);
                Console.ReadLine();
                Thread.Sleep(1000);
                CalculateVelocity(P, globalOpt);
                MovePopulation(P);
                Evaluation(P, ref globalOpt);
            }
            return globalOpt;
        }

        private static Entity[] InitPopulation()
        {
            var population = new Entity[POPULATION_SIZE];
            for (int i = 0; i < population.Length; i++)
            {
                population[i] = new();
            }
            return population;
        }

        private static void DrawMap(Entity[] P)
        {
            Console.Clear();
            int magnify_X = 100;
            int magnify_Y = 50;

            foreach (double[] store in STORES)
            {
                Console.SetCursorPosition((int)(store[0] * magnify_X), (int)(store[1] * magnify_Y));
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write('S');
            }//stores
            foreach (double[] res in RESIDENTIALS)
            {
                Console.SetCursorPosition((int)(res[0] * magnify_X), (int)(res[1] * magnify_Y));
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write('R');
            }//residental buildings

            for (int i = 0; i < P.Length; i++)
            {
                Console.SetCursorPosition((int)(P[i].Position[0] * magnify_X), (int)(P[i].Position[1] * magnify_Y));
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(i);
            }//particles

            for (int i = 0; i < magnify_Y; i++)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.SetCursorPosition(magnify_X, i);
                Console.Write('|');
            }//side border
            for (int i = 0; i < magnify_X; i++)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.SetCursorPosition(i, magnify_Y);
                Console.Write('-');
            }//bottom border
        }

        private static void MovePopulation(Entity[] P)
        {
            for (int i = 0; i < P.Length; i++)
            {
                for (int j = 0; j < P[i].Position.Length; j++)
                {
                    P[i].Position[j] += P[i].Velocity[j];

                    //can't go outside boundaries
                    if (P[i].Position[j] < 0)
                        P[i].Position[j] = 0;
                    else if (P[i].Position[j] > 1)
                        P[i].Position[j] = 1;
                }
            }
        }

        static void Evaluation(Entity[] Population, ref double[] globalOpt)
        {
            for (int i = 0; i < Population.Length; i++)
            {
                if (f(Population[i].Position) >= f(Population[i].Optimum!))
                {
                    Population[i].Optimum![0] = Population[i].Position[0];
                    Population[i].Optimum[1] = Population[i].Position[1];
                    if (f(Population[i].Optimum) >= f(globalOpt))
                    {
                        globalOpt = Population[i].Optimum;
                    }
                }
            }
            ;
        }

        private static void CalculateVelocity(Entity[] P, double[] globalOpt)
        {
            for (int i = 0; i < P.Length; i++)
            {
                double Rp = Random.Shared.NextDouble();
                double Rg = Random.Shared.NextDouble();
                for (int j = 0; j < P[i].Velocity.Length; j++)
                {
                    P[i].Velocity[j] = INERTIA_WEIGHT * P[i].Velocity[j]
                                    + ÖNFEJŰSÉG_WEIGHT * Rp * (P[i].Optimum[j] - P[i].Position[j])
                                    + KONVERGÁLÁS_WEIGHT * Rg * (globalOpt[j] - P[i].Position[j]);
                }
            }
        }
    }

    internal class Entity
    {
        public double[] Optimum { get; set; }
        public double[] Position { get; set; }
        public double[] Velocity { get; set; }

        public Entity()
        {
            Optimum = [Random.Shared.NextDouble(), Random.Shared.NextDouble()];
            Position = [Random.Shared.NextDouble(), Random.Shared.NextDouble()];
            Velocity = [Random.Shared.NextDouble() / 10, Random.Shared.NextDouble() / 10];
        }
    }
}
