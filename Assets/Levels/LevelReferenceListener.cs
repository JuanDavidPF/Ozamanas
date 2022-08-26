using System.Collections;
using System.Collections.Generic;
using JuanPayan.CodeSnippets.HelperComponents;
using UnityEngine;
using UnityEngine.Events;

namespace Ozamanas.Board.Levels
{
    public class LevelReferenceListener : MonobehaviourEvents
    {
        [SerializeField] private LevelReference levelReference;
        public UnityEvent<LevelData> processes;

        protected override void OnEnable()
        {
            base.OnEnable();

            if (levelReference)
                levelReference.OnLevelChanged += LevelSelectedChanged;
        }//Closes OnEnable method

        protected override void OnDisable()
        {
            base.OnDisable();

            if (levelReference)
                levelReference.OnLevelChanged -= LevelSelectedChanged;
        }//Closeos OnDisable method

        public override void Behaviour()
        {
            if (levelReference) LevelSelectedChanged(levelReference.level);
        }//Closes Begaviour method


        private void LevelSelectedChanged(LevelData levelData)
        {
            processes?.Invoke(levelData);
        }//Closes LevelSelectedChanged method


    }//Closes LevelSelectionListener class

}//Closes Namespace declaration