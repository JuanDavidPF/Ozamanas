using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Ozamanas.Machines;
using Ozamanas.Tags;

namespace Ozamanas.Board
{
    public class JunglesHeartController : MonoBehaviour
    {

        public UnityEvent LevelFailed;

        public void CheckForMachineEnter(HumanMachine machine)
        {
            if(!machine) return;

            if(machine.TryGetComponent<MachinePhysicsManager>(out MachinePhysicsManager physicsManager))
            {
                if(physicsManager.state == PhysicMode.Intelligent)
                LevelFailed?.Invoke();
            }

        }


        // Start is called before the first frame update
        
    }
}
