using System.Collections;
using System.Collections.Generic;
using JuanPayan.CodeSnippets.HelperComponents;
using UnityEngine;
using UnityEngine.Events;

namespace Ozamanas.Board.Levels
{
    public class LevelSelectionListener : MonobehaviourEvents
    {

        public UnityEvent<LevelData> processes;

        protected override void OnEnable()
        {
            base.OnEnable();
            LevelData.OnLevelSelectedChanged += LevelSelectedChanged;
        }//Closes OnEnable method

        protected override void OnDisable()
        {
            base.OnDisable();
            LevelData.OnLevelSelectedChanged -= LevelSelectedChanged;
        }//Closeos OnDisable method

        public override void Behaviour()
        {
            LevelSelectedChanged(LevelData.levelSelected);
        }//Closes Begaviour method


        private void LevelSelectedChanged(LevelData levelData)
        {
            processes?.Invoke(levelData);
        }//Closes Begaviour method


    }//Closes LevelSelectionListener class

}//Closes Namespace declaration