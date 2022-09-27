using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JuanPayan.Utilities
{
    public partial class Parabol
    {
        public static Vector3 EvaluateParabole(Vector3 start, Vector3 end, float height, float t)
        {
            float Func(float x) => 4 * (-height * x * x + height * x);

            var mid = Vector3.Lerp(start, end, t);

            return new Vector3(mid.x, Func(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
        }

        public static Vector2 EvaluateParabole(Vector2 start, Vector2 end, float height, float t)
        {
            float Func(float x) => 4 * (-height * x * x + height * x);

            var mid = Vector2.Lerp(start, end, t);

            return new Vector2(mid.x, Func(t) + Mathf.Lerp(start.y, end.y, t));
        }
    }//Closes Parabol class
}//Closes namespace declaration