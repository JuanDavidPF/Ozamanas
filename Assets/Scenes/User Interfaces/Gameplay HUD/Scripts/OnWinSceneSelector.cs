using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JuanPayan.Helpers;
using Ozamanas.Levels;
using Ozamanas.SaveSystem;

namespace Ozamanas.GameScenes
{
public class OnWinSceneSelector : MonoBehaviour
{
     [SerializeField] private PlayerDataHolder currentSaveState;
     [SerializeField] private SceneSwitcher sceneSwitcher;
    void Awake()
    {
        if(!sceneSwitcher) sceneSwitcher = GetComponent<SceneSwitcher>();
    }


    public void SwitchToScene()
    {
       LevelHolder temp = GameObject.FindGameObjectWithTag("SceneController").GetComponentInChildren<LevelHolder>();
       if(temp) sceneSwitcher.SceneToLoad =  temp.levelSelected.level.ifWinReturnToScene;
       sceneSwitcher.SetSingleMode();
       sceneSwitcher.Behaviour();

    }
}
}
