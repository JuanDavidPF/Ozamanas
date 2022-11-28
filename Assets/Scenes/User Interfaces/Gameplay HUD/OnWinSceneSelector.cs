using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JuanPayan.Helpers;
using Ozamanas.Levels;
using Ozamanas.SaveSystem;
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
       if(!string.IsNullOrEmpty(temp.levelSelected.level.ifWinSetDialogue)) currentSaveState.data.currentDialogueCode = temp.levelSelected.level.ifWinSetDialogue;
       sceneSwitcher.SetSingleMode();
       sceneSwitcher.Behaviour();

    }
}
