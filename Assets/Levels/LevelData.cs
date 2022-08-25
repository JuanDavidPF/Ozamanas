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
        public static LevelData levelSelected
        {
            get { return m_levelSelected; }
            set
            {
                bool skipUpdate = value == m_levelSelected;

                m_levelSelected = value;

                if (skipUpdate) return;
                OnLevelSelectedChanged?.Invoke(value);
            }
        }
        private static LevelData m_levelSelected;



        public static event Action<LevelData> OnLevelSelectedChanged;


        [HideInInspector] public GameObject board;
        public FloatReference creationDelay;
        public IntegerReference creationRate;



        public void SelectLevel()
        {
            levelSelected = this;
        }//Closes SelectLevel method

    }//Closes LevelData class

}//Closes Namespace declaration
