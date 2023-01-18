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

        public Transform NearMachine
        {
            get { return nearMachine; }
            set
            {
                if (nearMachine == value) return;

                nearMachine = value;

                if(markTrait) nearMachine.GetComponentInParent<HumanMachine>().AddTraitToMachine(markTrait);
            }
        }

        protected override void Awake()
        {
            base.Awake();

            clld = GetComponentInChildren<Collider>();

            controllers = GetComponentsInChildren<SnakeController>();
        }
        
        protected override void FinalPlacement()
        {

            base.FinalPlacement();

             if (!isPlaced) return;

            currentCell = Board.Board.GetCellByPosition(transform.position.ToFloat3().UnityToGrid());

            currentCell.isOccupied = true;

            transform.position = currentCell.worldPosition;

            ActivateTraits(currentCell);

            StartCoroutine(WaitToRemoveSnake());

            clld.enabled = false;

            if (!NearMachine)
            {
                base.DestroyForce();
                return;
            }

            if(markTrait) NearMachine.GetComponentInParent<HumanMachine>().RemoveTraitToMachine(markTrait);

            foreach (var snake in controllers)
            {
                if (!snake) continue;

                snake.Bite(NearMachine);
                
                if (NearMachine.TryGetComponentInParent(out HumanMachine machine))
                {
                    machine.SetCapturedStatus(this);
                }
            }

        }

        


        private void OnTriggerEnter(Collider other)
        {
            if (isPlaced || other.transform.tag != "Machine") return;
            
            if(other.TryGetComponentInParent(out Machines.MachinePhysicsManager physics))
            {
                if(physics.state == PhysicMode.Intelligent)    
                NearMachine = other.transform; 
            }
            
        }

        private void OnTriggerExit(Collider other)
        {
            if (isPlaced || other.transform.tag != "Machine" || other != NearMachine) return;

            NearMachine = null;
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
            if(!NearMachine) return;

            if (NearMachine.TryGetComponentInParent(out MachinePhysicsManager physics))
            {
                physics.SetPhysical();
            }

            if (NearMachine.TryGetComponentInParent(out HumanMachine machine))
            {
                machine.transform.SetParent(null,true);
            }

            if (currentCell) currentCell.isOccupied = false;

            base.DestroyForce();
        }

    }
}
