using UnityEngine;
using Ozamanas.Tags;

namespace Ozamanas.Machines
{
    [System.Serializable]
public class MachineAttributes : MonoBehaviour
{
   public HumanMachineToken _id;
   public MachineSpeed _machineReverseSpeed = MachineSpeed.Low;
   public MachineType _type = MachineType.Terrestrial;
   public MachineHierarchy _hierarchy = MachineHierarchy.Regular;
   public bool isAlpha = false;
   public Color _color;
   public int maxArmorPoints =1 ;
   public int armorPoints =1 ;
   public MachineSpeed machineSpeed = MachineSpeed.Medium;
   public int special = 0;

   public int alphaArmorPoints = 1;
   public MachineSpeed alphaMachineSpeed = MachineSpeed.High;
   public int alphaSpecial = 0;

   public HumanMachineToken ID { get { return _id; } set { _id = value; } }
   public MachineSpeed MachineReverseSpeed { get { return _machineReverseSpeed; } set { _machineReverseSpeed = value; } }
   public MachineType MachineType { get { return _type; } set { _type = value; } }
   public MachineHierarchy MachineHierarchy { get { return _hierarchy; } set { _hierarchy = value; } }
   public int GetMachineArmorPoints()
   {
      if (isAlpha) return alphaArmorPoints;
      return armorPoints;
   }
   public MachineSpeed GetMachineSpeed()
   {
      if (isAlpha) return alphaMachineSpeed;
      return machineSpeed;
   }
   public int GetMachineSpecial()
   {
      if (isAlpha) return alphaSpecial;
      return special;
   }

}
}