using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using Ozamanas.Tags;
using Ozamanas.Levels;
using DG.Tweening;

public class LevelPanelSelector : MonoBehaviour
{
   private LevelData level;
   private LocalizeStringEvent localizedStringEvent;

   private CanvasGroup canvasGroup;


   [Header("ID panel components references")]
        [Space(10)]

        [SerializeField] TextMeshProUGUI panelLevelIndex;
        [SerializeField] TextMeshProUGUI panelLevelName;
        [SerializeField] TextMeshProUGUI panelLevelState;
        [SerializeField] private string table;
        [SerializeField] private List<string> stateKeys;


          void Awake()
          {
               localizedStringEvent = panelLevelState.GetComponent<LocalizeStringEvent>();
               canvasGroup = GetComponent<CanvasGroup>();
          }

    public void SetInfoPanelData(LevelData level)
    {
        panelLevelIndex.text = "Level " +level.index;
        panelLevelName.text = level.levelName.GetLocalizedString();
        
        switch(level.state)
        {
          case LevelState.Blocked:
          localizedStringEvent.StringReference.SetReference(table, stateKeys[0]);
          break;
          case LevelState.Playable:
          localizedStringEvent.StringReference.SetReference(table, stateKeys[1]);          
          break;
           case LevelState.Finished:
          localizedStringEvent.StringReference.SetReference(table, stateKeys[2]);         
          break;
        }

    }

    public void Show(Vector3 pos)
    {
      gameObject.GetComponent<RectTransform>().position = pos;
     canvasGroup.DOFade(1f,0.2f).From(0f);
          
    }

    public void Hide()
    {
           canvasGroup.DOFade(0f,0.2f).From(1f);
    }

}
