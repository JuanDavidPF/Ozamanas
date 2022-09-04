using System.Collections;
using System.Collections.Generic;
using Ozamanas.Board;
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

        [SerializeField] private Transform liquid;
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

        }//Closes Awake methods


        public void UpdateLiquidLevel(int level)
        {
            if (!poolActive) return;
            if (!generatorReference
                 || generatorReference.lifetime != EnergyGenerator.LifetimeConfig.Limited
                 || !liquid) return;

            float normalizedLevel = (float)level
                                     / (float)generatorReference.fullLevel.value;

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

            if (data == emptyID) generatorReference.currentLevel = 0;
            else if (data == activeID) generatorReference.ResumeGeneration();
            else if (data == inactiveID) generatorReference.StopGeneration();
            else Debug.LogWarning("Invalid cell data for a Energy source");
        }//Closes OnCellDataChanged method

    }//Closes EnergyPool class
}//Closes Namespace declaration
