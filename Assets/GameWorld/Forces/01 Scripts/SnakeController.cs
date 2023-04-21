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

        private void Awake()
        {
            _T = transform;
        }

        public void Bite(Transform machine)
        {
            mawTrigger.enabled = true;
            bittedMachine = machine;

            Vector3 bitePosition = machine.position;

             if(machine.GetComponent<HumanMachine>().Machine_token.machineHierarchy == MachineHierarchy.Boss) bitePosition.y += 1 ;



            tweener = _T.DOMove(bitePosition, .3f).From(_T.position)
            .SetUpdate(UpdateType.Late)
            .OnUpdate(() =>
            {
                bitePosition = machine.position;
                if(machine.GetComponent<HumanMachine>().Machine_token.machineHierarchy == MachineHierarchy.Boss) bitePosition.y += 1 ;

                tweener.ChangeEndValue(bitePosition, true);
            });

        }//Closes Bite method


        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Machine") return;
            
            mawTrigger.enabled = false;
            tweener.Kill();
            MachinePhysicsManager physics = other.GetComponentInParent<MachinePhysicsManager>();
            physics.SetKinematic();

            if(physics.machine.Machine_token.machineHierarchy == MachineHierarchy.Boss) return;

            bittedMachine = physics.transform;


            bittedMachine.SetParent(maw, true);
            bittedMachine.localRotation = Quaternion.Euler(0, 0, 0);
            bittedMachine.localPosition = Vector3.zero;

            Vector3 machineOriginalScale = bittedMachine.localScale;

            

            if (snakeAnimator) snakeAnimator.SetTrigger("Thread");


        }
        private void OnDisable()
        {
            tweener.Kill();
        }//Closes OnDisable method


    }
}
