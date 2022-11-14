using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JuanPayan.Helpers;
using Ozamanas.Levels;

namespace Ozamanas.World
{
    public class EndGameManager : MonoBehaviour
    {

         [SerializeField] SceneSwitcher sceneSwitcher;
         [SerializeField] private string winSceneToLoad;
        [SerializeField] private string loseSceneToLoad;

        [SerializeField] private LevelReference levelSelected;
        public void CallVictory()
        {
            
            levelSelected.level.state = Tags.LevelState.Finished;
            sceneSwitcher.SceneToLoad = winSceneToLoad;
            sceneSwitcher.Behaviour();
        }

        public void CallDefeat()
        {
            sceneSwitcher.SceneToLoad = loseSceneToLoad;
            sceneSwitcher.Behaviour();
        }
    }
}
