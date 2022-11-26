using System.Collections;
using System.Collections.Generic;
using Ozamanas.Board;
using Ozamanas.Tags;
using Ozamanas.Energy;
using UnityEngine;
using UnityEngine.Events;

namespace Ozamanas.Machines
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(MachineArmor))]
    [RequireComponent(typeof(MachineMovement))]
    [SelectionBase]
    public class HumanMachine : MonoBehaviour
    {
        public static List<HumanMachine> machines = new List<HumanMachine>();
        [SerializeField] private MachineState machine_status;
        [SerializeField] private HumanMachineToken machine_token;
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


        private MachineArmor machineArmor;
        private MachineMovement machineMovement;
        private Animator animator;

        private Cell m_currentCell;
        public Cell CurrentCell
        {
            get { return m_currentCell; }
            set
            {

                if (m_currentCell == value) return;
                m_currentCell = value;
                OnCurrentCellChanged?.Invoke(value);
            }
        }

        public MachineState Machine_status { get => machine_status; set => machine_status = value; }

        [Space(20)]
        [Header("Events")]

        public UnityEvent OnBlockedMachine;
        public UnityEvent OnIddlingMachine;
        public UnityEvent OnRunningMachine;
        public UnityEvent<Cell> OnCurrentCellChanged;
        public UnityEvent<List<MachineTrait>> OnTraitsUpdated;
        public void SetMachineStatus(MachineState status)
        { Machine_status = status; }


        void Awake()
        {
            machineArmor = GetComponent<MachineArmor>();
            machineMovement = GetComponent<MachineMovement>();
            animator = GetComponent<Animator>();

        }

        private void OnEnable()
        {
            machines.Add(this);
        }//Closes OnEnable method


        #region States Management

        // Check Functions
        public bool CheckIfBlocked()
        {
            return Machine_status == MachineState.Blocked;
        }

        public bool CheckIfIdle()
        {
            return Machine_status == MachineState.Idling;
        }
        public bool CheckIfRunning()
        {
            return Machine_status == MachineState.Running;
        }
        public bool CheckIfActing()
        {
            return Machine_status == MachineState.Acting;
        }

        // Set Functions
        public void SetBlockedStatus()
        {
            Machine_status = MachineState.Blocked;
            animator.SetInteger("MachineState", (int)Machine_status);
            OnBlockedMachine?.Invoke();
        }

        public void SetIdlingStatus()
        {
            Machine_status = MachineState.Idling;
            animator.SetInteger("MachineState", (int)Machine_status);
            OnIddlingMachine?.Invoke();
        }

        public void SetRunningStatus()
        {
            Machine_status = MachineState.Running;
            animator.SetInteger("MachineState", (int)Machine_status);
            OnRunningMachine?.Invoke();
        }

        public void SetActingStatus()
        {
            Machine_status = MachineState.Acting;
            animator.SetInteger("MachineState", (int)Machine_status);
        }

        #endregion

        #region Traits Management

        public void AddTraitToMachine(MachineTrait trait)
        {
            activeTraits.Add(trait);
            if (!trait.isPermanentOnMachine) StartCoroutine(WaitToRemoveTrait(trait));
            SetMachineAttributes();
        }

        public void RemoveTraitToMachine(MachineTrait trait)
        {
            activeTraits.Remove(trait);
            SetMachineAttributes();
        }

        IEnumerator WaitToRemoveTrait(MachineTrait trait)
        {
            yield return new WaitForSeconds(trait.machineTimer);
            RemoveTraitToMachine(trait);
            SetMachineAttributes();
        }


        public void RestoreMachineAttributesAndTraits()
        {
            machineArmor.RestoreOriginalValues();
            machineMovement.RestoreOriginalValues();
            activeTraits = new List<MachineTrait>();
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

        public void SetMachineTraitsfromCell(Cell cell)
        {
            activeTraits.AddRange(cell.GetCellTraits());
            foreach (MachineTrait trait in cell.GetCellTraits())
            {
                if (!trait.isPermanentOnMachine) WaitToRemoveTrait(trait);
            }
            SetMachineAttributes();
        }

        public void RemoveMachineTraitsFromCell(Cell cell)
        {
            foreach (MachineTrait trait in cell.GetCellTraits())
            {
                activeTraits.Remove(trait);
            }
            SetMachineAttributes();
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

                    case MachineTraits.GotoBase:
                        machineMovement.GoToBase();
                        break;

                }
            }
        }
        #endregion


        private void OnDisable()
        {
            if (CurrentCell) CurrentCell.isOccupied = false;
            machines.Remove(this);
        }//Closes OnDisable method

        public bool CheckIfCurrentCellEqualsTo(CellData token)
        {
            if (!CurrentCell || !CurrentCell.data) return false;

            return token == CurrentCell.data;
        }
        public bool ReplaceCellDataToCurrent(CellData token)
        {
            if (CurrentCell == null) return false;

            CurrentCell.data = token;

            return true;
        }

        public bool ReduceEnergyLevelOnCurrentCell()
        {
            if (CurrentCell == null) return false;

            if(CurrentCell.TryGetComponent<EnergyGenerator>(out EnergyGenerator energyGenerator))
            {
                energyGenerator.currentLevel--;
            }

            return false;
        }


    }//Closes HumanMachine class
}