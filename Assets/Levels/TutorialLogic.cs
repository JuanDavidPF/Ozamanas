using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JuanPayan.Helpers;
using Ozamanas.Tags;
using DG.Tweening;
using Ozamanas.Board;


namespace Ozamanas.Levels
{
public class TutorialLogic : MonoBehaviour
{
    [SerializeField] private LevelReference levelSelected;
    [SerializeField] private string explicativeScene;
    [SerializeField] private string interactuableScene;
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


    private void PlayNextAction()
    {   


        if( currentActionIndex >= levelSelected.level.tutorialActions.Count ) return;

       

        levelSelected.level.currentAction = levelSelected.level.tutorialActions[currentActionIndex];

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

        if(! string.IsNullOrEmpty(levelSelected.level.currentAction.focus)) SetCameraFocus();
        else 
        {
            sceneSwitcher.Behaviour();
            currentActionIndex++;
        }
        
    }

    private void SetCameraFocus()
    {
         if(string.IsNullOrEmpty(levelSelected.level.currentAction.focus)) return;
    
        GameObject temp = GameObject.Find(levelSelected.level.currentAction.focus);
        if(!temp) return;
        anchorTween = cameraAnchor.transform.DOMove(temp.transform.position,focusSpeed,false);

        if(temp.TryGetComponent<CellSelectionHandler>(out CellSelectionHandler selection))
        {
            CellSelectionHandler.currentCellSelected = selection;
        }

        anchorTween.OnComplete(() =>
        {
            StartCoroutine(LoadScene());
        });

    }

     IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(1f);
       sceneSwitcher.Behaviour();
            currentActionIndex++;
    }


}
}