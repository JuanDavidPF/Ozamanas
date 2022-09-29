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
            DrawValidCells();
        }//Closes OnDrag method

        public virtual void FirstPlacement()
        {
            if (placementMode != PlacementMode.SinglePlacement) return;

            if (Board.CellSelectionHandler.currentCellHovered && validCells.Contains(Board.CellSelectionHandler.currentCellHovered.cellReference))
            {
                OnSuccesfulPlacement?.Invoke(this);
                isPlaced = true;
            }

            else OnFailedPlacement?.Invoke(this);
            EraseValidCells();
        }//Closes FirstPlacement method

        protected virtual void SecondPlacement()
        {
            if (placementMode != PlacementMode.DoublePlacement) return;

            if (Board.CellSelectionHandler.currentCellHovered && validCells.Contains(Board.CellSelectionHandler.currentCellHovered.cellReference))
            {
                OnSuccesfulPlacement?.Invoke(this);
                isPlaced = true;
            }
            else OnFailedPlacement?.Invoke(this);
            EraseValidCells();
        }//Closes SecondPlacement method

        protected virtual void ThirdPlacement()
        {
            if (placementMode != PlacementMode.TriplePlacement) return;

            if (Board.CellSelectionHandler.currentCellHovered && validCells.Contains(Board.CellSelectionHandler.currentCellHovered.cellReference))
            {
                OnSuccesfulPlacement?.Invoke(this);
                isPlaced = true;
            }

            else OnFailedPlacement?.Invoke(this);
            EraseValidCells();
        }//Closes ThirdPlacement method

        protected virtual bool IsValidPlacement(Cell cell)
        {
            if (!cell) return false;
            if (!data) return false;
            if (!data.whiteList.Contains(cell.data)) return false;

            return true;
        }//Closes IsValidPlacement method




        List<Board.Cell> validCells = new List<Cell>();

        protected virtual void DrawValidCells()
        {
            if (!data) return;






            EraseValidCells();

            //Gets plausibles cells to be placed based on range and conditions
            foreach (var cell in Board.Board.GetCellsByData(data.rangeAnchors.ToArray()))
            {

                if (!cell) continue;

                foreach (var cellOnRange in cell.GetCellsOnRange(data.range.value, false))
                {
                    if (!cellOnRange ||
                    cell == CellSelectionHandler.currentCellHovered ||
                    cell == CellSelectionHandler.currentCellSelected ||
                    !IsValidPlacement(cellOnRange)) continue;


                    UnityFx.Outline.OutlineBuilder.AddToLayer(0, cellOnRange.gameObject);

                    validCells.Add(cellOnRange);
                }
            }

        }//Closes DrawPlaceableCells method

        private void EraseValidCells()
        {
            UnityFx.Outline.OutlineBuilder.Remove(0, validCells);

        }//Closes s method
        protected virtual void OnDestroy()
        {
            EraseValidCells();
        }//Closes OnDestroy method


    }//Closes AncientForce class
}//Closes Namespace declaration
