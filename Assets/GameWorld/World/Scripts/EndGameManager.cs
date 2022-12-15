using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JuanPayan.Helpers;
using Ozamanas.Levels;
using Ozamanas.Board;
using UnityEngine.Events;


namespace Ozamanas.World
{
    public class EndGameManager : MonoBehaviour
    {
        [Space(15)]
        [Header("Scene Switcher")]
         [SerializeField] SceneSwitcher sceneSwitcher;
         [SerializeField] private string winSceneToLoad;
        [SerializeField] private string loseSceneToLoad;

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

        public void CallVictory()
        {
            // Step 1
            levelSelected.level.state = Tags.LevelState.Finished;
            // Step 2
            List<Cell> cells = Board.Board.GetCellsByData(cellsData.ToArray());
            foreach(Cell cell in cells)
            {
                cell.gameplayState = winState;
            }

            OnWinLevel?.Invoke();
            
        }

        public void CallDefeat()
        {
           
        }

        public void LoadWinScene()
        {
            sceneSwitcher.SceneToLoad = winSceneToLoad;
            sceneSwitcher.Behaviour();
        }

         public void LoadLoseScene()
        {
             sceneSwitcher.SceneToLoad = loseSceneToLoad;
            sceneSwitcher.Behaviour();
        }
    }
}
