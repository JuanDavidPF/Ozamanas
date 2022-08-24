using System.Collections;
using System.Collections.Generic;
using JuanPayan.References.Integers;
using UnityEngine;
using UnityEngine.Localization;

namespace Ozamanas.Board
{

    [CreateAssetMenu(menuName = "Cells/Cells Data", fileName = "new Cell Data")]
    public class CellData : ScriptableObject
    {
        [Header("UI Config")]
        public Sprite machineIcon;
        public LocalizedString machineName;
        public LocalizedString machineDescription;


        [Header("Heuristic Config")]
        public IntegerReference movemenCost;
        public IntegerReference expansionCost;

    }//Closes CellID class
}//Closes Namespace declaration