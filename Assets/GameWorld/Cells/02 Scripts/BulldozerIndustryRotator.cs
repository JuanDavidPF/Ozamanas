using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using Ozamanas.World;
using Sirenix.OdinInspector;


namespace Ozamanas.Board
{
    [RequireComponent(typeof(Rigidbody))]
    public class BulldozerIndustryRotator : MonoBehaviour
    {
       [Title("Rotator Setup:")]
        [SerializeField] Transform meshToRotate;

        [SerializeField] List<Transform> industryBlinds;

        [SerializeField] private bool alwaysRotate = false;

         [SerializeField] private float rotationSpeed = 20f;

        private bool m_OpenGate = false;
        public bool OpenGate{

            get { return m_OpenGate; }
            set
            {
                if(m_OpenGate == value) return;

                m_OpenGate = value;

                if(m_OpenGate) OpenIndustryGates();
                else CloseIndustryGates();
            }
        }

        List<Tween> tweeners = new List<Tween>();

        Tween rotateTween;

        [Space(15)]
        [Header("Events")]
        public UnityEvent OnIndustryTurnOff;
        public UnityEvent OnIndustryTurnOn;

        private void Start()
        {
            if(!alwaysRotate) return;

            rotateTween = meshToRotate.DORotate(new Vector3(0,360,0),rotationSpeed,RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Restart)
            .SetRelative()
            .SetEase(Ease.Linear);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Machine") return;

            rotateTween.Kill();

            OpenGate = true;
           
        }

        private void OpenIndustryGates()
        {
            StopTweeners();
            foreach (var blind in industryBlinds)
            {
                tweeners.Add(blind.DOScaleY(0, .5f));
            }
        }

        private void CloseIndustryGates()
        {
            StopTweeners();
            foreach (var blind in industryBlinds)
            {
                tweeners.Add(blind.DOScaleY(1, 1f).SetDelay(2f));
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag != "Machine") return;

             OpenGate = false;

             if(!alwaysRotate) return;

            rotateTween = meshToRotate.DORotate(new Vector3(0,360,0),rotationSpeed,RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Restart)
            .SetRelative()
            .SetEase(Ease.Linear);
            

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
            Quaternion rotation = lookPos == Vector3.zero
                                  ? Quaternion.identity
                                  : Quaternion.LookRotation(lookPos);
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