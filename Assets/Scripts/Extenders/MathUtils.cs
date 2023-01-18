using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ozamanas.Extenders
{
    public static class MathUtils
    {

        public static float Map(this float value, float min1, float max1, float min2, float max2)
        {
            return min2 + (max2 - min2) * ((value - min1) / (max1 - min1));

        }//Close TryGetComponentInParent method

         public static Vector3 LerpByDistance(Vector3 A, Vector3 B, float x)
        {
            Vector3 P = x * Vector3.Normalize(B - A) + A;
            return P;
        }


    }//Closes MathUtils class
}//Closes namespace declaration
