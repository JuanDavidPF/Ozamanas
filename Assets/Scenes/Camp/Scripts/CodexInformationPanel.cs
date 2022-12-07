using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.Forces;
using Ozamanas.Machines;
using Ozamanas.Board;
using UnityEngine.Localization.Components;
using TMPro;
using DG.Tweening;

public class CodexInformationPanel : MonoBehaviour
{
    // Start is called before the first frame update
     private  ForceData _forceSelected;
     private  HumanMachineToken _machineSelected; 

     private  CellData _cellSelected; 
    [SerializeField] UnityEngine.UI.Image panelIcon;
    [SerializeField] TMP_Text titleString;
    [SerializeField] TMP_Text descriptionString;
    [SerializeField] private float fadeTime = 0.01f;

    private CanvasGroup canvasGroup;

    public void Awake()
    {   
        canvasGroup= GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
    }

    public void Show()
    {
                 canvasGroup.DOFade(1f,fadeTime);

    }

    public void Hide()
    {
                 canvasGroup.DOFade(0f,fadeTime);

    }
    public void UpdatePanel(ForceData force)
    {

        if (!force) return;

        if(force==_forceSelected) return;

        _forceSelected = force;

        SetPanelIcon(force.forceIcon);

       if (titleString) titleString.text = force.forceName.GetLocalizedString();
      
       if (descriptionString) descriptionString.text = force.forceDescription.GetLocalizedString();

    }//Closes UpdatePanel method

     public void UpdatePanel(CellData cell)
    {

        if (!cell) return;

        if(cell==_cellSelected) return;

        _cellSelected = cell;

        SetPanelIcon(cell.cellIcon);

       if (titleString) titleString.text = cell.cellName.GetLocalizedString();
      
       if (descriptionString) descriptionString.text = cell.cellDescription.GetLocalizedString();

    }//Closes UpdatePanel method

    public void UpdatePanel(HumanMachineToken machine)
    {

        if (!machine) return;

        if(machine==_machineSelected) return;

        _machineSelected = machine;

        SetPanelIcon(machine.machineIcon);

       if (titleString) titleString.text = machine.machineName.GetLocalizedString();
      
       if (descriptionString) descriptionString.text = machine.machineDescription.GetLocalizedString();

    }//Closes UpdatePanel method

     private void SetPanelIcon(Sprite icon)
    {
        if (!panelIcon) return;
        panelIcon.sprite = icon;
        panelIcon.gameObject.SetActive(icon);
    }

}
