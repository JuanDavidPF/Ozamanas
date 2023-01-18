using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Ozamanas.Machines;
using Ozamanas.Tags;
using Ozamanas.World;

namespace Ozamanas.Board
{
    public class JunglesHeartController : MonoBehaviour
    {

        [Space(15)]
        [Header("Cell identificators")]
        [SerializeField] private GameplayState winState;
        [SerializeField] private GameplayState loseState;

        [Space(15)]
        [Header("Events")]
        public UnityEvent LevelFailed;
        public UnityEvent OnWinState;
        public UnityEvent OnLoseState;

        public void CheckForMachineEnter(HumanMachine machine)
        {
            if(!machine) return;

            if(machine.TryGetComponent<MachinePhysicsManager>(out MachinePhysicsManager physicsManager))
            {
                if(physicsManager.state == PhysicMode.Intelligent)
                LevelFailed?.Invoke();
            }

        }

        public void OnCellGameStateChange(GameplayState state)
        {
            if (!state) return;
            if (state == winState) ChangeToWinState();
            if (state == loseState) ChangeToLoseState();
        }

        private void ChangeToWinState()
        {
            OnWinState?.Invoke();
        }

        private void ChangeToLoseState()
        {
            OnLoseState?.Invoke();
        }
        // Start is called before the first frame update
        
    }
}
