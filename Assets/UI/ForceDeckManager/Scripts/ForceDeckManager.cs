using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.SaveSystem;
using Ozamanas.Extenders;

namespace Ozamanas.UI
{
    public class ForceDeckManager : MonoBehaviour
    {

        [SerializeField] private PlayerDataHolder currentSaveState;
        [SerializeField] private ForceCard cardPrefab;
        private void Awake()
        {
            transform.Clean();
        }//Closes Awake method
        public void LoadPlayerDeck()
        {
            if (!currentSaveState || !currentSaveState.data || !cardPrefab) return;


            foreach (var force in currentSaveState.data.selectedForces)
            {
                Instantiate(cardPrefab, transform).forceData = force;
            }
        }//Closes LoadPlayerDeck method

    }//Closes ForceDeckManager class
}//Closes Namespace declaration
