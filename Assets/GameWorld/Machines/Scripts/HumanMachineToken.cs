using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Ozamanas.Machines
{
[CreateAssetMenu(menuName = "ScriptableObjects/Human Machines/Token", fileName = "new HumanMachine Token")]
public class HumanMachineToken : ScriptableObject
{
    public Sprite machineIcon;
    public Sprite machineArt;
    public Sprite machineCard;
    public LocalizedString machineName = new LocalizedString();
    public LocalizedString machineDescription = new LocalizedString();
    //public NotificationStruct Notification_MachineSpawned;

}//Closes HumanMachineToken
}