using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using Ozamanas.Board;
using Ozamanas.Tags;
using Ozamanas.Machines;
namespace Ozamanas.Forces
{
    public class DestructionMammal : AncientForce
    {
        [SerializeField] private float mammalSpeed = 2f;
        [SerializeField] private CellDemolisher cellDemolisher;

        private Tween mammalTween;
        private Animator animator;

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

               AddForceToMachinesInRange();
                 SpawnCellDemolisher();
               DestroyForce();

            });

           

        }//Closes FirstPlacement method

        public void AddForceToMachinesInRange()
        {
               
            foreach (HumanMachine machine in machinesAffected)
            {
                if (!machine) continue;
                AddForceToMachine(machine);                    
            }
                
           
        }
        public override void Drag()
        {
            base.Drag();
            
            if (Board.CellSelectionHandler.currentCellHovered)
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


        private void SpawnCellDemolisher()
        {
            if(!cellDemolisher) return;
            GameObject temp = Instantiate(cellDemolisher.gameObject,currentCell.transform.position,Quaternion.identity);
            temp.GetComponent<CellDemolisher>().SpawnReplacement(currentCell);

        }

        

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

            currentCell.ResetCellData();

            base.DestroyForce();

        }
       
        


       

    }
}
