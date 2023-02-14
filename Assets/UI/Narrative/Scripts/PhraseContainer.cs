using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.Tags;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using TMPro;

namespace Ozamanas.UI
{
    public class PhraseContainer : MonoBehaviour
    {

        [System.Serializable]
        public class SubtitleDelays
        {
            public float characterDelay = 0.05f;
            public float sentenceDelay = 0.5f;
            public float commaDelay = 0.1f;
            public float finalDelay = 1.2f;
        }

        public SubtitleDelays subtitleDelays = new SubtitleDelays();

        public RectTransform waitInputIndicator;

        public RectTransform subtitlesGroup;



        [Header("Subtitles")]
        public TextMeshProUGUI actorSpeech;

        [Header("Events")]
        [SerializeField] public UnityEvent OnTypeCharacter;
        [SerializeField] public UnityEvent OnSkipOnInput;

        [Header("Input Options")]
        private bool skipOnInput;
        private bool waitForInput;
        private bool anyKeyDown;

         [Header("Narrator")]
        [SerializeField] private string narratorTag ="Narrator";
        private bool narratorIsPresent = false;
        private Animator narrator;

        private PhraseSequence sequence;

        private bool isLastSentence = false;

         [Header("Action Buttons")]

         [SerializeField] private List<ButtonContainer> buttons;

        private int index = 0;

        // Start is called before the first frame update
        void Awake()
        {
            FindNarrator();
            Hide();
        }

        void Update()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame || UnityEngine.InputSystem.Keyboard.current.anyKey.wasPressedThisFrame )
            {
                skipOnInput = true;
                anyKeyDown = true;
            }

        }
        // Update is called once per frame
        
        public void StartDialogue(PhraseSequence phraseSequences)
        {
           
            ResetContainer();

            if(narratorIsPresent) narrator.SetTrigger("Talk");
           
            sequence = phraseSequences;

            PrintNextPhraseOnPanel();
        }

        private void ResetContainer()
        {
            StopAllCoroutines();

            index = 0;

            actorSpeech.text = "";
        }

        private void PrintNextPhraseOnPanel()
        {

            if(index >= sequence.phrases.Count) 
            {
                OnDialogueFinished();
                return;
            }

            if(index == sequence.phrases.Count-1) isLastSentence = true;
            
            StartCoroutine(PrintPhraseOnPanel());

            skipOnInput = false;

            index++;

        }

        IEnumerator PrintPhraseOnPanel()
        {

            subtitlesGroup.gameObject.SetActive(true);
            actorSpeech.text = "";

            string text = sequence.phrases[index].GetLocalizedString();

            string tempText = "";

            for (int i = 0; i < text.Length; i++)
                {

                    if (skipOnInput && anyKeyDown)
                    {
                        OnSkipOnInput?.Invoke();
                        actorSpeech.text = text;
                        anyKeyDown = false;
                        yield return null;
                        break;
                    }

                    char c = text[i];
                    tempText += c;
                    yield return StartCoroutine(DelayPrint(subtitleDelays.characterDelay));
                    OnTypeCharacter?.Invoke();
                    if (c == '.' || c == '!' || c == '?')
                    {
                        yield return StartCoroutine(DelayPrint(subtitleDelays.sentenceDelay));
                         OnTypeCharacter?.Invoke();
                    }
                    if (c == ',')
                    {
                        yield return StartCoroutine(DelayPrint(subtitleDelays.commaDelay));
                         OnTypeCharacter?.Invoke();
                    }

                    actorSpeech.text = tempText;
                }

            if (!waitForInput)
            {
                yield return StartCoroutine(DelayPrint(subtitleDelays.finalDelay));
                waitForInput = true;
            }

            if (waitForInput)
            {
                waitInputIndicator.gameObject.SetActive(true);
                while (!anyKeyDown)
                {
                    yield return null;
                }
                waitInputIndicator.gameObject.SetActive(false);
            }

            yield return null;
            if(!isLastSentence)  subtitlesGroup.gameObject.SetActive(false);
            skipOnInput = false;
            PrintNextPhraseOnPanel();
        }

        public void SkipOnInput()
        {
            skipOnInput = true;
        }

        IEnumerator DelayPrint(float time)
        {
            var timer = 0f;
            while (timer < time)
            {
                timer += Time.deltaTime;
                yield return null;
            }
        }

        private void OnDialogueFinished()
        {
            Debug.Log("OnDialogueFinished");

            ShowActionButtons();

            if (sequence.state !=PhraseSequenceState.Default)  sequence.state = PhraseSequenceState.Finnished;

            if(narratorIsPresent) narrator.SetTrigger("StopTalking");

            Hide();
        }

        private void ShowActionButtons()
        {
            foreach(ButtonContainer button in buttons)
            {
                if(sequence.actionButtons.Contains(button.buttonType))
                button.gameObject.SetActive(true);

            }
        }
        private void FindNarrator()
        {
            GameObject temp = GameObject.FindGameObjectWithTag(narratorTag);

            if(!temp) 
            {
                narratorIsPresent = false;
                return;
            }

            narrator = temp.GetComponent<Animator>();

            if(!narrator) 
            {
                narratorIsPresent = false;
                return;
            }
            
            narratorIsPresent = true;

        }

        private void Hide()
        {

        }
    }
}
