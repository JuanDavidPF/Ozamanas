using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Ozamanas.Tags
{
    [System.Flags]
        public enum MachineType
    {
        Aerial = 1 << 0,
        Terrestrial = 1 << 1,
        Subterrestrial = 1 << 2,
        Nautical = 1 << 3,
        SubNautical = 1 << 4,
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
        Invulnerable
    }

    public enum MachineState
    {
        Running,
        Blocked,
        Idling,
        Acting
    }
}