using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JuanPayan.Helpers;
using UnityEngine.SceneManagement;
using Ozamanas.Tags;
using Ozamanas.SaveSystem;
using Sirenix.OdinInspector;

namespace Ozamanas.UI
{
    public class PhraseManager : MonobehaviourEvents
    {
        [Title("Narrative Components")]
        [SerializeField] private PhraseHolder phraseSequences;

        private PhraseSequence currentPhraseSequence;

        [SerializeField] private PhraseHolder defaultPhraseSequences;

         

        private PhraseContainer dialoguePanel;
        [Title("Narrative Setup")]
        [SerializeField] private PlayerDataHolder playerDataHolder;

        public override void Behaviour()
        {

            if(phraseSequences.data.Count == 0 || defaultPhraseSequences.data.Count == 0) return;

            dialoguePanel = FindObjectOfType<PhraseContainer>();
            
            if(!dialoguePanel) return;

            PhraseSequence phraseSequence = FindNextActivePhraseSequence();

            if(phraseSequence) StartDialogueNextSequence(phraseSequence);

            if(!phraseSequence) SetDialogueDefaultSequence();
            
        }

        private PhraseSequence FindNextActivePhraseSequence()
        {
            foreach (PhraseSequence phrase in phraseSequences.data)
            {
                if(phrase.state == PhraseSequenceState.Finnished ) continue;

                if(phrase.state == PhraseSequenceState.Default ) continue;

                if(!CheckPhraseSequencePreRequisites(phrase)) continue;

                return phrase;
            }

            return null;
        }

        private void StartDialogueNextSequence(PhraseSequence phraseSequence)
        {
            dialoguePanel.StartDialogue(phraseSequence);

            currentPhraseSequence = phraseSequence;
            
        }

        private void SetDialogueDefaultSequence()
        {
            List<PhraseSequence> defaultPhrases = new List<PhraseSequence>();
            foreach (PhraseSequence phrase in defaultPhraseSequences.data)
            {
              if(phrase.state != PhraseSequenceState.Default) continue;

               foreach (PreRequisite req in phrase.preRequisites)
               {
                    if(req.type == PreRequisiteType.OnScene && SceneManager.GetSceneByName(req.sceneName.ToString()).IsValid())
                    defaultPhrases.Add(phrase);
               }
            }
            
            if(defaultPhrases.Count==0) return;

            PhraseSequence temp = defaultPhrases[Random.Range(0,defaultPhrases.Count-1)];
            
            dialoguePanel.StartDialogue(temp);
        }

        private bool CheckPhraseSequencePreRequisites(PhraseSequence phraseSequence)
        {
            
            bool checkPreCond = false;

            foreach (PreRequisite req in phraseSequence.preRequisites)
            {
                switch (req.type)
                {
                    case PreRequisiteType.OnScene:
                    checkPreCond = SceneManager.GetSceneByName(req.sceneName.ToString()).IsValid();
                    break;
                    case PreRequisiteType.LevelComplete:
                    checkPreCond = req.level.state ==  LevelState.Finished;
                    break;
                    case PreRequisiteType.UnlockedForce:
                    checkPreCond = playerDataHolder.data.unlockedForces.Contains(req.force);
                    break; 
                }

                if(!checkPreCond) break;
                
            }
            
            return checkPreCond;
        }
      
    }
}
