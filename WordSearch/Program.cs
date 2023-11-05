using System;
using System.Collections.Generic;

namespace WordSearch
{
    class Program
    {
        static char[,] Grid = new char[,] {
            {'C', 'P', 'K', 'X', 'O', 'I', 'G', 'H', 'S', 'F', 'C', 'H'},
            {'Y', 'G', 'W', 'R', 'I', 'A', 'H', 'C', 'Q', 'R', 'X', 'K'},
            {'M', 'A', 'X', 'I', 'M', 'I', 'Z', 'A', 'T', 'I', 'O', 'N'},
            {'E', 'T', 'W', 'Z', 'N', 'L', 'W', 'G', 'E', 'D', 'Y', 'W'},
            {'M', 'C', 'L', 'E', 'L', 'D', 'N', 'V', 'L', 'G', 'P', 'T'},
            {'O', 'J', 'A', 'A', 'V', 'I', 'O', 'T', 'E', 'E', 'P', 'X'},
            {'C', 'D', 'B', 'P', 'H', 'I', 'A', 'W', 'V', 'X', 'U', 'I'},
            {'L', 'G', 'O', 'S', 'S', 'B', 'R', 'Q', 'I', 'A', 'P', 'K'},
            {'E', 'O', 'I', 'G', 'L', 'P', 'S', 'D', 'S', 'F', 'W', 'P'},
            {'W', 'F', 'K', 'E', 'G', 'O', 'L', 'F', 'I', 'F', 'R', 'S'},
            {'O', 'T', 'R', 'U', 'O', 'C', 'D', 'O', 'O', 'F', 'T', 'P'},
            {'C', 'A', 'R', 'P', 'E', 'T', 'R', 'W', 'N', 'G', 'V', 'Z'}
        };

        static string[] Words = new string[]
        {
            "CARPET",
            "CHAIR",
            "DOG",
            "BALL",
            "DRIVEWAY",
            "FISHING",
            "FOODCOURT",
            "FRIDGE",
            "GOLF",
            "MAXIMIZATION",
            "PUPPY",
            "SPACE",
            "TABLE",
            "TELEVISION",
            "WELCOME",
            "WINDOW"
        };

        static void Main(string[] args)
        {
            Console.WriteLine("Word Search");


            for (int y = 0; y < 12; y++)
            {
                for (int x = 0; x < 12; x++)
                {
                    Console.Write(Grid[y, x]);
                    Console.Write(' ');
                }
                Console.WriteLine("");

            }



            Console.WriteLine("");
            Console.WriteLine("Found Words");
            Console.WriteLine("------------------------------");

            FindWords();

            Console.WriteLine("------------------------------");
            Console.WriteLine("");
            Console.WriteLine("Press any key to end");
            Console.ReadKey();
        }

        private static void FindWords()
        {
            //Find each of the words in the grid, outputting the start and end location of
            //each word, e.g.
            //PUPPY found at (10,7) to (10, 3) 

            foreach (string word in Words)
            {
                Console.WriteLine("");
                FindWord(word);
            }

            Console.WriteLine("");
        }

        //Assuming the Grid will always be square.
        private static void FindWord(string wordToFind)
        {
            int length = Grid.GetLength(0);

            //This could be optimized further.
            for (int rOperation = 0; rOperation < 3; rOperation++)
            {
                for (int cOperation = 0; cOperation < 3; cOperation++)
                {
                    for (int rStart = 0; rStart < length; rStart++)
                    {
                        for (int cStart = 0; cStart < length; cStart++)
                        {
                            if (rStart == 0 || rStart == length - 1 || cStart == 0 || cStart == length - 1)
                            {
                                var items = Walker(rStart, cStart, length, rOperation, cOperation);
                                var isFound = Find(items, wordToFind);
                                if (isFound)
                                {
                                    return;
                                }
                            }
                        }
                    }
                }
            }

            Console.WriteLine($"{wordToFind} Could not be found!");
        }

        private static bool Find(List<(int, int)> items, string wordToFind)
        {
            if (items.Count < 2)
                return false;

            for (int i = 0; i < items.Count; i++)
            {
                var loc = items[i];
                var cursor = Grid[loc.Item1, loc.Item2];

                //if the first character match, try to match the other ones.
                if (cursor == wordToFind[0])
                {
                    int j = 1;
                    for (; j < wordToFind.Length; j++)
                    {
                        if (i + j >= items.Count)
                            break;

                        var x = Grid[items[i + j].Item1, items[i + j].Item2];
                        if (wordToFind[j] != x)
                            break;
                    }
                    if (j == wordToFind.Length)
                    {
                        var wordLocStart = items[i];
                        var wordLocEnd = items[i + (wordToFind.Length - 1)];
                        Console.Write($"{wordToFind} Found at {wordLocStart} to {wordLocEnd}");
                        return true;
                    }
                }
            }

            return false;
        }

        //Change = 0=Up, 1=Down, 2=Same
        public static int Change(int previous, int operation)
        {
            if (operation == 0)
                return previous + 1;

            if (operation == 1)
                return previous - 1;

            if (operation == 2)
                return previous;

            throw new Exception("Not expected operation");
        }


        public static List<(int, int)> Walker(int rstart,
                                       int cstart,
                                       int length,
                                       int rChange,
                                       int cChange)
        {
            var list = new List<(int, int)> { };
            do
            {
                list.Add((rstart, cstart));
                rstart = Change(rstart, rChange);
                cstart = Change(cstart, cChange);
            }
            //Make sure the row and column don't hit the boundaries of the grid.
            while (rstart < length && cstart < length && rstart >= 0 && cstart >= 0);

            return list;
        }
    }
}
