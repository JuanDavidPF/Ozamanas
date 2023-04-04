using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JuanPayan.Helpers;
using Ozamanas.Tags;
using DG.Tweening;
using Ozamanas.Board;
using JuanPayan.References;
using Ozamanas.Extenders;


namespace Ozamanas.Levels
{
public class TutorialLogic : MonoBehaviour
{
     public enum TutorialState
        {
            Inactive,
            Active,
            Waiting, 
            Complete
        }

    [SerializeField] private TutorialState state;
    [SerializeField] private LevelReference levelSelected;
    [SerializeField] private Scenes explicativeScene;
    [SerializeField] private Scenes interactuableScene;
    [Range(0.1f,10f)]
    [SerializeField] private float focusSpeed = 0.2f;

    private SceneSwitcher sceneSwitcher;

    private CameraAnchor cameraAnchor;

    private int currentActionIndex = 0;

     private Tween anchorTween;

    private void Awake()
    {
        if (!levelSelected || !levelSelected.level) return;

        sceneSwitcher = GetComponent<SceneSwitcher>();
        cameraAnchor = FindObjectOfType<CameraAnchor>();
    }

    private void Start()
    {
        if( levelSelected.level.tutorialActions.Count == 0) state = TutorialState.Inactive;
        else state = TutorialState.Active;
    }


    public void ListenToEventToPlayAction(GameEvent gameEvent)
    {
        if(state != TutorialState.Waiting) return;

        if(levelSelected.level.currentAction.trigger != gameEvent ) return;

        state = TutorialState.Active;

        ExecuteCurrentAction();
    }

    private void PlayNextAction()
    {   
        if(state == TutorialState.Inactive) return;
        
        if( currentActionIndex >= levelSelected.level.tutorialActions.Count ) 
        {
            state = TutorialState.Complete;
            return;
        }

        levelSelected.level.currentAction = levelSelected.level.tutorialActions[currentActionIndex];

        if(levelSelected.level.currentAction.trigger != null)
        {
            state = TutorialState.Waiting;
            return;
        }
        else
        {
            state = TutorialState.Active;
        }

        ExecuteCurrentAction();
       
    }

    private void ExecuteCurrentAction()
    {
        CellSelectionHandler.currentCellSelected = null;
        
         switch(levelSelected.level.currentAction.tutorialType)
        {
            case TutorialType.Explicative:
            sceneSwitcher.SceneToLoad = explicativeScene;
            break;
            case TutorialType.Interactuable:
            sceneSwitcher.SceneToLoad = interactuableScene;
            break;
        }

        StartCoroutine(InitDelay());
    }

    IEnumerator InitDelay()
    {
        yield return new WaitForSeconds(levelSelected.level.currentAction.initDelay);

        if(!SetCameraFocus())
        {
            sceneSwitcher.Behaviour();
            currentActionIndex++;
        }
        
    }

    private bool SetCameraFocus()
    {
        if(string.IsNullOrEmpty(levelSelected.level.currentAction.focus)) return false;

        GameObject temp = GameObject.Find(levelSelected.level.currentAction.focus);

        if(!temp) return false;

        anchorTween = cameraAnchor.transform.DOMove(temp.transform.position,focusSpeed,false);
       
        if(temp.transform.TryGetComponentInChildren<CellSelectionHandler>(out CellSelectionHandler selection))
        {
            CellSelectionHandler.currentCellSelected = selection;
        }

       

        anchorTween.OnComplete(() =>
        {
            StartCoroutine(LoadScene());
        });

        return true;
    }

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(1f);
        sceneSwitcher.Behaviour();
        currentActionIndex++;
        
    }


}
}