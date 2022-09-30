using System.Collections;
using System.Collections.Generic;
using Ozamanas.Board;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Ozamanas.Forces
{
    public abstract class AncientForce : MonoBehaviour
    {
        public bool isPlaced;
        public enum PlacementMode
        {
            SinglePlacement,
            DoublePlacement,
            TriplePlacement
        }


        private Camera cam;
        private GameObject g;
        private Transform t;

        public ForceData data;

        [SerializeField] private Vector3 draggedOffset;
        [SerializeField] private bool snapToGrid = true;


        [SerializeField] protected PlacementMode placementMode;

        [Space(10)]
        [Header("Traits")]
        [SerializeField] protected int traitRange = 1;
        [SerializeField] protected List<Machines.MachineTrait> traits;


        [Space(10)]
        [Header("Events")]
        public UnityEvent<AncientForce> OnSuccesfulPlacement;
        public UnityEvent<AncientForce> OnFailedPlacement;

        protected virtual void Awake()
        {
            g = gameObject;
            t = g.transform;
            cam = Camera.main;

            if (Board.Board.reference) Board.Board.reference.OnNewCellData.AddListener(OnNewCellData);
            CalculateArea();
        }//Closes Awake method

        Vector3 draggedPosition;
        public virtual void Drag()
        {
            if (!Board.CellSelectionHandler.currentCellHovered || !snapToGrid)
            {
                Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

                draggedPosition = (ray.origin + ray.direction * Mathf.Abs(cam.transform.position.z));
                if (!snapToGrid) draggedPosition += draggedOffset;
            }
            else
            {
                draggedPosition = Board.CellSelectionHandler.currentCellHovered.transform.position + draggedOffset;

            }


            t.position = draggedPosition;

        }//Closes OnDrag method

        public virtual void FirstPlacement()
        {
            if (placementMode != PlacementMode.SinglePlacement) return;

            FinalPlacement();
        }//Closes FirstPlacement method

        protected virtual void SecondPlacement()
        {
            if (placementMode != PlacementMode.DoublePlacement) return;

            FinalPlacement();
        }//Closes SecondPlacement method

        protected virtual void ThirdPlacement()
        {
            if (placementMode != PlacementMode.TriplePlacement) return;
            FinalPlacement();

        }//Closes ThirdPlacement method


        protected virtual void FinalPlacement()
        {
            if (Board.CellSelectionHandler.currentCellHovered && IsValidPlacement(Board.CellSelectionHandler.currentCellHovered.cellReference))
            {
                OnSuccesfulPlacement?.Invoke(this);
                isPlaced = true;
            }

            else OnFailedPlacement?.Invoke(this);

            if (Board.Board.reference) Board.Board.reference.OnNewCellData.RemoveListener(OnNewCellData);

            EraseValidCells();

        }


        protected virtual bool IsValidPlacement(Cell cell)
        {
            if (!cell) return false;
            if (!data) return false;
            if (!data.whiteList.Contains(cell.data)) return false;

            return true;
        }//Closes IsValidPlacement method



        List<Board.Cell> anchorCells = new List<Cell>();
        List<Board.Cell> validCells = new List<Cell>();

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


                    UnityFx.Outline.OutlineBuilder.AddToLayer(0, cellOnRange.gameObject);
                    validCells.Add(cellOnRange);
                    cellOnRange.OnCellChanged.AddListener(OnAreaChanged);
                }
            }

        }//Closes DrawPlaceableCells method

        private void OnAnchorChanged(Cell anchor)
        {

            anchor.OnCellChanged.RemoveListener(OnAnchorChanged);
            EraseValidCells();
            CalculateArea();
        }

        private void OnNewCellData(Cell possibleAnchor)
        {

            if (!possibleAnchor || !possibleAnchor.data || !data || !data.rangeAnchors.Contains(possibleAnchor.data)) return;

            EraseValidCells();
            CalculateArea();

        }

        private void OnAreaChanged(Cell cell)
        {
            if (!cell) return;
            if (!IsValidPlacement(cell)) UnityFx.Outline.OutlineBuilder.Remove(0, cell.gameObject);
            else UnityFx.Outline.OutlineBuilder.AddToLayer(0, cell.gameObject);
        }



        private void EraseValidCells()
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
                UnityFx.Outline.OutlineBuilder.Remove(0, valid.gameObject);
            }
            validCells.Clear();

        }//Closes s method



        protected virtual void OnDestroy()
        {
            EraseValidCells();
        }//Closes OnDestroy method


    }//Closes AncientForce class
}//Closes Namespace declaration
