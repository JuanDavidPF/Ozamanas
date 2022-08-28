using System;
using System.Collections;
using System.Collections.Generic;
using JuanPayan.References.Floats;
using JuanPayan.References.Integers;
using UnityEngine;

namespace Ozamanas.Board.Levels
{
    [CreateAssetMenu(menuName = "References/Level/LevelReference", fileName = "new LevelReference")]
    public class LevelReference : ScriptableObject
    {


        [SerializeField] private LevelData m_level;
        public LevelData level
        {
            get { return m_level; }
            set
            {
                bool skipUpdate = value == m_level;
                m_level = value;
                if (skipUpdate) return;
                OnLevelChanged?.Invoke(value);
            }
        }

        public event Action<LevelData> OnLevelChanged;

    }//Closes LevelData class

}//Closes Namespace declaration
