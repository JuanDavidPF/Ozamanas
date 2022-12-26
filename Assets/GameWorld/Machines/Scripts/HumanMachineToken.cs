using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using Sirenix.OdinInspector;
using Ozamanas.Board;
using Ozamanas.Tags;

namespace Ozamanas.Machines
{
[CreateAssetMenu(menuName = "ScriptableObjects/Human Machines/Token", fileName = "new HumanMachine Token")]
public class HumanMachineToken : ScriptableObject
{
    [VerticalGroup("Machine Information")]
    [PreviewField(Alignment =ObjectFieldAlignment.Center)]
    [HideLabel]
    public Sprite machineIcon;

    [VerticalGroup("Machine Information")]
    public LocalizedString machineName = new LocalizedString();

    [VerticalGroup("Machine Information")]
    public LocalizedString machineDescription = new LocalizedString();
     [Title("Show More Settings:")]
    [VerticalGroup("Machine Information")]
    [LabelText("Armor and destructable")]
    public bool showArmorInfo = false;
       [VerticalGroup("Machine Information")]
       [LabelText("Objectives & Tiles")]
    public bool showMovInfo = false;
   [VerticalGroup("Machine Information")]
      [LabelText("Speed & Others")]
    public bool showSpeedInfo = false;

     [VerticalGroup("Machine Setup")]
    [EnumToggleButtons]
    public MachineHierarchy machineHierarchy = MachineHierarchy.Regular;
    
    [VerticalGroup("Machine Setup")]
    [ProgressBar(0,"maxArmorPoints",0,1,1,Segmented = true)]
    public int armorPoints =1 ;

    [VerticalGroup("Machine Setup")]
    public MachineSpeed currentSpeed;

    
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
    [Space(5)]
    [ShowIf("secondObjective")]
    public CellData thirdObjective;
    [ShowIfGroup("Machine Setup/showMovInfo")]
   [VerticalGroup("Machine Setup")]
    [ShowIf("thirdObjective")]
    [Range(1, 20)]
    public int thirdObjectiveRange;
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
    [ShowIfGroup("Machine Setup/showSpeedInfo")]
   [VerticalGroup("Machine Setup")]
    [Range(1f, 5f)]
    public float timeMaxToReachDestination = 5f;


    //public NotificationStruct Notification_MachineSpawned;

}//Closes HumanMachineToken
}