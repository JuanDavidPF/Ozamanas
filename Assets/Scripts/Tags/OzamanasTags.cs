using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Ozamanas.Tags
{

        public enum MachineType
    {
        Aerial = -1,
        Terrestrial = 0,
        Subterrestrial = 1,
        Nautical = 2,
        SubNautical = 3,
    }

    public enum MachineHierarchy
{
    Boss = 0,
    Regular =1
}

    public enum MachineSpeed
    {
        Reverse = -1,
        VeryLow =0,
        Low =1,
        Medium = 2,
        High =3 ,
        VeryHigh = 4
    }

    public enum MachineTraits
    {
        IncreaseSpeed ,
        ReduceSpeed ,
        StopMachine ,
        DisarmMachine ,
        RepairMachine ,
        DoubleDamage,
        Invulnerable,
        GotoBase
    }

    public enum MachineState
    {
        Running,
        Blocked,
        Idling,
        Acting
    }
}