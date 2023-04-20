using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.Energy;
using Ozamanas.Board;

namespace Ozamanas.Machines
{
    public class LoggingTruck : HumanMachine
    {
        [SerializeField] private GameObject woddenLogs;

        [SerializeField] private CellData sawMill;
        [SerializeField] private CellData emptyEnergyPool;


        protected override void Start()
        {
            base.Start();
            SetMachineUnLoaded();

        }
        public void SetMachineLoaded()
        {
            MachineMovement.ReplaceSecondaryObjective(machine_token.humanBase,100);
            machineArmor.RepairMachine();
            ReduceEnergyLevelOnCurrentCell();
            woddenLogs.SetActive(true);

            if (CheckIfEnergyLevelIsDepleted())
            {
                MachineMovement.ReplaceSecondaryObjective(machine_token.secondObjective,machine_token.secondObjectiveRange);
                CurrentCell.data = emptyEnergyPool;
                CurrentCell.CurrentTopElement = emptyEnergyPool.defaultTopElement;
                return;
            }
        }

         public void SetMachineUnLoaded()
        {
            MachineMovement.ReplaceSecondaryObjective(sawMill,3);
            woddenLogs.SetActive(false);
        }


        public void BuildSawMill()
        {
            if(!sawMill || !sawMill.defaultTopElement) return;

            CurrentCell.data = sawMill;
            CurrentCell.CurrentTopElement = sawMill.defaultTopElement;

            MachineMovement.ReplaceSecondaryObjective(sawMill,3);
        
        }

        private void ReduceEnergyLevelOnCurrentCell()
        {
            if (CurrentCell == null) return;

            if(CurrentCell.TryGetComponent<EnergyGenerator>(out EnergyGenerator energyGenerator))
            {
                energyGenerator.currentLevel--;
            }

            return;
        }

        private bool CheckIfEnergyLevelIsDepleted()
        {
            if(CurrentCell.TryGetComponent<EnergyGenerator>(out EnergyGenerator energyGenerator))
            {
                return energyGenerator.currentLevel==0;
            }

            return false;
        }


        
    }
}
