using System.Collections;
using System.Collections.Generic;
using JuanPayan.References;
using UnityEngine;

namespace Ozamanas.Energy
{
   
    public class EnergyAbsorber : MonoBehaviour
    {
        [SerializeField] private  IntegerVariable energyAmount;

        [SerializeField] private  GameObject addEnergyVFX;

        public void AddEnergyAmount()
        {
             if (!energyAmount) return;

            energyAmount.value++;

            if(addEnergyVFX) Instantiate(addEnergyVFX,transform);
        }

    }//Closes EnergyAbsorber class
}//Closes Namespace declaration