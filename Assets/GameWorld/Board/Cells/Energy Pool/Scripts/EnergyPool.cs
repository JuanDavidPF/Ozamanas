using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.Board;
using Ozamanas.Tags;
using Ozamanas.Machines;
using Ozamanas.Forest;
using DG.Tweening;
using Ozamanas.Extenders;
using UnityEngine.Events;
using Ozamanas.World;

namespace Ozamanas.Energy
{

    [RequireComponent(typeof(Cell))]
    [RequireComponent(typeof(EnergyGenerator))]
    public class EnergyPool : MonoBehaviour
    {

        [Serializable]
        private class FlowerContainer
        {
            public Transform flowerTransform;
            public GameObject activeFlower;
            public GameObject inactiveFlower;
            public GameObject currentFlower;
        }

        [SerializeField] private List<FlowerContainer> flowers = new List<FlowerContainer>();
        [SerializeField] private Transform visuals;

        [SerializeField] private int flowersLimit = 4;

        [SerializeField] private bool poolActive;
        private Cell cellReference;
        private EnergyGenerator generatorReference;

        [SerializeField] private Transform liquid;
        [SerializeField] private string liquidColorKey;
        [SerializeField] private Color inactiveColor;
        [SerializeField] private Color activeColor;
        private MeshRenderer liquidRenderer;
        private float scaleMultiplier;

        [Space(15)]
        [Header("Cell identificators")]
        [SerializeField] private CellData inactiveID;
        [SerializeField] private CellData activeID;
        [SerializeField] private CellData emptyID;

                [SerializeField] private GameplayState winState;

         [SerializeField] private GameplayState loseState;


        [Space(15)]
        [Header("Events")]
        public UnityEvent PlayVFX;
        public UnityEvent StopVFX;

        public UnityEvent OnEmptyEnergyPool;
        private void Awake()
        {
            cellReference = GetComponent<Cell>();
            generatorReference = GetComponent<EnergyGenerator>();

            generatorReference.StopGeneration();
            
            PopulateFlowers();

            if (liquid) scaleMultiplier = liquid.transform.localScale.y;
            if (liquid) liquidRenderer = liquid.GetComponent<MeshRenderer>();
            SetLiquidColor(inactiveColor);
        }//Closes Awake methods

        private void PopulateFlowers()
        {
            DummyTree[] temp = GetComponentsInChildren<DummyTree>();
            for (int i = 0; i < temp.Length; i++)
            {
                FlowerContainer container = new FlowerContainer();
                container.flowerTransform = temp[i].transform;
                container.inactiveFlower = temp[i].ForestTree;
                container.activeFlower = temp[i].ExpansionTree;
                container.currentFlower = null;
                flowers.Add(container);
                temp[i].gameObject.SetActive(false);
            }
        } 

        private void UpdateLiquidLevel(int level)
        {
            float normalizedLevel = (float)level
                                     / (float)generatorReference.fullLevel.value;

            SetLiquidColor(activeColor);

            liquid.DOScaleY(normalizedLevel * scaleMultiplier, .3f).SetSpeedBased(true).SetEase(Ease.OutQuad);

            
        }//Closes UpdateLiquidLevel

        private void UpdateFlowersNumbers(int level)
        {
                if(level >= flowers.Count) return;
               DestroyCurrentFlower(level);
                flowersLimit = level;
        }
        public void UpdateEnergyLevel(int level)
        {
           
            if (!poolActive) return;
            if (!generatorReference
                 || generatorReference.lifetime != EnergyGenerator.LifetimeConfig.Limited
                 || !liquid) return;

            UpdateFlowersNumbers(level);
            UpdateLiquidLevel( level);
        
            if (level == 0 && emptyID) cellReference.data = emptyID;

        } 

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
                SetVisualsForEmptyPool();
                OnEmptyEnergyPool?.Invoke();
            } 
            else if (data == activeID)
            {
                generatorReference.ResumeGeneration();
                SetLiquidColor(activeColor);
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

        public void OnCellGameStateChange(GameplayState state)
        {
            if (!state) return;
            if (state == winState) SetVisualsForActivePool();
            if (state == loseState)  SetVisualsForInactivePool();
        }

        private void SetLiquidColor(Color color)
        {
            if (!liquidRenderer) return;
            liquidRenderer.material.SetColor(liquidColorKey, color);
        }//Closes SetLiquidColor method

         private void SetVisualsForEmptyPool()
        {
            StopVFX?.Invoke();
        }

        private void SetVisualsForInactivePool()
        {
            StopVFX?.Invoke();
            for (int i = 0; i < flowers.Count; i++)
            {
               DestroyCurrentFlower(i);
                if( i <flowersLimit)
               {
                GameObject temp = Instantiate(flowers[i].inactiveFlower, visuals);
                temp.transform.position = flowers[i].flowerTransform.position;
                temp.transform.rotation = flowers[i].flowerTransform.rotation;
                temp.GetComponentInChildren<JungleTree>().ForestIndex = i;
                flowers[i].currentFlower = temp;
               }
            }
        }

        private void SetVisualsForActivePool()
        {
            PlayVFX?.Invoke();
            for (int i = 0; i < flowers.Count; i++)
            {
               DestroyCurrentFlower(i);
               if(i<flowersLimit)
               {
                    GameObject temp = Instantiate(flowers[i].activeFlower, visuals);
                    temp.transform.position = flowers[i].flowerTransform.position;
                    temp.transform.rotation = flowers[i].flowerTransform.rotation;
                    temp.GetComponentInChildren<JungleTree>().ForestIndex = i;
                    flowers[i].currentFlower = temp;
                }
            }
        }

         private void SetVisualsForInvadedPool()
        {
            StopVFX?.Invoke();
            for (int i = 0; i < flowers.Count; i++)
            {
               DestroyCurrentFlower(i);
            }
        }

        private void DestroyCurrentFlower(int i)
        {
            if (!flowers[i].currentFlower) return;

            if (flowers[i].currentFlower.transform.TryGetComponentInChildren(out Forest.JungleTree jungleTree))
            {
                jungleTree.HideAndDestroy();
            }
        }

        public void OnMachineEnter(HumanMachine machine)
        {
             
            if(cellReference.data == emptyID) return;

            cellReference.data = inactiveID;

            SetVisualsForInvadedPool();
        }

        public void OnMachineExit(HumanMachine machine)
        {
            if(cellReference.data == emptyID) return;

            cellReference.data = inactiveID;

           SetVisualsForInactivePool();
            
        }


    }//Closes EnergyPool class
}//Closes Namespace declaration
