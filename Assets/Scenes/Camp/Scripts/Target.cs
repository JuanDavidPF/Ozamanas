using UnityEngine;
using UnityEngine.EventSystems;
using Ozamanas.Forces;
using Ozamanas.Machines;
using Ozamanas.Board;

public class Target : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{

   [SerializeField] private HumanMachineToken machineData;
   [SerializeField] private ForceData forceData;
    [SerializeField] private CellData cellData;
   [SerializeField] private CodexInformationPanel panel;

   private Animator animator;
   private bool isShown = false;


 void Start()
   {
   
      animator = GetComponent<Animator>();

   }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if(machineData) panel.UpdatePanel(machineData); 
        else if (forceData) panel.UpdatePanel(forceData);
        else panel.UpdatePanel(cellData);
        
        if(!isShown) 
        {
            panel.Show();
            isShown = true;
        }
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
       animator.SetTrigger("Select");
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        animator.SetTrigger("Idle");
        if(isShown) {panel.Hide(); isShown = false;}
    }

   

  

}
