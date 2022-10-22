using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.Extenders;
using Ozamanas.Machines;
namespace Ozamanas.Board
{

    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Animator))]
public class ElevatorController : MonoBehaviour
{

      private Animator animator;
      private bool gatesOpened = false;
      private bool machineActivated = false;

      private MachinePhysicsManager currentMachine = null; 


      void Awake()
        {
            animator = GetComponent<Animator>();
        }
    // Start is called before the first frame update
    public void OpenGates()
    {
        if(gatesOpened) return;
        animator.SetBool("OpenGates",true);
        gatesOpened = true;
    }
    
    public void ElevatorIsUP()
    {
        if(!currentMachine) return;
        currentMachine.ActivatePhysics(true);
    }
    public void CloseGates()
    {
        if(!gatesOpened) return;
        animator.SetBool("OpenGates",false);
        gatesOpened = false;
    }

     private void OnTriggerExit(Collider other)
        {
            if (other.tag != "Machine") return;
           
            CloseGates();
            machineActivated = false;
            currentMachine = null;
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.tag != "Machine") return;

            if(!gatesOpened) return;

            if (machineActivated) return;

             if (other.TryGetComponentInParent(out Machines.MachinePhysicsManager machine))
            {
                currentMachine = machine;
                currentMachine.ActivatePhysics(false);
                machineActivated = true;
            }
            else return;

        }
    
}
}
