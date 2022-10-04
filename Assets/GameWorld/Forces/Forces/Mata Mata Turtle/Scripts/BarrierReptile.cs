using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Ozamanas.Extenders;
using Ozamanas.Board;

namespace Ozamanas.Forces
{
    public class BarrierReptile : AncientForce
    {
        Collider collider;

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

            Cell currenCell = Board.Board.GetCellByPosition(transform.position.ToFloat3().UnityToGrid());

            currenCell.isOccupied = true;

            transform.position = currenCell.worldPosition;

            transform.DOScaleY(.5f, .5f).From(0);
        }//Closes FistPlacement method




        private void OnCollisionEnter(Collision other)
        {

            if (!isPlaced || (other.transform.tag != "Machine")) return;

            if (other.transform.TryGetComponent(out Machines.MachinePhysicsManager physics))
            {
                physics.ActivatePhysics(true);
            }

        }
    }//Closes BarrierReptile class
}//Closes Namespace declaration
