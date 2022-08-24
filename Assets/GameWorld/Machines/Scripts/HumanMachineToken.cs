using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;


[CreateAssetMenu(menuName = "ScriptableObjects/Human Machines/Token", fileName = "new HumanMachine Token")]
public class HumanMachineToken : ScriptableObject
{
    public Sprite machineIcon;
    public Sprite codexArt;
    public Sprite codexCard;
    public LocalizedString machineName = new LocalizedString();
    public LocalizedString machineDescription = new LocalizedString();
    //public NotificationStruct Notification_MachineSpawned;

}//Closes HumanMachineToken