using UnityEngine;
using Ozamanas.Tags;


namespace Ozamanas.Machines
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Human Machines/SpeedValues", fileName = "new HumanMachine SpeedValues")]
    public class MachineSpeedValues : ScriptableObject
    {

        public float reverse;

        public float veryLow;

        public float low;

        public float medium;

        public float high;

        public float veryHigh;

        public float GetSpeed(MachineSpeed value)
        {
            switch(value) 
            {
                
                case MachineSpeed.VeryLow:
                return veryLow;

                case MachineSpeed.Low:
                return low;

                case MachineSpeed.Medium:
                return medium;

                case MachineSpeed.High:
                return high;
               
                default:
                return veryHigh;

            } 
        }


        public MachineSpeed GetSpeedEnumByValue(float speed)
        {
            if (speed == veryLow) return MachineSpeed.VeryLow;
            else if (speed == low) return MachineSpeed.Low;
            else if (speed == medium) return MachineSpeed.Medium;
            else if (speed == high) return MachineSpeed.High;
            else return MachineSpeed.VeryHigh;
        }
    }

    
}