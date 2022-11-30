using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ozamanas.Levels;
using UnityEngine.EventSystems;
using JuanPayan.Helpers;
using Ozamanas.Tags;
using System;

public class LevelHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [Space(15)]
    [Header("Level Data")]
    [SerializeField] private LevelData levelData;

    [SerializeField] private string gameplayScene;

     [Space(15)]
    [Header("Followings Levels Data")]
    [SerializeField] private List<LevelHandler> nextLevels;

    private List<LevelHandler> predecesorsLevels = new List<LevelHandler>();


    [Space(15)]
    [Header("Level Settings")]
    [SerializeField] private GameObject dottedLine;
    [SerializeField] private LevelReference levelController;
    [SerializeField] private SceneSwitcher sceneSwither;
    [SerializeField] private LevelPanelSelector levelInfoPanel;

    private Animator animator;
   private Vector3 screenPos;
    private PlayerController player;


    private Collider levelCollider;
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

    public List<LevelHandler> NextLevels { get => nextLevels; set => nextLevels = value; }
    public List<LevelHandler> PredecesorsLevels { get => predecesorsLevels; set => predecesorsLevels = value; }

    public void Awake()
    {
        sceneSwither = gameObject.GetComponentInParent<SceneSwitcher>();
        levelCollider = gameObject.GetComponent<Collider>();
        animator = gameObject.GetComponent<Animator>();
        player= GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        Camera cam = Camera.main;
        screenPos = cam.WorldToScreenPoint(gameObject.transform.position);
    }

    public void Start()
    {
        if(!levelData) return;

        state = levelData.state;

        PrintDottedLines();

        BringPlayerToLevel();

        SetUpPredecesors();

    }

    private void SetUpPredecesors()
    {
        foreach ( LevelHandler next in NextLevels)
        {
            next.PredecesorsLevels.Add(this);
        }
    }

    private void BringPlayerToLevel()
    {
       if(levelController.level != levelData ) return;

       player.transform.position = transform.position;
    }

    private void PrintDottedLines()
    {
        foreach(LevelHandler level in NextLevels)
        {
            DottedLine line = Instantiate(dottedLine, gameObject.transform).GetComponent<DottedLine>();
            line.SetPositions(ClosestPointOnBounds(level.transform.position), level.ClosestPointOnBounds(gameObject.transform.position));
        }
    }

    public void PlayLevel()
    {
        if(!levelData ||!levelController) return;

        if(state == LevelState.Blocked) return;

        levelController.level = levelData;

        sceneSwither.SceneToLoad = gameplayScene;

        sceneSwither.SetSingleMode();

        sceneSwither.Behaviour();
    }

    public Vector3 ClosestPointOnBounds(Vector3 position)
    {
        return levelCollider.ClosestPointOnBounds(position);
    }

     void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
       if(levelData.state == LevelState.Blocked) return;
       
       if(player.transform.position == gameObject.transform.position ) PlayLevel();

       if (player.PlayerState == PlayerState.Running) return;

       

       if(MovementApproval(player.transform.position)) player.MoveToDestination(gameObject.transform.position);
    }

    private bool MovementApproval(Vector3 position)
    {
        foreach(LevelHandler level in PredecesorsLevels)
        {
            if(level.transform.position == position) return true;
        }

        foreach(LevelHandler level in NextLevels)
        {
            if(level.transform.position == position) return true;
        }

        return false;
    }
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
       animator.SetTrigger("Selected");
       levelInfoPanel.SetInfoPanelData(levelData);
       levelInfoPanel.Show(screenPos);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        animator.SetTrigger("UnSelected");
        levelInfoPanel.Hide();
    }

   
}
