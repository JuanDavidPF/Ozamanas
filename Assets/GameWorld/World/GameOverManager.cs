using System.Collections;
using System.Collections.Generic;
using JuanPayan.References;
using UnityEngine;

namespace Ozamanas.World
{
    public class GameOverManager : MonoBehaviour
    {
        [SerializeField] GameEvent OnGameOver;
        public void VerifyGameEndingConditions()
        {
            if (WavesManager.currentWave < WavesManager.wavesAmount) return;

            foreach (var machine in Machines.HumanMachine.machines)
            {
                if (!machine) continue;

                if (machine.TryGetComponent(out Machines.MachineMovement movement))
                {
                    if (!movement.CheckIfMachineIsBlocked()) return;
                }
            }

            if (OnGameOver) OnGameOver.Invoke();

        }//Closes OnMachineDestroyed method


    }//Closes GameEndingManager method
}//Closes Namespace declaration
