using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.SaveSystem;
using Ozamanas.Forces;
using DG.Tweening;



public class SelectedForcesUIPanel : MonoBehaviour
{
    public GameObject informationPanel;
    private SkyInformationPanel skyInformationPanel;
    public PlayerDataHolder playerDataHolder;
    private PlayerData playerDeck;

    private CanvasGroup canvasGroup;

    [SerializeField] private List<SelectedForcesUIButton> slotButtons = new List<SelectedForcesUIButton>();

    void Awake()
    {
        skyInformationPanel = informationPanel.GetComponent<SkyInformationPanel>();
        canvasGroup= informationPanel.GetComponent<CanvasGroup>();
        playerDeck = playerDataHolder.data;
    }
    void Start()
    {
        UpdateCards();
    }
   
    private void UpdateCards()
    {

        for (int i = 0; i < slotButtons.Count; i++)
        {
            SelectedForcesUIButton slot = slotButtons[i];
            slot.SetForceData(this, i < playerDeck.selectedForces.Count ? playerDeck.selectedForces[i] : null);
        }

    }//Closes UpdateCards method
    public bool AddDataToDeck(ForceData data)
    {
        if (!data || playerDeck.selectedForces.Contains(data)) return false;
        int index = playerDeck.selectedForces.FindIndex(0, playerDeck.selectedForces.Count, force => force == null);
        if (index == -1) return false;
        playerDeck.selectedForces[index] = data;
        UpdateCards();
        return true;
    }//closes AddDataToDeck method;

    public void RemoveDataFromDeck(ForceData data)
    {
        if (!data || !playerDeck.selectedForces.Contains(data)) return;

        playerDeck.selectedForces[playerDeck.selectedForces.IndexOf(data)] = null;

        UpdateCards();


    }//Closes RemoveDataFromDeck

    public bool IsForceOnDeck(ForceData data)
    {
        return playerDeck.selectedForces.Contains(data);
    }//Closes IsForceOnDeck method

    public void ShowInformationPanel(ForceData data)
    {
        if(!informationPanel || !data) return;

        informationPanel.SetActive(true);

        canvasGroup.DOFade(1f,.1f);

        skyInformationPanel.UpdatePanel(data);

        
    }

      public void HideInformationPanel(ForceData data)
    {
         if(!informationPanel || !data) return;

        skyInformationPanel.UpdatePanel(null);

        canvasGroup.DOFade(0f,.1f);
       
    }

}//Closes SeletctedForcesUIPanel class
