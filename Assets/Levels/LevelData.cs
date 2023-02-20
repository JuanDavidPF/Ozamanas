using System;
using System.Collections;
using System.Collections.Generic;
using JuanPayan.References;
using UnityEngine;
using UnityEngine.UI;
using Ozamanas.Tags;
using Ozamanas.Machines; 
using Ozamanas.Forces;
using Sirenix.OdinInspector;
using UnityEngine.Localization;


namespace Ozamanas.Levels
{
    [CreateAssetMenu(menuName = "References/Level/LevelData", fileName = "new LevelData")]
    public class LevelData : ScriptableObject
    {
        
         [Title("Level Status:")]
          [VerticalGroup("Level Information")]
        public LevelState state = LevelState.Blocked;
        [Title("Level Name:")]
        [VerticalGroup("Level Information")]
        public float index =0f;
        [VerticalGroup("Level Information")]
        [PreviewField(Alignment =ObjectFieldAlignment.Center)]
        public Sprite levelIcon;

        [VerticalGroup("Level Information")]

        public LocalizedString levelName;
        [VerticalGroup("Level Information")]

        public LocalizedString levelMainObjective;
        [VerticalGroup("Level Information")]
        public string levelSceneName;
        [Title("Level Settings:")]
        [VerticalGroup("Level Information")]
        public FloatReference creationDelay;
        [VerticalGroup("Level Information")]
        public IntegerReference creationRate;

        [VerticalGroup("Level Information")]
        public IntegerReference wavesAmount;

        [VerticalGroup("Level Information")]
        public IntegerReference wavesCooldown;
        [Title("Show More Settings:")]
        [VerticalGroup("Level Information")]
        [LabelText("Is a Tutorial Level?")]
        [ToggleLeft]
        public bool showIfTutorial = false;
         [Title("List of Machines:")]
         [VerticalGroup("Others")]
        public List<HumanMachine> machines;
       

        [VerticalGroup("Others")]
        [Title("End Level Settings:")]

        public List<ForceData> unlockForces;
           [VerticalGroup("Others")]
        public Scenes ifLoseReturnToScene = Scenes.Fire;
         [VerticalGroup("Others")]

        public Scenes ifWinReturnToScene = Scenes.Fire;
         [VerticalGroup("Others")]
        [SerializeField] private LevelReference saveAt;
           [Title("Tutorial Settings:")]
         [ShowIfGroup("Others/showIfTutorial")]
        [VerticalGroup("Others")]

        
        public List<TutorialAction> tutorialActions;
         [ShowIfGroup("Others/showIfTutorial")]
         [VerticalGroup("Others")]

        public TutorialAction currentAction;

        public void SelectLevel()
        {
            if (saveAt) saveAt.level = this;

        }

       

    }

}
