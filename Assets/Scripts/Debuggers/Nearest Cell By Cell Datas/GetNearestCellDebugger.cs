using System.Collections;
using System.Collections.Generic;
using JuanPayan.Helpers;
using Ozamanas.Board;

using UnityEngine;


namespace Ozamanas.Debuggers
{
    public class GetNearestCellDebugger : MonobehaviourEvents
    {

        [SerializeField] private CellData[] cellDatas = new CellData[0];


        private Cell cachedNearestCell;


        [ContextMenu("Execute Behavior")]
        public override void Behaviour()
        {

            Cell nearestCell = Board.Board.GetNearestCell(transform.position, cellDatas);

            if (!nearestCell) return;

            if (cachedNearestCell && nearestCell == cachedNearestCell)
            {

                return;
            }

            if (cachedNearestCell) cachedNearestCell.SendMessage("ToggleOutline", false);


            cachedNearestCell = nearestCell;




        }//Closes Behaviour method

    }//Closes CellLocator class
}//Closes Namespace declaration