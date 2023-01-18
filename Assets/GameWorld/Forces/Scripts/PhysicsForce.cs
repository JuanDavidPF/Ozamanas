using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Ozamanas.Tags;

namespace Ozamanas.Forces
{   
    [CreateAssetMenu(fileName = "new Force", menuName = "Forces/Physics Force")]
    public class PhysicsForce :  ScriptableObject
    {
        [Title("Add Force to Machine Setup")]
        [EnumToggleButtons]
        public AddForceType type;
        [Range(1,5)]
        public float jumpPower; 
        [Range(.5f,5f)]
        public float duration;
        [Range(1,5)]
        public int flips;
        [LabelText("Distance in Tiles")]
        [Range(1,5)]
        public float tiles = 1;

    }
}
