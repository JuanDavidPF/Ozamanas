using System;
using System.Collections;
using System.Collections.Generic;
using JuanPayan.References;
using UnityEngine;
using Ozamanas.Tags;

namespace Ozamanas.Levels
{
    [CreateAssetMenu(menuName = "References/Level/LevelData", fileName = "new LevelData")]
    public class LevelData : ScriptableObject
    {


        public GameObject board;
        [SerializeField] private LevelReference saveAt;
        public FloatReference creationDelay;
        public IntegerReference creationRate;
        public IntegerReference wavesAmount;
        public IntegerReference wavesCooldown;
        public LevelState state = LevelState.Blocked;

        public float index =0f;

        public string levelName;

        public void SelectLevel()
        {
            if (saveAt) saveAt.level = this;

        }//Closes SelectLevel method

        public void BakeCells()
        {
            if (board) return;

        }//Closes SelectLevel method

    }//Closes LevelData class

}//Closes Namespace declaration
