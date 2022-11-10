using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Ozamanas.Extenders;
using Ozamanas.Levels;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;


namespace Ozamanas.Board
{
    [ExecuteInEditMode]



    [RequireComponent(typeof(Grid))]
    public class Board : MonoBehaviour
    {
        public UnityEvent<Cell> OnNewCell;
        public UnityEvent<Cell> OnNewCellData;
        public static Board reference;

        public LevelData levelData;

        public UnityEvent OnBoardCreated;

        [HideInInspector] public Grid grid;
        private List<Cell> cells = new List<Cell>();
        private Dictionary<int3, Cell> cellsByGridPosition = new Dictionary<int3, Cell>();
        private Dictionary<CellData, List<Cell>> cellsByData = new Dictionary<CellData, List<Cell>>();


        private void SetReference()
        {
            if (reference && reference != this)
            {
                Debug.LogWarning("A Board reference is already load up, try to unload the previous reference first");
                Destroy(gameObject);
            }

            reference = this;
        }//Closes SetReference method

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

        public void AddCellToBoard(Cell cell)
        {
            if (!cell) return;

            cell.worldPosition = cell.transform.position.ToFloat3();
            cell.gridPosition = cell.worldPosition.UnityToGrid();

            //Removes previous cell in this position if existing;
            if (cellsByGridPosition.TryGetValue(cell.gridPosition, out Cell previousCell))
            {
                RemoveCellFromBoard(previousCell);
            }


            //Add new cell to cell atlases
            cellsByGridPosition[cell.gridPosition] = cell;

            if (!cells.Contains(cell)) cells.Add(cell);


            if (cell.data)
            {
                if (cellsByData.ContainsKey(cell.data)) cellsByData[cell.data].Add(cell);
                else cellsByData[cell.data] = new List<Cell>() { cell };
            }

            OnNewCell?.Invoke(cell);
        }//Closes AddCellToCollection method

        public static void RemoveCellFromBoard(Cell cell)
        {
            Board board = Board.reference;
            if (!cell || !board) return;



            board.cells.Remove(cell);

            if (board.cellsByGridPosition.ContainsKey(cell.gridPosition) && board.cellsByGridPosition[cell.gridPosition] == cell)
                board.cellsByGridPosition.Remove(cell.gridPosition);

            if (cell.data && board.cellsByData.ContainsKey(cell.data)) board.cellsByData[cell.data].Remove(cell);

            if (Application.isPlaying) Destroy(cell.gameObject);
            else DestroyImmediate(cell.gameObject);
        }//Closes RemoveCellFromCollections method


        public static void SwappedCellData(Cell cell, CellData oldData, CellData newData)
        {
            Board board = Board.reference;

            if (!reference || !cell) return;


            if (oldData && board.cellsByData.ContainsKey(oldData)) board.cellsByData[oldData].Remove(cell);

            if (board.cellsByData.ContainsKey(newData)) board.cellsByData[newData].Add(cell);
            else board.cellsByData[newData] = new List<Cell>() { cell };

            if (newData) Board.reference.OnNewCellData?.Invoke(cell);
        }

        public static List<Cell> GetCellsByData(params CellData[] datas)
        {
            Board board = Board.reference;
            List<Cell> cellsToReturn = new List<Cell>();

            if (reference)
            {
                foreach (var data in datas)
                {
                    if (!board.cellsByData.ContainsKey(data)) continue;
                    cellsToReturn.AddRange(board.cellsByData[data]);
                }
            }

            return cellsToReturn;

        }//Closes GetCellsByData method

        public static Cell GetNearestCell(float3 origin, params CellData[] datas)
        {

            if (datas.Equals(null)) return null;
            List<Cell> cellsByData = GetCellsByData(datas);


            cellsByData.Sort((Cell cellA, Cell cellB) =>
             {
                 int OriginToA = cellA.gridPosition.GridToAxial().DistanceTo(origin.UnityToGrid().GridToAxial());
                 int OriginToB = cellB.gridPosition.GridToAxial().DistanceTo(origin.UnityToGrid().GridToAxial());
                 return OriginToA - OriginToB;
             });

            return cellsByData.Count > 0 ? cellsByData[0] : null;

        }//Closes GetNearestCell method

