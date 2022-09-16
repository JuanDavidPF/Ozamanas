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
                m_data = value;
                OnCellDataChanged?.Invoke(value);
            }
        }

        [SerializeField] private List<MachineTrait> activeTraits = new List<MachineTrait>();


        public float3 worldPosition;
        public int3 gridPosition;


        public bool isOccupied;

        [Space(15)]
        [Header("Events")]
        public UnityEvent<CellData> OnCellDataChanged;

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


        public List<MachineTrait> GetCellTraits()
        {
            return activeTraits;
        }//Closes GetTraitsOnCell method


        public void AddTraitToMachine(MachineTrait trait)
        {
            activeTraits.Add(trait);
            if (!trait.isPermanetOnHolder) StartCoroutine(HandleTraitDuration(trait));
        }

        public void RemoveTraitToMachine(MachineTrait trait)
        {
            activeTraits.Remove(trait);
        }

        IEnumerator HandleTraitDuration(MachineTrait trait)
        {
            yield return new WaitForSeconds(trait.holderTimer);
            RemoveTraitToMachine(trait);
        }



    }//Closes Cell class
}//Closes Namespace declaration
