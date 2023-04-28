using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Ozamanas.Board
{
    public class CellDemolisher : MonoBehaviour
    {
        [SerializeField] private CellData waterCell;
        [SerializeField] private CellData emptyCell;

        public void SpawnReplacement(Cell currentCell)
        {

            if(!currentCell) return;

            if(!waterCell || !emptyCell) return;

            List<Cell> waterCells = BoardExtender.GetCellsOnRange(currentCell,1,false);

            bool hasWater = false;

            foreach(Cell cell in waterCells)
            {
                if(cell.data == waterCell) hasWater = true;
            }

            if(hasWater)  Board.reference.ReplaceCellOnBoard(waterCell, currentCell.transform.position);
            else Board.reference.ReplaceCellOnBoard(emptyCell, currentCell.transform.position);

          
           
        }
    }
}
