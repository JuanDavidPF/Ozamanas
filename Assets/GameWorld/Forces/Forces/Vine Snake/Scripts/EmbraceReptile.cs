using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Ozamanas.Extenders;
using Ozamanas.Board;
namespace Ozamanas.Forces
{
    public class EmbraceReptile :  AncientForce
    {
        Collider collider;
        [SerializeField] private float duration = 2f;
        [SerializeField] private float riseTime = 0.2f;
        private Cell currentCell;

         protected override void Awake()
        {
            base.Awake();
            collider = GetComponentInChildren<Collider>();
            collider.enabled = false;
        }
        public override void FirstPlacement()
        {
            base.FirstPlacement();
            collider.enabled = true;

            currentCell = Board.Board.GetCellByPosition(transform.position.ToFloat3().UnityToGrid());

            currentCell.isOccupied = true;

            transform.position = currentCell.worldPosition;

            transform.DOScaleY(.5f, riseTime).From(0);

            StartCoroutine(WaitToRemoveTrait());
        }//Closes FistPlacement method

        private void OnCollisionEnter(Collision other)
        {

            if (!isPlaced || (other.transform.tag != "Machine")) return;

            if (other.transform.TryGetComponent(out Machines.MachinePhysicsManager physics))
            {
                physics.ActivatePhysics(true);
            }

        }

        IEnumerator WaitToRemoveTrait()
        {
            yield return new WaitForSeconds(duration);
            transform.DOScaleY(0f, riseTime);
            if(currentCell) currentCell.isOccupied = false;
            base.OnDestroy();

        }


    }
}
