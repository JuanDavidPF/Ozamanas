using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Ozamanas.Machines;
using UnityEngine.Events;

namespace Ozamanas.Board
{
    [SelectionBase]
    [RequireComponent(typeof(Animator))]
    public class Cell : MonoBehaviour
    {

        private Animator m_animator;

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

        [SerializeField] private List<MachineTrait> activeTraits = new List<MachineTrait>();


        public float3 worldPosition;
        public int3 gridPosition;


        public bool isOccupied;

        [Space(15)]
        [Header("Events")]
        public UnityEvent<CellData> OnCellDataChanged;
        public UnityEvent<Cell> OnCellChanged;

        public UnityEvent<HumanMachine> OnMachineEntered;
        public UnityEvent<HumanMachine> OnMachineExited;

        private void Awake()
        {
            m_animator = m_animator ? m_animator : GetComponent<Animator>();
            OnCellDataChanged?.Invoke(m_data);
        }//Closes Awake method


        public void SetAnimatorTrigger(string triggerName)
        {
            if (!m_animator) return;
            m_animator.SetTrigger(triggerName);

        }//Closes SetAnimatorTrigger method

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



    }//Closes Cell class
}//Closes Namespace declaration
