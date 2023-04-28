using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Ozamanas.Board;
using UnityEditor;

namespace Ozamanas.Machines
{
    public class WormMovement : MachineMovement
    {   
        [SerializeField] private float wormElevation = 1.5f;

        [SerializeField] private GameObject pathPrefab;

        private CinemachineSmoothPath path;
        private CinemachineDollyCart cart;

        public CinemachineDollyCart Cart { get => cart; set => cart = value; }

        protected override void Awake()
        {
            humanMachine = GetComponent<HumanMachine>();
            Cart = GetComponent<CinemachineDollyCart>();
        }


        public override void SetCurrentDestination()
        {

            Cell initCell = Board.Board.GetCellByPosition(transform.position);
            
            if(initCell && initCell!= pathToDestination[0]) pathToDestination.Insert(0,initCell);

            CreateNewSmoothPath();

            path.m_Waypoints = CreateNewSmoothPathWayPoints();

            Cart.m_Path = path;

            Cart.m_Position = 0;

            Cart.m_Speed = humanMachine.Machine_token.speedValues.GetSpeed(currentSpeed);
        }

        public override void MoveToNextCell()
        {
            if (pathToDestination.Count == 0) return;

            nextCellOnPath = pathToDestination[0];
            pathToDestination.RemoveAt(0);
            CheckIfIsCloseToJungleHeart();
        }

        private CinemachineSmoothPath.Waypoint[] CreateNewSmoothPathWayPoints()
        {
            CinemachineSmoothPath.Waypoint[] waypoints = new CinemachineSmoothPath.Waypoint[pathToDestination.Count*2-1];

            Vector3 position = new Vector3(0,0,0);

            int j = 0;

            for(int i = 0; i < waypoints.Length;i++)
            {
                 if(i%2==0)
                 {
                    waypoints[i].position =  pathToDestination[j].transform.position;
                    j++;
                 }
                 else
                 {
                    position = Vector3.Lerp(pathToDestination[j].transform.position,pathToDestination[j-1].transform.position,0.5f);
                    if(j%2==0) position.y -= wormElevation;
                    else position.y += wormElevation;
                    waypoints[i].position = position;
                 }
            }

            CinemachineSmoothPath.Waypoint lastPoint = new CinemachineSmoothPath.Waypoint();
            lastPoint.position = pathToDestination[pathToDestination.Count-1].transform.position;
            lastPoint.position.y -= 1.5f;
            ArrayUtility.Add(ref waypoints,lastPoint);

            return waypoints;
        }

        
        private void CreateNewSmoothPath()
        {
           if(path) return;
           path = Instantiate(pathPrefab,new Vector3(0,0,0), Quaternion.identity).GetComponent<CinemachineSmoothPath>();
        }

       


    }
}
