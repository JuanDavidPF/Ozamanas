using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ozamanas.Levels;
using UnityEngine.EventSystems;
using JuanPayan.Helpers;
using Ozamanas.Tags;
using System;
using TMPro;
using Ozamanas.GameScenes;
using DG.Tweening;


namespace Ozamanas.UI
{
public class LevelHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [Space(15)]
    [Header("Level Data")]
    [SerializeField] private LevelData levelData;

    [Space(15)]
    [Header("Followings Levels Data")]
    [SerializeField] private List<LevelHandler> nextLevels;
    private List<LevelHandler> predecesorsLevels = new List<LevelHandler>();

    [Space(15)]
    [Header("Level Settings")]
    [SerializeField] private GameObject dottedLine;
    [SerializeField] private LevelReference levelController;
    [SerializeField] private ButtonContainer playButton;
    [SerializeField] private MachineDeckManager machineDeck;
    [SerializeField] private PhraseContainer phraseContainer;

     [SerializeField] private TextMeshProUGUI index;

      [SerializeField] private TextMeshProUGUI levelName;

     [SerializeField] private PhraseSequence phraseSequence;


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

    private bool isPlayerDestination = false;
     void Awake()
    {
        levelCollider = gameObject.GetComponent<Collider>();
        animator = gameObject.GetComponent<Animator>();
        player= GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        Camera cam = Camera.main;
        screenPos = cam.WorldToScreenPoint(gameObject.transform.position);
    }

     void Start()
    {
        if(!levelData) return;

        state = levelData.state;

       // PrintDottedLines();

        BringPlayerToLevel();

        SetUpPredecesors();

        

    }

    void Update()
    {
        if(player.PlayerState != PlayerState.Waiting ) return;

        if(!isPlayerDestination) return;

        player.PlayerState = PlayerState.Idling;

        player.transform.DOLookAt(GetNextLevelsViewPoint(),0.5f);

        SetToPlayLevel();

        

    }
    private void SetUpUI()
    {
        playButton.gameObject.SetActive(true);

        machineDeck.gameObject.SetActive(true);

        machineDeck.LevelData = levelData;

        machineDeck.LoadMachineDeck();

        index.text = levelData.index.ToString();

        index.transform.parent.gameObject.SetActive(true);

        levelName.text = levelData.levelName.GetLocalizedString(); 

        levelName.gameObject.SetActive(true);

        phraseSequence.ResetSequence();

        phraseSequence.phrases.Add(levelData.levelMainObjective);

        phraseContainer.StartDialogue(phraseSequence);
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

       GetNextLevelsViewPoint();

      
    }

    private void PrintDottedLines()
    {
        foreach(LevelHandler level in NextLevels)
        {
            DottedLine line = Instantiate(dottedLine, gameObject.transform).GetComponent<DottedLine>();
            line.SetPositions(ClosestPointOnBounds(level.transform.position), level.ClosestPointOnBounds(gameObject.transform.position));
        }
    }

    public void SetToPlayLevel()
    {
        if(!levelData ||!levelController) return;

        if(state == LevelState.Blocked) return;

        levelController.level = levelData;

        SetUpUI();
        
    }

    public Vector3 ClosestPointOnBounds(Vector3 position)
    {
        return levelCollider.ClosestPointOnBounds(position);
    }

     void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
       if(levelData.state == LevelState.Blocked) return;

        if (player.PlayerState == PlayerState.Running) return;
       
       if(player.transform.position == gameObject.transform.position ) 
       {
            SetToPlayLevel();
            return;
       }
       if(!MovementApproval(player.transform.position)) return; 
       
       player.MoveToDestination(transform);

       isPlayerDestination = true;

       playButton.gameObject.SetActive(false);

       machineDeck.ClearMachineDeck();

       machineDeck.gameObject.SetActive(false);

        LevelHandler tempLevel = GetPlayerLevelFromPosition();

         if(tempLevel) tempLevel.isPlayerDestination = false;
    }

    private LevelHandler GetPlayerLevelFromPosition()
    {
        foreach (LevelHandler level in predecesorsLevels )
        {
            if(level.transform.position == player.transform.position) return level;
        }

        return null;
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

    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        animator.SetTrigger("UnSelected");

    }

    public Vector3 GetNextLevelsViewPoint()
    {
        if(nextLevels.Count == 0) return Vector3.zero;

        else if(nextLevels.Count == 1) return nextLevels[0].transform.position;

        else if(nextLevels.Count == 2) return Vector3.Lerp(nextLevels[0].transform.position,nextLevels[1].transform.position,0.5f);

        else return nextLevels[1].transform.position;
    }

   
}
}