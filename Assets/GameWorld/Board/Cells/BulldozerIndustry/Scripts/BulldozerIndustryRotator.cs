using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


namespace Ozamanas.Board
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]

    public class BulldozerIndustryRotator : MonoBehaviour
    {
        [SerializeField] Transform meshToRotate;

        private void OnTriggerStay(Collider other)
        {
            if (other.tag == "Machine") RotateAtMachine(other.transform);

        }//Closes OnTriggerStay method

        private void RotateAtMachine(Transform machineTransform)
        {

            if (!meshToRotate) return;

            var lookPos = machineTransform.position - meshToRotate.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            meshToRotate.rotation = Quaternion.Slerp(meshToRotate.rotation, rotation, Time.deltaTime * 2f);
        }//Closes RotateAtMachine method

    }//Closes BulldozerIndustryRotator
}//Closes namespace declaration