using System;
using System.Collections;
using System.Collections.Generic;
using Ozamanas.Board;
using Unity.Mathematics;
using UnityEngine;
using DG.Tweening;
using JuanPayan.References;
using JuanPayan.Utilities;
using UnityEngine.Events;
namespace Ozamanas.Forces
{
    public class ExpansionAmphibian : AncientForce
    {
        public enum MutationMode
        {
            SwapCell,
            SwapToken
        }

        public enum ExpansionMode
        {
            Jump,
            Throw
        }

        [Serializable]
        public struct ExpansionRules
        {
            public MutationMode mode;
            public CellData condition;
            public Cell cellToSwap;
            public CellData tokenToSwap;

        }

         [SerializeField] public UnityEvent OnEnterCell;

        [Space(10)]
        [Header("Expansion Parameters")]
        [SerializeField] private ExpansionMode mode;
        [SerializeField] private bool fillPath;
        [SerializeField] private List<ExpansionRules> ruleList = new List<ExpansionRules>();
        [SerializeField] private FloatReference speed;
        [SerializeField] private float jumpHeight;
        int3 reptileOrigin;
        int3 reptileDestiny;
        private List<int3> reptilePath = new List<int3>();


        private Animator animator;

        protected override void Awake()
        {
            base.Awake();
            animator = GetComponent<Animator>();
        }

        private void Expand()
        {
            if (!CellSelectionHandler.currentCellHovered) return;

            Cell destinyCell = CellSelectionHandler.currentCellHovered.cellReference;
            reptileDestiny = destinyCell.gridPosition;
            Cell originCell = Board.Board.GetNearestCellInRange(destinyCell.worldPosition, data.range.value, data.rangeAnchors.ToArray());
            if (!originCell) return;

            reptileOrigin = originCell.gridPosition;

            transform.position = reptileOrigin.GridToUnity();

            switch (mode)
            {
                case ExpansionMode.Jump:
                    if (fillPath)
                    {
                        reptilePath = Board.Board.CellsOnLine(reptileOrigin, destinyCell.gridPosition);
                        //Remove origin from path   
                        if (reptilePath.Count > 0)
                            reptilePath.RemoveAt(0);
                    }

                    //Add destiny to path, jump always will be able to reach destiny
                    if (!reptilePath.Contains(destinyCell.gridPosition)) reptilePath.Add(destinyCell.gridPosition);

                    break;

                case ExpansionMode.Throw:

                    reptilePath = Board.Board.CellsOnLine(reptileOrigin, destinyCell.gridPosition);

                    //Remove origin from path   
                    if (reptilePath.Count > 0)
                    {
                        reptilePath.RemoveAt(0);

                        if (reptilePath.Count > 0)
                            reptileDestiny = reptilePath[reptilePath.Count - 1];
                    }
                    break;

            }

            Jump();

        }//Closes Expand method

        Tween reptileTween;
        private void Jump()
        {
            if (reptilePath.Count == 0) return;

            if (reptileTween != null) reptileTween.Kill();

            float3 positionReference = transform.position;

            OnEnterCell?.Invoke();

            animator.SetTrigger("OnRelease");

            reptileTween = DOTween.To(setter: value =>
                       {
                           Vector3 positionOnParabol = Parabol.EvaluateParabole(positionReference, reptilePath[0].GridToUnity(), jumpHeight, value);

                           transform.LookAt(positionOnParabol);
                           transform.position = positionOnParabol;
                        
                       }, startValue: 0, endValue: 1, duration: speed.value)
               .SetSpeedBased(true);

        }//Closes Jump method


       

        protected override void FinalPlacement()
        {
             base.FinalPlacement();

             if (!isPlaced) return;
            
             Expand();

        }
      
        protected override bool IsValidPlacement(Cell cell)
        {
            if (!base.IsValidPlacement(cell)) return false;
            //Insert aditional conditions for this Ancient Force inheritor class


            return true;
        }//Closes IsValidPlacement method

        private bool TryMutateCell(Cell cell)
        {

            if (!cell || !IsValidPlacement(cell)) return false;

            if (mode == ExpansionMode.Throw && !fillPath && !cell.gridPosition.Equals(reptileDestiny)) return true;

            ExpansionRules expRule = ruleList.Find(rule => rule.condition == cell.data);

            if (expRule.mode == MutationMode.SwapCell)
            {
                if (!expRule.cellToSwap) return false;
                Cell newCell = Instantiate(expRule.cellToSwap, Board.Board.reference.transform);
                if (!newCell) return false;

                newCell.transform.position = cell.worldPosition;

                Board.Board.reference.AddCellToBoard(newCell);
                if (newCell.TryGetComponent(out Animator cellAnimator)) cellAnimator.Play("Idle");

            }
            else if (expRule.mode == MutationMode.SwapToken)
            {
                if (!expRule.tokenToSwap) return false;
                cell.data = expRule.tokenToSwap;
            }

            return true;
        }//Closes MutateCell method

        private void OnTriggerEnter(Collider other)
        {
            if (!isPlaced) return;

            Cell cellArrived = other.transform.GetComponentInParent<Cell>();

            if (!cellArrived
            || cellArrived.gridPosition.Equals(reptileOrigin)
            || (reptilePath.Count > 0 && !cellArrived.gridPosition.Equals(reptilePath[0]))) return;

            if (reptileTween != null) reptileTween.Kill();

            reptilePath.Remove(cellArrived.gridPosition);

            if (!TryMutateCell(cellArrived)) if (mode == ExpansionMode.Throw) base.DestroyForce();

            if (!cellArrived.gridPosition.Equals(reptileDestiny)) Jump();
            else base.DestroyForce();


        }//Closes OnTriggerEnter method


         void OnDestroy()
        {
            if (reptileTween != null) reptileTween.Kill();
        }//Closes OnDestroy method

    }//Closes ExpansionReptile class
}//Closes namespace declaratio
