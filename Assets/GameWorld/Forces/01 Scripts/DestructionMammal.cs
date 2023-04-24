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
        [SerializeField] private GameObject obstacle;
        [SerializeField] private CellData waterCell;
        private Tween mammalTween;
        private Animator animator;
        private Cell currentCell;

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
               DestroyCell();
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


        private void DestroyCell()
        {
            SpawnObstacle();
            SpawnReplacement();
        }

        private void SpawnObstacle()
        {
            if(!obstacle) return;

            GameObject temp = Instantiate(obstacle,currentCell.transform.position,Quaternion.identity);

        }

        private void SpawnReplacement()
        {
            if(!waterCell) return;

            List<Cell> waterCells = Board.BoardExtender.GetCellsOnRange(currentCell,1,false);

            bool hasWater = false;

            foreach(Cell cell in waterCells)
            {
                if(cell.data == waterCell) hasWater = true;
            }

            if( hasWater)  currentCell.data = waterCell;
            else
            {
                Board.Board.RemoveCellFromBoard(currentCell);
                Board.Board.reference.CombineTileMeshes();
            }
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
