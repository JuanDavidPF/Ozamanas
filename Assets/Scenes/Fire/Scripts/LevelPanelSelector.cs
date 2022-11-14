using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using Ozamanas.Tags;
using Ozamanas.Levels;

public class LevelPanelSelector : MonoBehaviour
{
   private LevelData level;
   private LocalizeStringEvent localizedStringEvent;


   [Header("ID panel components references")]
        [Space(10)]

        [SerializeField] TextMeshProUGUI panelLevelIndex;
        [SerializeField] TextMeshProUGUI panelLevelName;
        [SerializeField] TextMeshProUGUI panelLevelState;
        [SerializeField] private string table;
        [SerializeField] private List<string> stateKeys;


   void Awake()
   {
        level = GetComponentInParent<LevelSelectionHandler>().LevelData;
        localizedStringEvent = panelLevelState.GetComponent<LocalizeStringEvent>();
   }

   void Start()
   {
        SetInfoPanelData();
       
   }

    public void SetInfoPanelData()
    {
        panelLevelIndex.text = "Level " +level.index;
        panelLevelName.text = level.levelName;
        
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

}
