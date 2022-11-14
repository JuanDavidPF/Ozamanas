using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ozamanas.Levels;
using UnityEngine.EventSystems;
using JuanPayan.Helpers;
using Ozamanas.Tags;
using System;

public class LevelSelectionHandler : MonoBehaviour
{
    [Space(15)]
    [Header("Level Data")]
    [SerializeField] private LevelData levelData;
     [SerializeField] private GameObject charcoal;

     [Space(15)]
    [Header("Followings Levels Data")]
    [SerializeField] private List<LevelSelectionHandler> nextLevels;

    [Space(15)]
    [Header("Level Settings")]
    [SerializeField] private GameObject dottedLine;
    [SerializeField] private LevelReference levelController;
    [SerializeField] private SceneSwitcher sceneSwither;
    [SerializeField] private GameObject levelInfoPanel;


    [SerializeField] private Camera cam;

    private LevelState _state;
    private LevelState state
    {
        get { return _state; }
        set
        {
            _state = value;
            LevelData.state = value;
            UpdateLevelInfoPanel();
            UpdateCharcoalState();
        }
    }

    public GameObject Charcoal { get => charcoal; set => charcoal = value; }
    public LevelData LevelData { get => levelData; set => levelData = value; }

    private void UpdateCharcoalState()
    {
        Charcoal.GetComponent<CharcoalVisuals>().SetVisuals(state);
    }

    private void UpdateLevelInfoPanel()
    {
       
    }

    public void Awake()
    {
        sceneSwither = gameObject.GetComponentInParent<SceneSwitcher>();
    }

    public void Start()
    {
        if(!LevelData) return;

        state = LevelData.state;

        if(!cam || !Charcoal) return;

        Vector3 screenPos = cam.WorldToScreenPoint(Charcoal.transform.position);

        gameObject.GetComponent<RectTransform>().position = screenPos;

        PrintDottedLines();

    }

    private void UpdateNextLevelsStatus()
    {
        if(state != LevelState.Finished) return;

        foreach(LevelSelectionHandler level in nextLevels)
        {
            level.state = LevelState.Playable;
        }
    }
    private void PrintDottedLines()
    {
        foreach(LevelSelectionHandler level in nextLevels)
        {
            DottedLine line = Instantiate(dottedLine, null).GetComponent<DottedLine>();
            line.SetPositions(Charcoal.transform.position, level.Charcoal.transform.position);
        }
    }
    public void PlayLevel()
    {
        if(!LevelData ||!levelController) return;

        if(state == LevelState.Blocked) return;

        Charcoal.GetComponent<Animator>().SetTrigger("Pressed");

        levelController.level = LevelData;

        sceneSwither.Behaviour();
    }

    public void SetAnimation(string trigger)
    {
        Charcoal.GetComponent<Animator>().SetTrigger(trigger);
    }


    public void ActivateLevelPanel()
    {
        if (!levelInfoPanel) return;
        
         levelInfoPanel.SetActive(true);
         levelInfoPanel.GetComponent<LevelPanelSelector>().SetInfoPanelData();
    }

    public void DeactivateLevelPanel()
    {
        if (levelInfoPanel) levelInfoPanel.SetActive(false);

    }
   
}
