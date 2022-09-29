using System.Collections.Generic;
using JuanPayan.Helpers;
using Ozamanas.Board;
using Ozamanas.Extenders;

using UnityEngine;


namespace Ozamanas.Debuggers
{
    public class GetCellsUnderRangeDebugger : MonobehaviourEvents
    {

        [SerializeField] int range;


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

            }
        }

        public void EraseRange()
        {
            foreach (var cell in cellsUnderRange)
            {

            }

        }
    }//Closes CellLocator class
}//Closes Namespace declaration