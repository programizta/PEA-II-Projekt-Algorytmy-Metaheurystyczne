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
        const double e = 2.718281828459045;
        public Stack Route { get; private set; }
        private double temperatureCoefficient;
        private double minTemperature;
        private double currentTemperature;
        private int tempCost;
        private Stack tempRoute;
        private Stack finalRoute;

        public SimulatedAnnealing(string filename, int choice) : base(filename, choice)
        {
            randomGenerator = new Random();
            tempRoute = new Stack();
            finalRoute = new Stack();
            temperatureCoefficient = 0;
            minTemperature = 0;
            currentTemperature = 0;
        }

        private double GenerateProbability()
        {
            double value = Math.Pow(e, (((double)(BestCycleCost - tempCost)) / currentTemperature));
            if (value < 1.0) return value;
            return 1.0;
        }

        /// <summary>
        /// Metoda zwracająca losową wartość z przedziału [0; 1)
        /// </summary>
        /// <returns></returns>
        private double GenerateRandomProbability()
        {
            double probability = randomGenerator.NextDouble();
            return probability;
        }

        private void SwitchRandomVertexes()
        {
            int auxNumber;
            int indexOfFirstVertex = randomGenerator.Next(0, numOfCities - 1);
            int indexOfSecondVertex;

            // losuj drugą liczbę dopóki są takie same
            do
            {
                indexOfSecondVertex = randomGenerator.Next(0, numOfCities - 1);
            } while (indexOfFirstVertex == indexOfSecondVertex);

            auxNumber = finalRoute.numbersOnStack[indexOfFirstVertex];
            tempRoute = finalRoute;  //???? co ja tu miałem na myśli?
            tempRoute.numbersOnStack[indexOfFirstVertex] = tempRoute.numbersOnStack[indexOfSecondVertex];
            tempRoute.numbersOnStack[indexOfSecondVertex] = auxNumber;
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

        private int GetPathLength(Stack currentRoute)
        {
            int weightOfPath = 0;
            int rowHolder = 0;

            // indeksuje od 1, ponieważ wierzchołek startowy, dla którego
            // obliczam koszt ścieżki jest równy 0
            for (int i = 1; i < numOfCities; i++)
            {
                // nie mozemy dopuscic do doliczenia do cyklu wartosc na przekatnej
                if (costMatrix[rowHolder, currentRoute.numbersOnStack[i]] != int.MaxValue)
                {
                    weightOfPath += costMatrix[rowHolder, currentRoute.numbersOnStack[i]];
                }
                rowHolder = currentRoute.numbersOnStack[i];
            }

            // dodanie ścieżki do wierzchołka początkowego (zamykamy cykl)
            weightOfPath += costMatrix[rowHolder, 0];
            return weightOfPath;
        }

        public void StartSA(double tMax, double tMin, double tCoefficient)
        {
            temperatureCoefficient = tCoefficient;
            currentTemperature = tMax;
            minTemperature = tMin;

            // wygenerowanie losowego wierzcholka startowego
            int x0 = randomGenerator.Next(0, numOfCities - 1);

            // wrzucenie cyklu do finalnej drogi od wierzcholka poczatkowego
            for (int i = x0; i < numOfCities; i++)
            {
                finalRoute.Push(i);
            }
            for (int i = 0; i < x0; i++)
            {
                finalRoute.Push(i);
            }
            //finalRoute.push_back(cities) = x0;

            tempRoute = finalRoute;
            BestCycleCost = GetPathLength(tempRoute);
            tempCost = BestCycleCost;

            while (currentTemperature > minTemperature)
            {
                SwitchRandomVertexes();
                tempCost = GetPathLength(tempRoute);

                if (tempCost < BestCycleCost)
                {
                    BestCycleCost = tempCost;
                    finalRoute = tempRoute;
                }
                else if (GenerateRandomProbability() < GenerateProbability())
                {
                    BestCycleCost = tempCost;
                    finalRoute = tempRoute;
                }
                GeometricTemperatureComputation();
                //ArithmeticTemperatureComputation();
            }
        }
    }
}
