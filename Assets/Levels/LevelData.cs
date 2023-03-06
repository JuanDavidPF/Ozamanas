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
        [LabelText("Show Level Data")]
        [ToggleLeft]
        public bool showLevelData = false;
        [VerticalGroup("Level Information")]
        [LabelText("Is a Tutorial Level?")]
        [ToggleLeft]
        public bool showIfTutorial = false;
        [VerticalGroup("Level Information")]
        [LabelText("Edit 'End Level' Settings")]
        [ToggleLeft]
        public bool showEndLevel = false;
        [VerticalGroup("Level Information")]
        [LabelText("Show Level Climate")]
        [ToggleLeft]
        public bool showClimate = false;
        [Title("List of Machines:")]
        [VerticalGroup("Others")]
        public List<HumanMachineToken> machines;
        [VerticalGroup("Others")]
        [Title("End Level Settings:")]
        [ShowIf("showEndLevel")]
        public List<ForceData> unlockForces;
        [VerticalGroup("Others")]
        [ShowIf("showEndLevel")]
        [LabelText("On 'Lose' Go To (Scene)?")]
        public Scenes ifLoseReturnToScene = Scenes.Fire;
        [VerticalGroup("Others")]
        [ShowIf("showEndLevel")]
        [LabelText("On 'Win' Go To (Scene)?")]
        public Scenes ifWinReturnToScene = Scenes.Fire;
        [Title("Tutorial Settings:")]
        [ShowIfGroup("Others/showIfTutorial")]
        [VerticalGroup("Others")]
        public List<TutorialAction> tutorialActions;
        [ShowIfGroup("Others/showIfTutorial")]
        [VerticalGroup("Others")]
        public TutorialAction currentAction;

          [ShowIf("showClimate")]
        [Title("Level Climate:")]
        [VerticalGroup("Others")]
        public Material skyBox;
        
        [ShowIf("showLevelData")]
        [Title("Level Data:")]
        [VerticalGroup("Others")]
        [PreviewField(Alignment =ObjectFieldAlignment.Center)]

        public Sprite levelIcon;
        [ShowIf("showLevelData")]
        [VerticalGroup("Others")]
        public LocalizedString levelName;
        [ShowIf("showLevelData")]
        [VerticalGroup("Others")]
        public LocalizedString levelMainObjective;
        [ShowIf("showLevelData")]
        [VerticalGroup("Others")]
        public string levelSceneName;
        [ShowIf("showLevelData")]
        [VerticalGroup("Others")]
        [SerializeField] private LevelReference saveAt;

        public void SelectLevel()
        {
            if (saveAt) saveAt.level = this;

        }

       

    }

}
