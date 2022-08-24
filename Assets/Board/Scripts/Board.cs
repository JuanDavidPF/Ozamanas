using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Ozamanas.Board
{
    [ExecuteInEditMode]

    [RequireComponent(typeof(Grid))]
    public class Board : MonoBehaviour
    {
        public static Grid m_grid;

        public static Dictionary<Vector3Int, Cell> cellsByGridPosition = new Dictionary<Vector3Int, Cell>();
        public static Dictionary<CellData, List<Cell>> cellsByData = new Dictionary<CellData, List<Cell>>();



        private void Awake()
        {
            m_grid = m_grid ? m_grid : GetComponent<Grid>();

        }//Closes Awake method

        private void Update()
        {
            if (!Application.isPlaying) return;
            if (Input.GetKeyDown(KeyCode.Escape)) CellSelectionHandler.currentCellSelected = null;
        }//Closes Update method

        public static void AddCellToBoard(Cell cell)
        {

            if (!cell || !m_grid) return;

            cell.gridPosition = cell.transform.position.UnityToGrid();

            //Removes previous cell in this position if existing;
            Cell previousCell = null; cellsByGridPosition.TryGetValue(cell.gridPosition, out previousCell);
            if (previousCell) RemoveCellFromBoard(previousCell);



            //Add new cell to cell atlases
            cellsByGridPosition[cell.gridPosition] = cell;

            if (cell.data)
            {
                if (cellsByData.ContainsKey(cell.data)) cellsByData[cell.data].Add(cell);
                else cellsByData[cell.data] = new List<Cell>() { cell };
            }


        }//Closes AddCellToCollection method

        public static void RemoveCellFromBoard(Cell cell)
        {
            if (!cell || !m_grid) return;

            if (cellsByGridPosition.ContainsKey(cell.gridPosition) && cellsByGridPosition[cell.gridPosition] == cell)
                cellsByGridPosition.Remove(cell.gridPosition);

            if (cell.data && cellsByData.ContainsKey(cell.data)) cellsByData[cell.data].Remove(cell);

            Destroy(cell.gameObject);
        }//Closes RemoveCellFromCollections method

        public static List<Cell> GetCellsByData(params CellData[] datas)
        {
            List<Cell> cellsToReturn = new List<Cell>();

            foreach (var data in datas)
            {
                if (!cellsByData.ContainsKey(data)) continue;
                cellsToReturn.AddRange(cellsByData[data]);
            }

            return cellsToReturn;

        }//Closes GetCellsByData method

        public static Cell GetNearestCell(Vector3 origin, params CellData[] datas)
        {


            List<Cell> cellsByData = GetCellsByData(datas);
            origin.y = 0;


            cellsByData.Sort((Cell cellA, Cell cellB) =>
             {
                 int OriginToA = cellA.gridPosition.GridToAxial().DistanceTo(origin.UnityToGrid().GridToAxial());
                 int OriginToB = cellB.gridPosition.GridToAxial().DistanceTo(origin.UnityToGrid().GridToAxial());
                 return OriginToA - OriginToB;
             });

            return cellsByData.Count > 0 ? cellsByData[0] : null;

        }//Closes GetNearestCell method

    }//Closes Board class

    public static class BoardExtender
    {

        public static int DistanceTo(this Vector3Int axialCoordA, Vector3Int axialCoordB)
        {
            return Mathf.Max(Mathf.Abs(axialCoordA.x - axialCoordB.x), Mathf.Abs(axialCoordA.y - axialCoordB.y), Mathf.Abs(axialCoordA.z - axialCoordB.z));
        }//Close DistanceTo method

        public static int DistanceTo(this Cell cellA, Cell cellB)
        {

            Vector3Int axialCoordA = cellA.gridPosition.GridToAxial();
            Vector3Int axialCoordB = cellB.gridPosition.GridToAxial();

            return axialCoordA.DistanceTo(axialCoordB);
        }//Close DistanceTo method

        public static Vector3Int AxialToGrid(this Vector3Int axialVector)
        {
            var x = axialVector.x;
            var z = axialVector.z;
            var col = x + (z - (z & 1)) / 2;
            var row = z;

            return new Vector3Int(col, row, 0);
        }//Close ToGrid method


        public static Vector3Int GridToAxial(this Vector3Int gridVector)
        {
            var yCell = gridVector.x;
            var xCell = gridVector.y;
            var x = yCell - (xCell - (xCell & 1)) / 2;
            var z = xCell;
            var y = -x - z;
            return new Vector3Int(x, y, z);
        }//Close ToAxial method

        public static Vector3Int UnityToGrid(this Vector3 unityVector)
        {
            if (!Board.m_grid) return Vector3Int.zero;

            return Board.m_grid.WorldToCell(unityVector);
        }//Close ToAxial method


        public static Vector3 GridToUnity(this Vector3Int gridVector)
        {
            if (!Board.m_grid) return Vector3.zero;
            return Board.m_grid.CellToWorld(gridVector);
        }//Close ToAxial method




    }//Closes BoardExtender class
}//Closes Namespace declaration