using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Ozamanas.Machines;

namespace Ozamanas.Forces
{
    public class SnakeController : MonoBehaviour
    {
        [SerializeField] private Transform maw;
        [SerializeField] private float movementSpeed;
        [SerializeField] private Machines.MachineTrait trait;
        Transform biteObjective;
        private Tweener tweener;

        public void Bite(Transform machine)
        {
            Machines.HumanMachine humanMachine = machine.GetComponentInParent<Machines.HumanMachine>();

            humanMachine.CurrentCell.AddTraitToMachine(trait);


            tweener = transform.DOMove(machine.position, movementSpeed)
            .SetSpeedBased(true).From(new Vector3(transform.position.x, 0, transform.position.z))
            .OnUpdate(() =>
            {
                tweener.ChangeEndValue(machine.position, true);
            })
            .OnComplete(() =>
            {

                machine.transform.SetParent(maw);
                MachinePhysicsManager physics = machine.GetComponentInParent<MachinePhysicsManager>();


                if (physics) physics.ActivatePhysics(true);

            });

        }//Closes Bite method
        private void OnDisable()
        {
            tweener.Kill();
        }//Closes OnDisable method
        // Update is called once per frame
        void Update()
        {
            if (!biteObjective) return;

            transform.position = biteObjective.position;
        }
    }
}
