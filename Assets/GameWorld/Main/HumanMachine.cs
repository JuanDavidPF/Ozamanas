using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.Tags;
using Ozamanas.Machines;



namespace Ozamanas.Main
{

[RequireComponent(typeof(MachineArmor))]  
[RequireComponent(typeof(MachineMovement))]
[SelectionBase]
public class HumanMachine : MonoBehaviour
{
    public MachineState machine_status;
    public GameObject flag;

    [SerializeField] private List<MachineTrait> activeTraits = new List<MachineTrait>();

    private MachineArmor machineArmor;
    private MachineMovement machineMovement;
    

    public void SetMachineStatus(MachineState status)
    { machine_status = status; }


    void Awake()
    {
        machineArmor = GetComponent<MachineArmor>();
        machineMovement = GetComponent<MachineMovement>();
    }


    #region States Management

    // Check Functions
    public bool CheckIfBlocked()
    {
        return machine_status == MachineState.Blocked;
    }

       public bool CheckIfIdle()
    {
        return machine_status == MachineState.Idling;
    }
       public bool CheckIfRunning()
    {
        return machine_status == MachineState.Running;
    }
       public bool CheckIfActing()
    {
        return machine_status == MachineState.Acting;
    }

      // Set Functions
    public void SetBlockedStatus()
    {
        flag.SetActive(true);
        machine_status = MachineState.Blocked;
    }

     public void SetIdlingStatus()
    {
        machine_status = MachineState.Idling;
    }

     public void SetRunningStatus()
    {
        machine_status = MachineState.Running;
    }

      public void SetActingStatus()
    {
        machine_status = MachineState.Acting;
    }

    #endregion

    #region Traits Management

    public void AddTraitToMachine(MachineTrait trait)
    {
        activeTraits.Add(trait);
        if(!trait.isPermanentOnMachine) waitToRemoveTrait(trait);
    }

    public void removeTraitToMachine(MachineTrait trait)
    {
        activeTraits.Remove(trait);
    }

    IEnumerator waitToRemoveTrait(MachineTrait trait)
    {
        yield return new WaitForSeconds(trait.machineTimer);
        removeTraitToMachine(trait);
        SetMachineAttributes();
    }

    public void SetMachineAttributes()
    {
        machineArmor.RestoreOriginalValues();
        machineMovement.RestoreOriginalValues();

       foreach (MachineTrait trait in activeTraits)
       {
            SetMachineTrait(trait);
       }
    }

    private void SetMachineTrait(MachineTrait trait)
    {
        foreach (MachineTraits type in trait.types)
       {
            switch(type)
            {
                
                case MachineTraits.DisarmMachine:
                machineArmor.DisarmMachine();
                break;

                case MachineTraits.DoubleDamage:
                machineArmor.SetDoubleDamageOn();
                    break;

                case MachineTraits.IncreaseSpeed:
                machineMovement.IncreaseMachineSpeed();
                     break;

                case MachineTraits.Invulnerable:
                machineArmor.SetInvulnerable();
                  break;

                case MachineTraits.ReduceSpeed:
                machineMovement.DecreaseMachineSpeed();
                   break;
               
                case MachineTraits.RepairMachine:
                machineArmor.RepairMachine();
                break;

                case MachineTraits.StopMachine:
                machineMovement.StopMachine();
                 break;

            }
       }
    }
       #endregion

}
}