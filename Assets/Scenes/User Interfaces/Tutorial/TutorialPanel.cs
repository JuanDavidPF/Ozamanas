using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ozamanas.Levels;
using JuanPayan.References;
using Ozamanas.Tags;
using UnityEngine.Events;

public class TutorialPanel : MonoBehaviour
{
        [SerializeField] TextMeshProUGUI titleString;
        [SerializeField] TextMeshProUGUI descriptionString;
        [SerializeField] private LevelReference levelController;

         [SerializeField] private GameEvent OnExitTutorialAction;

 void Awake()
    {
       if (!levelController || !levelController.level || !levelController.level.currentAction) return;

       UpdatePanel();
    }

  
    
    public void UpdatePanel()
    {
       if (titleString) titleString.text = levelController.level.currentAction.title.GetLocalizedString();
       if (descriptionString) descriptionString.text = levelController.level.currentAction.description.GetLocalizedString();

    }

    public void ExitTutorialAction()
    {
        if (OnExitTutorialAction) OnExitTutorialAction.Invoke();
    }



}
