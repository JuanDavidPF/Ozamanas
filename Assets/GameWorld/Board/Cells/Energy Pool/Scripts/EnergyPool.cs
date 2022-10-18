using System.Collections;
using System.Collections.Generic;
using Ozamanas.Board;
using Ozamanas.Machines;
using UnityEngine;
using DG;
using DG.Tweening;

namespace Ozamanas.Energy
{

    [RequireComponent(typeof(Cell))]
    [RequireComponent(typeof(EnergyGenerator))]
    public class EnergyPool : MonoBehaviour
    {
        [SerializeField] private bool poolActive;
        private Cell cellReference;
        private EnergyGenerator generatorReference;
        [SerializeField] private List<GameObject> flowers;

        [SerializeField] private List<GameObject> whispers;
        [SerializeField] private Transform liquid;
        [SerializeField] private string liquidColorKey;
        [SerializeField] private Color inactiveColor;
        [SerializeField] private Gradient activeColor;
        private MeshRenderer liquidRenderer;
        private float scaleMultiplier;

        [Space(15)]
        [Header("Cell identificators")]
        [SerializeField] private CellData inactiveID;
        [SerializeField] private CellData activeID;
        [SerializeField] private CellData emptyID;

        private void Awake()
        {
            cellReference = GetComponent<Cell>();
            generatorReference = GetComponent<EnergyGenerator>();

            generatorReference.StopGeneration();

            if (liquid) scaleMultiplier = liquid.transform.localScale.y;
            if (liquid) liquidRenderer = liquid.GetComponent<MeshRenderer>();
            SetLiquidColor(inactiveColor);
        }//Closes Awake methods

        void Start()
        {
        }

        public void UpdateLiquidLevel(int level)
        {
            if (!poolActive) return;
            if (!generatorReference
                 || generatorReference.lifetime != EnergyGenerator.LifetimeConfig.Limited
                 || !liquid) return;

            float normalizedLevel = (float)level
                                     / (float)generatorReference.fullLevel.value;

            SetLiquidColor(activeColor.Evaluate(normalizedLevel));

            liquid.DOScaleY(normalizedLevel * scaleMultiplier, .3f).SetSpeedBased(true).SetEase(Ease.OutQuad);

            if (level == 0 && emptyID) cellReference.data = emptyID;
        }//Closes UpdateLiquidLevel


        public void ActivatePool(bool active)
        {
            poolActive = active;
            OnCellDataChanged(cellReference.data);
            UpdateLiquidLevel(generatorReference.currentLevel);
        }//Closes OnCellDataChanged method
        public void OnCellDataChanged(CellData data)
        {
            if (!poolActive) return;

            if (data == emptyID)
            {
                generatorReference.currentLevel = 0;
            }
            else if (data == activeID)
            {
                generatorReference.ResumeGeneration();
                SetVisualsForActivePool();
            }
            else if (data == inactiveID)
            {
                generatorReference.StopGeneration();
                SetLiquidColor(inactiveColor);
                SetVisualsForInactivePool();
            }
            else Debug.LogWarning("Invalid cell data for a Energy source");
        }//Closes OnCellDataChanged method


        private void SetLiquidColor(Color color)
        {
            if (!liquidRenderer) return;
            liquidRenderer.material.SetColor(liquidColorKey, color);
        }//Closes SetLiquidColor method

        private void SetVisualsForInactivePool()
        {

            foreach (GameObject flower in flowers)
            {
                flower.SetActive(true);
            }

            foreach (GameObject whisper in whispers)
            {
                whisper.SetActive(false);
            }
        }

        private void SetVisualsForActivePool()
        {
            foreach (GameObject flower in flowers)
            {
                flower.SetActive(false);
            }

            foreach (GameObject whisper in whispers)
            {
                whisper.SetActive(true);
            }
        }

        public void OnMachineEnter(HumanMachine machine)
        {

            if (cellReference.data != activeID) return;

            cellReference.data = inactiveID;
        }


    }//Closes EnergyPool class
}//Closes Namespace declaration
