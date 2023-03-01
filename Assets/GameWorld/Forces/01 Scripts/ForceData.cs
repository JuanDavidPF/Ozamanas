using System.Collections;
using System.Collections.Generic;
using JuanPayan.References;
using Ozamanas.Board;
using UnityEngine;
using UnityEngine.Localization;
using Ozamanas.Tags;
using Sirenix.OdinInspector;
using Ozamanas.World;
using System;

namespace Ozamanas.Forces
{
    [CreateAssetMenu(fileName = "new Force data", menuName = "Forces/Reference")]
    public class ForceData : ScriptableObject
    {
        [Serializable]
        public struct SwapRules
        {
            public CellData condition;
            public CellTopElement topElementToSwap;
            public CellData tokenToSwap;

        }
        [Space]
        [VerticalGroup("Ancient Force Information")]
        [PreviewField(Alignment =ObjectFieldAlignment.Center)]
        public Sprite forceIcon;
        [VerticalGroup("Ancient Force Information")]
        [PreviewField(Alignment =ObjectFieldAlignment.Center)]
        public Sprite forceCard;
        [VerticalGroup("Ancient Force Information")]
        public FamilyType forceFamily;

        [VerticalGroup("Ancient Force Information")]
        [LabelText("Force Prefab")]
        public AncientForce force;
        [VerticalGroup("Ancient Force Information")]
        public LocalizedString forceName;
        [VerticalGroup("Ancient Force Information")]
        public LocalizedString forceDescription;

        [VerticalGroup("Ancient Force Information")]
        public LocalizedString forceCodexDescription;
        
        [Title("Show More Settings:")]
         [VerticalGroup("Ancient Force Information")]
        [LabelText("Placement Setup")]
        [ToggleLeft]
        public bool showPlacement;

         [VerticalGroup("Ancient Force Information")]
        [LabelText("Constellation Setup")]
        [ToggleLeft]
        public bool showConstellation;

        [VerticalGroup("Ancient Force Setup")]
          [Space]
        public List<CellData> whiteList = new List<CellData>();
        [VerticalGroup("Ancient Force Setup")]
        public IntegerReference price;
        [VerticalGroup("Ancient Force Setup")]
        public IntegerReference cooldown;
        [VerticalGroup("Ancient Force Setup")]
        public IntegerReference range;
        [VerticalGroup("Ancient Force Setup")]
        public List<CellData> rangeAnchors = new List<CellData>();
         [Title("Swap Tile Rule List:")]
         [VerticalGroup("Ancient Force Setup")]
        [SerializeField] private List<SwapRules> ruleList = new List<SwapRules>();
        [Title("Traits Setup:")]
        [VerticalGroup("Ancient Force Setup")]
        [LabelText("List of traits")]
        public List<Machines.MachineTrait> traits;
        [VerticalGroup("Ancient Force Setup")]
        public int traitRange = 1;
        [VerticalGroup("Ancient Force Setup")]
        [LabelText("Add Force to Machine")]
        
        public PhysicsForce physicsForce;
         [ShowIf("showPlacement")]
        [Title("Placement Setup")]
        [VerticalGroup("Ancient Force Setup")]
        public PlacementMode placementMode;
         [ShowIf("showPlacement")]

        [VerticalGroup("Ancient Force Setup")]
         public Vector3 draggedOffset;
         [ShowIf("showPlacement")]
        [VerticalGroup("Ancient Force Setup")]
         public bool snapToGrid = true;

           [ShowIf("showConstellation")]
        [Title("Constellation Setup")]
        [VerticalGroup("Ancient Force Setup")]
        [PreviewField(Alignment =ObjectFieldAlignment.Center)]

        public Sprite selectedImage;
         [ShowIf("showConstellation")]
        [VerticalGroup("Ancient Force Setup")]
        [PreviewField(Alignment =ObjectFieldAlignment.Center)]
        public Sprite highlightedImage;
         [ShowIf("showConstellation")]
        [VerticalGroup("Ancient Force Setup")]
        [PreviewField(Alignment =ObjectFieldAlignment.Center)]
        public Sprite unlockedImage;
         [ShowIf("showConstellation")]
        [VerticalGroup("Ancient Force Setup")]
        [PreviewField(Alignment =ObjectFieldAlignment.Center)]
        public Sprite constellation;

        
        public CellTopElement GetTopElementToSwap(Cell cell)
        {
            SwapRules expRule = ruleList.Find(rule => rule.condition == cell.data);

            if (!expRule.topElementToSwap) return null;

            return expRule.topElementToSwap;
        }

        public CellData GetTokenToSwap(Cell cell)
        {
            SwapRules expRule = ruleList.Find(rule => rule.condition == cell.data);

            if (!expRule.tokenToSwap) return null;

            return expRule.tokenToSwap;
        }

      




        


    }//closes ForceData class
}//Closes namespace declaration
