using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Ozamanas.Board
{
    public class MiningIndustry : Cell
    {
         [Title("Mining Industry Setup:")]

        [SerializeField] CellTopElement spawnMachineRule;

        public void SetTopElementToSpawn()
        {
            CurrentTopElement = spawnMachineRule;
        }

         public void SetTopElementToIdle()
        {
            CurrentTopElement = data.defaultTopElement;
        }
    }
}
