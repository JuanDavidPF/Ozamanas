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
using Sirenix.OdinInspector;


namespace Ozamanas.Energy
{

    [RequireComponent(typeof(EnergyGenerator))]
    public class EnergyPool : Cell
    {

        [Title("Energy Pool Setup:")]
        private bool poolActive;
        private EnergyGenerator generatorReference;
        [SerializeField] private CellData emptyEnergyCellData;

        [SerializeField] private CellData inactiveEnergyCellData;
        [SerializeField] private CellTopElement emptyEnergyTopElement;

        public UnityEvent OnEmptyEnergyPool;
        protected override void Awake()
        {
            base.Awake();
            generatorReference = GetComponent<EnergyGenerator>();
            generatorReference.StopGeneration();
           
        }

        private void PopulateFlowers()
        {

             UpdateFlowersNumbers(generatorReference.currentLevel);
        } 

        private void UpdateFlowersNumbers(int level)
        {
               EnergyFlower[] allFlowers = GetComponentsInChildren<EnergyFlower>();
               
               if(level >= allFlowers.Length) return;

              for (int i =0 ; i< allFlowers.Length - level; i++)
              Destroy(allFlowers[i].gameObject);

              
              
        }
        public void UpdateEnergyLevel(int level)
        {
           
            if (!poolActive) return;

            if (!generatorReference || generatorReference.lifetime != EnergyGenerator.LifetimeConfig.Limited) return;

            UpdateFlowersNumbers(level);
        
            if (level != 0) return;
            
            data = emptyEnergyCellData;

            CurrentTopElement = emptyEnergyTopElement;

        } 

        public void ActivatePool(bool active)
        {
            poolActive = active;

            OnUpdateCellData();

        }
       

       protected override void OnUpdateCellData()
       {
            base.OnUpdateCellData();

            if(data == inactiveEnergyCellData) generatorReference.StopGeneration();
            else generatorReference.ResumeGeneration();

            PopulateFlowers();
       }
      

        public override void SetOnMachineEnter(HumanMachine machine)
        {
            base.SetOnMachineEnter(machine);

            Debug.Log("SetOnMachineEnter");

            ShowOrHideFlowers();

        }

        private void ShowOrHideFlowers()
        {
            bool showFlowers = false;

            if(CurrentHumanMachines.Count == 0) showFlowers = true;

            EnergyFlower[] allFlowers = GetComponentsInChildren<EnergyFlower>();

            Debug.Log(allFlowers.Length+"show:"+showFlowers);

            foreach(EnergyFlower flower in allFlowers)
            flower.ShowOrHideFlower(showFlowers);

        }

        public override void SetOnMachineExit(HumanMachine machine)
        {
            base.SetOnMachineExit(machine);

             Debug.Log("SetOnMachineExit");

            ShowOrHideFlowers();
        }


    }//Closes EnergyPool class
}//Closes Namespace declaration
