using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Ozamanas.Levels;

public class LevelPanelSelector : MonoBehaviour
{
   private LevelData level;

   [Header("ID panel components references")]
        [Space(10)]

        [SerializeField] TextMeshProUGUI panelLevelIndex;
        [SerializeField] TextMeshProUGUI panelLevelName;

   void Awake()
   {
        level = GetComponentInParent<LevelSelectionHandler>().LevelData;
   }

   void Start()
   {
        SetInfoPanelData();
   }

    public void SetInfoPanelData()
    {
        panelLevelIndex.text = "Level " +level.index;
        panelLevelName.text = level.levelName;
    }

}
