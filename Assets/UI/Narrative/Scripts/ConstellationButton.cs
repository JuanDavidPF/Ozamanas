
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Ozamanas.Forces;
using DG.Tweening;
using TMPro;
using Sirenix.OdinInspector;



namespace Ozamanas.UI
{
public class ConstellationButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Title("AncientForce Data")]
    [SerializeField] private ForceData data;
    [SerializeField] private TextMeshProUGUI forceName;

    [Title("Ancient Force Sprites")]
    [SerializeField] private Image selectedImage;
    [SerializeField] private Image highlightedImage;
    [SerializeField] private Image unlockedImage;
    [SerializeField] private Image constellation;
    

    private ConstellationManager constellationManager;
    private StatsContainer statsContainer;
    private bool isCurrentlySelected = false;

    public bool IsCurrentlySelected { 
        get { return isCurrentlySelected;} 
        set {

                if(isCurrentlySelected == value) return;

                isCurrentlySelected = value;

                if(isCurrentlySelected) 
                {
                    selectedImage.gameObject.SetActive(true);
                    unlockedImage.gameObject.SetActive(false);
                }
                else
                {
                    selectedImage.gameObject.SetActive(false);
                    unlockedImage.gameObject.SetActive(true);
                } 
            
            }  
    }

    public ForceData Data { get => data; set => data = value; } 
    void Start()
    {
        statsContainer = FindObjectOfType<StatsContainer>();
        constellationManager = FindObjectOfType<ConstellationManager>(); 
        CreateConstellation();
    }

    private void CreateConstellation()
    {
        if (!Data) return;
        
        selectedImage.sprite = Data.selectedImage;
        highlightedImage.sprite = Data.highlightedImage;
        unlockedImage.sprite = Data.unlockedImage;
        constellation.sprite = Data.constellation;
        forceName.text = Data.forceName.GetLocalizedString();
        selectedImage.gameObject.SetActive(false);
        highlightedImage.gameObject.SetActive(false);
        unlockedImage.gameObject.SetActive(true);
    }

   
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(Data) statsContainer.UpdatePanel(Data);

        highlightedImage.gameObject.SetActive(true);
        forceName.gameObject.SetActive(true);

    }//Closes OnPointerEnter method

    public void OnPointerExit(PointerEventData eventData)
    {
        statsContainer.UpdatePanel(null);

        highlightedImage.gameObject.SetActive(false);
        forceName.gameObject.SetActive(false);
    }

    public void OnPointerClick()
    {
        if(IsCurrentlySelected)
        {
            bool result = constellationManager.RemoveSelectedForce(Data);

            if(result) IsCurrentlySelected = false;
        }
        else
        {
            bool result = constellationManager.AddSelectedForce(Data);

            if(result) IsCurrentlySelected = true;
        }
    }


}
}

