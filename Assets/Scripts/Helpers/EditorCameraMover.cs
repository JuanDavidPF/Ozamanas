using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace JuanPayan.Helpers
{
    public class EditorCameraMover : MonoBehaviour
    {
        public CursorLockMode lockMode;
        public float lookSpeedH = 2f;
        public float lookSpeedV = 2f;
        public float zoomSpeed = 2f;
        public float dragSpeed = 6f;

        private float yaw = 0f;
        private float pitch = 0f;

        void Update()
        {
            Cursor.lockState = lockMode;
            //Look around with Right Mouse
            if (Mouse.current.rightButton.isPressed)
            {
                yaw += lookSpeedH * Mouse.current.delta.x.ReadValue() * Time.deltaTime;
                pitch -= lookSpeedV * Mouse.current.delta.y.ReadValue() * Time.deltaTime;

                transform.eulerAngles = new Vector3(pitch, yaw, 0f);
            }

            //drag camera around with Middle Mouse
            if (Mouse.current.middleButton.isPressed)
            {
                transform.Translate(-Mouse.current.delta.x.ReadValue() * Time.deltaTime * dragSpeed, -Mouse.current.delta.y.ReadValue() * Time.deltaTime * dragSpeed, 0);
            }

            //Zoom in and out with Mouse Wheel
            transform.Translate(0, 0, Mouse.current.scroll.y.ReadValue() * zoomSpeed * Time.deltaTime, Space.Self);
        }
    }//closes EditorCameraMover class
}//Closes namespace declaration
