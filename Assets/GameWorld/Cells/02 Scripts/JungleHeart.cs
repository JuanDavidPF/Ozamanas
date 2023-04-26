using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Ozamanas.Machines;
using Ozamanas.Tags;
using Ozamanas.Forces;
using Sirenix.OdinInspector;

namespace Ozamanas.Board
{
    public class JungleHeart : Cell
    {

        [Space(15)]
        [Title("JungleHeart Setup:")]
        public UnityEvent LevelFailed;
        private  bool levelComplete = false;

        [SerializeField] private PhysicsForce physicsForce;
        public override void SetOnMachineEnter(HumanMachine machine)
        {
            base.SetOnMachineEnter(machine);

            if(levelComplete) return;

            if(!machine) return;

            if(machine.TryGetComponent<MachinePhysicsManager>(out MachinePhysicsManager physicsManager))
            {
                if(physicsManager.state == PhysicMode.Intelligent)
                LevelFailed?.Invoke();
                else 
                physicsManager.AddForceToMachine(physicsForce,transform.position);

            }
        }
       

        
    }
}
