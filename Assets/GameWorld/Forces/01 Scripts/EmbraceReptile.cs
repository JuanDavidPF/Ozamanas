using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Ozamanas.Extenders;
using Ozamanas.Board;
using Ozamanas.Tags;
using Ozamanas.Machines;

namespace Ozamanas.Forces
{
    public class EmbraceReptile : AncientForce
    {
        Collider clld;

        [SerializeField] private float duration = 2f;

        [SerializeField] private GameObject VFXBite;

        [SerializeField]  private MachineTrait markTrait;
        [SerializeField]  private MachineTrait poisonedTrait;
        
        private Cell currentCell;

        private SnakeController[] controllers;

        private Transform nearMachine;

        private Animator animator;

       
        protected override void Awake()
        {
            base.Awake();

            clld = GetComponentInChildren<Collider>();

            controllers = GetComponentsInChildren<SnakeController>();

            animator = GetComponent<Animator>();
        }
        protected override void FixedUpdate()
        {
            base.FixedUpdate();

             if (isPlaced) return;

            base.GetMachinesOnAttackRange();

            SetUpNearMachine();

            if(nearMachine) nearMachine.GetComponentInParent<HumanMachine>().AddTraitToMachine(markTrait);
        }
        protected override void FinalPlacement()
        {

            base.FinalPlacement();

            if (!isPlaced) return;

            currentCell = Board.Board.GetCellByPosition(transform.position.ToFloat3().UnityToGrid());

            currentCell.isOccupied = true;

            SetUpNearMachine();

            if (!nearMachine)
            {
                base.DestroyForce();
                currentCell.isOccupied = false;
                return;
            }

            animator.SetTrigger("OnRelease");

            transform.position = currentCell.worldPosition;

            StartCoroutine(WaitToRemoveSnake());

            clld.enabled = false;

            currentCell.CurrentTopElement = data.GetTopElementToSwap(currentCell);

            currentCell.data = data.GetTokenToSwap(currentCell);

            transform.LookAt(nearMachine);

            foreach (var snake in controllers)
            {
                if (!snake) continue;

                snake.Bite(nearMachine);
                
                if (nearMachine.TryGetComponentInParent(out HumanMachine machine))
                {
                    machine.SetCapturedStatus(this);
                }
            }

        }

        private void SetUpNearMachine()
        {
            foreach(HumanMachine machine in machinesAffected)
            {
                if(!machine) continue;
                
                if (machine.TryGetComponent(out MachineMovement movement))
                {
                    if(movement.CurrentAltitude != MachineAltitude.Terrestrial) continue;
                }
                if(machine.TryGetComponentInParent(out Machines.MachinePhysicsManager physics))
                {
                    if(physics.state != PhysicMode.Intelligent) continue;
                    nearMachine = physics.transform; 
                    return;
                }
            }

            nearMachine = null;
        }


        IEnumerator WaitToRemoveSnake()
        {
            yield return new WaitForSeconds(duration);

            DetachHumanMachine();

        }



        public void ActivateOnDragAnimation()
        {
             animator.SetTrigger("OnDrag");
        }

        public void OnBiteMachine(Transform machine)
        {
            if(VFXBite) Instantiate(VFXBite,machine.position,Quaternion.identity);
            if(poisonedTrait) machine.GetComponentInParent<HumanMachine>().AddTraitToMachine(poisonedTrait);
        }

        public override void DetachHumanMachine()
        {
            if (currentCell) 
            {
                currentCell.isOccupied = false;

                if(currentCell.CurrentHumanMachines.Count == 0) currentCell.ResetCellData();
            }

            if(!nearMachine) return;

            if (nearMachine.TryGetComponentInParent(out MachinePhysicsManager physics))
            {
                physics.SetPhysical();
            }

            if (nearMachine.TryGetComponentInParent(out HumanMachine machine))
            {
                machine.transform.SetParent(null,true);
                if(poisonedTrait) machine.RemoveTraitToMachine(poisonedTrait);
            }

            base.DestroyForce();
        }

    }
}
