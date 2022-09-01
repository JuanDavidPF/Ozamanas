using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JuanPayan.Utilities
{
    [System.Serializable]
    public class Ellipse
    {
        [SerializeField]
        [Range(0, 1)] private float xAxis;


        [SerializeField]
        [Range(0, 1)] private float yAxis;


        public Ellipse(float xAxis, float yAxis)
        {
            this.xAxis = xAxis;
            this.yAxis = yAxis;
        }//closes Ellipse Constructor

        public Vector2 Evaluate(float t)
        {
            float angle = Mathf.Deg2Rad * 360f * t;
            float x = Mathf.Sin(angle) * xAxis;
            float y = Mathf.Cos(angle) * yAxis;
            return new Vector3(x, -y);

        }//closes Evaluate method



    }//Closes Ellipse class
}//Closes namespace declaration
