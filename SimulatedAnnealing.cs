using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace II_Projekt
{
    class SimulatedAnnealing : Graph
    {
        Random randomGenerator;
        public Stack Route { get; set; }
        const double e = 2.718281828459045;
        private double temperatureCoefficient;
        private double currentTemperature;
        private readonly int[] tempRoute;
        private readonly int[] finalRoute;

        public SimulatedAnnealing(string filename, int choice) : base(filename, choice)
        {
            Route = new Stack();
            randomGenerator = new Random();
            tempRoute = new int[numOfCities];
            finalRoute = new int[numOfCities];
            temperatureCoefficient = 0;
            currentTemperature = 0;
        }

        private int GenerateProbability(int a, int b)
        {
            double probabilityValueFromEquation = Math.Pow(e, ((-1 * (b - a)) / currentTemperature));

            double normalProbabilityValue = (double)randomGenerator.Next(int.MaxValue) / int.MaxValue;

            if (normalProbabilityValue < probabilityValueFromEquation)
            {
                return 1;
            }
            return 0;
        }

        private void GeneratePermutation(int[] arrayOfIndexes)
        {
            int randomIndex;
            int[] auxMatrix = new int[numOfCities];

            for (int i = 0; i < numOfCities; i++)
            {
                auxMatrix[i] = i;
            }

            for (int i = numOfCities; i > 0; i--)
            {
                randomIndex = randomGenerator.Next(int.MaxValue) % i;
                arrayOfIndexes[i - 1] = auxMatrix[randomIndex];
                auxMatrix[randomIndex] = auxMatrix[i - 1];
            }
        }

        private void ArithmeticTemperatureComputation()
        {
            currentTemperature -= temperatureCoefficient;
        }

        private void GeometricTemperatureComputation()
        {
            currentTemperature *= temperatureCoefficient;
        }

        void SetTemperature(double tempMax)
        {
            currentTemperature = tempMax;
        }

        private int GetPathLength(int[] indexMatrix)
        {
            int weightOfPath = 0;

            for (int i = 0; i < numOfCities - 1; i++)
            {
                weightOfPath += costMatrix[indexMatrix[i], indexMatrix[i + 1]];
            }
            weightOfPath += costMatrix[indexMatrix[numOfCities - 1], indexMatrix[0]];

            return weightOfPath;
        }

        private void CopyFromTo(int[] from, int[] to)
        {
            for (int i = 0; i < numOfCities; i++)
            {
                to[i] = from[i];
            }
        }

        public void StartSA(double tMax, double tMin, double tCoefficient)
        {
            int firstIndex;
            int secondIndex;
            int a, b;
            double temporaryDifference, difference = 0;
            currentTemperature = tMax;
            temperatureCoefficient = tCoefficient;

            for (int i = 0; i < numOfCities; i++)
            {
                GeneratePermutation(finalRoute);
                GeneratePermutation(tempRoute);

                temporaryDifference = Math.Abs(GetPathLength(finalRoute) - GetPathLength(tempRoute));

                if (temporaryDifference > difference)
                {
                    difference = temporaryDifference;
                }
            }
            currentTemperature = difference;

            GeneratePermutation(finalRoute);
            a = GetPathLength(finalRoute);
            CopyFromTo(finalRoute, tempRoute);

            while (currentTemperature > tMin)
            {
                firstIndex = randomGenerator.Next(int.MaxValue) % numOfCities;

                do
                {
                    secondIndex = randomGenerator.Next(int.MaxValue) % numOfCities;
                } while (firstIndex == secondIndex);

                tempRoute[secondIndex] = finalRoute[firstIndex];
                tempRoute[firstIndex] = finalRoute[secondIndex];

                b = GetPathLength(tempRoute);

                if (b <= a || GenerateProbability(a, b) == 1)
                {
                    a = b;

                    if (a <= BestCycleCost)
                    {
                        BestCycleCost = a;
                        Route.Clear();

                        for (int i = 0; i < numOfCities; i++)
                        {
                            Route.Push(tempRoute[i]);
                        }
                        Route.Push(tempRoute[0]);
                    }

                    finalRoute[firstIndex] = tempRoute[firstIndex];
                    finalRoute[secondIndex] = tempRoute[secondIndex];
                }
                else
                {
                    tempRoute[firstIndex] = finalRoute[firstIndex];
                    tempRoute[secondIndex] = finalRoute[secondIndex];
                }
                GeometricTemperatureComputation();
            }
        }
    }
}
