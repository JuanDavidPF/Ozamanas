using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.Extenders;
using DG.Tweening;
using Ozamanas.Board;
using Ozamanas.Machines;


namespace Ozamanas.Forces
{


    public class MindControlInsect : AncientForce
    {

        private enum ControlMode
        {
            Machine,
            Cell
        }

        private Transform AOETransform;
        List<Machines.HumanMachine> machinesAffected = new List<Machines.HumanMachine>();
        private Tween insectTween;

        [SerializeField] private GameObject minion;
        [SerializeField] private ControlMode mode;
        [SerializeField] private MeshRenderer AOERenderer;

        protected override void Awake()
        {
            base.Awake();
            if (AOERenderer) AOETransform = AOERenderer.transform;

             AOETransform.position = AOETransform.position - data.draggedOffset;
        }//Closes Awake method


        protected override void FinalPlacement()
        {

            base.FinalPlacement();
          
            if (!isPlaced) return;

            AOERenderer.gameObject.SetActive(false);

            if (mode == ControlMode.Machine)
            {
                    foreach (var machine in machinesAffected.ToArray())
                    {
                        if (!machine) continue;
                        ActivateMinion(machine);
                    }

                    Destroy(gameObject);
            }
            else if(mode != ControlMode.Machine)
            {
                Cell currentCell = Board.Board.GetCellByPosition(transform.position.ToFloat3().UnityToGrid());
                InsectMinion temp = Instantiate(minion, transform.position, transform.rotation).GetComponent<InsectMinion>();
                temp.Objective = currentCell.gameObject;
                temp.Traits = data.traits;
            }

        }



        private void OnTriggerEnter(Collider other)
        {
           
            if (isPlaced || other.tag != "Machine") return;

            if (other.TryGetComponentInParent(out Machines.HumanMachine machine))
            {
                if (!machinesAffected.Contains(machine)) machinesAffected.Add(machine);

            }
            else return;

            UpdateAOEColor();
        }//Closes OnTriggerEnter method

        private void OnTriggerExit(Collider other)
        {
            if (isPlaced || other.tag != "Machine") return;

            if (other.TryGetComponentInParent(out Machines.HumanMachine machine))
                machinesAffected.Remove(machine);
            else return;
            UpdateAOEColor();

        }//Closes OnTriggerExit method

        private void UpdateAOEColor()
        {
            if (!AOERenderer) return;
            if (machinesAffected.Count == 0) AOERenderer.material.color = new Vector4(1,1,1,0.2f);
            else AOERenderer.material.color = new Vector4(0,1,0,0.2f);
        }//Closes UpdateAOEColor method


         void OnDestroy()
        {
            if (insectTween != null) insectTween.Kill();
        }//Closes OnDestroy method
    

        private void ActivateMinion(Machines.HumanMachine machine)
        {
            if (!machine || machine.tag != "Machine") return;

            machinesAffected.Remove(machine);
            InsectMinion temp = Instantiate(minion, transform.position, transform.rotation).GetComponent<InsectMinion>();
            temp.Objective = machine.gameObject;
            temp.Traits = data.traits;

        }//Closes AttemptMachineDamage method






    }//Closes OffensiveBird class
}//closes Namespace declaration
