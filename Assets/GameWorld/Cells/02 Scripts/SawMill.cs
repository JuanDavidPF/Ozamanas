using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.Machines;
using Ozamanas.Extenders;

namespace Ozamanas.Board
{
    public class SawMill : MonoBehaviour
    {   
        private Animator animator;

        [SerializeField] private HumanMachineToken loggingTruckToken;


        private void Awake()
        {
            animator = GetComponent<Animator>();
        }
         private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Machine") return;

            if(other.TryGetComponentInParent<LoggingTruck>(out LoggingTruck machine))
            {
                if(machine.Machine_token == loggingTruckToken ) animator.SetTrigger("Drop");

            }

           
           
        }
    }
}