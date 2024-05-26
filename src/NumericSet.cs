using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace NumericSetCalculator
{
    public class NumericSet
    {
        public ObservableCollection<double> Set { get; set; }

        public NumericSet() { Set = new ObservableCollection<double>(); }

        public NumericSet(double[] array)
        {
            Set = new ObservableCollection<double>(array);
        }

        public override string ToString()
        {
            string result = "{ ";
            foreach (double item in Set) result += $"{item}, ";
            result += "}";
            return result;
        }

        #region Default operations
        public void Add(double number) => Set.Add(number);
        public void Remove(double number) => Set.Remove(number);
        public void Update(double number, int index)
        {
            Set.RemoveAt(index);
            Set.Insert(index, number);
        }
        public void Clear() => Set.Clear();
        #endregion

        #region Properties of a number set.
        public double GetArifmeticMean() => GetSum() / Set.Count;

        public double GetSpan() => Set.Max() - Set.Min();

        public double GetMedian()
        {
            double[] array = GetOrderByAscending().ToArray();

            int middleIndex = array.Length / 2;

            // Find Median 
            if (array.Length % 2 == 0) return (array[middleIndex - 1] + array[middleIndex]) / 2;
            else return array[middleIndex];
        }

        public double GetSum() => Set.Sum();

        public double GetDispersion()
        {
            double arifmeticMean = GetArifmeticMean();
            List<double> squaresOfDeviation = new List<double>();

            foreach (double n in Set)
            {
                squaresOfDeviation.Add(Math.Pow(n - arifmeticMean, 2));
            }
            return squaresOfDeviation.Sum() / Set.Count;
        }

        // Sorted in descending
        public IOrderedEnumerable<double> GetOrderByDescending() => Set.OrderByDescending(n => n);

        // Sorting by ascending
        public IOrderedEnumerable<double> GetOrderByAscending() => Set.OrderBy(n => n);
        #endregion
    }
}