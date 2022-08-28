using System;
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
        [SerializeField] private LevelReference saveAt;
        public FloatReference creationDelay;
        public IntegerReference creationRate;
        public IntegerReference wavesAmount;
        public IntegerReference wavesCooldown;

        public void SelectLevel()
        {
            if (saveAt) saveAt.level = this;

        }//Closes SelectLevel method

    }//Closes LevelData class

}//Closes Namespace declaration
