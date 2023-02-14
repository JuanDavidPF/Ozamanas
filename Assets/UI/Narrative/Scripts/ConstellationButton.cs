
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Ozamanas.Forces;
using DG.Tweening;

namespace Ozamanas.UI
{
public class ConstellationButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] private ForceData data;
    private Button button;
    private ConstellationManager constellationManager;
    private RectTransform rectTransform;
    private StatsContainer statsContainer;
    private bool isCurrentlySelected = false;

     [SerializeField] private Outline imageOutline;

    public bool IsCurrentlySelected { 
        get { return isCurrentlySelected;} 
        set {

            if(isCurrentlySelected == value) return;

            isCurrentlySelected = value;

            if(value) imageOutline.enabled = true;
            else imageOutline.enabled = false;
            
            }  
    }

    
    public void SetData(ForceData data,ConstellationManager manager)
    {
        this.data = data;
        constellationManager = manager;
        CreateConstellation();
    }

    void Awake()
    {
        imageOutline = GetComponent<Outline>();
        button = GetComponent<Button>();
        rectTransform = GetComponent<RectTransform>();
    }

    void Start()
    {
        statsContainer = FindObjectOfType<StatsContainer>();
    }


    private void CreateConstellation()
    {
    
        if (!data) return;
        
        Instantiate(data.stars, transform);
        
        gameObject.GetComponent<Image>().sprite = data.forceConstellation;

    }

   
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!button.interactable) return;

        if(data) statsContainer.UpdatePanel(data);

        rectTransform.DOScale(new Vector3(1.2f,1.2f,1.2f),0.3f);

    }//Closes OnPointerEnter method

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!button.interactable) return;

        statsContainer.UpdatePanel(null);

        rectTransform.DOScale(new Vector3(1f,1f,1f),0.3f);


    }

    public void OnPointerClick()
    {
        if(IsCurrentlySelected)
        {
            bool result = constellationManager.RemoveSelectedForce(data);

            if(result) IsCurrentlySelected = false;
        }
        else
        {
            bool result = constellationManager.AddSelectedForce(data);

            if(result) IsCurrentlySelected = true;
        }
    }


}
}

