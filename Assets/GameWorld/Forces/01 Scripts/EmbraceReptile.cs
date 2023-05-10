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
        [SerializeField]  private MachineTrait poisonedTrait;

         [SerializeField]  private CellData forest;
        
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
      
        public override void FirstPlacement()
        {
             if (data.placementMode != PlacementMode.SinglePlacement) CheckFirstPlacement();
            
            base.FirstPlacement();
        }
        public override void SecondPlacement()
        {

            base.SecondPlacement();
        }

         private void CheckFirstPlacement()
        {
            CellSelectionHandler cellHovered = Board.CellSelectionHandler.currentCellHovered;

            if (cellHovered && cellHovered.cellReference) cellHovered.cellReference.CellOverLay.DeActivateAllPointers();
 
            if (cellHovered &&
            IsValidPlacement(cellHovered.cellReference) && validCells.Contains(cellHovered.cellReference))
            {
                firstPlacementComplete = cellHovered.cellReference;
                firstPlacementComplete.isOccupied = true;
                firstPlacementComplete.CurrentTopElement = data.GetTopElementToSwap(firstPlacementComplete);
                firstPlacementComplete.data = data.GetTokenToSwap(firstPlacementComplete);
                transform.position = firstPlacementComplete.worldPosition;
                stopForceDragging = true;
                CalculateSecondPlacementArea();
            }
            else 
            {
               OnForceFailedPlacement();
            }

            if (Board.Board.reference) Board.Board.reference.OnNewCellData.RemoveListener(OnNewCellData);

            EraseValidCells();
        
        }

        private void CalculateSecondPlacementArea()
        {
            if (!data) return;

            if(!firstPlacementComplete) return;

            EraseAttackRangeCells();

            stopAttackRangePrinting = true;

            firstPlacementComplete.OnCellChanged.AddListener(OnAnchorChanged);

            foreach (var cellOnRange in firstPlacementComplete.GetCellsOnRange(data.attackRange.value-1, true))
                {
                    if (!cellOnRange) continue;

                    cellOnRange.CellOverLay.ActivatePointer(CellPointerType.AttackRangePointer);
                    cellsOnAttackRange.Add(cellOnRange);
                    cellOnRange.OnCellChanged.AddListener(OnAreaChanged);
                    
                }
        }
        protected override void FinalPlacement()
        {
            placements = 0;
            
            CellSelectionHandler cellHovered = Board.CellSelectionHandler.currentCellHovered;

            if (cellHovered && cellHovered.cellReference) cellHovered.cellReference.CellOverLay.DeActivateAllPointers();

            DeActivateAllPointers();

            if (cellHovered && cellsOnAttackRange.Contains(cellHovered.cellReference))
            {
                OnSuccesfulPlacement?.Invoke(this);
                isPlaced = true;
            }
            else 
            {
                OnForceFailedPlacement();
            }
            
            if (Board.Board.reference) Board.Board.reference.OnNewCellData.RemoveListener(OnNewCellData);

            EraseValidCells();
            EraseAttackRangeCells();

           switch (data.placementMode)
            { 
                case PlacementMode.SinglePlacement:
                SetUpFirstPlacement();
                break;
                case PlacementMode.DoublePlacement:
                SetUpSecondPlacement();
                break;
             }

           }   
           
           private void SetUpFirstPlacement()
           {

           }

            private void SetUpSecondPlacement()
           {

                animator.SetTrigger("OnRelease");

                if(Board.CellSelectionHandler.currentCellHovered.cellReference == null) base.DestroyForce();

                currentCell = Board.CellSelectionHandler.currentCellHovered.cellReference;

                SetUpNearMachine();

                if (!nearMachine)
                {
                    base.DestroyForce();
                    firstPlacementComplete.isOccupied = true;
                    firstPlacementComplete.data = forest;
                    firstPlacementComplete.CurrentTopElement = forest.defaultTopElement;
                    return;
                }


                StartCoroutine(WaitToRemoveSnake());

                clld.enabled = false;

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
            List<HumanMachine> machines = currentCell.CurrentHumanMachines;
            foreach(HumanMachine machine in machines)
            {
                if(!machine) continue;
                
                if (machine.TryGetComponent(out MachineMovement movement))
                {
                    if(movement.CurrentAltitude != MachineAltitude.Terrestrial) continue;
                }
                 if (machine.TryGetComponent(out SandWormBoss boss))
                {
                    continue;
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
            nearMachine.DOLookAt(transform.position,0.3f);
        }

        protected override void OnForceFailedPlacement()
        {
            base.OnForceFailedPlacement();
            DetachHumanMachine();
        }

        public override void DetachHumanMachine()
        {
            if (firstPlacementComplete) 
            {
                firstPlacementComplete.isOccupied = false;
                if(firstPlacementComplete.TryGetComponentInChildren<GhostCell>(out GhostCell ghost))
                {
                    firstPlacementComplete.ResetCellData();
                }
            }

            if(nearMachine) 
            {
                if (!nearMachine.TryGetComponentInParent(out HumanMachine machine)) return;
                machine.transform.SetParent(null,true);
                if(poisonedTrait) machine.RemoveTraitToMachine(poisonedTrait);
                nearMachine.GetComponentInParent<MachinePhysicsManager>().SetIntelligent();
            }
            base.DestroyForce();
        }

    }
}
