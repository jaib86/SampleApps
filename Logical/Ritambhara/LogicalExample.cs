using System;
using System.Collections.Generic;
using System.Text;

namespace Ritambhara
{
    public static class LogicalExample
    {
        /// <summary>
        /// Fibonacci numbers: 0, 1, 1, 2, 3, 5, 8, 13, 21, 34...
        /// </summary>
        /// <param name="nthNumber"></param>
        /// <returns></returns>
        public static int GetNthFibonacciNumber(int nthNumber)
        {
            if (nthNumber < 1)
                throw new ArgumentException("Must be positive natural number.", nameof(nthNumber));

            int first = 0, second = 1;

            if (nthNumber == 1)
                return first;

            if (nthNumber == 2 || nthNumber == 3)
                return second;

            while (nthNumber-- > 2)
            {
                second += first;
                first = second - first;
            }

            return second;
        }

        public static void Sort_NxM_Matrix(int[,] data)
        {
            int[] linearArray = new int[data.GetLength(0) * data.GetLength(1)];
            int k = 0;

            // Initialize Linear Array
            for (int i = 0; i < data.GetLength(0); i++)
            {
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    linearArray[k++] = data[i, j];
                }
            }

            Array.Sort(linearArray);

            k = 0;

            // Initialize sorted items back to matrix
            for (int i = 0; i < data.GetLength(0); i++)
            {
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    data[i, j] = linearArray[k++];
                }
            }
        }

        public static int ConvertBinaryToDecimal(string binaryString)
        {
            int result = 0;

            foreach (var ch in binaryString)
            {
                if (ch == '0')
                    result *= 2;
                else if (ch == '1')
                    result = (result * 2) + 1;
                else
                    throw new ArgumentException("Input string is not a valid binary number.", nameof(binaryString));
            }

            return result;
        }

        /// <summary>
        /// This will return result like below example 
        /// if array is [1, 3, 2, 4] result should be 1324
        /// if array is [2, 4, 5, 7, 1] result should be 24571
        /// </summary>
        /// <param name="numberArray"></param>
        /// <returns></returns>
        public static int GetNumberFromSingleDigitArray(int[] singleDigitArray)
        {
            int result = 0;

            foreach (var digit in singleDigitArray)
            {
                result = (result * 10) + digit;
            }

            return result;
        }
    }
}
