using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Ozamanas.Extenders;
using Ozamanas.Board;
namespace Ozamanas.Forces
{
    public class EmbraceReptile : AncientForce
    {
        Collider clld;
        [SerializeField] private float duration = 2f;
        [SerializeField] private float riseTime = 0.2f;
        private Cell currentCell;

        private SnakeController[] controllers;

        protected override void Awake()
        {
            base.Awake();
            clld = GetComponentInChildren<Collider>();
            clld.enabled = false;
            controllers = GetComponentsInChildren<SnakeController>();
        }
        public override void FirstPlacement()
        {
            base.FirstPlacement();
            clld.enabled = true;

            currentCell = Board.Board.GetCellByPosition(transform.position.ToFloat3().UnityToGrid());

            currentCell.isOccupied = true;

            transform.position = currentCell.worldPosition;



            ActivateTraits(currentCell);




            StartCoroutine(WaitToRemoveSnake());
        }//Closes FistPlacement method

        Transform machineAffected;

        private void OnTriggerEnter(Collider other)
        {
            if (!isPlaced || (other.transform.tag != "Machine")) return;

            foreach (var snake in controllers)
            {
                if (!snake) continue;
                snake.Bite(other.transform);
            }

            Debug.Log("other");
        }


        private void OnCollisionEnter(Collision other)
        {

            if (!isPlaced || (other.transform.tag != "Machine")) return;

            if (other.transform.TryGetComponent(out Machines.MachinePhysicsManager physics))
            {

                physics.ActivatePhysics(true);
            }

        }

        IEnumerator WaitToRemoveSnake()
        {
            yield return new WaitForSeconds(duration);

            if (currentCell) currentCell.isOccupied = false;
            Destroy(gameObject);

        }

        private void ActivateTraits(Cell origin)
        {
            if (!origin) return;

            foreach (var cell in origin.GetCellsOnRange(traitRange))
            {
                if (!cell) continue;

                foreach (var trait in traits)
                {
                    if (!trait) continue;
                    cell.AddTraitToMachine(trait);
                }
            }


        }//Closes ActivateTraits method


    }
}
