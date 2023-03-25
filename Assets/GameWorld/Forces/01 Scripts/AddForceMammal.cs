using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.Extenders;
using DG.Tweening;
using Ozamanas.Board;
using Ozamanas.Tags;
using Ozamanas.Machines;

namespace Ozamanas.Forces
{
    public class AddForceMammal : AncientForce
    {
       
        private Transform AOETransform;
        [SerializeField] private MeshRenderer AOERenderer;
        private List<MachineArmor> machinesAffected = new List<MachineArmor>();
        private List<Cell> cellsAffected = new List<Cell>();
        [SerializeField] private float mammalSpeed = 2f;

        private Tween mammalTween;
        private Animator animator;

        private Cell currentCell;

         [SerializeField] private bool addForceOnTweenComplete;

        protected override void Awake()
        {
            base.Awake();

            if (AOERenderer) AOETransform = AOERenderer.transform;

            AOETransform.position = AOETransform.position - data.draggedOffset;

            animator = GetComponent<Animator>();
        }//Closes Awake method

        protected override void FinalPlacement()
        {

            base.FinalPlacement();

            if (!isPlaced) return;

            animator.SetTrigger("OnRelease");

            AOERenderer.gameObject.SetActive(false);

            mammalTween = transform.DOMoveY(0, mammalSpeed, false).SetSpeedBased();

            mammalTween.OnComplete(() =>
            {

               currentCell.CurrentTopElement = data.GetTopElementToSwap(currentCell);

               currentCell.data = data.GetTokenToSwap(currentCell);

               if(!addForceOnTweenComplete) return;
               
               AddForceToMachinesInRange();

               base.DestroyForce();

            });

           

        }//Closes FirstPlacement method

        public void AddForceToMachinesInRange()
        {
             ActivateTraits(Board.Board.GetCellByPosition(transform.position.ToFloat3().UnityToGrid()));
                
                foreach (var machine in machinesAffected.ToArray())
                {
                    if (!machine) continue;
                    AddForceToMachine(machine);                    
                }
                
           
        }
        public override void Drag()
        {
            base.Drag();
            
            if (Board.CellSelectionHandler.currentCellHovered || !data.snapToGrid)
            {
                if(data.physicsForce.type == AddForceType.FrontFlip)
                Board.CellSelectionHandler.currentCellHovered.cellReference.CellOverLay.ActivatePointer(CellPointerType.PullPointer);
                else
                Board.CellSelectionHandler.currentCellHovered.cellReference.CellOverLay.ActivatePointer(CellPointerType.PushPointer);

                if(currentCell != Board.CellSelectionHandler.currentCellHovered.cellReference)
                {
                    if(currentCell)
                    {
                        currentCell.CellOverLay.DeActivatePointer(CellPointerType.PushPointer);
                        currentCell.CellOverLay.DeActivatePointer(CellPointerType.PullPointer);
                    }

                    currentCell = Board.CellSelectionHandler.currentCellHovered.cellReference;
                }
            }
            
        }

        private void ActivateTraits(Cell origin)
        {
            if (!origin) return;

            foreach (var cell in origin.GetCellsOnRange(data.traitRange))
            {
                if (!cell) continue;

                foreach (var trait in data.traits)
                {
                    if (!trait) continue;
                    cell.AddTraitToMachine(trait);
                }
            }


        }//Closes ActivateTraits method


        private void AddForceToMachine(Machines.MachineArmor machine)
        {
           
            if (!machine || machine.tag != "Machine") return;

            machinesAffected.Remove(machine);


             if (machine.TryGetComponent(out Machines.HumanMachine h_machine))
            {
                h_machine.SetMachineStatus(MachineState.AddForce);
            }

            if (machine.TryGetComponent(out Machines.MachinePhysicsManager physics))
            {
                if(!data.physicsForce) return;

                physics.AddForceToMachine(data.physicsForce,gameObject.transform.position);
            }

           

        }//Closes AttemptMachineDamage method

        private void OnTriggerEnter(Collider other)
        {

            if (isPlaced) return;
            
            if (other.tag == "Machine" && other.TryGetComponentInParent(out Machines.MachineArmor armor))
            {
                if (!machinesAffected.Contains(armor)) machinesAffected.Add(armor);
                UpdateCellsAffected();
            }



        }//Closes OnTriggerEnter method


        private void OnTriggerExit(Collider other)
        {
            if (isPlaced) return;

            if (other.tag == "Machine" && other.TryGetComponentInParent(out Machines.MachineArmor armor))
            {
                machinesAffected.Remove(armor);
                UpdateCellsAffected();
            }


        }
        

        void OnDestroy()
        {
            if (mammalTween != null) mammalTween.Kill();

            foreach ( Cell cell in cellsAffected)
            cell.CellOverLay.DeActivatePointer(CellPointerType.AttackRangePointer);
        }

        private void UpdateCellsAffected()
        {
            HumanMachine machine;

            foreach ( Cell cell in cellsAffected)
            cell.CellOverLay.DeActivatePointer(CellPointerType.AttackRangePointer);

            cellsAffected = new List<Cell>();

            foreach ( MachineArmor armor in machinesAffected)
            {
                machine = armor.GetComponent<HumanMachine>();

                if(cellsAffected.Contains(machine.CurrentCell)) continue;

                cellsAffected.Add(machine.CurrentCell);
            }

            foreach ( Cell cell in cellsAffected)
            cell.CellOverLay.ActivatePointer(CellPointerType.AttackRangePointer);
        }

       


    }



}
