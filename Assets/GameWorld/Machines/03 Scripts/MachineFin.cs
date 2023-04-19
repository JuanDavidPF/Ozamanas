using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ozamanas.Machines
{
    public class MachineFin : MonoBehaviour
    {
        private float yPos;
        private float xRot;
        private float zRot;


        void Start()
        {
            yPos = transform.position.y;
            xRot = transform.rotation.eulerAngles.x;
            zRot = transform.rotation.eulerAngles.z;
        }

        // Update is called once per frame
        void Update()
        {
            Quaternion myRotation = Quaternion.identity;
            myRotation.eulerAngles = new Vector3(xRot,transform.rotation.y,zRot);
            transform.localRotation = myRotation;
            transform.position = new Vector3(transform.position.x,yPos,transform.position.z);
        }
    }
}
