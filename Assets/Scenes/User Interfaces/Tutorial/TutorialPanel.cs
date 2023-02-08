using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ozamanas.Levels;
using JuanPayan.References;
using Ozamanas.Tags;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class TutorialPanel : MonoBehaviour
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


        [Header("Events")]
        [SerializeField] public UnityEvent OnTypeCharacter;
        [SerializeField] public UnityEvent OnSkipOnInput;

        [Header("Input Options")]
        private bool skipOnInput;
        private bool waitForInput;
        private bool anyKeyDown;

         [SerializeField] TextMeshProUGUI titleString;
        [SerializeField] TextMeshProUGUI descriptionString;
        [SerializeField] private LevelReference levelController;

         [SerializeField] private GameEvent OnExitTutorialAction;

 void Awake()
    {
       if (!levelController || !levelController.level || !levelController.level.currentAction) return;

       UpdatePanel();
    }

  
    
    public void UpdatePanel()
    {
      IEnumerator coroutine;

      coroutine = PrintTextOnPanel(descriptionString,levelController.level.currentAction.description.GetLocalizedString());
      StartCoroutine(coroutine);
       //if (titleString) titleString.text = levelController.level.currentAction.title.GetLocalizedString();
       //if (descriptionString) descriptionString.text = levelController.level.currentAction.description.GetLocalizedString();

    }

    public void ExitTutorialAction()
    {
        if (OnExitTutorialAction) OnExitTutorialAction.Invoke();
    }

     

        // Start is called before the first frame update
       
      void Update()
      {
         if (Mouse.current.leftButton.wasPressedThisFrame || UnityEngine.InputSystem.Keyboard.current.anyKey.wasPressedThisFrame )
         {
               skipOnInput = true;
               anyKeyDown = true;
         }

      }
        // Update is called once per frame
        
   
        IEnumerator PrintTextOnPanel(TextMeshProUGUI textMeshPro,string phrase)
        {
            Debug.Log("PrintTextOnPanel:"+phrase);

            textMeshPro.text = "";

            string text = phrase;

            string tempText = "";

            for (int i = 0; i < text.Length; i++)
                {
                    Debug.Log("Inside FOR:"+i);
                    if (skipOnInput && anyKeyDown)
                    {
                      Debug.Log("skipOnInput:"+i);
                        OnSkipOnInput?.Invoke();
                        textMeshPro.text = text;
                        anyKeyDown = false;
                        yield return null;
                        break;
                    }

                    char c = text[i];
                    tempText += c;
                   // yield return StartCoroutine(DelayPrint(subtitleDelays.characterDelay));
                     yield return new WaitForSeconds(subtitleDelays.characterDelay);
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

                    textMeshPro.text = tempText;
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
            skipOnInput = false;
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

       
        
        

        



}
