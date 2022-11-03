using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JuanPayan.Helpers
{
    [RequireComponent(typeof(Canvas))]
    public class CanvasCameraAssigner : MonoBehaviour
    {
        [SerializeField] private string cameraKey;
        private void Start()
        {
            if (TryGetComponent(out Canvas canvas))
            {
                canvas.worldCamera = CameraAtlas.Get(cameraKey);
            }
        }//Closes Start method

    }//Closes CanvasCameraAssigner class
}//Closes namespace declaration
