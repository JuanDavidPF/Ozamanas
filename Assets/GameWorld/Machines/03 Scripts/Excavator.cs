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
            if (MachineMovement.CurrentAltitude == MachineAltitude.Subterrestrial) return;

            MachineMovement.CurrentAltitude = MachineAltitude.Subterrestrial;
        }

        public void GoTerrestrial()
        {
             if (MachineMovement.CurrentAltitude == MachineAltitude.Terrestrial) return;

             TryToDestroyMountain();

            MachineMovement.CurrentAltitude = MachineAltitude.Terrestrial;
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
