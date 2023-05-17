using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Ozamanas.Machines;
using Ozamanas.Extenders;
using Ozamanas.World;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Ozamanas.UI
{
    public class MachineMiniMap : MonoBehaviour
    {
        [SerializeField] private Image objectArt;
        [SerializeField] private GameObject toolTip;
        [SerializeField] private GameObject machineIcon;
        [SerializeField] private TextMeshProUGUI objectName;
        [SerializeField] private TextMeshProUGUI description_01;
        [SerializeField] private TextMeshProUGUI speed;
        [SerializeField] private InputActionReference miniMapAction;

        private bool miniMapActive = false;

    

        void Start()
        {
            if(TransformExtender.TryGetComponentInParent<HumanMachine>(transform,out HumanMachine machine))
            {
                objectArt.sprite = machine.Machine_token.machineIcon;
                objectName.text = machine.Machine_token.machineName.GetLocalizedString();
                description_01.text = machine.MachineMovement.CurrentAltitude.ToString();
                speed.text = "Speed: "+machine.MachineMovement.CurrentSpeed.ToString();
            }

            miniMapActive = FindObjectOfType<MiniMapManager>().MiniMapActive;
            machineIcon.SetActive(false);
            toolTip.SetActive(false);
        }

        private void OnEnable()
        {
            miniMapAction.action.Enable();
            miniMapAction.action.performed += _ => SwitchState();
        }

        private void OnDisable()
        {
            miniMapAction.action.Disable();
        }

    

       public void OnPointerExit()
       {
            toolTip.SetActive(false);
       }

       
        public void OnPointerEnter()
        {
             toolTip.SetActive(true);
        }

        private void SwitchState()
        {
            if(!machineIcon) return;
            
            if(miniMapActive) machineIcon.SetActive(false);
            else machineIcon.SetActive(true);
            
            miniMapActive = !miniMapActive;
        }
    }
}
