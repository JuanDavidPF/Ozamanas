using System.Collections;
using System.Collections.Generic;
using DataStructures.PriorityQueue;
using Ozamanas.Board;
using UnityEngine.Events;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using Ozamanas.Tags;
using Sirenix.OdinInspector;


namespace Ozamanas.Machines
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(HumanMachine))]
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
        private HumanMachine humanMachine;
        private CellData mainObjective;
        private CellData mainObjectiveBackUP;
        private CellData secondObjective;
        private int secondObjectiveRange;

        private MachineSpeed currentSpeed;
        private MachineAltitude currentAltitude;
        private float timeMaxToReachDestination = 20f;
        private float timeToReachDestination = 0f;

        [Space(15)]
        [Header("Parameters")]

        public PathFindingResult result = PathFindingResult.NonCalculated;
        [SerializeField] private Cell currentDestination;
        [SerializeField] private Cell nextCellOnPath;
        [SerializeField] private List<Cell> pathToDestination = new List<Cell>();


        [Space(15)]
        [Header("Events")]
        [SerializeField] private UnityEvent OnCloseToJungleHeart;

        [SerializeField] private UnityEvent OnGoingAerial;

         [SerializeField] private UnityEvent OnGoingSubterrestrial;

          [SerializeField] private UnityEvent OnGoingTerrestrial;

        private int distanceToHeart = 0;

        [Space(15)]
        [Header("Debug")]
        [SerializeField] private bool debugPathFinding;


        public bool hasPathToMain
        {
            get
            {
                return (mainObjective && pathToDestination.Count > 0 && mainObjective == pathToDestination[pathToDestination.Count - 1].data);
            }
        }

        public MachineAltitude CurrentAltitude { get => currentAltitude; set => currentAltitude = value; }





        // Start is called before the first frame update
        void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            humanMachine = GetComponent<HumanMachine>();
            
        }//Closes Awake method

        void Start()
        {
            LoadMachineObjectivesInformation();
            LoadMachineSpeedInformation();
            RestoreOriginalValues();
           
        }

        private void LoadMachineObjectivesInformation()
        {
            mainObjective=humanMachine.Machine_token.mainObjective;
            mainObjectiveBackUP = mainObjective;
            secondObjective=humanMachine.Machine_token.secondObjective;
            secondObjectiveRange=humanMachine.Machine_token.secondObjectiveRange;
            distanceToHeart=humanMachine.Machine_token.OnDistanceToHeartNotification;
        }

        private void LoadMachineSpeedInformation()
        {
            currentSpeed=humanMachine.Machine_token.currentSpeed;
            CurrentAltitude=humanMachine.Machine_token.currentAltitude;
        }

       /* private void Update()
        {
            DebugPathProjection();
        }//Closes Update method*/

        private void DebugPathProjection()
        {
            // if (debugPathFinding)
            // {
            //     foreach (var cell in pathToDestination)
            //     {
            //         if (!cell) continue;

            //         cell.gameObject.SendMessage("DrawOutline", debugOutline);
            //         cell.gameObject.SendMessage("ToggleOutline", true);
            //     }
            // }
            // else
            // {
            //     foreach (var cell in pathToDestination)
            //     {
            //         if (!cell) continue;


            //         cell.gameObject.SendMessage("ToggleOutline", false);
            //     }
            // }
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

            if (navMeshAgent.pathPending) return false;

            if ( timeMaxToReachDestination < Time.time - timeToReachDestination ) return true;

           return navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance + 0.001f;


        }




        public void CalculatePathToDestination()
        {
            if (!currentDestination)
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

            Cell goal = currentDestination;

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
                    if (CurrentAltitude == MachineAltitude.Terrestrial && next.data && humanMachine.Machine_token.cellBlacklist.Contains(next.data)) continue;
                    if (CurrentAltitude == MachineAltitude.Terrestrial && next.isOccupied) continue;
                    /*Implement here early exits for board cell that dont met the conditions*/


                    /*Implement here early exits for board cell that dont met the conditions*/

                    int new_cost = cost_so_far[current] + current.DistanceTo(next) + next.MovementCost;

                    if (!cost_so_far.ContainsKey(next) || new_cost < cost_so_far[next])
                    {
                        cost_so_far[next] = new_cost;
                        int priority = new_cost + goal.DistanceTo(next) + next.MovementCost;
                        frontier.Insert(next, priority);
                        came_from[next] = current;
                    }
                }//Cloes for to check adjacent BoardCells
            }//Closes While there is BoardCells on frontier


            List<Cell> pathRoute = new List<Cell>();

            Cell currentNode = currentDestination;
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
            //There is no main objective
            if (mainObjective == null) return true;


            //There is no cell matching the mainobjective tag
            Cell newCell = Board.Board.GetNearestCell(transform.position, mainObjective);
            if (newCell == null) return true;

            //There is no path to main objective cell
            currentDestination = newCell;
            CalculatePathToDestination();

            if (result != PathFindingResult.PathComplete) return true;

            return false;
        }
        public void SetCurrentDestination()
        {
            // Guard: Machine does not have Main Objective or is Blocked
            
            if( currentDestination == null || result != PathFindingResult.PathComplete) return;
            
            // Machine does not have 2nd Objective
            
            if (secondObjective == null) return;
  
            Cell secondCell = Board.Board.GetNearestCellInRange(transform.position, secondObjectiveRange, secondObjective);

             // Machine 2nd Objective does not exist in Map
            
            if (secondCell == null) return;

            float3 origin = transform.position;
            int distanceToMain = currentDestination.gridPosition.GridToAxial().DistanceTo(origin.UnityToGrid().GridToAxial());
            int distanceToSecondary = secondCell.gridPosition.GridToAxial().DistanceTo(origin.UnityToGrid().GridToAxial());

            if(humanMachine.Machine_token.obviateMainObjective || distanceToMain > distanceToSecondary)
            {
                CalculatePathToDestination(secondCell);
            }
            
            if(result != PathFindingResult.PathComplete)
            {
                Cell mainObjectiveCell = Board.Board.GetNearestCell(transform.position,mainObjective);
                CalculatePathToDestination(mainObjectiveCell);
            }
            
        }

        private void CalculatePathToDestination(Cell destination)
        {
            currentDestination = destination;
            CalculatePathToDestination();
        }

        public void MoveToNextCell()
        {
            if (pathToDestination.Count == 0) return;

            nextCellOnPath = pathToDestination[0];
            navMeshAgent.SetDestination(nextCellOnPath.transform.position);
            pathToDestination.RemoveAt(0);
            timeToReachDestination = Time.time;
             CheckIfIsCloseToJungleHeart();
            
        }

        private void CheckIfIsCloseToJungleHeart()
        {
            if(distanceToHeart >= pathToDestination.Count) return;
           
            if(pathToDestination[distanceToHeart].data == mainObjectiveBackUP)
            {
                OnCloseToJungleHeart?.Invoke();
            }
           
        }

        public bool CheckIfNextCellOnPath(CellData cell)
        {
            if (pathToDestination.Count == 0) return false;

            return pathToDestination[0].data == cell;
        }

        #region Height Management

        public void GoAerial()
        {
            if (CurrentAltitude == MachineAltitude.Aerial) return;

            CurrentAltitude = MachineAltitude.Aerial;

            OnGoingAerial?.Invoke();
        }

        public void GoTerrestrial()
        {
            if (CurrentAltitude == MachineAltitude.Terrestrial) return;

            CurrentAltitude = MachineAltitude.Terrestrial;

            OnGoingTerrestrial?.Invoke();
        }

        public void GoSubterrestrial()
        {
           if (CurrentAltitude == MachineAltitude.Subterrestrial) return;

            CurrentAltitude = MachineAltitude.Subterrestrial;

            OnGoingSubterrestrial?.Invoke();
        }

        
        #endregion

        #region Objectives Management

        public void ReplaceSecondaryObjective(CellData cell, int range)
        {
            if (cell == null || range <= 0) return;

            secondObjective = cell;
            secondObjectiveRange = range;
        }



        public bool CheckIfCurrentDestinationOnRange(int range)
        {
            return range <= Board.BoardExtender.DistanceTo(currentDestination, Board.Board.GetCellByPosition(transform.position));
        }

        public bool CheckIfCurrentDestination(CellData cell)
        {
            return currentDestination.data == cell;
        }

        public void GoToBase()
        {
            if (mainObjective == humanMachine.Machine_token.humanBase) return;
            mainObjective = humanMachine.Machine_token.humanBase;
        }

        public void GotoMainObjective()
        {
            if (mainObjective == mainObjectiveBackUP) return;
            mainObjective = mainObjectiveBackUP;
        }

        #endregion

        #region Speed Management
        public void RestoreOriginalValues()
        {
            GotoMainObjective();
            currentSpeed=humanMachine.Machine_token.currentSpeed;
            navMeshAgent.speed = humanMachine.Machine_token.speedValues.GetSpeed(currentSpeed);
            if (navMeshAgent.isActiveAndEnabled && navMeshAgent.isStopped) navMeshAgent.isStopped = false;
        }

        public void IncreaseMachineSpeed()
        {
            int speed = (int)currentSpeed;
            speed++;
            speed = Mathf.Clamp(speed, 0, 4);
            currentSpeed = (MachineSpeed)speed;
            if(! navMeshAgent.isActiveAndEnabled) return;
            navMeshAgent.speed = humanMachine.Machine_token.speedValues.GetSpeed(currentSpeed);
        }

        public void DecreaseMachineSpeed()
        {
            int speed = (int)currentSpeed;
            speed--;
            speed = Mathf.Clamp(speed, 0, 4);
            currentSpeed = (MachineSpeed)speed;
             if(! navMeshAgent.isActiveAndEnabled) return;
            navMeshAgent.speed = humanMachine.Machine_token.speedValues.GetSpeed(currentSpeed);
        }

        public void StopMachine()
        {
            if(! navMeshAgent.isActiveAndEnabled) return;
            navMeshAgent.isStopped = true;
        }

        public int GetRemainingDistance()
        {
            return pathToDestination.Count;
        }




        #endregion
    }//Closes MachineMovement class


}
//Closes Namespace declaration 