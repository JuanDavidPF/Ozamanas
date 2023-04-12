using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.Tags;
using Ozamanas.Board;
using Ozamanas.Forest;


namespace Ozamanas.Machines
{
    public class Excavator : HumanMachine
    {

        [SerializeField] private CellData barrierId;
        public void GoSubterrestrial()
        {
            if (machineMovement.CurrentAltitude == MachineAltitude.Subterrestrial) return;

            machineMovement.CurrentAltitude = MachineAltitude.Subterrestrial;
        }

        public void GoTerrestrial()
        {
             if (machineMovement.CurrentAltitude == MachineAltitude.Terrestrial) return;

             TryToDestroyMountain();

            machineMovement.CurrentAltitude = MachineAltitude.Terrestrial;
        }

        public void TryToDestroyMountain()
        {
            if(CurrentCell.data != barrierId) return;

            Mountain temp = CurrentCell.GetComponentInChildren<Mountain>();

            if(!temp) return;

            temp.DestroyMountain();
        }
    }
}
