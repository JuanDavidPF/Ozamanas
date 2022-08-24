using System.Collections;
using System.Collections.Generic;
using JuanPayan.CodeSnippets.HelperComponents;
using Ozamanas.Board;
using UnityEngine;


namespace Ozamanas.Debuggers
{
    public class GetNearestCellDebugger : MonobehaviourEvents
    {

        [SerializeField] private Ozamanas.Board.CellData[] cellDatas = new Ozamanas.Board.CellData[0];

        [SerializeField] private Outline.OutlineConfig debugOutline;

        private CellSelectionHandler cachedNearestCell;


        [ContextMenu("Execute Behavior")]
        public override void Behaviour()
        {

            Cell nearestCell = Board.Board.GetNearestCell(transform.position, cellDatas);

            if (!nearestCell || (cachedNearestCell && nearestCell == cachedNearestCell.cellReference)) return;

            if (cachedNearestCell) cachedNearestCell.ToggleOutline(false);

            if (nearestCell.TryGetComponent(out CellSelectionHandler selectionHandler))
            {
                cachedNearestCell = selectionHandler;
                selectionHandler.DrawOutline(debugOutline);
                selectionHandler.ToggleOutline(true);
            }


        }//Closes Behaviour method

    }//Closes CellLocator class
}//Closes Namespace declaration