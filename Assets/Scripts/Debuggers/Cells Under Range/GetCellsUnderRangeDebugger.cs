using System.Collections;
using System.Collections.Generic;
using JuanPayan.CodeSnippets.HelperComponents;
using JuanPayan.Extenders;
using Ozamanas.Board;
using Ozamanas.Outline;
using Unity.Mathematics;
using UnityEngine;


namespace Ozamanas.Debuggers
{
    public class GetCellsUnderRangeDebugger : MonobehaviourEvents
    {

        [SerializeField] int range;

        [SerializeField] private Outline.OutlineConfig debugOutline;

        [SerializeField] private List<Cell> cellsUnderRange = new List<Cell>();


        private int storedRange = int.MinValue;
        private Cell storedOriginCell;


        [ContextMenu("Execute Behavior")]
        public override void Behaviour()
        {

            if (range == storedRange && storedOriginCell && storedOriginCell.gridPosition.Equals(transform.position.ToFloat3().UnityToGrid()))
            {
                PaintRange();
                return;
            }


            Cell originCell = Board.Board.GetCellByPosition(transform.position);
            storedOriginCell = originCell;
            if (!storedOriginCell)
            {
                EraseRange();
                cellsUnderRange.Clear();
                return;
            }


            EraseRange();
            cellsUnderRange = originCell.GetCellsOnRange(range, true);
            PaintRange();
            storedRange = range;
        }//Closes Behaviour method


        public void PaintRange()
        {

            foreach (var cell in cellsUnderRange)
            {
                cell.gameObject.SendMessage("DrawOutline", debugOutline);
                cell.gameObject.SendMessage("ToggleOutline", true);
            }
        }

        public void EraseRange()
        {
            foreach (var cell in cellsUnderRange)
            {
                cell.gameObject.SendMessage("ToggleOutline", false);
            }

        }
    }//Closes CellLocator class
}//Closes Namespace declaration