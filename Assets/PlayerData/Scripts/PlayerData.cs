using System.Collections;
using System.Collections.Generic;
using Ozamanas.Forces;
using UnityEngine;


namespace Ozamanas.SaveSystem
{
    [CreateAssetMenu(fileName = "New PlayerData", menuName = "PlayerData/Reference")]
    public class PlayerData : ScriptableObject
    {
        [SerializeField] private PlayerDataHolder holder;
        private bool isEmpty = true;

        [Space(5)]
        [Header("Collections")]
        public List<ForceData> unlockedForces = new List<ForceData>();
        public List<ForceData> selectedForces = new List<ForceData>();

        [Space(5)]
        [Header("Narrative")]

        public string currentDialogueCode;

        public void ClearData(PlayerData emptyState)
        {
            if (!emptyState) return;

            this.isEmpty = emptyState.isEmpty;
            this.unlockedForces = emptyState.unlockedForces;
            this.selectedForces = emptyState.selectedForces;
        }//Close ClearData method

        public void ClearData()
        {
            if (!holder || !holder.emptySaveState) return;
            ClearData(holder.emptySaveState);
        }//Close ClearData method

        public void SelectData()
        {
            if (!holder) return;
            holder.data = this;
        }//Closes SelectData method


    }//Closes PlayerData class
}//closes namespace declaration
