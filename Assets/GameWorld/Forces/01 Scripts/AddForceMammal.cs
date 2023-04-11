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
    public class AddForceMammal : AncientForce
    {
          

         [SerializeField] private CellTopElement forestTopElement;

        [SerializeField] private float mammalSpeed = 2f;

        private Tween mammalTween;
        private Animator animator;

        private Cell currentCell;

         [SerializeField] private bool addForceOnTweenComplete;

        protected override void Awake()
        {
            base.Awake();

            animator = GetComponent<Animator>();
        }//Closes Awake method

        protected override void FinalPlacement()
        {

            base.FinalPlacement();

            if (!isPlaced) return;

            animator.SetTrigger("OnRelease");

            mammalTween = transform.DOMoveY(0, mammalSpeed, false).SetSpeedBased();

            currentCell.CurrentTopElement = data.GetTopElementToSwap(currentCell);

            currentCell.data = data.GetTokenToSwap(currentCell);

            mammalTween.OnComplete(() =>
            {

               

               if(!addForceOnTweenComplete) return;
               
               AddForceToMachinesInRange();

               DestroyForce();

            });

           

        }//Closes FirstPlacement method

        public void AddForceToMachinesInRange()
        {
             ActivateTraits(Board.Board.GetCellByPosition(transform.position.ToFloat3().UnityToGrid()));
                
            foreach (HumanMachine machine in machinesAffected)
            {
                if (!machine) continue;
                AddForceToMachine(machine);                    
            }
                
           
        }
        public override void Drag()
        {
            base.Drag();
            
            if (Board.CellSelectionHandler.currentCellHovered || !data.snapToGrid)
            {
                if(data.physicsForce.type == AddForceType.FrontFlip)
                Board.CellSelectionHandler.currentCellHovered.cellReference.CellOverLay.ActivatePointer(CellPointerType.PullPointer);
                else
                Board.CellSelectionHandler.currentCellHovered.cellReference.CellOverLay.ActivatePointer(CellPointerType.PushPointer);

                if(currentCell != Board.CellSelectionHandler.currentCellHovered.cellReference)
                {
                    if(currentCell)
                    {
                        currentCell.CellOverLay.DeActivatePointer(CellPointerType.PushPointer);
                        currentCell.CellOverLay.DeActivatePointer(CellPointerType.PullPointer);
                    }

                    currentCell = Board.CellSelectionHandler.currentCellHovered.cellReference;
                }
            }
            
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


        private void AddForceToMachine(HumanMachine machine)
        {
           
            if (!machine || machine.tag != "Machine") return;


             if (machine.TryGetComponent(out Machines.HumanMachine h_machine))
            {
                h_machine.SetMachineStatus(MachineState.AddForce);
            }

            if (machine.TryGetComponent(out Machines.MachinePhysicsManager physics))
            {
                if(!data.physicsForce) return;

                physics.AddForceToMachine(data.physicsForce,gameObject.transform.position);
            }

           

        }//Closes AttemptMachineDamage method

       
        public override void DestroyForce()
        {
            if (mammalTween != null) mammalTween.Kill();

            currentCell.CurrentTopElement = forestTopElement;

            base.DestroyForce();

        }
       
        


       

       


    }



}
