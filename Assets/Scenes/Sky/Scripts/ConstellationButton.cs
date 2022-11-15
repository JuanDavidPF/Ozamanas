
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Ozamanas.Forces;

public class ConstellationButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public static ForceData constelattionSelected;
    [SerializeField] private ForceData data;
    [SerializeField] private SelectedForcesUIPanel forcesManager;




    private Button button;

    void Awake()
    {
    }
    public void SetData(ForceData data, SelectedForcesUIPanel forcesManager)
    {
        this.data = data;
        this.forcesManager = forcesManager;
    }//Closes SetData method

    private void Start()
    {
        button = GetComponent<Button>();
        CreateConstellation();

    }

    private void CreateConstellation()
    {
    
        if (!data) return;
        
       Instantiate(data.stars, transform);
        
        gameObject.GetComponent<Image>().sprite = data.forceConstellation;


    }//ClosesCreateConstellation method

    public void UpdateConstellationProgress()
    {
        
    }//Closes UpdateConstellationProgress method

    

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!button.interactable) return;
        forcesManager.ShowInformationPanel(data);

    }//Closes OnPointerEnter method

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!button.interactable) return;
        forcesManager.HideInformationPanel(data);

    }//Closes OnPointerExit method



    public void AddData()
    {
        if (!forcesManager || !data) return;
        if (!forcesManager.IsForceOnDeck(data)) forcesManager.AddDataToDeck(data);
        else forcesManager.RemoveDataFromDeck(data);
    }

}//Closes ConstelationButton class


