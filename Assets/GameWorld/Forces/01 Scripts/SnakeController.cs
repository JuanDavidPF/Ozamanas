using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Ozamanas.Machines;
using Ozamanas.Tags;

namespace Ozamanas.Forces
{
    public class SnakeController : MonoBehaviour
    {
        [SerializeField] private Animator snakeAnimator;
        private Transform _T;
        [SerializeField] private Collider mawTrigger;
        [SerializeField] private Transform maw;
        [SerializeField] private float snapDuration;
        [SerializeField] private Machines.MachineTrait trait;

        private Tweener tweener;

        private Transform bittedMachine;

        private Vector3 initPosition;

        private EmbraceReptile embraceReptile;


        private void Awake()
        {
            _T = transform;

            embraceReptile = GetComponentInParent<EmbraceReptile>();

        }

        public void Bite(Transform machine)
        {
            mawTrigger.enabled = true;
            bittedMachine = machine;
            initPosition = _T.position;

            tweener = _T.DOMove(machine.position, 0.3f).From(_T.position)
            .SetUpdate(UpdateType.Late)
            .OnUpdate(() =>
            {
                tweener.ChangeEndValue(machine.position, true);
            });

           
           

        }//Closes Bite method

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Machine") return;

            embraceReptile.OnBiteMachine(other.transform);
            mawTrigger.enabled = false;
            MachinePhysicsManager physics = other.GetComponentInParent<MachinePhysicsManager>();
            physics.SetKinematic();
            tweener.Kill();

            if(physics.machine.Machine_token.machineHierarchy != MachineHierarchy.Boss)
            {
                bittedMachine = physics.transform;
                bittedMachine.SetParent(maw, true);
                bittedMachine.localRotation = Quaternion.Euler(0, 0, 0);
                bittedMachine.localPosition = Vector3.zero;
                Vector3 machineOriginalScale = bittedMachine.localScale;
                if (snakeAnimator) snakeAnimator.SetTrigger("Thread");
            }
            else
            {   
                
                tweener = _T.DOMove(initPosition, 0.7f).From(_T.position);
                tweener.OnComplete(() =>
                {
                    embraceReptile.transform.DOLookAt(other.transform.position,0.1f);
                    embraceReptile.ActivateOnDragAnimation();
                });
                
                
            }

            

        }
        private void OnDisable()
        {
            tweener.Kill();
        }//Closes OnDisable method


    }
}
