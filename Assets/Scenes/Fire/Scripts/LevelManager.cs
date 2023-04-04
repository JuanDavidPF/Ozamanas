using System.Collections.Generic;
using UnityEngine;
using Ozamanas.Levels;
using Ozamanas.UI;
using JuanPayan.Helpers;
using TMPro;
using Ozamanas.Tags;

namespace Ozamanas.GameScenes
{
    public class LevelManager : MonoBehaviour
    {
        
        [Space(15)]
        [Header("Levels Settings")]
        [SerializeField] private List<LevelController> levels = new List<LevelController>();

        private LevelController currentLevel;
        [SerializeField] private PlayerController player;
        [SerializeField] private LevelReference levelReference;

        [Space(15)]
        [Header("UI Settings")]

        [SerializeField] private ButtonContainer playButton;
        [SerializeField] private MachineDeckManager machineDeck;
        [SerializeField] private PhraseContainer phraseContainer;
        [SerializeField] private TextMeshProUGUI index;
        [SerializeField] private TextMeshProUGUI levelName;
        [SerializeField] private PhraseSequence phraseSequence;

        [Space(15)]
        [Header("Camera Settings")]
       
        [SerializeField] private string cameraKey;

        public LevelController CurrentLevel { get => currentLevel; set => currentLevel = value; }


        void Start()
        {
            SetPlayerAndCamPosition();
        }

        void Update()
         {
           if(player.PlayerState != PlayerState.Waiting ) return;

            player.PlayerState = PlayerState.Idling;

           SetUPToPLayLevel();

        }

        private void SetPlayerAndCamPosition()
        {
            Transform cameraTransform;

            foreach (LevelController level in levels)
            {
                if(level.LevelData == levelReference.level)
                {
                    player.transform.position = level.transform.position;
                    player.transform.position += player.playerOffset;
                    currentLevel = level;
                    cameraTransform = CameraAtlas.Get(cameraKey).GetComponent<Transform>();
                    if (!cameraTransform) return;
                    cameraTransform.position = level.CamAnchor.position;
                    cameraTransform.rotation = level.CamAnchor.rotation;
                    
                }
            }
        }

        private void SetUpUI()
        {
            playButton.gameObject.SetActive(true);

            machineDeck.gameObject.SetActive(true);

            machineDeck.LevelData = currentLevel.LevelData;

            machineDeck.LoadMachineDeck();

            index.text = currentLevel.LevelData.index.ToString();

            index.transform.parent.gameObject.SetActive(true);

            levelName.text = currentLevel.LevelData.levelName.GetLocalizedString(); 

            levelName.gameObject.SetActive(true);

            phraseSequence.ResetSequence();

            phraseSequence.phrases.Add(currentLevel.LevelData.levelMainObjective);

            phraseContainer.StartDialogue(phraseSequence);
        }

        public void ClearUI()
        {
            playButton.gameObject.SetActive(false);

            machineDeck.ClearMachineDeck();

            machineDeck.gameObject.SetActive(false);
        }

        public void SetUPToPLayLevel()
        {
            levelReference.level = CurrentLevel.LevelData;
            SetUpUI();
        }
      
    }
}
