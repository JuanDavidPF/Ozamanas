using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JuanPayan.Helpers;
using System;

namespace Ozamanas.Levels
{
    public class LevelHolder : MonoBehaviour
    {
        public LevelReference levelSelected;
        public SceneSwitcher sceneSwitcher;
        private static GameObject m_currentLevel;
        public static GameObject currentLevel
        {
            get { return m_currentLevel; }
            set
            {
                if (m_currentLevel)
                {
                    m_currentLevel.SetActive(false);
                    Destroy(m_currentLevel);
                    m_currentLevel = null;
                }

                if (value) value.SetActive(true);
                m_currentLevel = value;

            }
        }

        void Awake()
        {
            sceneSwitcher = GetComponent<SceneSwitcher>();
        }

        public void SpawnLevel()
        {
            if (!levelSelected || !levelSelected.level || !levelSelected.level.board) return;

            currentLevel = Instantiate(levelSelected.level.board);

        }
        
        public void InstantiateLevel()
        {
                if (!levelSelected || !levelSelected.level || !levelSelected.level.board) return;

                if(String.IsNullOrEmpty(levelSelected.level.levelSceneName)) return;

                sceneSwitcher.Behaviour(levelSelected.level.levelSceneName);
        }
        
        //Closes SpawnLevel method


    }//Closes LevelHolder method
}//Closes Levels class
