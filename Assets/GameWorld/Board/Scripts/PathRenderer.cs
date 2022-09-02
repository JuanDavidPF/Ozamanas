using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ozamanas
{

    [RequireComponent(typeof(TrailRenderer))]
    public class PathRenderer : MonoBehaviour
    {

        private TrailRenderer trail;
        private void Awake()
        {
            trail = GetComponent<TrailRenderer>();


        }//Closes Awake method

    }//Closes PathRenderer class
}//Closes Namespace declaration
