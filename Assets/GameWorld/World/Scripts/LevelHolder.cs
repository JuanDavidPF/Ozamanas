using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ozamanas.Levels
{
    public class LevelHolder : MonoBehaviour
    {
        public LevelReference levelSelected;

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

        public void SpawnLevel()
        {
            if (!levelSelected || !levelSelected.level || !levelSelected.level.board) return;

            currentLevel = Instantiate(levelSelected.level.board);

        }//Closes SpawnLevel method


    }//Closes LevelHolder method
}//Closes Levels class
