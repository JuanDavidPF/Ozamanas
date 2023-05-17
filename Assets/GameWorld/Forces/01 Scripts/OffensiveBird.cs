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
                foreach (var machine in machinesAffected.ToArray())
                {
                    if (!machine) continue;
                    AttackMachine(machine);
                }
            });

        }//Closes FirstPlacement method

       
    

        private void AttackMachine(HumanMachine machine)
        {
             if(!machine) return;

             if (machine.TryGetComponent(out MachineMovement movement))
             {
                if(movement.CurrentAltitude != MachineAltitude.Terrestrial) return;
             }
             
            machine.SetMachineStatus(MachineState.Attacked);

            if (machine.TryGetComponent(out Machines.MachineArmor armor))
            {
                armor.TakeDamage(damageAmount);
            }
    
            if (machine.TryGetComponent(out Machines.MachinePhysicsManager physics))
            {
                physics.AddForceToMachine(data.physicsForce,transform.position);
            }
  
        }

        public override void DestroyForce()
        {
            if (birdTween != null) birdTween.Kill();
            base.DestroyForce();
        }




    }//Closes OffensiveBird class
}//closes Namespace declaration
