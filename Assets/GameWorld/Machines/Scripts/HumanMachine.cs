using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class HumanMachine : MonoBehaviour
{
    public MachineState machine_status;
    public GameObject flag;

    public void SetMachineStatus(MachineState status)
    { machine_status = status; }

    public void SetBlockedStatus()
    {
        flag.SetActive(true);
    }

}
