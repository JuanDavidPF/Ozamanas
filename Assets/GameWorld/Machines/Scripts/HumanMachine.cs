using System.Collections;
using System.Collections.Generic;
using Ozamanas.Board;
using Ozamanas.Tags;
using UnityEngine;
using UnityEngine.Events;

namespace Ozamanas.Machines
{

    [RequireComponent(typeof(MachineArmor))]
    [RequireComponent(typeof(MachineMovement))]
    [SelectionBase]
    public class HumanMachine : MonoBehaviour
    {
        public MachineState machine_status;
        public GameObject flag;

        [SerializeField] private List<MachineTrait> m_activeTraits = new List<MachineTrait>();

        public List<MachineTrait> activeTraits
        {
            get { return m_activeTraits; }
            set
            {
                m_activeTraits = value;
                OnTraitsUpdated?.Invoke(value);
            }
        }

        public UnityEvent<List<MachineTrait>> OnTraitsUpdated;
        private MachineArmor machineArmor;
        private MachineMovement machineMovement;

        private Cell currentCell;

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
            if (!trait.isPermanentOnMachine) waitToRemoveTrait(trait);
            OnTraitsUpdated?.Invoke(activeTraits);
        }

        public void removeTraitToMachine(MachineTrait trait)
        {
            activeTraits.Remove(trait);
            OnTraitsUpdated?.Invoke(activeTraits);
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
                switch (type)
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



        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Cell") return;

            if (other.TryGetComponent(out Cell cell))
            {
                currentCell = cell;
            }

        }//Closes OnTriggerEnter method


        private void OnTriggerExit(Collider other)
        {
            if (other.tag != "Cell") return;

            if (other.TryGetComponent(out Cell cell))
            {
                cell.isOccupied = false;
                activeTraits = new List<MachineTrait>();
            }

        }//Closes OnTriggerExit method


        private void OnTriggerStay(Collider other)
        {
            if (other.tag != "Cell") return;

            if (currentCell.gameObject != other.gameObject) other.TryGetComponent(out currentCell);

            currentCell.isOccupied = true;
            activeTraits = currentCell.GetCellTraits();

        }//Closes OnTriggerStay method



        private void OnDisable()
        {
            if (currentCell) currentCell.isOccupied = false;
        }//Closes OnDisable method

    }//Closes HumanMachine class
}