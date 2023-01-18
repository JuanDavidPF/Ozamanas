using System.Collections;
using System.Collections.Generic;
using JuanPayan.References;
using Ozamanas.Board;
using UnityEngine;
using UnityEngine.Localization;
using Ozamanas.Tags;
using Sirenix.OdinInspector;

namespace Ozamanas.Forces
{
    [CreateAssetMenu(fileName = "new Force data", menuName = "Forces/Reference")]
    public class ForceData : ScriptableObject
    {
        [Space]
        [VerticalGroup("Ancient Force Information")]
        [PreviewField(Alignment =ObjectFieldAlignment.Center)]
        public Sprite forceIcon;
        [VerticalGroup("Ancient Force Information")]
        [PreviewField(Alignment =ObjectFieldAlignment.Center)]
        public Sprite forceCard;
        [VerticalGroup("Ancient Force Information")]
        [LabelText("Force Prefab")]
        public AncientForce force;
        [VerticalGroup("Ancient Force Information")]
        public LocalizedString forceName;
        [VerticalGroup("Ancient Force Information")]
        public LocalizedString forceDescription;

        [Title("Sky view Setup")]
        [VerticalGroup("Ancient Force Information")]
        [PreviewField(Alignment =ObjectFieldAlignment.Center)]
        public Sprite forceConstellation;
        [VerticalGroup("Ancient Force Information")]
        [LabelText("Force Stars Prefab")]
        public GameObject stars;
        
        [Title("Show More Settings:")]
         [VerticalGroup("Ancient Force Information")]
        [LabelText("Placement Setup")]
        [ToggleLeft]
        public bool showPlacement;

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

        
     




        


    }//closes ForceData class
}//Closes namespace declaration
