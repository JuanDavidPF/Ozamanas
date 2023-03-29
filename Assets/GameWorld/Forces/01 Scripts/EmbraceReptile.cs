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
        
        protected override void FinalPlacement()
        {

            base.FinalPlacement();

             if (!isPlaced) return;

              animator.SetTrigger("OnRelease");

              SetUpNearMachine();

            currentCell = Board.Board.GetCellByPosition(transform.position.ToFloat3().UnityToGrid());

            currentCell.isOccupied = true;

            transform.position = currentCell.worldPosition;

            ActivateTraits(currentCell);

            StartCoroutine(WaitToRemoveSnake());

            clld.enabled = false;

            if (!nearMachine)
            {
                base.DestroyForce();
                currentCell.isOccupied = false;
                return;
            }

            currentCell.CurrentTopElement = data.GetTopElementToSwap(currentCell);

            currentCell.data = data.GetTokenToSwap(currentCell);


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
                if (machine.TryGetComponent(out MachineMovement movement))
                {
                    if(movement.CurrentAltitude != MachineAltitude.Terrestrial) continue;
                }
                if(machine.TryGetComponentInParent(out Machines.MachinePhysicsManager physics))
                {
                    if(physics.state != PhysicMode.Intelligent) continue;
                    nearMachine = physics.transform; 
                }
            }
        }


        IEnumerator WaitToRemoveSnake()
        {
            yield return new WaitForSeconds(duration);

            DetachHumanMachine();

        }

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

        public override void DetachHumanMachine()
        {
            if(!nearMachine) return;

            if (nearMachine.TryGetComponentInParent(out MachinePhysicsManager physics))
            {
                physics.SetPhysical();
            }

            if (nearMachine.TryGetComponentInParent(out HumanMachine machine))
            {
                machine.transform.SetParent(null,true);
            }

            if (currentCell) currentCell.isOccupied = false;

            base.DestroyForce();
        }

    }
}
