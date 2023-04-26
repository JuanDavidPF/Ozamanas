using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Ozamanas.Board
{
    public class CellDemolisher : MonoBehaviour
    {
        [SerializeField] private CellData waterCell;

        public void SpawnReplacement(Cell currentCell)
        {
            if(!waterCell) return;

            List<Cell> waterCells = BoardExtender.GetCellsOnRange(currentCell,1,false);

            bool hasWater = false;

            foreach(Cell cell in waterCells)
            {
                if(cell.data == waterCell) hasWater = true;
            }

            if(hasWater)  currentCell.data = waterCell;
            else
            {
                Board.RemoveCellFromBoard(currentCell);
                Board.reference.CombineTileMeshes();
            }
        }
    }
}
