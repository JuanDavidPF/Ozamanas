using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using Stem;

namespace Ozamanas
{
public class NotificationPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text titleString;

    [SerializeField] private CanvasGroup canvasGroup;

     private Tween notificationTween;

    void Awake()
    {
        if(!titleString) titleString = GetComponentInChildren<TMP_Text>();
        if(!canvasGroup) canvasGroup = GetComponent<CanvasGroup>();
    }
    public void UpdatePanel(Notification notification)
    {

       if (!notification) return;
       
       if (!titleString) return;

       notificationTween = canvasGroup.DOFade(1f,notification.fadeInTime);
       
       titleString.text = notification.notification.GetLocalizedString();

       titleString.color = notification.textColor;

       if(notification.sound != Stem.ID.None) SoundManager.Play(notification.sound);

       notificationTween.OnComplete(() =>
        {
            StartCoroutine(CompleteTransition(notification));
        });

    }

    IEnumerator CompleteTransition(Notification notification)
    {
        yield return new WaitForSeconds(notification.lifeTime);

        notificationTween = canvasGroup.DOFade(0f,notification.fadeOutTime);

        notificationTween.OnComplete(() =>
        {
            notificationTween.Kill();
            Destroy(gameObject);
        });

    }

}
}
