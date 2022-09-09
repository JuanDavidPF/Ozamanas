using System;
using System.Collections;
using System.Collections.Generic;
using Ozamanas.Board;
using Unity.Mathematics;
using UnityEngine;
using DG.Tweening;
using JuanPayan.References;
using JuanPayan.Utilities;
namespace Ozamanas.Forces
{
    public class ExpansionReptile : AncientForce
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



        [Space(10)]
        [Header("Expansion Parameters")]
        [SerializeField] private ExpansionMode mode;
        [SerializeField] private bool fillPath;
        [SerializeField] private List<ExpansionRules> ruleList = new List<ExpansionRules>();
        [SerializeField] private FloatReference reptileSpeed;


        private void Expand()
        {
            Cell destinyCell = null;

            if (CellSelectionHandler.currentCellHovered) destinyCell = CellSelectionHandler.currentCellHovered.cellReference;

            if (!destinyCell) return;

            float3 reptileDestiny = destinyCell.worldPosition;
            reptileDestiny.y = .5f;


            Cell nearestCell = Board.Board.GetNearestCellInRange(destinyCell.worldPosition, data.range.value, data.rangeAnchors.ToArray());
            if (!nearestCell) return;

            float3 reptileOrigin = nearestCell.worldPosition;



            if (mode == ExpansionMode.Jump) Jump(reptileOrigin, reptileDestiny);
            else if (mode == ExpansionMode.Throw) Throw();


        }//Closes Expand method

        Tween reptileTween;
        private void Jump(Vector3 from, Vector3 to)
        {
            if (reptileTween != null) reptileTween.Kill();
            reptileTween = DOTween.To(setter: value =>
                       {
                           transform.position = Parabol.EvaluateParabole(from, to, 1.5f, value);
                       }, startValue: 0, endValue: 1, duration: reptileSpeed.value)
               .SetSpeedBased(true)
               .OnComplete(() =>
               {
                   Cell destiny = Board.Board.GetCellByPosition(to);
                   if (!destiny) return;

                   ExpansionRules expRule = ruleList.Find(rule => rule.condition == destiny.data);

                   if (expRule.mode == MutationMode.SwapCell)
                   {
                       if (!expRule.cellToSwap) return;
                       Cell newCell = Instantiate(expRule.cellToSwap, Board.Board.reference.transform);
                       if (!newCell) return;

                       newCell.transform.position = destiny.worldPosition;

                       Board.Board.reference.AddCellToBoard(newCell);
                       if (newCell.TryGetComponent(out Animator cellAnimator)) cellAnimator.Play("Idle");

                   }
                   else if (expRule.mode == MutationMode.SwapToken)
                   {
                       if (!expRule.tokenToSwap) return;
                       destiny.data = expRule.tokenToSwap;
                   }

                   Destroy(gameObject);
               });

        }//Closes Jump method

        private void Throw()
        {

        }//Closes Jump method

        public override void FirstPlacement()
        {
            base.FirstPlacement();
            Expand();

        }//Closes FirstPlacement method

        protected override void SecondPlacement()
        {
            base.SecondPlacement();
        }//Closes SecondPlacement method

        protected override void ThirdPlacement()
        {
            base.ThirdPlacement();
        }//Closes SecondPlacement method


        protected override bool IsValidPlacement(Cell cell)
        {
            if (!base.IsValidPlacement(cell)) return false;

            //Insert aditional conditions for this Ancient Force inheritor class
            return true;
        }//Closes IsValidPlacement method


        private void OnDestroy()
        {
            if (reptileTween != null) reptileTween.Kill();
        }//Closes OnDestroy method

    }//Closes ExpansionReptile class
}//Closes namespace declaratio
