using System.Collections;
using System.Collections.Generic;
using Ozamanas.Extenders;
using Unity.Mathematics;
using UnityEngine;

namespace Ozamanas.Board
{
    public static class BoardExtender
    {
        public static int DistanceTo(this int3 axialCoordA, int3 axialCoordB)
        {
            return Mathf.Max(Mathf.Abs(axialCoordA.x - axialCoordB.x), Mathf.Abs(axialCoordA.y - axialCoordB.y), Mathf.Abs(axialCoordA.z - axialCoordB.z));
        }//Close DistanceTo method

        public static int DistanceTo(this Cell cellA, Cell cellB)
        {

            int3 axialCoordA = cellA.gridPosition.GridToAxial();
            int3 axialCoordB = cellB.gridPosition.GridToAxial();

            return axialCoordA.DistanceTo(axialCoordB);
        }//Close DistanceTo method

        public static int3 AxialToGrid(this int3 axialVector)
        {
            var x = axialVector.x;
            var z = axialVector.z;
            var col = x + (z - (z & 1)) / 2;
            var row = z;

            return new int3(col, row, 0);
        }//Close ToGrid method

        public static int3 GridToAxial(this int3 gridVector)
        {
            var yCell = gridVector.x;
            var xCell = gridVector.y;
            var x = yCell - (xCell - (xCell & 1)) / 2;
            var z = xCell;
            var y = -x - z;
            return new int3(x, y, z);
        }//Close ToAxial method

        public static int3 UnityToGrid(this float3 unityVector)
        {
            Board board = Board.reference;

            unityVector.y = 0;
            if (!board || !board.grid) return int3.zero;

            return board.grid.WorldToCell(unityVector).ToInt3();
        }//Close ToAxial method

        public static float3 GridToUnity(this int3 gridVector)
        {
            Board board = Board.reference;
            if (!board || !board.grid) return float3.zero;
            return board.grid.CellToWorld(gridVector.ToVector());
        }//Close ToAxial method

        public static List<Cell> GetCellsOnRange(this Cell originCell, int range = 1, bool includeOrigin = true)
        {
            if (!originCell) return null;
            List<Cell> cellsUnderRange = new List<Cell>();

            for (int x = -range; x <= range; x++)
            {
                for (int y = math.max(-range, -x - range); y <= math.min(range, -x + range); y++)
                {
                    int z = -x - y;



                    int3 originPositionToAxis = originCell.gridPosition.GridToAxial();

                    int3 underRangePosition = (originPositionToAxis + new int3(x, z, y)).AxialToGrid();


                    Cell cell = Board.GetCellByPosition(underRangePosition);


                    if (!cell) continue;
                    if (!includeOrigin && cell == originCell) continue;

                    cellsUnderRange.Add(cell);

                }

            }//closes the doubleFor


            return cellsUnderRange;
        }//Closes GetCellsUnderRange method


    }//Closes BoardExtender class
}//Closes Namespace declaration