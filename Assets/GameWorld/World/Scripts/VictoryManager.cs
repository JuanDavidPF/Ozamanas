using System.Collections;
using System.Collections.Generic;
using JuanPayan.References;
using UnityEngine;
using Ozamanas.Tags;

namespace Ozamanas.World
{
    public class VictoryManager : MonoBehaviour
    {
        [SerializeField] GameEvent VictoryEvent;
        public void VerifyGameEndingConditions()
        {
           
            

            if (WavesManager.currentWave < WavesManager.wavesAmount) return;

            

            foreach (var machine in Machines.HumanMachine.machines)
            {
                if (!machine) continue;

                 

                if (machine.TryGetComponent(out Machines.HumanMachine humanMachine))
                {
                   
                    if (humanMachine.Machine_status != MachineState.Blocked && humanMachine.Machine_status != MachineState.Destroyed  ) return;

                }
            }


            if (VictoryEvent) VictoryEvent.Invoke();

        }//Closes OnMachineDestroyed method


    }//Closes GameEndingManager method
}//Closes Namespace declaration
