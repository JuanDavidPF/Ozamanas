using System.Collections;
using System.Collections.Generic;
using JuanPayan.Helpers;
using UnityEngine;

namespace JuanPayan.Utilities
{

    public class AxisFightSolver : MonobehaviourEvents
    {


        [System.Flags]
        public enum Axis
        {
            X = 1 << 0,
            Y = 1 << 1,
            Z = 1 << 2
        }
        public Axis axis;
        public Vector2 offsetRange;

        public override void Behaviour()
        {
            Vector3 newPosition = transform.localPosition;

            if (axis.HasFlag(Axis.X)) newPosition.x += Random.Range(offsetRange.x, offsetRange.y);
            if (axis.HasFlag(Axis.Y)) newPosition.y += Random.Range(offsetRange.x, offsetRange.y);
            if (axis.HasFlag(Axis.Z)) newPosition.z += Random.Range(offsetRange.x, offsetRange.y);


            transform.localPosition = newPosition;
        }//Closes Behaviour class

    }//Closes ZFightSolver class
}//Closes NameSpace declaration
