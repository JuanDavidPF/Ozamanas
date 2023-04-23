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

      
        private Tween insectTween;

        [SerializeField] private GameObject minion;

        [SerializeField]  private List<MachineTrait> traits;

        protected override void Awake()
        {
            base.Awake();

        }//Closes Awake method


        protected override void FinalPlacement()
        {

            base.FinalPlacement();
          
            if (!isPlaced) return;

           foreach (var machine in machinesAffected.ToArray())
            {
                if (!machine) continue;
                ActivateMinion(machine);
            }

            base.DestroyForce();
        }



       
      
       

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
            temp.Traits = traits;

        }//Closes AttemptMachineDamage method






    }//Closes OffensiveBird class
}//closes Namespace declaration
