using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JuanPayan.Helpers;
using Ozamanas.Levels;
using Ozamanas.SaveSystem;

namespace Ozamanas
{
public class OnLoseSceneSelector : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private PlayerDataHolder currentSaveState;
     [SerializeField] private SceneSwitcher sceneSwitcher;
    void Awake()
    {
        if(!sceneSwitcher) sceneSwitcher = GetComponent<SceneSwitcher>();
    }


    public void SwitchToScene()
    {
       LevelHolder temp = GameObject.FindGameObjectWithTag("SceneController").GetComponentInChildren<LevelHolder>();
       if(temp) sceneSwitcher.SceneToLoad =  temp.levelSelected.level.ifLoseReturnToScene;
       sceneSwitcher.SetSingleMode();
       sceneSwitcher.Behaviour();

    }
}
}
