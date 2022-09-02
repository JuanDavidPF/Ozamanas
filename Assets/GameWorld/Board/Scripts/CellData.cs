using System.Collections;
using System.Collections.Generic;
using JuanPayan.References;
using UnityEngine;
using UnityEngine.Localization;

namespace Ozamanas.Board
{

    [CreateAssetMenu(menuName = "Cells/Cells Data", fileName = "new Cell Data")]
    public class CellData : ScriptableObject
    {
        [Header("UI Config")]
        public Sprite machineIcon;
        public LocalizedString cellName;
        public LocalizedString cellDescription;


        [Header("Heuristic Config")]
        public IntegerReference movemenCost;
        public IntegerReference expansionCost;

    }//Closes CellID class
}//Closes Namespace declaration