using System.Collections;
using System.Collections.Generic;
using JuanPayan.References.Floats;
using JuanPayan.References.Integers;
using UnityEngine;

namespace Ozamanas.Board.Levels
{
    [CreateAssetMenu(menuName = "References/Level/LevelData", fileName = "new LevelData")]
    public class LevelData : ScriptableObject
    {
        [HideInInspector] public GameObject board;
        public FloatReference creationDelay;
        public IntegerReference creationRate;

    }//Closes LevelData class
}//Closes Namespace declaration