        public static Cell GetNearestCellInRange(float3 origin, int range, params CellData[] datas)
        {

            if (datas.Equals(null)) return null;
            List<Cell> cellsByData = GetCellsByData(datas);


            cellsByData.Sort((Cell cellA, Cell cellB) =>
             {
                 int OriginToA = cellA.gridPosition.GridToAxial().DistanceTo(origin.UnityToGrid().GridToAxial());
                 int OriginToB = cellB.gridPosition.GridToAxial().DistanceTo(origin.UnityToGrid().GridToAxial());
                 return OriginToA - OriginToB;
             });

            Cell result = cellsByData.Count > 0 ? cellsByData[0] : null;

            if (result && result.gridPosition.GridToAxial().DistanceTo(origin.UnityToGrid().GridToAxial()) <= range)
                return result;
            else
                return null;
        }


        public static List<Cell> GetNearestsCellInRange(float3 origin, int range, params CellData[] datas)
        {

            if (datas.Equals(null)) return null;
            List<Cell> cellsByData = GetCellsByData(datas);


            cellsByData.Sort((Cell cellA, Cell cellB) =>
             {
                 int OriginToA = cellA.gridPosition.GridToAxial().DistanceTo(origin.UnityToGrid().GridToAxial());
                 int OriginToB = cellB.gridPosition.GridToAxial().DistanceTo(origin.UnityToGrid().GridToAxial());
                 return OriginToA - OriginToB;
             });


            return cellsByData;
        }//Closes GetNearestsCellInRange method

        public static List<int3> CellsOnLine(int3 origin, int3 destiny)
        {
            List<int3> listToReturn = new List<int3>();
            if (!reference) return listToReturn;

            int3 originAxial = origin.GridToAxial();
            int3 destinyAxial = destiny.GridToAxial();



            int distance = origin.DistanceTo(destiny);

            float step = 0;

            for (int i = 0; i < distance; i++)
            {

                step = 1f / distance * i;

                int3 position = new float3(
                   originAxial.x + (destinyAxial.x - originAxial.x) * step,
                 originAxial.y + (destinyAxial.y - originAxial.y) * step,
                    originAxial.z + (destinyAxial.z - originAxial.z) * step
                ).RoundAxialVector();

                if (reference.cellsByGridPosition.TryGetValue(position.AxialToGrid(), out Cell cell))
                {
                    listToReturn.Add(cell.gridPosition);
                    if (i == distance - 1) listToReturn.Add(destiny);
                }
                else break;

                if (!cell) break;
            }

            return listToReturn;

        }//Closes LineBetweenCell method

        public static List<Cell> GetCellsByPosition(params float3[] worldPositions)
        {

            List<Cell> cellsByPosition = new List<Cell>();

            foreach (var position in worldPositions)
            {
                Cell cellToAdd = GetCellByPosition(position);
                if (!cellToAdd) continue;
                cellsByPosition.Add(cellToAdd);
            }

            return cellsByPosition;

        }//Closes GetNearestCell method

        public static Cell GetCellByPosition(float3 worldPosition)
        {
            Board board = Board.reference;
            if (!reference) return null;

            Cell cellAtPosition = null;

            int3 gridPosition = worldPosition.UnityToGrid();
            reference.cellsByGridPosition.TryGetValue(gridPosition, out cellAtPosition);
            return cellAtPosition;

        }//Closes GetNearestCell method

        public static Cell GetCellByPosition(int3 gridPosition)
        {
            Board board = Board.reference;
            Cell cellToReturn = null;

            if (reference) reference.cellsByGridPosition.TryGetValue(gridPosition, out cellToReturn);

            return cellToReturn;

        }//Closes GetNearestCell method





        private void Update()
        {
            if (!Application.isEditor) return;
            grid = GetComponent<Grid>();
            if (levelData) levelData.board = gameObject;
            // SetReference();
            BakeCollections();

        }//Closes Awake method



        private void Start()
        {
            if (!Application.isPlaying) return;
            SetReference();
            StartCoroutine(HandleBoardCreation());
        }//Closes Start method

        private IEnumerator HandleBoardCreation()
        {
            YieldInstruction initialDelay = levelData ? new WaitForSeconds(levelData.creationDelay.value) : new WaitForSeconds(1f);
            YieldInstruction cooldown = levelData && levelData.creationRate.value > 0 ? new WaitForSeconds(1f / levelData.creationRate.value) : new WaitForSeconds(.1f);

            yield return initialDelay;


            foreach (var cell in new List<Cell>(cells))
            {
                if (!cell.visuals) continue;

                cell.visuals.transform.DOMoveY(0, .3f).From(5);
                cell.visuals.gameObject.SetActive(true);
                yield return cooldown;
            }

            OnBoardCreated?.Invoke();
        }//Closes HandleBoardCreation method


        private void OnDestroy()
        {
            if (reference == this) reference = null;
        }//Closes OnDisalbeMethod
    }//Closes Board class


}//Closes Namespace declaration