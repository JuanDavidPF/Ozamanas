using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.Forces;
using UnityEngine.Localization.Components;
using TMPro;


public class SkyInformationPanel : MonoBehaviour
{
      private  ForceData _forceSelected;

    [Space(20)]
    [SerializeField] UnityEngine.UI.Image panelIcon;
    [SerializeField] TMP_Text titleString;
    [SerializeField] TMP_Text descriptionString;

    [SerializeField] TMP_Text price;

    [SerializeField] TMP_Text range;

    [SerializeField] TMP_Text coolDown;

    public void UpdatePanel(ForceData force)
    {

        if (!force) return;

        if(force==_forceSelected) return;

        _forceSelected = force;

        SetPanelIcon(force.forceIcon);

       if (titleString) titleString.text = force.forceName.GetLocalizedString();
      

       if (descriptionString) descriptionString.text = force.forceDescription.GetLocalizedString();

       price.text = force.price.ToString();

       range.text = force.range.ToString();

       coolDown.text = force.cooldown.ToString();


    }//Closes UpdatePanel method

     private void SetPanelIcon(Sprite icon)
    {
        if (!panelIcon) return;
        panelIcon.sprite = icon;
        panelIcon.gameObject.SetActive(icon);
    }


}
