using System.Collections;
using System.Collections.Generic;
using Ozamanas.Board;
using Ozamanas.Machines;
using Ozamanas.Tags;
using UnityEngine;

namespace Ozamanas.Machines
{

    [RequireComponent(typeof(TrailRenderer))]
    public class PathRenderer : MonoBehaviour
    {
        private Transform _t;
        private Vector3 newPosition;

        private Transform machineTransform;
        private MachinePhysicsManager parentPhysics;
        private TrailRenderer trail;


        private List<Cell> validatedCells = new List<Cell>();


        [SerializeField] private bool conditionalPathing;
        [SerializeField] private List<CellData> allowedCells = new List<CellData>();
        float yOffset = 0;

        private void Awake()
        {
            yOffset = Random.Range(0f, .01f);

            _t = transform;
            yOffset = Random.Range(0f, .01f);
            trail = GetComponent<TrailRenderer>();

            parentPhysics = GetComponentInParent<MachinePhysicsManager>();
            if (parentPhysics) machineTransform = parentPhysics.transform;


            _t.SetParent(null);

        }//Closes Awake method

        private void Update()
        {
            if (!machineTransform) return;

            newPosition = machineTransform.position;
            newPosition.y = yOffset;
            _t.position = newPosition;
        }



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
            && (!parentPhysics || parentPhysics.state == PhysicMode.Intelligent);
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
