using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Ozamanas.UI
{
    public class EnergyCounter : MonoBehaviour
    {

        [SerializeField] private TextMeshProUGUI energyCounterTMP;

        public void UpdateCounter(int value)
        {
            if (!energyCounterTMP) return;
            energyCounterTMP.text = value.ToString();

        }//Closes UpdateCOunter method

    }//Closes EnergyCounter class


}//Closes namespace declaration
