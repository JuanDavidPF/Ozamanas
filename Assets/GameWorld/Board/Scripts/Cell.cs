using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Ozamanas.Machines;

namespace Ozamanas.Board
{
    [SelectionBase]
    [RequireComponent(typeof(Animator))]
    public class Cell : MonoBehaviour
    {

        private Animator m_animator;
        public CellData data;

        [SerializeField] private List<MachineTrait> activeTraits = new List<MachineTrait>();


        public float3 worldPosition;
        public int3 gridPosition;


        public bool isOccupied;



        private void Awake()
        {
            m_animator = m_animator ? m_animator : GetComponent<Animator>();
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
            if (!trait.isPermanetOnHolder) waitToRemoveTrait(trait);
        }

        public void removeTraitToMachine(MachineTrait trait)
        {
            activeTraits.Remove(trait);
        }

        IEnumerator waitToRemoveTrait(MachineTrait trait)
        {
            yield return new WaitForSeconds(trait.holderTimer);
            removeTraitToMachine(trait);
        }



    }//Closes Cell class
}//Closes Namespace declaration
