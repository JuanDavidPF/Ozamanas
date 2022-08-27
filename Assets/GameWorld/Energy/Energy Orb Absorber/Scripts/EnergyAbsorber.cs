using System.Collections;
using System.Collections.Generic;
using JuanPayan.References.Integers;
using UnityEngine;

namespace Ozamanas.Energy
{
    [RequireComponent(typeof(Collider))]
    public class EnergyAbsorber : MonoBehaviour
    {
        public static Transform mainAbsorber;
        [SerializeField] private IntegerVariable energyAmount;

        private void OnEnable()
        {
            mainAbsorber = transform;
        }//Closes OnEnable method
        private void OnDisable()
        {
            if (mainAbsorber == transform) mainAbsorber = null;
        }//Closes ONDisable method

        private void OnTriggerEnter(Collider other)
        {

            if (!energyAmount || other.tag != "EnergyOrb") return;

            energyAmount.value++;
            Destroy(other.gameObject);

        }//Closes OnTriggerEnter method

    }//Closes EnergyAbsorber class
}//Closes Namespace declaration