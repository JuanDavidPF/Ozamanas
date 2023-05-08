using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.SaveSystem;
using Ozamanas.Extenders;
using Ozamanas.Forces;

namespace Ozamanas.UI
{
    public class ForceDeckManager : MonoBehaviour
    {

        [SerializeField] private PlayerDataHolder currentSaveState;

        private List<ForceCard> selectedForces = new List<ForceCard>();


        private void Awake()
        {
            selectedForces.AddRange(GetComponentsInChildren<ForceCard>());
        }//Closes Awake method
        public void LoadPlayerDeck()
        {
            if (!currentSaveState || !currentSaveState.data ) return;

            for (int i = 0;i<selectedForces.Count;i++)
            {
                if(i < currentSaveState.data.selectedForces.Count)
                selectedForces[i].forceData = currentSaveState.data.selectedForces[i];
                else
                selectedForces[i].gameObject.SetActive(false);
        
            }
           
        }//Closes LoadPlayerDeck method

    }//Closes ForceDeckManager class
}//Closes Namespace declaration
