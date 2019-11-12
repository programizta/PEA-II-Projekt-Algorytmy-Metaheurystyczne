using System;
using System.Diagnostics;

namespace II_Projekt
{
    class Program
    {
        static string filename;
        static int choice;

        static void Tests()
        {
            int repeats;
            int choiceNumber;
            Console.Clear();
            Console.Write("Ile powtórzeń chciałbyś wykonać? Podaj liczbę: ");
            repeats = int.Parse(Console.ReadLine());
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\nMożliwości do wyboru:\n");
                Console.WriteLine("1. Symulowane wyżarzanie");
                Console.WriteLine("2. Powrót do głównego menu\n");
                Console.Write("Którą opcję wybierasz? Wprowadź jej numer: ");
                choiceNumber = int.Parse(Console.ReadLine());

                switch (choiceNumber)
                {
                    case 1:
                        {
                            Stopwatch stopWatch = new Stopwatch();
                            stopWatch.Start();
                            for (int i = 0; i < repeats; i++)
                            {
                                //BruteForce bf = new BruteForce(filename, choice);
                                //bf.StartBruteForce(0);
                            }
                            stopWatch.Stop();
                            TimeSpan ts = stopWatch.Elapsed;
                            Console.WriteLine("\nŚredni czas wykonania algorytm symulowanego wyżarzania wynosi " + ts.TotalMilliseconds / repeats + " ms");
                            Console.ReadKey();
                            break;
                        }
                    case 2: return;
                    default:
                        {
                            Console.Clear();
                            Console.WriteLine("Taka opcja nie istnieje!");
                            Console.ReadKey();
                            break;
                        }
                }
            }
        }

        static void Main(string[] args)
        {
            Graph g = new Graph();
            string filename;
            int numOfChoice;

            while (true)
            {
                Console.Clear();
                Console.Write("Program do wyznaczania szacowanego, optymalnego cyklu Hamiltona dla asymetrycznego problemu komiwojażera (ATSP)");

                if (g.GetNumberOfCities() != 0)
                {
                    Console.WriteLine("\n\nLiczba wierzchołków aktualnie wczytanego grafu: " + g.GetNumberOfCities());
                    Console.Write(Environment.NewLine);
                }
                else
                {
                    Console.WriteLine("\n\nAktualnie nie wczytano żadnego grafu");
                    Console.Write(Environment.NewLine);
                }

                Console.WriteLine("Możliwości do wyboru: ");
                Console.WriteLine("1. Wczytaj małą macierz grafu");
                Console.WriteLine("2. Wczytaj dużą macierz grafu");
                Console.WriteLine("3. Wyświetl macierz kosztów");
                Console.WriteLine("4. Rozwiąż problem komiwojażera za pomocą metody symulowanego wyżarzania");
                Console.WriteLine("5. Przeprowadź testy seryjne");
                Console.WriteLine("6. Zakończ działanie programu\n");
                Console.Write("Którą opcję chcesz wybrać? Podaj numer: ");
                numOfChoice = int.Parse(Console.ReadLine());

                switch (numOfChoice)
                {
                    case 1:
                        {
                            choice = 0;
                            Console.Clear();
                            Console.Write("Podaj nazwę pliku z małym grafem: ");
                            filename = Console.ReadLine();
                            g = new Graph(filename, choice);
                            Console.Write("Wczytano graf z " + g.GetNumberOfCities() + " wierzchołkami\nAby kontynuować kliknij [ENTER]");
                            Program.filename = filename;
                            Console.ReadKey();
                            break;
                        }
                    case 2:
                        {
                            choice = 1;
                            Console.Clear();
                            Console.Write("Podaj nazwę pliku z dużym grafem: ");
                            filename = Console.ReadLine();
                            g = new Graph(filename, choice);
                            Console.Write("Wczytano graf z " + g.GetNumberOfCities() + " wierzchołkami\nAby kontynuować kliknij [ENTER]");
                            Program.filename = filename;
                            Console.ReadKey();
                            break;
                        }
                    case 3:
                        {
                            Console.Clear();
                            if (g.GetNumberOfCities() != 0) g.DisplayCostMatrix();
                            else Console.WriteLine("Nie wczytano żadnego grafu do programu!");
                            Console.Write("\nAby kontynuować kliknij [ENTER]");
                            Console.ReadKey();
                            break;
                        }
                    case 4:
                        {
                            double t0, tMin, tCoefficient;
                            Console.Clear();
                            Console.Write("Podaj temperaturę początkową wyżarzania: ");
                            t0 = double.Parse(Console.ReadLine());
                            Console.Write("Podaj minimalną temperaturę wyżarzania: ");
                            tMin = double.Parse(Console.ReadLine());
                            Console.Write("Podaj współczynnik wyżarzania z zakresu [0; 1): ");
                            tCoefficient = double.Parse(Console.ReadLine());
                            SimulatedAnnealing sa = new SimulatedAnnealing(g.Filename, choice);
                            sa.StartSA(t0, tMin, tCoefficient);
                            Console.WriteLine("Najlepszy, oszacowany cykl ma wagę: " + sa.BestCycleCost);
                            sa.Route.Display();
                            Console.WriteLine("\nKoniec. Aby wrócić do głównego menu, kliknij dowolny klawisz...");
                            Console.ReadKey();
                            break;
                        }
                    case 5:
                        {
                            Tests();
                            break;
                        }
                    case 6:
                        {
                            Console.Write("\nZakończono działanie programu\nAby kontynuować kliknij [ENTER]");
                            Console.ReadKey();
                            return;
                        }
                }
            }
        }
    }
}
