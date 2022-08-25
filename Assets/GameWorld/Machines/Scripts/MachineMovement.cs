using System.Collections;
using System.Collections.Generic;
using DataStructures.PriorityQueue;
using Ozamanas.Board;
using Ozamanas.Outline;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;


namespace Ozamanas.Machines
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class MachineMovement : MonoBehaviour
    {

        public enum PathFindingResult
        {
            NonCalculated,
            InvalidDestiny,
            PathIncomplete,
            PathComplete,

        }//Closes


        private NavMeshAgent navMeshAgent;
        [Header("Setup")]
        [SerializeField] private Board.CellData mainObjective;
           [Space(5)]
        [SerializeField] private Board.CellData secondObjective;
        [SerializeField] private int secondObjectiveRange;
           [Space(5)]
        [SerializeField] private Board.CellData thirdObjective;
         [SerializeField] private int thirdObjectiveRange;
         [Space(15)]
        [SerializeField] private List<Board.CellData> blacklist = new List<CellData>();
        
        [Space(15)]
        [Header("Parameters")]
        public PathFindingResult result = PathFindingResult.NonCalculated;
        [SerializeField] private Cell currentDestination;
        [SerializeField] private List<Cell> pathToDestination = new List<Cell>();
        
        [Space(15)]
        [Header("Debug")]
        [SerializeField] private bool debugPathFinding;
        [SerializeField] private OutlineConfig debugOutline;

        public bool hasPathToMain
        {
            get
            {
                return (mainObjective && pathToDestination.Count > 0 && mainObjective == pathToDestination[pathToDestination.Count - 1].data);
            }
        }


        // Start is called before the first frame update
        void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }//Closes Awake method


        private void Update()
        {
            DebugPathProjection();
        }//Closes Update method

        private void DebugPathProjection()
        {
            if (debugPathFinding)
            {
                foreach (var cell in pathToDestination)
                {
                    if (!cell) continue;

                    cell.gameObject.SendMessage("DrawOutline", debugOutline);
                    cell.gameObject.SendMessage("ToggleOutline", true);
                }
            }
            else
            {
                foreach (var cell in pathToDestination)
                {
                    if (!cell) continue;


                    cell.gameObject.SendMessage("ToggleOutline", false);
                }
            }
        }

        public void ResetPath()
        {
            if (debugPathFinding)
            {
                foreach (var cell in pathToDestination)
                {
                    if (!cell) continue;
                    cell.gameObject.SendMessage("ToggleOutline", false);
                }
            }
            pathToDestination.Clear();
        }//Closes ResetPath method


        public bool CheckIfReachDestination()
        {

            if (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance + 0.5f) return false;
            else return true;
        }




        public void CalculatePathToCell(Cell destiny)
        {
            if (!destiny)
            {
                result = PathFindingResult.InvalidDestiny;
                return;
            }


            ResetPath();
            /*For more explanation of how this works, please reafer to:
            https://erdiizgi.com/data-structure-for-games-priority-queue-for-unity-in-c/
            https://www.redblobgames.com/pathfinding/a-star/introduction.html
            https://pavcreations.com/pathfinding-with-a-star-algorithm-in-unity-small-game-project/
            https://www.redblobgames.com/pathfinding/a-star/implementation.html
            I love the Unity community <3
            */
            Cell start = Board.Board.GetCellByPosition(transform.position);



            Cell goal = destiny;

            PriorityQueue<Cell, int> frontier = new PriorityQueue<Cell, int>(0);
            frontier.Insert(start, 0);

            Dictionary<Cell, Cell> came_from = new Dictionary<Cell, Cell>();
            came_from[start] = null;

            Dictionary<Cell, int> cost_so_far = new Dictionary<Cell, int>();
            cost_so_far.Add(start, 0);


            while (!frontier.IsEmpty)
            {
                Cell current = frontier.Pop();

                //The algoryhtim has finished
                if (current == goal) break;

                foreach (Cell next in current.GetCellsOnRange(1, false))
                {

                    if (!next) continue;
                    if (next.data && blacklist.Contains(next.data)) continue;
                    if (next.isOccupied) continue;
                    /*Implement here early exits for board cell that dont met the conditions*/


                    /*Implement here early exits for board cell that dont met the conditions*/

                    int new_cost = cost_so_far[current] + current.DistanceTo(next) + next.data.movemenCost.value;

                    if (!cost_so_far.ContainsKey(next) || new_cost < cost_so_far[next])
                    {
                        cost_so_far[next] = new_cost;
                        int priority = new_cost + goal.DistanceTo(next) + next.data.movemenCost.value;
                        frontier.Insert(next, priority);
                        came_from[next] = current;
                    }
                }//Cloes for to check adjacent BoardCells
            }//Closes While there is BoardCells on frontier


            List<Cell> pathRoute = new List<Cell>();

            Cell currentNode = destiny;
            while (currentNode != start)
            {

                if (!currentNode)
                {
                    result = PathFindingResult.PathIncomplete;
                    return;
                }


                pathRoute.Add(currentNode);
                came_from.TryGetValue(currentNode, out currentNode);
            }

            pathRoute.Reverse();
            result = PathFindingResult.PathComplete;
            pathToDestination = pathRoute;

        }//Closes Calculate PathtoCell alogorythym


        public bool CheckIfMachineIsBlocked()
        {

         if(mainObjective==null) return true;

         Board.Cell newCell = Board.Board.GetNearestCell(transform.position,mainObjective);

         if(newCell==null) return true;

         CalculatePathToCell(newCell);

         if (result == PathFindingResult.PathComplete) return false;
         else return true;
        }

        public void SetCurrentDestination()
        {
            float3 origin = transform.position;
            Cell  firstCell = null,secondCell = null, thirdCell = null;

            if( mainObjective!= null) firstCell = Board.Board.GetNearestCell(transform.position,mainObjective);
            if( secondObjective!= null) secondCell = Board.Board.GetNearestCellInRange(transform.position,secondObjectiveRange,secondObjective);
            if( thirdObjective!= null) thirdCell = Board.Board.GetNearestCellInRange(transform.position,thirdObjectiveRange,thirdObjective);

            int distanceToMain = 1000;
            int distanceToSecondary = 1000;
            int distanceToTertiary = 1000;
            
            if(firstCell!= null) distanceToMain = firstCell.gridPosition.GridToAxial().DistanceTo(origin.UnityToGrid().GridToAxial());
            if(secondCell!= null) distanceToSecondary = secondCell.gridPosition.GridToAxial().DistanceTo(origin.UnityToGrid().GridToAxial());
            if(thirdCell!= null)  distanceToTertiary = thirdCell.gridPosition.GridToAxial().DistanceTo(origin.UnityToGrid().GridToAxial());
            
            if(distanceToMain<=distanceToSecondary) currentDestination = firstCell;
    
            else if(distanceToSecondary<=distanceToTertiary ) currentDestination = secondCell;
        
            else currentDestination = thirdCell;

            CalculatePathToCell(currentDestination);

        }


    public void MoveToNextCell()
    {
        navMeshAgent.SetDestination(pathToDestination[0].transform.position);
        pathToDestination.RemoveAt(0);
    }

    }//Closes MachineMovement class
    
  
}     
//Closes Namespace declaration 