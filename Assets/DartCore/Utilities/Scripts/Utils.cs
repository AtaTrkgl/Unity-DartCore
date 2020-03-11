using System.Collections.Generic;
using UnityEngine;

namespace DartCore.Utilities
{
    public class Utils
    {
        #region Average
        /// <summary>
        /// Returns the average of the elements of given array/list
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns></returns>
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
        /// Returns the average of the elements of given array/list
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns></returns>
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
        /// Returns the average of the elements of given array/list
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns></returns>
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
        /// Returns the average of the elements of given array/list
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns></returns>
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
    }
}
