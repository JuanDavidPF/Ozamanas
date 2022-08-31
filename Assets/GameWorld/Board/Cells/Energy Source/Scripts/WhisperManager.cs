using System.Collections;
using System.Collections.Generic;
using Ozamanas.Board;
using Ozamanas.Extenders;
using UnityEngine;

namespace Ozamanas.Energy
{
    [RequireComponent(typeof(EnergyGenerator))]
    [RequireComponent(typeof(Cell))]
    public class WhisperManager : MonoBehaviour
    {
        private Cell cellReference;
        private EnergyGenerator generatorReference;


        [SerializeField] private Whisper whisperPrefab;
        [SerializeField] private Transform whisperContainer;


        private void Awake()
        {

            cellReference = GetComponent<Cell>();
            generatorReference = GetComponent<EnergyGenerator>();
            if (whisperContainer) whisperContainer.Clean();

        }//Closes Awake method



        public void InitializeWhispers()
        {
            if (!generatorReference || !whisperPrefab) return;

            for (int i = 0; i < generatorReference.maxCycles.value; i++)
            {
                Instantiate(whisperPrefab, whisperContainer);
            }
        }//Closes InitializeWhisper method




    }//Closes WhisperManager class
}//Closes namespace declaration
