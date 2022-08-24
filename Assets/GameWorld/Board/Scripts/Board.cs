using System.Collections;
using System.Collections.Generic;
using JuanPayan.Extenders;
using Unity.Mathematics;
using UnityEngine;


namespace Ozamanas.Board
{
    [ExecuteInEditMode]

    [RequireComponent(typeof(Grid))]
    public class Board : MonoBehaviour
    {
        public static Board instance;

        public Grid grid;
        [SerializeField] private List<Cell> cells = new List<Cell>();
        private Dictionary<int3, Cell> cellsByGridPosition = new Dictionary<int3, Cell>();
        private Dictionary<CellData, List<Cell>> cellsByData = new Dictionary<CellData, List<Cell>>();


        private void BakeCollections()
        {
            cells.Clear();
            cellsByData.Clear();
            cellsByGridPosition.Clear();

            foreach (Transform cellChild in transform)
            {
                if (cellChild.tag != "Cell") continue;
                if (cellChild.TryGetComponent(out Cell cell)) AddCellToBoard(cell);

            }
        }//Closes BakeCollections Methods;

        public void DeselectSelectedCell()
        {
            CellSelectionHandler.currentCellSelected = null;
        }//Closes DeselectSelectedCell method

        private void AddCellToBoard(Cell cell)
        {
            if (!cell) return;

            cell.gridPosition = cell.transform.position.ToFloat3().UnityToGrid();

            //Removes previous cell in this position if existing;
            Cell previousCell = null; cellsByGridPosition.TryGetValue(cell.gridPosition, out previousCell);
            if (previousCell) RemoveCellFromBoard(previousCell);


            //Add new cell to cell atlases
            cellsByGridPosition[cell.gridPosition] = cell;

            if (!cells.Contains(cell)) cells.Add(cell);


            if (cell.data)
            {
                if (cellsByData.ContainsKey(cell.data)) cellsByData[cell.data].Add(cell);
                else cellsByData[cell.data] = new List<Cell>() { cell };
            }


        }//Closes AddCellToCollection method

        public static void RemoveCellFromBoard(Cell cell)
        {
            if (!cell || !instance) return;

            if (instance.cellsByGridPosition.ContainsKey(cell.gridPosition) && instance.cellsByGridPosition[cell.gridPosition] == cell)
                instance.cellsByGridPosition.Remove(cell.gridPosition);

            if (cell.data && instance.cellsByData.ContainsKey(cell.data)) instance.cellsByData[cell.data].Remove(cell);

            if (Application.isPlaying) Destroy(cell.gameObject);
            else DestroyImmediate(cell.gameObject);
        }//Closes RemoveCellFromCollections method

        public static List<Cell> GetCellsByData(params CellData[] datas)
        {
            if (!instance) return null;

            List<Cell> cellsToReturn = new List<Cell>();

            foreach (var data in datas)
            {
                if (!instance.cellsByData.ContainsKey(data)) continue;
                cellsToReturn.AddRange(instance.cellsByData[data]);
            }

            return cellsToReturn;

        }//Closes GetCellsByData method

        public static Cell GetNearestCell(float3 origin, params CellData[] datas)
        {
            if (!instance) return null;

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

        private void Update()
        {
            if (Application.isPlaying) return;
            grid = grid ? grid : GetComponent<Grid>();
            BakeCollections();
        }//Closes Update method

        private void Awake()
        {

            instance = this;
        }//Closes Awake method

        private void OnDestroy()
        {
            if (instance == this) instance = null;
        }//Closes OnDestroy method


        private void Start()
        {
            StartCoroutine(HandleBoardCreation());
        }//Closes Start method

        private IEnumerator HandleBoardCreation()
        {
            yield return null;
        }//Closes HandleBoardCreation method


    }//Closes Board class


}//Closes Namespace declaration