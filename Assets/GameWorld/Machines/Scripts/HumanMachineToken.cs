using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using Sirenix.OdinInspector;
using Ozamanas.Board;
using Ozamanas.Forest;
using Ozamanas.Tags;
using Ozamanas.World;

namespace Ozamanas.Machines
{
[CreateAssetMenu(menuName = "ScriptableObjects/Human Machines/Token", fileName = "new HumanMachine Token")]
public class HumanMachineToken : ScriptableObject
{
     
      [VerticalGroup("Machine Information")]
      [PreviewField(Alignment =ObjectFieldAlignment.Center)]
      [HideLabel]
      public Sprite machineIcon;
      [ShowIf("showData")]

      [VerticalGroup("Machine Information")]
      [PreviewField(Alignment =ObjectFieldAlignment.Center)]
      public Sprite machineCard;

      [VerticalGroup("Machine Information")]
       [ShowIf("showData")]
      public FamilyType machineFamily;

      [VerticalGroup("Machine Information")]
       [ShowIf("showData")]
      public LocalizedString machineName = new LocalizedString();

      [VerticalGroup("Machine Information")]
       [ShowIf("showData")]
      public LocalizedString machineDescription = new LocalizedString();
        [VerticalGroup("Machine Information")]
         [ShowIf("showData")]
      public GameObject machinePrefab;
      [Title("Show More Settings:")]
      [VerticalGroup("Machine Information")]
      [LabelText("Armor and destructable")]
       [ToggleLeft]
      public bool showArmorInfo = false;
      [VerticalGroup("Machine Information")]
      [LabelText("Objectives & Tiles")]
      [ToggleLeft]
      public bool showMovInfo = false;
      [VerticalGroup("Machine Information")]
      [LabelText("Speed & Others")]
       [ToggleLeft]
      public bool showSpeedInfo = false;

       [VerticalGroup("Machine Information")]
      [LabelText("Show Machine Data")]
       [ToggleLeft]
      public bool showData = false;

     
      [VerticalGroup("Machine Setup")]
      [EnumToggleButtons]
      public MachineHierarchy machineHierarchy = MachineHierarchy.Regular;

      [VerticalGroup("Machine Setup")]
      [ProgressBar(0,"maxArmorPoints",0,1,1,Segmented = true)]
      public int armorPoints =1 ;

      [VerticalGroup("Machine Setup")]
      public MachineSpeed currentSpeed;

       [VerticalGroup("Machine Setup")]
      [Title("Swap Tile Rule List:")]

        [SerializeField] private List<SwapRules> ruleList = new List<SwapRules>();


      [ShowIfGroup("Machine Setup/showArmorInfo")]
      [VerticalGroup("Machine Setup")]
      [Title("Machine Armor")]

      public int maxArmorPoints =5 ;
      [ShowIfGroup("Machine Setup/showArmorInfo")]
      [VerticalGroup("Machine Setup")]
      [Required]
      public GameObject destroyedMachine;
      [ShowIfGroup("Machine Setup/showArmorInfo")]
      [VerticalGroup("Machine Setup")]
      [Range(1f, 5f)]
      public float lifeSpan = 2f;
      [ShowIfGroup("Machine Setup/showArmorInfo")]
      [VerticalGroup("Machine Setup")]
      [Range(100, 1000)]
      public float explosionPower = 2f;

      [ShowIfGroup("Machine Setup/showMovInfo")]
      [VerticalGroup("Machine Setup")]
      [Title("Machine Movement")]
      [Required]
      public CellData humanBase;
      [ShowIfGroup("Machine Setup/showMovInfo")]
      [VerticalGroup("Machine Setup")]
      [Required]
      public CellData mainObjective;
      [ShowIfGroup("Machine Setup/showMovInfo")]
      [VerticalGroup("Machine Setup")]
      [Range(1, 5)]
      public int OnDistanceToHeartNotification ;
      [ShowIfGroup("Machine Setup/showMovInfo")]
      [VerticalGroup("Machine Setup")]
      [Space(5)]
      public CellData secondObjective;
      [ShowIfGroup("Machine Setup/showMovInfo")]
      [VerticalGroup("Machine Setup")]
      [ShowIf("secondObjective")]
      [Range(1, 20)]
      public int secondObjectiveRange;
      [ShowIfGroup("Machine Setup/showMovInfo")]
      [VerticalGroup("Machine Setup")]
      [ShowIf("secondObjective")]
      public bool obviateMainObjective;
      [ShowIfGroup("Machine Setup/showMovInfo")]
      [VerticalGroup("Machine Setup")]
      public List<CellData> cellBlacklist;

      [ShowIfGroup("Machine Setup/showSpeedInfo")]
      [VerticalGroup("Machine Setup")]
      [Title("Machine Speed")]
      [Required]
      public MachineSpeedValues speedValues;
      [ShowIfGroup("Machine Setup/showSpeedInfo")]
      [VerticalGroup("Machine Setup")]
      public MachineAltitude currentAltitude;
      [ShowIfGroup("Machine Setup/showSpeedInfo")]
      [VerticalGroup("Machine Setup")]
      [Range(0.1f, 5f)]
      public float height = 5f;
     

      public CellTopElement GetTopElementToSwap(Cell cell)
        {
            SwapRules expRule = ruleList.Find(rule => rule.condition == cell.data);

            if (expRule== null) return null;

            return expRule.topElementToSwap;
        }

        public CellData GetTokenToSwap(Cell cell)
        {
            SwapRules expRule = ruleList.Find(rule => rule.condition == cell.data);

            if (expRule == null) return null;

            return expRule.tokenToSwap;
        }


    //public NotificationStruct Notification_MachineSpawned;

}//Closes HumanMachineToken
}