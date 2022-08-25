using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Ozamanas.Machines
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
}