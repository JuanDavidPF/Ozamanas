using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JuanPayan.Helpers;
using Ozamanas.Board;
using Ozamanas.Tags;

namespace Ozamanas.Machines
{
    public class TrackMarksSpawner : MonoBehaviour
    {
        private Vector3 lastPosition;
         [SerializeField] private float trackDistance = 0.2f;
         [SerializeField] private GameObject trackPrefab;
        [SerializeField] int objectPoolSize = 50;

        private ObjectPool objectPool;

        [SerializeField] private bool conditionalPathing;

         [SerializeField] private List<CellData> allowedCells = new List<CellData>();

         private bool cellCheck = true;

         private MachinePhysicsManager parentPhysics;

          private float yOffset = 0;
            private Vector3 newPosition;

        private void Awake()
        {
            objectPool = GetComponent<ObjectPool>();
            parentPhysics = GetComponentInParent<MachinePhysicsManager>();
             yOffset = Random.Range(0f, .01f);
        }

        private void Start()
        {
            lastPosition = transform.position;
            objectPool.Initialize(trackPrefab, objectPoolSize);
        }

        private void Update()
        {
        
           if(!cellCheck) return;

           if(!CheckDistanceDriven()) return;

           SpawnTrackMark();
           
        }

        private bool CheckDistanceDriven()
        {
             var distanceDriven = Vector3.Distance(transform.position, lastPosition);

             return distanceDriven >= trackDistance;
        }


        private void SpawnTrackMark()
        {
            lastPosition = transform.position;
            var tracks = objectPool.CreateObject();
            tracks.transform.rotation = transform.rotation;
            newPosition = transform.position;
            newPosition.y = yOffset;
            tracks.transform.position = newPosition;

        }

         public void OnNewCell(Cell cell)
        {

            if (conditionalPathing)
            {
                cellCheck = Validate(cell);
            }

        }//Closes OnNewCell method


        public bool Validate(Cell cell)
        {
            return cell && cell.data
            && allowedCells.Contains(cell.data)
            && (!parentPhysics || parentPhysics.state == PhysicMode.Intelligent);
        }
    }
}
