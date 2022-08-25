using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JuanPayan.References.Floats;

namespace Ozamanas.Machines
{
[System.Serializable]
public class MachineAttributes : MonoBehaviour
{
   

   public HumanMachineToken _id;
   public FloatReference _machineReverseSpeed;
   public MachineType _type = MachineType.Terrestrial;
   public MachineHierarchy _hierarchy = MachineHierarchy.Regular;
   public bool isAlpha = false;
   public Color _color;
   public int armorPoints =1 ;
   public FloatReference machineSpeed;
   public int special = 0;

   public int alphaArmorPoints = 1;
   public FloatReference alphaMachineSpeed;
   public int alphaSpecial = 0;

   public HumanMachineToken ID { get { return _id; } set { _id = value; } }
   public FloatReference MachineReverseSpeed { get { return _machineReverseSpeed; } set { _machineReverseSpeed = value; } }
   public MachineType MachineType { get { return _type; } set { _type = value; } }
   public MachineHierarchy MachineHierarchy { get { return _hierarchy; } set { _hierarchy = value; } }
   public int MachineArmorPoints()
   {
      if (isAlpha) return alphaArmorPoints;
      return armorPoints;
   }
   public FloatReference MachineSpeed()
   {
      if (isAlpha) return alphaMachineSpeed;
      return machineSpeed;
   }
   public int MachineSpecial()
   {
      if (isAlpha) return alphaSpecial;
      return special;
   }

}
}