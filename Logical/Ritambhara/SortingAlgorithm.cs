using System;
using System.Collections.Generic;
using System.Text;

namespace Ritambhara
{
    public static class SortingAlgorithm
    {
        public static int[] BubbleSort(params int[] numbers)
        {
            int temp;

            for (int i = 0; i < numbers.Length; i++)
            {
                for (int j = 0; j < numbers.Length - 1 - i; j++)
                {
                    if (numbers[j] > numbers[j + 1])
                    {
                        // Swap large number to right
                        temp = numbers[j];
                        numbers[j] = numbers[j + 1];
                        numbers[j + 1] = temp;
                    }
                }
            }

            return numbers;
        }

        public static int[] SelectionSort(params int[] numbers)
        {
            int maxNumIndex = 0;

            for (int k = 0; k < numbers.Length - 1; k++)
            {
                for (int i = 1; i < numbers.Length - k; i++)
                {
                    if (numbers[i] > numbers[maxNumIndex])
                    {
                        maxNumIndex = i;
                    }
                }

                // swap number
                int maxNumber = numbers[maxNumIndex];
                numbers[maxNumIndex] = numbers[numbers.Length - 1 - k];
                numbers[numbers.Length - 1 - k] = maxNumber;
            }

            return numbers;
        }
    }
}
