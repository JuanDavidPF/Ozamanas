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

            controllers = GetComponentsInChildren<SnakeController>();
        }
        public override void FirstPlacement()
        {
            base.FirstPlacement();


            currentCell = Board.Board.GetCellByPosition(transform.position.ToFloat3().UnityToGrid());

            currentCell.isOccupied = true;

            transform.position = currentCell.worldPosition;



            ActivateTraits(currentCell);




            StartCoroutine(WaitToRemoveSnake());
        }//Closes FistPlacement method

        protected override void FinalPlacement()
        {

            base.FinalPlacement();
            clld.enabled = false;


            if (!nearMachine)
            {
                Destroy(gameObject);
                return;

            }

            foreach (var snake in controllers)
            {
                if (!snake) continue;
                snake.Bite(nearMachine);
            }

        }

        Transform nearMachine;


        private void OnTriggerEnter(Collider other)
        {

            if (isPlaced || other.transform.tag != "Machine") return;


            nearMachine = other.transform;


        }

        private void OnTriggerExit(Collider other)
        {

            if (isPlaced || other.transform.tag != "Machine" || other != nearMachine) return;

            nearMachine = null;
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
