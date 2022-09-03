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

        private Cell cellReference;
        private EnergyGenerator generatorReference;

        [SerializeField] private Transform liquid;
        private float scaleMultiplier;

        private void Awake()
        {
            if (liquid) scaleMultiplier = liquid.transform.localScale.y;
            cellReference = GetComponent<Cell>();
            generatorReference = GetComponent<EnergyGenerator>();
        }//Closes Awake methods


        public void UpdateLiquidLevel(int level)
        {

            if (!generatorReference
                 || generatorReference.lifetime != EnergyGenerator.LifetimeConfig.Limited
                 || !liquid) return;

            float normalizedLevel = (float)level
                                     / (float)generatorReference.fullLevel.value;

            liquid.DOScaleY(normalizedLevel * scaleMultiplier, .3f).SetSpeedBased(true).SetEase(Ease.OutQuad);
        }//Closes UpdateLiquidLevel

        public void PlayerAtPool()
        {
            if (!generatorReference) return;
            generatorReference.ResumeGeneration();
        }//Closes StartPool method

        public void MachineAtPool()
        {
            if (!generatorReference) return;
            generatorReference.StopGeneration();
        }//Closes StartPool method

    }//Closes EnergyPool class
}//Closes Namespace declaration
