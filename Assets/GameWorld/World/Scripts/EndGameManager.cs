using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JuanPayan.Helpers;
using Ozamanas.Levels;
using Ozamanas.Board;
using UnityEngine.Events;
using DG.Tweening;
using Ozamanas.Tags;


namespace Ozamanas.World
{
    public class EndGameManager : MonoBehaviour
    {

        

         [SerializeField] private string ObjectToFocus;

          [SerializeField] private float focusSpeed = 0.2f;
        [SerializeField] private CameraAnchor cameraAnchor;
        [SerializeField] private CameraZoom cameraZoom;
        [Space(15)]
        [Header("Scene Switcher")]
        [SerializeField] private float sceneSwitcherDelay = 3f;
         [SerializeField] SceneSwitcher sceneSwitcher;
         [SerializeField] SceneUnloader sceneUnloader;
         [SerializeField] private Scenes winSceneToLoad;
        [SerializeField] private Scenes loseSceneToLoad;
        [SerializeField] private Scenes gameplayHUD;

        [Space(15)]
        [Header("Game States")]
         [SerializeField] private GameplayState winState;

         [SerializeField] private GameplayState loseState;

        [SerializeField] private LevelReference levelSelected;

        [Space(15)]
        [Header("Cell tags")]

        [SerializeField] private List<CellData> cellsData;

        [Space(15)]
        [Header("Events")]
        public UnityEvent OnWinLevel;
        public UnityEvent OnLoseLevel;

        void Awake()
        {
            if(!sceneSwitcher) sceneSwitcher = GetComponent<SceneSwitcher>();
            if(!sceneUnloader) sceneUnloader = GetComponent<SceneUnloader>();
            if(!cameraZoom) cameraZoom = FindObjectOfType<CameraZoom>();
            if(!cameraAnchor) cameraAnchor = FindObjectOfType<CameraAnchor>();
        }

        public void CallVictory()
        {
           
            levelSelected.level.state = Tags.LevelState.Finished;

            UnloadGameplayHUD();

            OnWinLevel?.Invoke();

            SetCameraZoom();

            SetCameraFocus();
           
            StartCoroutine(UpdateCellGameState(winState));

            StartCoroutine(LoadScene(winSceneToLoad));
            
            
        }

         public void CallDefeat()
        {
            UnloadGameplayHUD();

            OnLoseLevel?.Invoke();

            SetCameraZoom();

            SetCameraFocus();
           
            StartCoroutine(UpdateCellGameState(loseState));

            StartCoroutine(LoadScene(loseSceneToLoad));
        }


        IEnumerator UpdateCellGameState(GameplayState state)
        {
            List<Cell> cells = Board.Board.GetCellsByData(cellsData.ToArray());
            foreach(Cell cell in cells)
            {
                cell.gameplayState = state;
                yield return new WaitForSeconds(0.1f);
            }
            
        }

        private void SetCameraFocus()
        {
            if(string.IsNullOrEmpty(ObjectToFocus)) return;
        
            GameObject temp = GameObject.Find(ObjectToFocus);

            if(!temp) return;

            cameraAnchor.transform.DOMove(temp.transform.position,focusSpeed,false);

        }
        private void SetCameraZoom()
        {
            cameraZoom.UpdateCameraZoom(0f);
        }

        private void UnloadGameplayHUD()
        {
            sceneUnloader.SceneToUnload = gameplayHUD;
            sceneUnloader.Behaviour();
        }

       

        IEnumerator LoadScene(Scenes scene)
        {
            yield return new WaitForSeconds(sceneSwitcherDelay);
            sceneSwitcher.SceneToLoad = scene;
            sceneSwitcher.Behaviour();
        }

       
    }
}
