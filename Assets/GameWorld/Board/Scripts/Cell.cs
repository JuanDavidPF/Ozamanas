using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Ozamanas.Machines;
using UnityEngine.Events;
using DG.Tweening;

namespace Ozamanas.Board
{
    [SelectionBase]
    public class Cell : MonoBehaviour
    {

        [SerializeField] private CellData m_data;
        public CellData data
        {
            get { return m_data; }
            set
            {
                if (m_data == value) return;


                CellData originalValue = m_data;

                m_data = value;

                OnCellDataChanged?.Invoke(m_data);
                OnCellChanged?.Invoke(this);

                if (Board.reference) Board.SwappedCellData(this, originalValue, m_data);
            }
        }

        public List<MachineTrait> ActiveTraits { get => activeTraits; set => activeTraits = value; }
        public MeshFilter TileMeshFilter { get => tileMeshFilter; set => tileMeshFilter = value; }

        [SerializeField] private List<MachineTrait> activeTraits = new List<MachineTrait>();
        [SerializeField] private MeshFilter tileMeshFilter;

        public Transform visuals;

        private List<HumanMachine> currentHumanMachines;

        [HideInInspector] public float3 worldPosition;
        [HideInInspector] public int3 gridPosition;
        [HideInInspector] public bool isOccupied;

        [Space(15)]
        [Header("Events")]
        public UnityEvent<CellData> OnCellDataChanged;
        public UnityEvent<Cell> OnCellChanged;

        public UnityEvent<HumanMachine> OnMachineEntered;
        public UnityEvent<HumanMachine> OnMachineExited;

        private void Start()
        {
            currentHumanMachines = new List<HumanMachine>();
            OnCellDataChanged?.Invoke(m_data);
            if (visuals) visuals.gameObject.SetActive(false);
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

        public void SetOnMachineEnter(HumanMachine machine)
        {
            if(currentHumanMachines.Contains(machine)) return;

            currentHumanMachines.Add(machine);

            OnMachineEntered?.Invoke(machine);
        }

        public void SetOnMachineExit(HumanMachine machine)
        {
            if(!currentHumanMachines.Contains(machine)) return;

             currentHumanMachines.Remove(machine);

            OnMachineExited?.Invoke(machine);
        }



    }//Closes Cell class
}//Closes Namespace declaration
