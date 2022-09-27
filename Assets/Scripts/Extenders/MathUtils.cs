using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ozamanas.Entenders
{
    public static class MathUtils
    {

        public static float Map(this float value, float min1, float max1, float min2, float max2)
        {
            return min2 + (max2 - min2) * ((value - min1) / (max1 - min1));

        }//Close TryGetComponentInParent method


    }//Closes MathUtils class
}//Closes namespace declaration
