using System;
using System.Collections;
using System.Collections.Generic;
using JuanPayan.References;
using UnityEngine;
using Ozamanas.Tags;
using Ozamanas; 

namespace Ozamanas.Levels
{
    [CreateAssetMenu(menuName = "References/Level/LevelData", fileName = "new LevelData")]
    public class LevelData : ScriptableObject
    {

        [Space(20)]
        [Header("Level Data")]
        public GameObject board;
        [SerializeField] private LevelReference saveAt;
        public FloatReference creationDelay;
        public IntegerReference creationRate;
        public IntegerReference wavesAmount;
        public IntegerReference wavesCooldown;
        public LevelState state = LevelState.Blocked;

        public float index =0f;

        public string levelName;

        public string levelSceneName;

         [Space(20)]
        [Header("Narrative")]

        public string ifLoseReturnToScene = "Fire";

        public string ifLoseSetDialogue;

        public string ifWinReturnToScene = "Fire";

        public string ifWinSetDialogue;


        [Space(20)]
        [Header("Tutorial")]

        
        public List<TutorialAction> tutorialActions;

        public TutorialAction currentAction;

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
