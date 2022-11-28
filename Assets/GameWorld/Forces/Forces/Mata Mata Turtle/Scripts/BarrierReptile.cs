using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Ozamanas.Extenders;
using Ozamanas.Board;
using UnityEngine.Events;
using Ozamanas.Tags;

namespace Ozamanas.Forces
{
    public class BarrierReptile : AncientForce
    {
        Collider reptileCollider;
        [SerializeField] private float lifetime = 1f;
        private bool alreadyTriggered = false;
        private Tween reptileTween;
        [SerializeField] private GameObject fragmentedModel;
        private bool isReady = false;
        [SerializeField] private CellData barrierID;
        [SerializeField] private CellData forestID;

         [SerializeField] public UnityEvent OnDestruction;

         private Cell currentCell;

         private Animator animator;
        protected override void Awake()
        {
            base.Awake();
            reptileCollider = GetComponentInChildren<Collider>();
            reptileCollider.enabled = false;
            animator = GetComponent<Animator>();
        }

        public override void FirstPlacement()
        {
            base.FirstPlacement();
            reptileCollider.enabled = true;

            currentCell = Board.Board.GetCellByPosition(transform.position.ToFloat3().UnityToGrid());

            animator.SetTrigger("OnRelease");

            transform.position = currentCell.worldPosition;

           
        }//Closes FistPlacement method


        public void ChangeTokenToCuurentCell()
        {
            if (barrierID) currentCell.data = barrierID;
            ActivateTraits(Board.Board.GetCellByPosition(transform.position.ToFloat3().UnityToGrid()));
                isReady = true;    
        }

        private void OnCollisionEnter(Collision other)
        {

            if (!isPlaced || (other.transform.tag != "Machine")) return;

            if (!isReady && other.transform.TryGetComponent(out Machines.MachinePhysicsManager physics))
            {
                physics.SetPhysical();
            }
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

  

    }//Closes BarrierReptile class
}//Closes Namespace declaration
