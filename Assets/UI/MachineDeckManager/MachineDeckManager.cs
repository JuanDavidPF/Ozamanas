using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.Levels;
using Ozamanas.Extenders;

namespace Ozamanas.UI
{
    public class MachineDeckManager : MonoBehaviour
    {
         [SerializeField] private LevelData levelData;
        [SerializeField] private MachineCard cardPrefab;

        public LevelData LevelData { get => levelData; set => levelData = value; }

        private void Awake()
        {
            transform.Clean();
        }//Closes Awake method
        public void LoadMachineDeck()
        {
            if (!LevelData|| !cardPrefab) return;


            foreach (var machine in LevelData.machines)
            {
                Instantiate(cardPrefab, transform).MachineData = machine;
            }
        }

        public void ClearMachineDeck()
        {    
            int children = transform.childCount;

            for (int i = children - 1; i >= 0; i--) 
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }

        }


    }
}
