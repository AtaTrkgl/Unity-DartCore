using System.Collections.Generic;
using UnityEngine;

namespace DartCore.Utilities
{
    public class Mathd
    {
        #region Average

        /// <summary>
        /// Returns the arithmetic mean of the elements of given array/list
        /// </summary>
        public static float Average(int[] numbers)
        {
            var average = 0f;
            foreach (var number in numbers)
                average += number;

            if (numbers.Length != 0)
                average /= numbers.Length;
            return average;
        }
        /// <summary>
        /// Returns the arithmetic mean of the elements of given array/list
        /// </summary>
        public static float Average(float[] numbers)
        {
            var average = 0f;
            foreach (var number in numbers)
                average += number;

            if (numbers.Length != 0)
                average /= numbers.Length;
            return average;
        }
        /// <summary>
        /// Returns the arithmetic mean of the elements of given array/list
        /// </summary>
        public static float Average(List<int> numbers)
        {
            var average = 0f;
            foreach (var number in numbers)
                average += number;

            if (numbers.Count != 0)
                average /= numbers.Count;
            return average;
        }
        /// <summary>
        /// Returns the arithmetic mean of the elements of given array/list
        /// </summary>
        public static float Average(List<float> numbers)
        {
            var average = 0f;
            foreach (var number in numbers)
                average += number;

            if (numbers.Count != 0)
                average /= numbers.Count;
            return average;
        }
        #endregion

        #region UnitCirclePosition

        /// <summary>
        /// returns a position as Vector2(rcosθ, rsinθ)
        /// </summary>
        /// <param name="θ">angle, in radians</param>
        /// <param name="r">radius</param>
        /// <returns></returns>
        public static Vector2 UnitCirclePosRadians(float θ, float r = 1f)
        { 
            var rawPos =  new Vector2(Mathf.Cos(θ), Mathf.Sin(θ));
            return rawPos * r;
        }
        /// <summary>
        /// returns a position as Vector2(rcosθ, rsinθ)
        /// </summary>
        /// <param name="θ">angle, in degrees</param>
        /// <param name="r">radius</param>
        /// <returns></returns>
        public static Vector2 UnitCirclePosDegrees(float θ, float r = 1f)
        {
            θ *= Mathf.Deg2Rad;
            var rawPos = new Vector2(Mathf.Cos(θ), Mathf.Sin(θ));
            return rawPos * r;
        }
        #endregion

        #region Random
        public static bool RandomChance(float numerator, float denominator = 100f)
        {
            float randNum = Random.Range(0, denominator);
            if (randNum <= numerator)
                return true;
            else
                return false;
        }
        public static Vector2 RandomVector2()
        {
            return new Vector2(Random.Range(0,1), Random.Range(0, 1)).normalized;
        }
        public static Vector3 RandomVector3()
        {
            return new Vector3(Random.Range(0, 1), Random.Range(0, 1), Random.Range(0, 1)).normalized;
        }
        public static Vector4 RandomVector4()
        {
            return new Vector4(Random.Range(0, 1), Random.Range(0, 1), Random.Range(0, 1), Random.Range(0, 1)).normalized;
        }

        public static float[] RandomFloatArray(int length, float minVal, float maxVal)
        {
            float[] array = new float[length];
            for (int i = 0; i < length; i++)
                array[i] = Random.Range(minVal, maxVal);

            return array;
        }
        public static List<float> RandomFloatList(int length, float minVal, float maxVal)
        {
            List<float> list = new List<float>();
            for (int i = 0; i < length; i++)
                list.Add(Random.Range(minVal, maxVal));

            return list;
        }
        public static int[] RandomIntArray(int length, int minVal, int maxVal)
        {
            int[] array = new int[length];
            for (int i = 0; i < length; i++)
                array[i] = Random.Range(minVal, maxVal);

            return array;
        }
        public static List<int> RandomIntList(int length, int minVal, int maxVal)
        {
            List<int> list = new List<int>();
            for (int i = 0; i < length; i++)
                list.Add(Random.Range(minVal, maxVal));

            return list;
        }
        #endregion

        #region Range

        /// <summary>
        /// Order of the bounds does not matter. However
        /// if both of them are equal to each other,
        /// the function will return true if the number
        ///  is equal to the bounds
        /// </summary>
        public static bool IsInRange(float num, float bound1, float bound2)
        {
            if (bound1 > bound2)
                return bound1 > num && num > bound2;
            else if (bound2 > bound1)
                return bound2 > num && num > bound1;

            // if the boundry is a number instead
            // of a range, check if the number is
            // equal to it
            else if (num == bound1)
                return true;
            else
                return false;

        }

        #endregion

        #region sum

        public static float SumList(List<int> listToSum)
        {
            float sum = 0;
            if (listToSum == null)
                return sum;

            foreach (var item in listToSum)
                sum += item;
            return sum;
        }
        public static float SumList(List<float> listToSum)
        {
            float sum = 0;
            if (listToSum == null)
                return sum;

            foreach (var item in listToSum)
                sum += item;
            return sum;
        }
		public static float SumArray(int[] arrayToSum)
        {
            float sum = 0;
            if (arrayToSum == null)
                return sum;

            foreach (var item in arrayToSum)
                sum += item;
            return sum;
        }
        public static float SumArray(float[] arrayToSum)
        {
            float sum = 0;
            if (arrayToSum == null)
                return sum;

            foreach (var item in arrayToSum)
                sum += item;
            return sum;
        }

        #endregion

    }
}
