using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ozamanas.Levels;
using UnityEngine.EventSystems;
using JuanPayan.Helpers;
using Ozamanas.Tags;
using System;

public class LevelHandler : MonoBehaviour
{
    [Space(15)]
    [Header("Level Data")]
    [SerializeField] private LevelData levelData;

     [Space(15)]
    [Header("Followings Levels Data")]
    [SerializeField] private List<LevelHandler> nextLevels;

    [Space(15)]
    [Header("Level Settings")]
    [SerializeField] private GameObject dottedLine;
    [SerializeField] private LevelReference levelController;
    [SerializeField] private SceneSwitcher sceneSwither;
    [SerializeField] private GameObject levelInfoPanel;

    private LevelState _state;
    private LevelState state
    {
        get { return _state; }
        set
        {
            _state = value;
            levelData.state = value;
        }
    }

    public void Awake()
    {
        sceneSwither = gameObject.GetComponentInParent<SceneSwitcher>();
    }

    public void Start()
    {
        if(!levelData) return;

        state = levelData.state;

        PrintDottedLines();

    }

    private void PrintDottedLines()
    {
        foreach(LevelHandler level in nextLevels)
        {
            DottedLine line = Instantiate(dottedLine, null).GetComponent<DottedLine>();
            line.SetPositions(gameObject.transform.position, level.transform.position);
        }
    }

    public void PlayLevel()
    {
        if(!levelData ||!levelController) return;

        if(state == LevelState.Blocked) return;

        levelController.level = levelData;

        sceneSwither.Behaviour();
    }


}
