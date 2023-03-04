using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Ozamanas.Machines;
using UnityEngine.Events;
using Ozamanas.Extenders;
using Ozamanas.World;
using Sirenix.OdinInspector;
using System;

namespace Ozamanas.Board
{
    [SelectionBase]
    public class Cell : MonoBehaviour
    {   
         [Title("Cell Setup:")]

        [SerializeField] private CellData m_data;

        [SerializeField] private Overlay cellOverLay;
        public Transform visuals;

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
        public MeshFilter TileMeshFilter { get => tileMeshFilter; set => tileMeshFilter = value; }
        public Overlay CellOverLay { get => cellOverLay; set => cellOverLay = value; }
        public List<HumanMachine> CurrentHumanMachines { get => currentHumanMachines; set => currentHumanMachines = value; }
        [SerializeField] private List<MachineTrait> activeTraits = new List<MachineTrait>();
        [SerializeField] private MeshFilter tileMeshFilter;
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
            
            
        }
        protected virtual void Start()
        {
            UpdateTopElement(data.defaultTopElement);
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
            if(CurrentHumanMachines.Contains(machine)) return;

            CurrentHumanMachines.Add(machine);

            OnMachineEntered?.Invoke(machine);
        }

        public virtual void SetOnMachineExit(HumanMachine machine)
        {
            if(!CurrentHumanMachines.Contains(machine)) return;

             CurrentHumanMachines.Remove(machine);

            OnMachineExited?.Invoke(machine);
        }

        protected virtual void OnUpdateCellData()
        {
            
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

        public void onLevelCompleteEvent()
        {
            if(onLevelComplete == null) return;

            CurrentTopElement = onLevelComplete.topElementToSwap;
            data = onLevelComplete.tokenToSwap;
        }

        public void onLevelFailedEvent()
        {
            if(onLevelFailed == null) return;

            CurrentTopElement = onLevelFailed.topElementToSwap;
            data = onLevelFailed.tokenToSwap;
        }




    }//Closes Cell class
}//Closes Namespace declaration
