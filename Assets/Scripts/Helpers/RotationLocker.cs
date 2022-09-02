using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JuanPayan.Utilities
{
    public class RotationLocker : MonoBehaviour
    {
        private Transform t;
        private Quaternion lockRotation;
        private void OnEnable()
        {
            t = transform;
            lockRotation = t.rotation;
        }//Closes Awake method

        private void LateUpdate()
        {
            t.rotation = lockRotation;
        }//Closes LateUpdate method

    }//Closes RotationLocker class
}//Closes Namespace declaration
