using System.Collections;
using System.Collections.Generic;
using Ozamanas.Entenders;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CreditsHandler : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] RectTransform creditsContent;



    [Header("Faster Scrolling")]
    [Space(20)]
    [SerializeField] KeyCode fasterKey;
    private float originalSpeed = 100;
    [SerializeField] float scrollSpeed = 100;
    [SerializeField] float scrollSpeedMultiplier = 2;
    [SerializeField] float scrollSpeedAcceleration = 1;
    [SerializeField] float maxScrollSpeed = 1000;
    [SerializeField] TextMeshProUGUI fastForwardLabel;
    [SerializeField] Image fastForwardProgress;

    [Header("Skip Credits")]
    [Space(20)]
    [SerializeField] KeyCode skipKey;
    float skipProgress = 0;
    [SerializeField] float triggerSkipValue = 3f;
    [SerializeField] TextMeshProUGUI skipLabel;
    [SerializeField] Image skipHoldProgress;

    bool isScrolling;

    private void Awake()
    {
        originalSpeed = 100;
    }//Closes Awake method
    private void Update()
    {
        if (Keyboard.current.anyKey.IsPressed() && animator) animator.SetTrigger("Skip");

        SetScrollSpeed();
        CheckSkipDecision();
    }//Closes Update method


    private void CheckSkipDecision()
    {
        if (!isScrolling) return;
        // if (Input.GetKeyDown(skipKey)) skipProgress = 0;
        // else if (Input.GetKeyUp(skipKey))
        // {
        //     if (skipProgress == triggerSkipValue) FinishCredits();
        //     else skipProgress = 0;
        // }

        // if (Input.GetKey(skipKey))
        // {
        //     skipProgress += Time.deltaTime;
        //     skipProgress = Mathf.Min(skipProgress, triggerSkipValue);
        // }
        if (skipHoldProgress) skipHoldProgress.fillAmount = MathUtils.Map(skipProgress, 0, triggerSkipValue, 0, 1);


    }//Closes CheckSkipDecision method

    private void SetScrollSpeed()
    {
        if (!isScrolling) return;
        // if (Input.GetKeyDown(fasterKey)) scrollSpeed *= scrollSpeedMultiplier;
        // else if (Input.GetKeyUp(fasterKey)) scrollSpeed = originalSpeed;
        // if (Input.GetKey(fasterKey))
        // {
        //     scrollSpeed += Time.deltaTime * scrollSpeedAcceleration;
        //     scrollSpeed = Mathf.Min(scrollSpeed, maxScrollSpeed);
        // }
        if (fastForwardProgress) fastForwardProgress.fillAmount = MathUtils.Map(scrollSpeed, originalSpeed, maxScrollSpeed, 0, 1);

    }//Closes SetScrollSpeed method


    public void StartScrolling()
    {
        StartCoroutine(HandleScrolling());
    }//Closes StartScrolling

    private IEnumerator HandleScrolling()
    {
        if (!creditsContent) yield break;
        isScrolling = true;
        creditsContent.anchoredPosition = Vector2.zero;

        Vector2 finalPosition = new Vector2(creditsContent.anchoredPosition.x, creditsContent.sizeDelta.y - 1080);

        while (!Mathf.Approximately(creditsContent.anchoredPosition.y, finalPosition.y))
        {
            creditsContent.anchoredPosition = Vector2.MoveTowards(creditsContent.anchoredPosition, finalPosition, scrollSpeed * Time.deltaTime);


            yield return null;
        }

        FinishCredits();

    }//Closes HandleScrolling


    public void UpdateFastForwardLabel(string label)
    {
        if (!fastForwardLabel) return;
        fastForwardLabel.text = "<b>[ " + fasterKey.ToString() + " ]</b>" + " " + label;
        // fastForwardLabel.transform.parent.GetComponent<ContentSizeFitter>().UpdateContentSizeFitter();
    }//Closes UpdateFastForwardLabel method

    public void UpdateSkipLabel(string label)
    {
        if (!skipLabel) return;
        skipLabel.text = "<b>[ " + skipKey.ToString() + " ]</b>" + " " + label;
        // skipLabel.transform.parent.GetComponent<ContentSizeFitter>().UpdateContentSizeFitter();
    }//Closes UpdateFastForwardLabel method


    public void FinishCredits()
    {
        isScrolling = false;
        if (animator) animator.SetTrigger("Close");
    }//Closes FinishCredits method
}//Closes CreditsHandler
