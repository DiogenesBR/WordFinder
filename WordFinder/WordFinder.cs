using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace WordFinder
{
    public class WordFinder
    {

        IEnumerable<string> _horizontalMatrix;
        IEnumerable<string> _verticalMatrix;

        public WordFinder(IEnumerable<string> matrix)
        {
            ValidateMatrix(matrix);
            _horizontalMatrix = matrix;
            _verticalMatrix = RotateMatrix(matrix);
        }

        public IEnumerable<string> Find(IEnumerable<string> wordstream)
        {
            ValidateWordStream(wordstream);

            List<(string Word, int MatchCount)> returnList = new List<(string Word, int MatchCount)>();

            Parallel.ForEach(wordstream.Distinct(), word =>
            {
                int horizontalCount = CountWordInList(_horizontalMatrix, word);
                int verticalCount = CountWordInList(_verticalMatrix, word);

                var matchCount = horizontalCount + verticalCount;

                if (matchCount > 0)
                {
                    lock (returnList)
                    {
                        returnList.Add((word, matchCount));
                    }
                }
            });

            return returnList.OrderByDescending(x => x.MatchCount)
                             .Take(10)
                             .Select(x => x.Word)
                             .ToList();
        }

        public int CountWordInList(IEnumerable<string> individualMatrix, string word)
        {
            return individualMatrix.AsParallel().Sum(line => CountWordInLine(line, word));
        }

        public int CountWordInLine(string line, string word)
        {
            return Regex.Matches(line, word).Count;
        }

        public IEnumerable<string> RotateMatrix(IEnumerable<string> matrix)
        {
            var numberOfCharacters = matrix.First().Length;

            string[] returnArray = new string[numberOfCharacters];


            for (int i = 0; i < numberOfCharacters; i++)
            {
                var builder = new StringBuilder();

                foreach (var line in matrix)
                {
                    builder.Append(line[i]);
                }

                returnArray[i] = builder.ToString();
            }

            return returnArray.ToList();
        }


        public static void ValidateMatrix(IEnumerable<string> matrix)
        {
            if (matrix == null)
            {
                throw new ArgumentNullException("Error: Matrix is null");
            }

            if (matrix.Count() == 0)
            {
                throw new ArgumentException("Error: Matrix don't have lines");
            }

            if (matrix.Count() > 64)
            {
                throw new ArgumentException("Error: Limit of lines excedded");
            }

            var firstLineLength = matrix.First().Length;
            //if (matrix.Count() != firstLineLength)
            //{
            //    throw new ArgumentException("The number of lines must be the same as the number of characters of a line");
            //}

            foreach (var matrixLine in matrix)
            {
                if (string.IsNullOrEmpty(matrixLine))
                {
                    throw new ArgumentException($"Line: \"{matrixLine}\"\nError: Line is null, empty");
                }

                if (matrixLine.Length > 64)
                {
                    throw new ArgumentException($"Line: \"{matrixLine}\"\nError: Number of characters excedded");
                }

                if (matrixLine.Length != firstLineLength)
                {
                    throw new ArgumentException($"Line: \"{matrixLine}\"\nError: All lines must contain the same number of characters.");
                }
            }
        }

        public static void ValidateWordStream(IEnumerable<string> wordstream)
        {
            if (wordstream == null)
            {
                throw new ArgumentNullException("Error: Wordstream is null");
            }

            if (wordstream.Count() == 0)
            {
                throw new ArgumentException("Error: Wordstream don't have words");
            }

            foreach (var word in wordstream)
            {
                if (string.IsNullOrEmpty(word))
                {
                    throw new ArgumentException($"Word: \"{word}\"\nError: Word is null, empty");
                }
            }
        }
    }
}
