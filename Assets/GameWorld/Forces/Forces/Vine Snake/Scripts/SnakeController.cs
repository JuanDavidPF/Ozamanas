using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Ozamanas.Machines;

namespace Ozamanas.Forces
{
    public class SnakeController : MonoBehaviour
    {
        [SerializeField] private Collider mawTrigger;
        [SerializeField] private Transform maw;
        [SerializeField] private float movementSpeed;
        [SerializeField] private Machines.MachineTrait trait;

        private Tweener tweener;

        private Transform bittedMachine;


        public void Bite(Transform machine)
        {
            mawTrigger.enabled = true;

            tweener = transform.DOMove(machine.position, movementSpeed)
            .SetSpeedBased(true).From(new Vector3(transform.position.x, 0, transform.position.z))
            .OnUpdate(() =>
            {
                tweener.ChangeEndValue(machine.position, true);
            });

        }//Closes Bite method

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Machine") return;

            tweener.Kill();
            MachinePhysicsManager physics = other.GetComponentInParent<MachinePhysicsManager>();
            physics.SetKinematic();

            bittedMachine = physics.transform;



            bittedMachine.SetParent(maw, true);
            Vector3 machineOriginalScale = bittedMachine.localScale;

            tweener = transform.DOLocalMove(new Vector3(0, 10, 0), movementSpeed)
                            .SetSpeedBased(true)
                            .OnUpdate(() =>
                            {
                                bittedMachine.localScale = machineOriginalScale;
                            }).OnComplete(() =>
                            {
                                bittedMachine.localScale = machineOriginalScale;

                            });



        }
        private void OnDisable()
        {
            tweener.Kill();
            if (!bittedMachine) return;
            bittedMachine.SetParent(null, true);
            if (bittedMachine.TryGetComponent(out MachinePhysicsManager physics))
            {
                physics.SetPhysical();
            }



        }//Closes OnDisable method


    }
}
