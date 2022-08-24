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
        public LocalizedString machineName;
        public LocalizedString machineDescription;

        public IntegerReference movemenCost;


    }//Closes CellID class
}//Closes Namespace declaration