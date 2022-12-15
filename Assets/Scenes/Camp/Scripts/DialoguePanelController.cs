using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NodeCanvas.DialogueTrees;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using TMPro;



    public class DialoguePanelController : MonoBehaviour
    {

        [System.Serializable]
        public class SubtitleDelays
        {
            public float characterDelay = 0.05f;
            public float sentenceDelay = 0.5f;
            public float commaDelay = 0.1f;
            public float finalDelay = 1.2f;
        }


        //Options...
        [Header("Input Options")]
        public bool skipOnInput;
        public bool waitForInput;

        [Header("Localization Options")]

        public string tableName;

        //Group...
        [Header("Subtitles")]
        public RectTransform subtitlesGroup;
        public TextMeshProUGUI actorSpeech;
        public TextMeshProUGUI actorName;
        public Image actorPortrait;
        public RectTransform waitInputIndicator;
        public SubtitleDelays subtitleDelays = new SubtitleDelays();
        public List<AudioClip> typingSounds;
        private AudioSource playSource;

        //Group...
        [Header("Multiple Choice")]
        public RectTransform optionsGroup;
        public Button optionButton;
        private Dictionary<Button, int> cachedButtons;
        private Vector2 originalSubsPosition;
        private bool isWaitingChoice;

        private AudioSource _localSource;
        private AudioSource localSource
        {
            get { return _localSource != null ? _localSource : _localSource = gameObject.AddComponent<AudioSource>(); }
        }

        bool anyKeyDown
        {
            get
            {
#if ENABLE_LEGACY_INPUT_MANAGER
                return Input.anyKeyDown;
#elif ENABLE_INPUT_SYSTEM
                return UnityEngine.InputSystem.Keyboard.current.anyKey.wasPressedThisFrame || UnityEngine.InputSystem.Mouse.current.leftButton.wasPressedThisFrame;
#else
                return Input.anyKeyDown;
#endif
            }
        }
        [SerializeField] private string narratorTag ="Narrator";
        private bool narratorIsPresent = false;
        private NarratorAnimationController narrator;

        void Awake() 
        { 
            Subscribe(); 
            Hide(); 
            FindNarrator();
        }
        void OnEnable() { UnSubscribe(); Subscribe(); }
        void OnDisable() { UnSubscribe(); }

        void FindNarrator()
        {
            GameObject temp = GameObject.FindGameObjectWithTag(narratorTag);

            if(!temp) 
            {
                narratorIsPresent = false;
                return;
            }

            narrator = temp.GetComponent<NarratorAnimationController>();

            if(!narrator) 
            {
                narratorIsPresent = false;
                return;
            }
            
            narratorIsPresent = true;

        }
        void Subscribe()
        {
            DialogueTree.OnDialogueStarted += OnDialogueStarted;
            DialogueTree.OnDialoguePaused += OnDialoguePaused;
            DialogueTree.OnDialogueFinished += OnDialogueFinished;
            DialogueTree.OnSubtitlesRequest += OnSubtitlesRequest;
            DialogueTree.OnMultipleChoiceRequest += OnMultipleChoiceRequest;
        }

        void UnSubscribe()
        {
            DialogueTree.OnDialogueStarted -= OnDialogueStarted;
            DialogueTree.OnDialoguePaused -= OnDialoguePaused;
            DialogueTree.OnDialogueFinished -= OnDialogueFinished;
            DialogueTree.OnSubtitlesRequest -= OnSubtitlesRequest;
            DialogueTree.OnMultipleChoiceRequest -= OnMultipleChoiceRequest;
        }

        void Hide()
        {
            subtitlesGroup.gameObject.SetActive(false);
            optionsGroup.gameObject.SetActive(false);
            optionButton.gameObject.SetActive(false);
            waitInputIndicator.gameObject.SetActive(false);
            originalSubsPosition = subtitlesGroup.transform.position;
        }

        void OnDialogueStarted(DialogueTree dlg)
        {
                    if(narratorIsPresent) narrator.StartTalking();
        }

        void OnDialoguePaused(DialogueTree dlg)
        {
            subtitlesGroup.gameObject.SetActive(false);
            optionsGroup.gameObject.SetActive(false);
            StopAllCoroutines();
            if (playSource != null) playSource.Stop();
            if(narratorIsPresent) narrator.StopTalking();
        }

        void OnDialogueFinished(DialogueTree dlg)
        {
            subtitlesGroup.gameObject.SetActive(false);
            optionsGroup.gameObject.SetActive(false);
            if (cachedButtons != null)
            {
                foreach (var tempBtn in cachedButtons.Keys)
                {
                    if (tempBtn != null)
                    {
                        Destroy(tempBtn.gameObject);
                    }
                }
                cachedButtons = null;
            }
            StopAllCoroutines();
            if (playSource != null) playSource.Stop();

           if(narratorIsPresent) narrator.StopTalking();
        }


        void OnSubtitlesRequest(SubtitlesRequestInfo info)
        {
            StartCoroutine(Internal_OnSubtitlesRequestInfo(info));
        }

        IEnumerator Internal_OnSubtitlesRequestInfo(SubtitlesRequestInfo info)
        {

            var text = info.statement.text;
            var audio = info.statement.audio;
            var actor = info.actor;

            text = LocalizationSettings.StringDatabase.GetLocalizedString( tableName,  text);

            if(string.IsNullOrEmpty(text)) text = "Error Missing Text";
            
            subtitlesGroup.gameObject.SetActive(true);
            actorSpeech.text = "";

            actorName.text = actor.name;
            actorSpeech.color = actor.dialogueColor;

            actorPortrait.gameObject.SetActive(actor.portraitSprite != null);
            actorPortrait.sprite = actor.portraitSprite;

            if (audio != null)
            {
                var actorSource = actor.transform != null ? actor.transform.GetComponent<AudioSource>() : null;
                playSource = actorSource != null ? actorSource : localSource;
                playSource.clip = audio;
                playSource.Play();
                actorSpeech.text = text;
                var timer = 0f;
                while (timer < audio.length)
                {
                    if (skipOnInput && anyKeyDown)
                    {
                        playSource.Stop();
                        break;
                    }
                    timer += Time.deltaTime;
                    yield return null;
                }
            }

            if (audio == null)
            {
                var tempText = "";
                var inputDown = false;
                if (skipOnInput)
                {
                    StartCoroutine(CheckInput(() => { inputDown = true; }));
                }

                for (int i = 0; i < text.Length; i++)
                {

                    if (skipOnInput && inputDown)
                    {
                        actorSpeech.text = text;
                        yield return null;
                        break;
                    }

                    if (subtitlesGroup.gameObject.activeSelf == false)
                    {
                        yield break;
                    }

                    char c = text[i];
                    tempText += c;
                    yield return StartCoroutine(DelayPrint(subtitleDelays.characterDelay));
                    PlayTypeSound();
                    if (c == '.' || c == '!' || c == '?')
                    {
                        yield return StartCoroutine(DelayPrint(subtitleDelays.sentenceDelay));
                        PlayTypeSound();
                    }
                    if (c == ',')
                    {
                        yield return StartCoroutine(DelayPrint(subtitleDelays.commaDelay));
                        PlayTypeSound();
                    }

                    actorSpeech.text = tempText;
                }

                if (!waitForInput)
                {
                    yield return StartCoroutine(DelayPrint(subtitleDelays.finalDelay));
                }
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
            subtitlesGroup.gameObject.SetActive(false);
            info.Continue();
        }

        void PlayTypeSound()
        {
            if (typingSounds.Count > 0)
            {
                var sound = typingSounds[Random.Range(0, typingSounds.Count)];
                if (sound != null)
                {
                    localSource.PlayOneShot(sound, Random.Range(0.6f, 1f));
                }
            }
        }

        IEnumerator CheckInput(System.Action Do)
        {
            while (!anyKeyDown)
            {
                yield return null;
            }
            Do();
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




        void OnMultipleChoiceRequest(MultipleChoiceRequestInfo info)
        {
            if(narratorIsPresent) narrator.StopTalking();

            optionsGroup.gameObject.SetActive(true);
            var buttonHeight = optionButton.GetComponent<RectTransform>().rect.height;
            optionsGroup.sizeDelta = new Vector2(optionsGroup.sizeDelta.x, (info.options.Values.Count * buttonHeight) + 30);

            cachedButtons = new Dictionary<Button, int>();
            int i = 0;

            foreach (KeyValuePair<IStatement, int> pair in info.options)
            {
                var btn = (Button)Instantiate(optionButton);
                btn.gameObject.SetActive(true);
                btn.transform.SetParent(optionsGroup.transform, false);
                btn.transform.localPosition = (Vector3)optionButton.transform.localPosition - new Vector3(0, buttonHeight * i, 0);
                string temp = LocalizationSettings.StringDatabase.GetLocalizedString( tableName,  pair.Key.text);
                btn.GetComponent<TextMeshProUGUI>().text = temp;
                cachedButtons.Add(btn, pair.Value);
                btn.onClick.AddListener(() => { Finalize(info, cachedButtons[btn]); });
                i++;
            }

            if (info.showLastStatement)
            {
                subtitlesGroup.gameObject.SetActive(true);
                var newY = optionsGroup.position.y + optionsGroup.sizeDelta.y + 1;
                //subtitlesGroup.position = new Vector3(subtitlesGroup.position.x, newY, subtitlesGroup.position.z);
            }

            if (info.availableTime > 0)
            {
                StartCoroutine(CountDown(info));
            }
        }

        IEnumerator CountDown(MultipleChoiceRequestInfo info)
        {
            isWaitingChoice = true;
            var timer = 0f;
            while (timer < info.availableTime)
            {
                if (isWaitingChoice == false)
                {
                    yield break;
                }
                timer += Time.deltaTime;
                SetMassAlpha(optionsGroup, Mathf.Lerp(1, 0, timer / info.availableTime));
                yield return null;
            }

            if (isWaitingChoice)
            {
                Finalize(info, info.options.Values.Last());
            }
        }

        void Finalize(MultipleChoiceRequestInfo info, int index)
        {
            isWaitingChoice = false;
            SetMassAlpha(optionsGroup, 1f);
            optionsGroup.gameObject.SetActive(false);
            if (info.showLastStatement)
            {
                subtitlesGroup.gameObject.SetActive(false);
                subtitlesGroup.transform.position = originalSubsPosition;
            }
            foreach (var tempBtn in cachedButtons.Keys)
            {
                Destroy(tempBtn.gameObject);
            }
            info.SelectOption(index);
        }

        void SetMassAlpha(RectTransform root, float alpha)
        {
            foreach (var graphic in root.GetComponentsInChildren<CanvasRenderer>())
            {
                graphic.SetAlpha(alpha);
            }
        }
    }