using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Ozamanas.Forces;
using DG.Tweening;

public class SelectedForcesUIButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private ForceData _data;

    [SerializeField] private Image buttonImage;
    [SerializeField] private Sprite slotIcon;

    private SelectedForcesUIPanel forcesManager;


    public void SetForceData(SelectedForcesUIPanel manager, ForceData data)
    {
        if (!manager) return;
        forcesManager = manager;
        if (ConstellationButton.constelattionSelected == _data) ConstellationButton.constelattionSelected = data;
        _data = data;

        if (buttonImage)
        {
            buttonImage.sprite = data ? data.forceIcon : slotIcon;
            buttonImage.preserveAspect = true;
        }
    }//Closes SetForceData method

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_data) forcesManager.RemoveDataFromDeck(_data);


    }//Closes OnPointerClick method

    public void OnPointerEnter(PointerEventData eventData)
    {
         forcesManager.ShowInformationPanel(_data);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        forcesManager.HideInformationPanel(_data);
    }
}//Closes SelectedForcesUIButton
