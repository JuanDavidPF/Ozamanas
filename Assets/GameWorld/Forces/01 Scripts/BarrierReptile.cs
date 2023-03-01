using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Ozamanas.Extenders;
using Ozamanas.Board;
using UnityEngine.Events;
using Ozamanas.Tags;
using Ozamanas.Forest;

namespace Ozamanas.Forces
{
    public class BarrierReptile : AncientForce
    {
        Collider reptileCollider;
        private Tween reptileTween;
        [SerializeField] private GameObject mountain;
        private bool isReady = false;
        [SerializeField] private CellData barrierID;

         [SerializeField] private CellTopElement emptyTopElementID;
         [SerializeField] public UnityEvent OnDestruction;

        [Space(15)]
        [Header("Intro Force")]
         [SerializeField] private int reptileRange = 1;

          [SerializeField] private float reptileSpeed = 1;

          [SerializeField] private float moveDelay = 1;

         private Animator animator;

        protected override void Awake()
        {
            base.Awake();
            reptileCollider = GetComponentInChildren<Collider>();
            reptileCollider.enabled = false;
            animator = GetComponent<Animator>();
        }


        protected override void SecondPlacement()
        {

            base.SecondPlacement();
        }


        public override void FirstPlacement()
        {
             if (data.placementMode != PlacementMode.SinglePlacement) CheckFirstPlacement();
            
            base.FirstPlacement();
        }

        private void CalculateSecondPlacementArea()
        {
            if (!data) return;

            if(!firstPlacementComplete) return;

            firstPlacementComplete.OnCellChanged.AddListener(OnAnchorChanged);

            foreach (var cellOnRange in firstPlacementComplete.GetCellsOnRange(reptileRange, false))
                {
                    if (!cellOnRange || !IsValidPlacement(cellOnRange)) continue;


                    UnityFx.Outline.OutlineBuilder.AddToLayer(0, cellOnRange.CellOverLay.gameObject);
                    validCells.Add(cellOnRange);
                    cellOnRange.OnCellChanged.AddListener(OnAreaChanged);
                }

        }

        private void CheckFirstPlacement()
        {
            CellSelectionHandler cellHovered = Board.CellSelectionHandler.currentCellHovered;

            if (cellHovered && cellHovered.cellReference) cellHovered.cellReference.CellOverLay.DeActivateAllPointers();
 
            if (cellHovered &&
            IsValidPlacement(cellHovered.cellReference) && validCells.Contains(cellHovered.cellReference))
            {
               cellHovered.cellReference.CellOverLay.ActivatePointer(CellPointerType.MountainPointer);
               firstPlacementComplete = cellHovered.cellReference;
            }
            else OnFailedPlacement?.Invoke(this);

            if (Board.Board.reference) Board.Board.reference.OnNewCellData.RemoveListener(OnNewCellData);

            EraseValidCells();

            CalculateSecondPlacementArea();

        }
        protected override void FinalPlacement()
        {
            base.FinalPlacement();

            if(!isPlaced) return;

            reptileCollider.enabled = true;

            animator.SetTrigger("OnRelease");

            switch (data.placementMode)
            { 
                case PlacementMode.SinglePlacement:
                SetUpFirstPlacement();
                break;
                case PlacementMode.DoublePlacement:
                SetUpSecondPlacement();
                break;
             }

             

        }//Closes FistPlacement method


        private void SetUpFirstPlacement()
        {
            firstPlacementComplete = Board.Board.GetCellByPosition(transform.position.ToFloat3().UnityToGrid());

            transform.position = firstPlacementComplete.worldPosition;

            ChangeTokenToCells();
        }

        private void SetUpSecondPlacement()
        {
            secondPlacementComplete = Board.Board.GetCellByPosition(transform.position.ToFloat3().UnityToGrid());

            transform.position = firstPlacementComplete.worldPosition;

            transform.DOLookAt(secondPlacementComplete.transform.position,0.1f);

            ChangeTokenToCells();

            reptileTween = transform.DOMove(secondPlacementComplete.transform.position,reptileSpeed,false).SetSpeedBased().SetDelay(moveDelay);

            reptileTween.OnComplete(() =>
            {
                InstantiateBarrier();
                
            });

        }

        public void ChangeTokenToCells()
        {

            if (!barrierID) return;

            if(firstPlacementComplete)  
            {
                firstPlacementComplete.CurrentTopElement = data.GetTopElementToSwap(firstPlacementComplete);
                firstPlacementComplete.data = data.GetTokenToSwap(firstPlacementComplete);
                
                ActivateTraits(Board.Board.GetCellByPosition(firstPlacementComplete.transform.position.ToFloat3().UnityToGrid()));
            }
         
            if(secondPlacementComplete)
            {
                secondPlacementComplete.CurrentTopElement = data.GetTopElementToSwap(secondPlacementComplete);
                secondPlacementComplete.data = data.GetTokenToSwap(secondPlacementComplete);
                ActivateTraits(Board.Board.GetCellByPosition(secondPlacementComplete.transform.position.ToFloat3().UnityToGrid()));
            }                 
        }

        public void InstantiateBarrier()
        {
            isReady = true;   

            if(!mountain) return;

            if(secondPlacementComplete && secondPlacementComplete.TryGetComponent<ForestBehaviour>(out ForestBehaviour forest))
            {
                forest.CurrentTopElement=data.GetTopElementToSwap(forest);
            }

            if(firstPlacementComplete && firstPlacementComplete.TryGetComponent<ForestBehaviour>(out ForestBehaviour forest_))
            {
                forest_.CurrentTopElement=data.GetTopElementToSwap(forest_);
            }
            
            base.DestroyForce();
        }

        private void OnCollisionEnter(Collision other)
        {

            if (!isPlaced || (other.transform.tag != "Machine")) return;

            if(isReady) return;

            if (other.transform.TryGetComponent(out Machines.HumanMachine h_machine))
            {
                h_machine.SetMachineStatus(MachineState.AddForce);
            }

            if (other.transform.TryGetComponentInParent(out Machines.MachinePhysicsManager physics))
            {
                if(physics.state != PhysicMode.Intelligent ) return;

                physics.AddForceToMachine(data.physicsForce,transform.position);
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

  

    }//Closes BarrierReptile class
}//Closes Namespace declaration
