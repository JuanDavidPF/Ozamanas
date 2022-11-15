using System.Collections;
using System.Collections.Generic;
using JuanPayan.References;
using Ozamanas.Board;
using UnityEngine;
using UnityEngine.Localization;

namespace Ozamanas.Forces
{
    [CreateAssetMenu(fileName = "new Force data", menuName = "Forces/Reference")]
    public class ForceData : ScriptableObject
    {
        public AncientForce force;

        [Space(5)]
        [Header("Display content")]

        public LocalizedString forceName;
        public LocalizedString forceDescription;

        [Space(5)]

        public Sprite forceIcon;
        public Sprite forceArt;
        public Sprite forceCard;

        public Sprite forceConstellation;
        public GameObject stars;

        [Space(10)]
        [Header("Config")]
        public List<CellData> whiteList = new List<CellData>();
        public IntegerReference price;
        public IntegerReference cooldown;


        [Space(10)]
        [Header("Range")]
        public IntegerReference range;
        public List<CellData> rangeAnchors = new List<CellData>();


    }//closes ForceData class
}//Closes namespace declaration
