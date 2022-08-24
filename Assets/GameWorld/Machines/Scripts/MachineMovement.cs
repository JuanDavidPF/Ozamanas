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

        [SerializeField] private Board.CellData mainObjective;
        [SerializeField] private List<Board.CellData> blacklist = new List<CellData>();
        [SerializeField] private List<Cell> pathToObjective = new List<Cell>();


        [Space(15)]
        [Header("Debug")]
        [SerializeField] private bool debugPathFinding;
        [SerializeField] private OutlineConfig debugOutline;
        public float3 current_destination;

        [Space(15)]
        [Header("Parameters")]
        public PathFindingResult result = PathFindingResult.NonCalculated;

        public bool hasPathToMain
        {
            get
            {
                return (mainObjective && pathToObjective.Count > 0 && mainObjective == pathToObjective[pathToObjective.Count - 1].data);
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
                foreach (var cell in pathToObjective)
                {
                    if (!cell) continue;

                    cell.gameObject.SendMessage("DrawOutline", debugOutline);
                    cell.gameObject.SendMessage("ToggleOutline", true);
                }
            }
            else
            {
                foreach (var cell in pathToObjective)
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
                foreach (var cell in pathToObjective)
                {
                    if (!cell) continue;
                    cell.gameObject.SendMessage("ToggleOutline", false);
                }
            }
            pathToObjective.Clear();
        }//Closes ResetPath method



        [ContextMenu("Go to main")]
        public void GoToMainObjective()
        {
            if (!mainObjective) return;
            Cell objective = Board.Board.GetNearestCell(transform.position, mainObjective);

            if (objective)
            {
                CalculatePathToCell(objective);
                SetDestination(objective.transform.position);

            }
        }//Closes GoToMainObjective method;


        public void SetDestination(float3 destination)
        {
            current_destination = destination;
        }

        public bool GoToDestination()
        {
            if (current_destination.Equals(null)) return false;

            return navMeshAgent.SetDestination(current_destination);

        }

        public NavMeshPathStatus CalculatePath()
        {
            NavMeshPath path = new NavMeshPath();
            navMeshAgent.CalculatePath(current_destination, path);
            return path.status;
        }

        public bool CheckIfReachDestination()
        {

            if (navMeshAgent.pathPending) return false;
            if (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance) return false;
            if (navMeshAgent.velocity.sqrMagnitude != 0f) return false;
            return true;
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
            pathToObjective = pathRoute;

        }//Closes Calculate PathtoCell alogorythym
    }//Closes MachineMovement class



    public struct PathFindingJob : IJob
    {
        public void Execute()
        {

        }//Closes Execute method
    }//Closes PathFindingJob method
}//Closes Namespace declaration 