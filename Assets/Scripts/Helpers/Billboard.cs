using System.Collections;
using System.Collections.Generic;
using JuanPayan.Helpers;
using UnityEngine;

namespace JuanPayan.Utilities
{
    public class Billboard : MonoBehaviour
    {
        private GameObject _go;
        private Transform _t;

        [SerializeField] private string cameraKey;
        private Transform cameraTransform;


        private void Awake()
        {
            _go = gameObject;
            _t = _go.transform;
        }//Closes a method

        private void Start()
        {
            Camera camera = CameraAtlas.Get(cameraKey);
            if (camera) cameraTransform = camera.transform;

        }//Closes Start method

        private void LateUpdate()
        {
            if (!cameraTransform) return;
            _t.rotation = cameraTransform.transform.rotation;

        }//Closes LateUpdate method

    }//Closes Billboard class
}//Closes namespace declaration
