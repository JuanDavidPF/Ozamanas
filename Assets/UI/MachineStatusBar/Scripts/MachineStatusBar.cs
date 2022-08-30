using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.Machines;
using Ozamanas.Extenders;
using UnityEngine.UI;

namespace Ozamanas.UI
{
    public class MachineStatusBar : MonoBehaviour
    {

        [Header("Containers")]

        [SerializeField] Transform traitIcons;
        [SerializeField] Transform healthBar;

        [Space(20)]
        [Header("Elements")]

        [SerializeField] Transform trait;
        [SerializeField] Transform health;

        private void Awake()
        {
            if (traitIcons) traitIcons.Clean();
            if (healthBar) healthBar.Clean();
        }//Closes Awake methods


        public void UpdateArmor(int armorPoints)
        {
            if (!healthBar || !health) return;

            healthBar.Clean();

            for (int i = 0; i < armorPoints; i++) Instantiate(health, healthBar);


        }//Closes UpdateArmor method

        public void UpdateTraits(List<MachineTrait> machineTraits)
        {
            if (!traitIcons || !trait) return;
            traitIcons.Clean();



            foreach (var machineTrait in machineTraits)
            {
                Transform newTrait = Instantiate(trait, traitIcons);

                if (newTrait.TryGetComponent(out Image traitIcon))
                {
                    traitIcon.sprite = machineTrait.traitIcon;
                }
            }
        }//Closes UpdateTraits method

    }//Closes MachineStatusBar class
}//Closes namespace declaration
