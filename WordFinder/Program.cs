using System;
using WordFinder;
using System.Collections.Generic;
using System.Linq;

namespace WordFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            var wordStream = new string[] { "CHILL", "WIND", "SNOW", "COLD", "COLD" };
            var matrix = new string[] { "ABCDCC", "FGWIOH", "CHILLI", "PQNSDL", "UVDXYL" };

            Console.WriteLine("\nMatrix of Characters\n");
            matrix.ToList().ForEach(i => Console.WriteLine("         " + i.ToString()));

            Console.WriteLine("\nWords to find\n");
            wordStream.ToList().ForEach(i => Console.WriteLine("         " + i.ToString()));

            Console.WriteLine("\nPress a Key for the Results\n");
            Console.ReadKey();

            try
            {
                var result = new WordFinder(matrix).Find(wordStream);
                foreach (var item in result)
                {
                    Console.WriteLine("         " + item.ToString());
                }
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("There a problem with the inputed data\n");
                Console.WriteLine(e.Message);
            }

            Console.ReadKey();
        }
    }
}
