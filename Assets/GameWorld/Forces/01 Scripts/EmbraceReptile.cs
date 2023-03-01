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

        [SerializeField] private MachineTrait markTrait;
        
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

            currentCell = Board.Board.GetCellByPosition(transform.position.ToFloat3().UnityToGrid());

            currentCell.isOccupied = true;

            transform.position = currentCell.worldPosition;

            ActivateTraits(currentCell);

            StartCoroutine(WaitToRemoveSnake());

            clld.enabled = false;

            if (!nearMachine)
            {
                base.DestroyForce();
                return;
            }

            nearMachine.GetComponentInParent<HumanMachine>().RemoveTraitToMachine(markTrait);

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

        


        private void OnTriggerEnter(Collider other)
        {
            if (isPlaced || other.transform.tag != "Machine" || other.transform == nearMachine) return;
            
            if(other.TryGetComponentInParent(out Machines.MachinePhysicsManager physics))
            {
                if(physics.state != PhysicMode.Intelligent) return;

                if(nearMachine) nearMachine.GetComponentInParent<HumanMachine>().RemoveTraitToMachine(markTrait);

                nearMachine = other.transform; 

                nearMachine.GetComponentInParent<HumanMachine>().AddTraitToMachine(markTrait);
            }
            
        }

        private void OnTriggerExit(Collider other)
        {
            if (isPlaced || other.transform.tag != "Machine" || other.transform != nearMachine) return;

            if(nearMachine) nearMachine.GetComponentInParent<HumanMachine>().RemoveTraitToMachine(markTrait);

            nearMachine = null;
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
