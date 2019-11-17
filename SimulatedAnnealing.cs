using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public double TotalCost { get; set; }
        public double Time { get; set; }

        int tempCost;

        public SimulatedAnnealing(string filename, int choice) : base(filename, choice)
        {
            Route = new Stack();
            randomGenerator = new Random();
            tempRoute = new int[numOfCities];
            finalRoute = new int[numOfCities];
            temperatureCoefficient = 0;
            currentTemperature = 0;
            tempCost = 0;
        }

        /// <summary>
        /// Metoda zwracająca wartość prawdopodobieństwa, która jest zgodna ze wzorem
        /// p = e^((najlepszy_koszt_cyklu - aktualny_koszt_cyklu) / aktualna_temperatura_wyzarzania)
        /// </summary>
        /// <returns></returns>
        private double GenerateProbability()
        {
            double value = Math.Pow(e, ((BestCycleCost - tempCost) / currentTemperature));

            if (value < 1.0) return value;
            return 1.0;
        }

        /// <summary>
        /// Metoda zwracająca wartość prawdopodobieństwa z przedziału [0, 1]
        /// </summary>
        /// <returns></returns>
        private double GenerateRandomProbability()
        {
            return (double)randomGenerator.Next(0, int.MaxValue) / int.MaxValue;
        }

        /// <summary>
        /// Metoda służąca do wygenerowania nowej permutacji wierzchołków w grafie
        /// dokonując przestawienia dwóch losowych wierzchołków
        /// </summary>
        /// <param name="firstIndex"></param>
        /// <param name="secondIndex"></param>
        private void GeneratePermutation()
        {
            int auxNumber;
            int firstIndex = randomGenerator.Next(0, numOfCities - 1);
            int secondIndex;

            do
            {
                secondIndex = randomGenerator.Next(0, numOfCities - 1);
            } while (firstIndex == secondIndex);

            auxNumber = finalRoute[firstIndex];

            CopyFromTo(finalRoute, tempRoute);

            tempRoute[firstIndex] = tempRoute[secondIndex];
            tempRoute[secondIndex] = auxNumber;
        }

        /// <summary>
        /// Metoda generująca w sposób geometryczny nową temperaturę wyżarzania
        /// </summary>
        private void GeometricTemperatureComputation()
        {
            currentTemperature *= temperatureCoefficient;
        }

        /// <summary>
        /// Metoda służąca do obliczenia kosztu cyklu Hamiltona
        /// </summary>
        /// <param name="indexMatrix"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Metoda kopiująca zawartość tablic
        /// from - tablica, z której chcemy skopiować zawartość
        /// to - tablica, do której chcemy przekopiować zawartość z tablicy from
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        private void CopyFromTo(int[] from, int[] to)
        {
            for (int i = 0; i < numOfCities; i++)
            {
                to[i] = from[i];
            }
        }

        /// <summary>
        /// Metoda rozwiązująca problem komiwojażera wykonując algorytm symulowanego wyżarzania
        /// </summary>
        /// <param name="minTemperature"></param>
        /// <param name="maxTemperature"></param>
        /// <param name="tCoefficient"></param>
        public void StartSA(double minTemperature, double maxTemperature, double tCoefficient)
        {
            //TotalCost = 0;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            //for (int i = 0; i < 100; i++)
            //{
                temperatureCoefficient = tCoefficient;
                currentTemperature = maxTemperature;

                for (int j = 0; j < numOfCities; j++)
                {
                    finalRoute[j] = j;
                }

                CopyFromTo(finalRoute, tempRoute);

                BestCycleCost = GetPathLength(tempRoute);
                tempCost = BestCycleCost;

                while (currentTemperature > minTemperature)
                {
                    GeneratePermutation();
                    tempCost = GetPathLength(tempRoute);

                    if ((tempCost < BestCycleCost) || (GenerateRandomProbability() < GenerateProbability()))
                    {
                        BestCycleCost = tempCost;
                        CopyFromTo(tempRoute, finalRoute);
                    }
                    GeometricTemperatureComputation();
                }
            //TotalCost += BestCycleCost;

            for (int j = 0; j < numOfCities; j++)
            {
                Route.Push(finalRoute[j]);
            }
            Route.Push(finalRoute[0]);
            //}

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;

            //TotalCost /= 100;
            //Time = ts.TotalMilliseconds / 100;
            Time = ts.TotalMilliseconds;
        }
    }
}
