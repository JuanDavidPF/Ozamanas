using System.Collections;
using System.Collections.Generic;
using JuanPayan.Helpers;
using Ozamanas.Board;
using Ozamanas.Outlines;
using UnityEngine;


namespace Ozamanas.Debuggers
{
    public class GetNearestCellDebugger : MonobehaviourEvents
    {

        [SerializeField] private CellData[] cellDatas = new CellData[0];

        [SerializeField] private OutlineConfig debugOutline;

        private Cell cachedNearestCell;


        [ContextMenu("Execute Behavior")]
        public override void Behaviour()
        {

            Cell nearestCell = Board.Board.GetNearestCell(transform.position, cellDatas);

            if (!nearestCell) return;

            if (cachedNearestCell && nearestCell == cachedNearestCell)
            {
                cachedNearestCell.SendMessage("DrawOutline", debugOutline, SendMessageOptions.DontRequireReceiver);
                cachedNearestCell.SendMessage("ToggleOutline", true, SendMessageOptions.DontRequireReceiver);

                return;
            }

            if (cachedNearestCell) cachedNearestCell.SendMessage("ToggleOutline", false);


            cachedNearestCell = nearestCell;
            cachedNearestCell.SendMessage("DrawOutline", debugOutline, SendMessageOptions.DontRequireReceiver);
            cachedNearestCell.SendMessage("ToggleOutline", true, SendMessageOptions.DontRequireReceiver);



        }//Closes Behaviour method

    }//Closes CellLocator class
}//Closes Namespace declaration