using System.Collections;
using System.Collections.Generic;
using Ozamanas.Board;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Ozamanas.Tags;

namespace Ozamanas.Forces
{
    public abstract class AncientForce : MonoBehaviour
    {
        public bool isPlaced;
       
        protected Cell firstPlacementComplete;
        protected Cell secondPlacementComplete;

        private Camera cam;
        private GameObject g;
        private Transform t;

        public ForceData data;

       
        [Space(10)]
        [Header("Events")]
        public UnityEvent<AncientForce> OnSuccesfulPlacement;
        public UnityEvent<AncientForce> OnFailedPlacement;
        public UnityEvent OnForceDestroy;

        [SerializeField] private GameObject onDestroyVFX;
       
        protected virtual void Awake()
        {
            g = gameObject;
            t = g.transform;
            cam = Camera.main;

            if (Board.Board.reference) Board.Board.reference.OnNewCellData.AddListener(OnNewCellData);
            CalculateArea();
        }//Closes Awake method

        Vector3 draggedPosition;

        public int placements = 0;
        protected void Update()
        {
           if(placements > 0) Drag();

            CheckMultiplePlacements();
        }//Closes Update method

        public virtual void CheckMultiplePlacements()
        {
            if (data.placementMode == PlacementMode.SinglePlacement) return;
            if (!Mouse.current.leftButton.wasPressedThisFrame) return;
            if (placements == 0) return;

            // This happens if this force was already finished its first placement, 
            // and everytime the left mouse button was pressed after that first placement

            placements++;
            switch (placements)
            {
                case 2:
                    SecondPlacement();
                    break;

                case 3:
                    ThirdPlacement();
                    break;
            }
        }//Closes CheckMultiplePlacements method


        public virtual void Drag()
        {
            if (!Board.CellSelectionHandler.currentCellHovered || !data.snapToGrid)
            {
                Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

                draggedPosition = (ray.origin + ray.direction * Mathf.Abs(cam.transform.position.z));
                
                if (!data.snapToGrid) draggedPosition += data.draggedOffset;

            }
            else
            {
                draggedPosition = Board.CellSelectionHandler.currentCellHovered.transform.position + data.draggedOffset;
                Board.CellSelectionHandler.currentCellHovered.cellReference.CellOverLay.ActivatePointer(CellPointerType.Pointer);
            }

            t.position = draggedPosition;

        }//Closes OnDrag method

        protected virtual void DestroyForce()
        {
            if(onDestroyVFX) Instantiate(onDestroyVFX,transform.position,transform.rotation);

            EraseValidCells();

             OnForceDestroy?.Invoke();

            Destroy(gameObject);
        }

        public virtual void FirstPlacement()
        {
            placements = 1;
            if (data.placementMode != PlacementMode.SinglePlacement) return;

            FinalPlacement();
        }//Closes FirstPlacement method

        protected virtual void SecondPlacement()
        {
            if (data.placementMode != PlacementMode.DoublePlacement) return;

            FinalPlacement();
        }//Closes SecondPlacement method

        protected virtual void ThirdPlacement()
        {
            if (data.placementMode != PlacementMode.TriplePlacement) return;
            FinalPlacement();

        }//Closes ThirdPlacement method


        protected virtual void FinalPlacement()
        {
            placements = 0;
            CellSelectionHandler cellHovered = Board.CellSelectionHandler.currentCellHovered;

            if (cellHovered && cellHovered.cellReference) cellHovered.cellReference.CellOverLay.DeActivateAllPointers();

            DeActivateAllPointers();

            if (cellHovered &&
            IsValidPlacement(cellHovered.cellReference) && validCells.Contains(cellHovered.cellReference))
            {
                OnSuccesfulPlacement?.Invoke(this);
                isPlaced = true;
            }

            else OnFailedPlacement?.Invoke(this);

            if (Board.Board.reference) Board.Board.reference.OnNewCellData.RemoveListener(OnNewCellData);

            EraseValidCells();

        }


        protected virtual void DeActivateAllPointers()
        {
            if(firstPlacementComplete) firstPlacementComplete.CellOverLay.DeActivateAllPointers();
            if(secondPlacementComplete) secondPlacementComplete.CellOverLay.DeActivateAllPointers();
        }

        protected virtual bool IsValidPlacement(Cell cell)
        {
            if (!cell) return false;
            if (!data) return false;
            if (!data.whiteList.Contains(cell.data)) return false;
            return true;
        }//Closes IsValidPlacement method



        protected List<Board.Cell> anchorCells = new List<Cell>();
        protected List<Board.Cell> validCells = new List<Cell>();

        protected virtual void CalculateArea()
        {
            if (!data) return;

            anchorCells = Board.Board.GetCellsByData(data.rangeAnchors.ToArray());

            //Gets plausibles cells to be placed based on range and conditions
            foreach (var cell in anchorCells)
            {

                if (!cell) continue;

                cell.OnCellChanged.AddListener(OnAnchorChanged);

                foreach (var cellOnRange in cell.GetCellsOnRange(data.range.value, false))
                {
                    if (!cellOnRange || !IsValidPlacement(cellOnRange)) continue;


                    UnityFx.Outline.OutlineBuilder.AddToLayer(0, cellOnRange.CellOverLay.gameObject);
                    validCells.Add(cellOnRange);
                    cellOnRange.OnCellChanged.AddListener(OnAreaChanged);
                }
            }

        }//Closes DrawPlaceableCells method

        protected void OnAnchorChanged(Cell anchor)
        {

            anchor.OnCellChanged.RemoveListener(OnAnchorChanged);
            EraseValidCells();
            CalculateArea();
        }

        protected void OnNewCellData(Cell possibleAnchor)
        {

            if (!possibleAnchor || !possibleAnchor.data || !data || !data.rangeAnchors.Contains(possibleAnchor.data)) return;

            EraseValidCells();
            CalculateArea();

        }

        protected void OnAreaChanged(Cell cell)
        {
            if (!cell) return;
            if (!IsValidPlacement(cell)) UnityFx.Outline.OutlineBuilder.Remove(0, cell.CellOverLay.gameObject);
            else UnityFx.Outline.OutlineBuilder.AddToLayer(0, cell.CellOverLay.gameObject);
        }



        protected void EraseValidCells()
        {
            foreach (var anchor in anchorCells)
            {
                if (!anchor) continue;
                anchor.OnCellChanged.RemoveListener(OnAnchorChanged);
            }
            anchorCells.Clear();

            foreach (var valid in validCells)
            {
                if (!valid) continue;
                valid.OnCellChanged.RemoveListener(OnAreaChanged);
                UnityFx.Outline.OutlineBuilder.Remove(0, valid.CellOverLay.gameObject);
            }
            validCells.Clear();

        }//Closes s method

        public virtual void DetachHumanMachine()
        {
            
        }


    }//Closes AncientForce class
}//Closes Namespace declaration
