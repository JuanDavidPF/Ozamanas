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

        [SerializeField] Transform healthBar;

        [SerializeField] Transform toolTipHealthBar;

        [Space(20)]
        [Header("Elements")]

        [SerializeField] Transform trait;
        [SerializeField] Transform empty;
        [SerializeField] Transform full;

        private void Awake()
        {
            if (healthBar) healthBar.Clean();
             if (toolTipHealthBar) toolTipHealthBar.Clean();
        }//Closes Awake methods


        public void UpdateArmor(int armorPoints, int armorEmpty)
        {
            if (!healthBar || !empty || !full || !toolTipHealthBar) return;

            healthBar.Clean();
            toolTipHealthBar.Clean();

            for (int i = 0; i < armorPoints; i++) Instantiate(full, healthBar);
            for (int i = 0; i < armorEmpty; i++) Instantiate(empty, healthBar);

            for (int i = 0; i < armorPoints; i++) Instantiate(full, toolTipHealthBar);
            for (int i = 0; i < armorEmpty; i++) Instantiate(empty, toolTipHealthBar);

        }//Closes UpdateArmor method
}
}//Closes namespace declaration
