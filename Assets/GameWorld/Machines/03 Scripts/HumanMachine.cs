using System.Collections;
using System.Collections.Generic;
using Ozamanas.Board;
using Ozamanas.Forces;
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
        [SerializeField] protected HumanMachineToken machine_token;
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

        private AncientForce hijacker;
        protected  MachineArmor machineArmor;
        private MachineMovement machineMovement;

        private MachinePhysicsManager machinePhysics;
        private Animator animator;

        private TraitVFXController traitVFXController;


        [SerializeField] private Cell m_currentCell;
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
        public HumanMachineToken Machine_token { get => machine_token; set => machine_token = value; }
        public MachineMovement MachineMovement { get => machineMovement; set => machineMovement = value; }

        [Space(20)]
        [Header("Events")]

        public UnityEvent OnBlockedMachine;
        public UnityEvent OnIddlingMachine;
        public UnityEvent OnRunningMachine;
        public UnityEvent<Cell> OnCurrentCellChanged;
        public UnityEvent<List<MachineTrait>> OnTraitsUpdated;
        
        public void SetMachineStatus(MachineState status) 
        { 
            if(Machine_status == MachineState.Captured && status != MachineState.Captured && hijacker)
            {
                hijacker.DetachHumanMachine();
                hijacker = null;
            } 

            Machine_status = status;
         }

         protected virtual void Start()
         {
         }

        protected virtual void Awake()
        {
            machineArmor = GetComponent<MachineArmor>();
            MachineMovement = GetComponent<MachineMovement>();
            animator = GetComponent<Animator>();
            machinePhysics = GetComponent<MachinePhysicsManager>();
            traitVFXController = GetComponentInChildren<TraitVFXController>();

        }

        private void OnEnable()
        {
            machines.Add(this);
        }//Closes OnEnable method


        #region States Management

        public MachineHierarchy GetMachineType()
        {
            return machine_token.machineHierarchy;
        }

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
            SetMachineStatus(MachineState.Blocked);
            animator.SetInteger("MachineState", (int)Machine_status);
            OnBlockedMachine?.Invoke();
        }

        public void SetIdlingStatus()
        {
            SetMachineStatus(MachineState.Idling);
            animator.SetInteger("MachineState", (int)Machine_status);
            OnIddlingMachine?.Invoke();
        }

        public void SetRunningStatus()
        {
            SetMachineStatus(MachineState.Running);
            animator.SetInteger("MachineState", (int)Machine_status);
            OnRunningMachine?.Invoke();
        }

        public void SetActingStatus()
        {
            SetMachineStatus(MachineState.Acting);
            animator.SetInteger("MachineState", (int)Machine_status);
        }

        public void SetCapturedStatus(AncientForce force)
        {
            if(!force) return;

            hijacker = force;
            SetMachineStatus(MachineState.Captured);
            animator.SetInteger("MachineState", (int)Machine_status);
        }

        #endregion

        #region Traits Management

        public void AddTraitToMachine(MachineTrait trait)
        {
            if(!trait) return;

            if(activeTraits.Contains(trait)) return;

            activeTraits.Add(trait);
            
            if (!trait.isPermanentOnMachine) StartCoroutine(WaitToRemoveTrait(trait));
            
            SetMachineAttributes();
        }

        public void RemoveTraitToMachine(MachineTrait trait)
        {
             if(!activeTraits.Contains(trait)) return;
            
            if(activeTraits.Remove(trait)) SetMachineAttributes();
        }

        IEnumerator WaitToRemoveTrait(MachineTrait trait)
        {
            yield return new WaitForSeconds(trait.machineTimer);
            RemoveTraitToMachine(trait);
        }


        public void RestoreMachineAttributesAndTraits()
        {
            machineArmor.RestoreOriginalValues();
            MachineMovement.RestoreOriginalValues();
            activeTraits = new List<MachineTrait>();
            traitVFXController.DeActivateAllTraitVFX();
        }

        public void SetMachineAttributes()
        {
            machineArmor.RestoreOriginalValues();
            MachineMovement.RestoreOriginalValues();
            traitVFXController.DeActivateAllTraitVFX();

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
                        MachineMovement.IncreaseMachineSpeed();
                        break;

                    case MachineTraits.Invulnerable:
                        machineArmor.SetInvulnerable();
                        break;

                    case MachineTraits.ReduceSpeed:
                        MachineMovement.DecreaseMachineSpeed();
                        break;

                    case MachineTraits.RepairMachine:
                        machineArmor.RepairMachine();
                        break;

                    case MachineTraits.StopMachine:
                        MachineMovement.StopMachine();
                        break;

                    case MachineTraits.GotoBase:
                        MachineMovement.GoToBase();
                        break;

                }
            }
           traitVFXController.ActivateTraitVFX(trait);
        }

     
        #endregion


        protected virtual void OnDestroy()
        {
            if (CurrentCell) CurrentCell.SetOnMachineExit(this);
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

         
        public void UpdateCurrentCellTopElement(CellTopElement topElement)
        {
            if(!topElement) return;

            if(!CurrentCell || !CurrentCell.CurrentTopElement) return;
            
            CurrentCell.CurrentTopElement = topElement;
        }

        public void RemoveMachineFromCurrentCell()
        {
            if(!CurrentCell) return;

            CurrentCell.SetOnMachineExit(this);

            CurrentCell.CleanMachineList();

        }

       


    }//Closes HumanMachine class
}