using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ozamanas.Extenders
{
    public static class TransformExtender
    {

        public static void Clean(this Transform transform)
        {
            foreach (Transform child in transform)
            {
                MonoBehaviour.Destroy(child.gameObject);
            }
        }//Close ToVector method


    }//Closes TransformExtender class
}//Closes Namespace declaration