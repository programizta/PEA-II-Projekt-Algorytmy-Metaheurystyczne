using System;

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
<<<<<<< HEAD
        int tempCost;
=======
>>>>>>> 76221b8411b8de11b27990b67913853dc599f560

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

<<<<<<< HEAD
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
=======
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
>>>>>>> 76221b8411b8de11b27990b67913853dc599f560

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

<<<<<<< HEAD
        /// <summary>
        /// Metoda służąca do obliczenia kosztu cyklu Hamiltona
        /// </summary>
        /// <param name="indexMatrix"></param>
        /// <returns></returns>
=======
        void SetTemperature(double tempMax)
        {
            currentTemperature = tempMax;
        }

>>>>>>> 76221b8411b8de11b27990b67913853dc599f560
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

<<<<<<< HEAD
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
            temperatureCoefficient = tCoefficient;
            currentTemperature = maxTemperature;

            for (int j = 0; j < numOfCities; j++)
            {
                finalRoute[j] = j;
            }

            CopyFromTo(finalRoute, tempRoute);

            BestCycleCost = GetPathLength(tempRoute);
            tempCost = BestCycleCost;
=======
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
>>>>>>> 76221b8411b8de11b27990b67913853dc599f560

            while (currentTemperature > tMin)
            {
<<<<<<< HEAD
                GeneratePermutation();
                tempCost = GetPathLength(tempRoute);

                if ((tempCost < BestCycleCost) || (GenerateRandomProbability() < GenerateProbability()))
                {
                    BestCycleCost = tempCost;
                    CopyFromTo(tempRoute, finalRoute);
=======
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
>>>>>>> 76221b8411b8de11b27990b67913853dc599f560
                }
                GeometricTemperatureComputation();
            }

            for (int j = 0; j < numOfCities; j++)
            {
                Route.Push(finalRoute[j]);
            }
            Route.Push(finalRoute[0]);
        }
    }
}
