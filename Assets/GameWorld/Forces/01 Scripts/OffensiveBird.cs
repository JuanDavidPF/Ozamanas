using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.Extenders;
using DG.Tweening;
using Ozamanas.Board;
using Ozamanas.Tags;
using Ozamanas.Machines;

namespace Ozamanas.Forces
{

    [RequireComponent(typeof(Animator))]
    public class OffensiveBird : AncientForce
    {
        private Tween birdTween;
        private Animator animator;
        [SerializeField] private float roosterSpeed = 2f;
        [SerializeField] private int damageAmount;
        [SerializeField] private Transform visuals;


        protected override void Awake()
        {
            base.Awake();

            animator = gameObject.GetComponent<Animator>();


        }//Closes Awake method



        protected override void FinalPlacement()
        {

            base.FinalPlacement();

            if (!isPlaced) return;
            
            visuals.gameObject.SetActive(false);

            birdTween = transform.DOMoveY(0, roosterSpeed, false).SetSpeedBased();

            birdTween.OnComplete(() =>
            {
                visuals.gameObject.SetActive(true);
                animator.SetTrigger("OnRelease");
                ActivateTraits(Board.Board.GetCellByPosition(transform.position.ToFloat3().UnityToGrid()));
                foreach (var machine in machinesAffected.ToArray())
                {
                    if (!machine) continue;
                    AttackMachine(machine);
                }
            });

        }//Closes FirstPlacement method

       
         void OnDestroy()
        {
            if (birdTween != null) birdTween.Kill();
        }//Closes OnDestroy method


       
        private void ActivateTraits(Cell origin)
        {
            if (!origin) return;

            foreach (var cell in origin.GetCellsOnRange(data.traitRange))
            {
                if (!cell) continue;

                foreach (var trait in data.traits)
                {
                    if (!trait) continue;
                    cell.AddTraitToMachine(trait);
                }
            }


        }//Closes ActivateTraits method

        private void AttackMachine(HumanMachine machine)
        {

            if (machine.TryGetComponent(out Machines.MachineArmor armor))
            {
                armor.TakeDamage(damageAmount);
            }
            
            if (machine.TryGetComponent(out Machines.HumanMachine h_machine))
            {
                h_machine.SetMachineStatus(MachineState.Attacked);
            }

            if (machine.TryGetComponent(out Machines.MachinePhysicsManager physics))
            {
                physics.AddForceToMachine(data.physicsForce,transform.position);
            }
  
        }

        public override void DestroyForce()
        {
            base.DestroyForce();
        }




    }//Closes OffensiveBird class
}//closes Namespace declaration
