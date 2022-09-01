using System.Collections;
using System.Collections.Generic;
using JuanPayan.Utilities;
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



        [Space(20)]
        [Header("Orbit config")]

        [SerializeField] private Ellipse orbitPath;



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
                Whisper whisper = Instantiate(whisperPrefab, whisperContainer);

                Vector2 positionInOrbit = orbitPath.Evaluate(i / generatorReference.maxCycles.value);

                whisper.transform.localPosition = new Vector3(positionInOrbit.x, whisper.transform.position.y, positionInOrbit.y);
            }
        }//Closes InitializeWhisper method




    }//Closes WhisperManager class
}//Closes namespace declaration
