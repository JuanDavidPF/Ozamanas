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
    [Title("Machine Information")]
    public Sprite machineIcon;
    public LocalizedString machineName = new LocalizedString();
    public LocalizedString machineDescription = new LocalizedString();
    [EnumToggleButtons]
    public MachineHierarchy machineHierarchy = MachineHierarchy.Regular;

    [Title("Armor Setup")]
    [ProgressBar(0,"maxArmorPoints",0,1,1,Segmented = true)]
   // [PropertyRange(1,"maxArmorPoints")]
    public int armorPoints =1 ;
    public int maxArmorPoints =5 ;
    [Required]
    public GameObject destroyedMachine;
    [Range(1f, 5f)]
    public float lifeSpan = 2f;
    [Range(100, 1000)]
    public float explosionPower = 2f;

     
    [Title("Movement Setup")]
    [Required]
    public CellData humanBase;
    [Required]
    public CellData mainObjective;
    [Range(1, 5)]
    public int OnDistanceToHeartNotification ;
    [Space(5)]
    public CellData secondObjective;
    [ShowIf("secondObjective")]
    [Range(1, 20)]
    public int secondObjectiveRange;
    [Space(5)]
    [ShowIf("secondObjective")]
    public CellData thirdObjective;
    [ShowIf("thirdObjective")]
    [Range(1, 20)]
    public int thirdObjectiveRange;
    public List<CellData> cellBlacklist;

    [Title("Speed Setup")]

    [Required]
    public MachineSpeedValues speedValues;
    public MachineSpeed currentSpeed;
     [EnumToggleButtons]
    public MachineAltitude currentAltitude;
    [Range(0.1f, 5f)]
    public float height = 5f;
    [Range(1f, 5f)]
    public float timeMaxToReachDestination = 5f;


    //public NotificationStruct Notification_MachineSpawned;

}//Closes HumanMachineToken
}