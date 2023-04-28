using System.Collections;
using System.Collections.Generic;
using JuanPayan.References;
using UnityEngine;
using UnityEngine.Localization;
using Ozamanas.Machines;

namespace Ozamanas.Board
{

    [CreateAssetMenu(menuName = "Cells/Cells Data", fileName = "new Cell Data")]
    public class CellData : ScriptableObject
    {
        [Header("Top Element Default")]

        public GameObject cellPrefab;

        public CellTopElement defaultTopElement;

        public MachineTrait speedUPTrait;

        [Header("UI Config")]
        public Sprite cellIcon;
        public Sprite cellArt;
        public Sprite cellCard;

        public LocalizedString cellName;
        public LocalizedString cellDescription;


        [Header("Heuristic Config")]
        public IntegerReference movemenCost;
        public IntegerReference expansionCost;

    }//Closes CellID class
}//Closes Namespace declaration