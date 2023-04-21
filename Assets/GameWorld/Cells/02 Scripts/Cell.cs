using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Ozamanas.Machines;
using Ozamanas.Tags;
using UnityEngine.Events;
using Ozamanas.Extenders;
using Sirenix.OdinInspector;
using System;

namespace Ozamanas.Board
{
    [SelectionBase]
    public class Cell : MonoBehaviour
    {   
         [Title("Cell Setup:")]

        private int m_movementCost;

        [SerializeField] private CellData m_data;

        [SerializeField] private Overlay cellOverLay;
        public Transform visuals;


        public int MovementCost
        {
            get { return m_movementCost; }
            set {

                if (value <= 0) m_movementCost = data.movemenCost.value;

                else m_movementCost = data.movemenCost.value + value;

            }
        }

        public CellData data
        {
            get { return m_data; }
            set
            {
                if(value == null) return;

                if (m_data == value) return;

                CellData originalValue = m_data;

                m_data = value;

                OnCellDataChanged?.Invoke(m_data);
                OnCellChanged?.Invoke(this);

                OnUpdateCellData();

                if (Board.reference) Board.SwappedCellData(this, originalValue, m_data);
            }
        }

      

        [Title("Cell Top Element:")] 

        [SerializeField] private Transform topElementTransform;

        private CellTopElement currentTopElement;

        public CellTopElement CurrentTopElement
        {
            get { return currentTopElement; }
            set
            {
                if (!value) return;

                if (currentTopElement == value) return;

                currentTopElement = value;

                OnTopElementChanged?.Invoke(currentTopElement);

                UpdateTopElement(currentTopElement);
                
            }
        }
       
        public UnityEvent<CellTopElement> OnTopElementChanged;
         [Space(10)]
        [Title("Cell GameplayStates:")]
        [SerializeField] private SwapRules onLevelComplete;
        [SerializeField] private SwapRules onLevelFailed;
    
        public List<MachineTrait> ActiveTraits { get => activeTraits; set => activeTraits = value; }
        public Overlay CellOverLay { get => cellOverLay; set => cellOverLay = value; }
        public List<HumanMachine> CurrentHumanMachines { get => currentHumanMachines; set => currentHumanMachines = value; }
        public GameObject HollowTile { get => hollowTile; set => hollowTile = value; }

        [SerializeField] private List<MachineTrait> activeTraits = new List<MachineTrait>();
        [SerializeField] private GameObject hollowTile;
        private List<HumanMachine> currentHumanMachines = new List<HumanMachine>();
        [HideInInspector] public float3 worldPosition;
        [HideInInspector] public int3 gridPosition;
        [HideInInspector] public bool isOccupied;

        [Title("Events:")]
        public UnityEvent<CellData> OnCellDataChanged;
        public UnityEvent<Cell> OnCellChanged;
        public UnityEvent<HumanMachine> OnMachineEntered;
        public UnityEvent<HumanMachine> OnMachineExited;


        protected virtual void Awake()
        {
            if(!cellOverLay && gameObject.transform.TryGetComponentInChildren( out Overlay cell ))
            {
                cellOverLay = cell;
            }

            if(!HollowTile && gameObject.transform.TryGetComponentInChildren( out HollowTile hollow ))
            {
                HollowTile = hollow.gameObject;
            }

            visuals.gameObject.SetActive(false);
            
            
        }
        protected virtual void Start()
        {
            UpdateTopElement(data.defaultTopElement);
            m_movementCost = data.movemenCost.value;

        }//Closes Awake method

       
        private void OnDisable()
        {
            Board.RemoveCellFromBoard(this);
        }//Closes OnDisable event

        private void OnDestroy()
        {
            OnCellChanged.Invoke(this);
        }

        public List<MachineTrait> GetCellTraits()
        {
            return ActiveTraits;
        }//Closes GetTraitsOnCell method


        public void AddTraitToMachine(MachineTrait trait)
        {
            ActiveTraits.Add(trait);
            if (!trait.isPermanetOnHolder) StartCoroutine(HandleTraitDuration(trait));
        }

        public void RemoveTraitToMachine(MachineTrait trait)
        {
            ActiveTraits.Remove(trait);
        }

        IEnumerator HandleTraitDuration(MachineTrait trait)
        {
            yield return new WaitForSeconds(trait.holderTimer);
            RemoveTraitToMachine(trait);
        }

        public virtual void SetOnMachineEnter(HumanMachine machine)
        {
            if(!machine) return;
           
            if(CurrentHumanMachines.Contains(machine)) return;

            CurrentHumanMachines.Add(machine);

            OnMachineEntered?.Invoke(machine);

            MovementCost = CurrentHumanMachines.Count;

            SpeedUpFirstMachine();
        }

        public virtual void SetOnMachineExit(HumanMachine machine)
        {
            if(!machine) return;
            
            if(!CurrentHumanMachines.Contains(machine)) return;

            CurrentHumanMachines.Remove(machine);

            OnMachineExited?.Invoke(machine);

            MovementCost = CurrentHumanMachines.Count;

            machine.RemoveTraitToMachine(data.speedUPTrait);
        }

        public virtual void SpeedUpFirstMachine()
        {
            if(CurrentHumanMachines.Count <= 1) return;

            CurrentHumanMachines[0].AddTraitToMachine(data.speedUPTrait);
        }

        protected virtual void OnUpdateCellData()
        {
            CallNearbyMachines();   
        }

        private void CallNearbyMachines()
        {
            List<Cell> cells = BoardExtender.GetCellsOnRange(this,3,true);
            List<MachinePhysicsManager> nearbyMachines = new List<MachinePhysicsManager>();

            foreach ( Cell cell in cells)
            {
                foreach(HumanMachine machine in cell.currentHumanMachines)
                {
                    if(!machine) continue;
                    if(!nearbyMachines.Contains(machine.GetComponent<MachinePhysicsManager>())) nearbyMachines.Add(machine.GetComponent<MachinePhysicsManager>());
                }
            }

            foreach(MachinePhysicsManager nearbyMachine in nearbyMachines)
            {
                if(nearbyMachine.state !=  PhysicMode.Intelligent) continue;

                if(nearbyMachine.machine.CheckIfActing()) continue;

                if(nearbyMachine.machine.MachineMovement.CurrentAltitude != MachineAltitude.Terrestrial ) continue;

                nearbyMachine.ResetMachineNavAndAI();
            }



        }

        private void UpdateTopElement(CellTopElement topElement)
        {
            if(!topElement) return;

            GameObject temp = topElement.GetTopElement();

            if(!temp) return;
            
            DestroyCurrentTopElement();

            Instantiate(temp,topElementTransform);
        }


        private void DestroyCurrentTopElement()
        {
            if(!topElementTransform) return;
            
            if (topElementTransform.childCount== 0) return;

            Destroy(topElementTransform.GetChild(0).gameObject);

        }


        public void OnLevelCompleteEvent()
        {
            if(onLevelComplete == null) return;

            CurrentTopElement = onLevelComplete.topElementToSwap;
            data = onLevelComplete.tokenToSwap;
        }

        public void OnLevelFailedEvent()
        {
            if(onLevelFailed == null) return;

            CurrentTopElement = onLevelFailed.topElementToSwap;
            data = onLevelFailed.tokenToSwap;
        }




    }//Closes Cell class
}//Closes Namespace declaration
