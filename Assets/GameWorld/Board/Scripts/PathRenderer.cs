using System.Collections;
using System.Collections.Generic;
using Ozamanas.Board;
using Ozamanas.Machines;
using UnityEngine;

namespace Ozamanas
{

    [RequireComponent(typeof(TrailRenderer))]
    public class PathRenderer : MonoBehaviour
    {
        private MachinePhysicsManager parentPhysics;
        private TrailRenderer trail;


        private List<Cell> validatedCells = new List<Cell>();


        [SerializeField] private bool conditionalPathing;
        [SerializeField] private List<CellData> allowedCells = new List<CellData>();

        private void Awake()
        {
            trail = GetComponent<TrailRenderer>();
            parentPhysics = GetComponentInParent<MachinePhysicsManager>();

            //This prevents ZFighting 
            Vector3 newPosition = transform.position;
            newPosition.y = Random.Range(0f, .01f) + newPosition.y;
            transform.position = newPosition;

        }//Closes Awake method


        public void OnNewCell(Cell cell)
        {

            if (conditionalPathing)
            {
                if (trail) trail.emitting = Validate(cell);
            }

        }//Closes OnNewCell method


        public bool Validate(Cell cell)
        {
            return cell && cell.data
            && allowedCells.Contains(cell.data)
            && (!parentPhysics || parentPhysics.state == MachinePhysicsManager.PhysicMode.Intelligent);
        }//Closes Validate method

        [ContextMenu("Reset")]
        public void ResetTrail()
        {
            if (!trail && TryGetComponent(out TrailRenderer _trail))
            {
                _trail.Clear();
            }

        }//Closes ResetTrail metdho

    }//Closes PathRenderer class
}//Closes Namespace declaration
