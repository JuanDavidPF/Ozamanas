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

        public static void SetLayer(this GameObject go, int layerNumber)
        {
            if (go == null) return;
            foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
            {
                trans.gameObject.layer = layerNumber;
            }
        }

        public static bool TryGetComponentInParent<T>(this Component transform, out T component)
        {
            component = transform.GetComponentInParent<T>();
            return component != null;
        }//Close TryGetComponentInParent method


        public static bool TryGetComponentInChildren<T>(this Component transform, out T component)
        {
            component = transform.GetComponentInChildren<T>();
            return component != null;
        }//Close TryGetComponentInChildren method

    }//Closes TransformExtender class
}//Closes Namespace declaration