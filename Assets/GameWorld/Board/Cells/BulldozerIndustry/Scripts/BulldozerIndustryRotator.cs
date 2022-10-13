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

        [SerializeField] List<Transform> industryBlinds;
        private Animator animator;


        List<Tween> tweeners = new List<Tween>();

        void Awake()
        {
            animator = GetComponent<Animator>();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Machine") return;
            StopTweeners();
            foreach (var blind in industryBlinds)
            {
                tweeners.Add(blind.DOScaleY(0, .5f));
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag != "Machine") return;
            StopTweeners();
            foreach (var blind in industryBlinds)
            {
                tweeners.Add(blind.DOScaleY(1, .5f).SetDelay(1f));
            }

        }
        private void OnTriggerStay(Collider other)
        {
            if (other.tag != "Machine") return;
            RotateAtMachine(other.transform);

        }//Closes OnTriggerStay method

        private void RotateAtMachine(Transform machineTransform)
        {

            if (!meshToRotate) return;

            var lookPos = machineTransform.position - meshToRotate.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            meshToRotate.rotation = Quaternion.Slerp(meshToRotate.rotation, rotation, Time.deltaTime * 2f);
        }//Closes RotateAtMachine method


        private void StopTweeners()
        {
            foreach (var tween in tweeners)
            {
                tween.Kill();
            }
            tweeners.Clear();
        }
    }//Closes BulldozerIndustryRotator
}//Closes namespace declaration