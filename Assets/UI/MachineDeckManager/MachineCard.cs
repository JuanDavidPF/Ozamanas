using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ozamanas.Machines;
using TMPro;
using DG.Tweening;

namespace Ozamanas.UI
{
    public class MachineCard : MonoBehaviour
    {
         [Header("Card components")]

        [SerializeField] private Image cardArt;

          private TextMeshProUGUI machineNameText;

          [SerializeField] private RectTransform machineName;

         [Header("Card Setup")]
        [SerializeField] private HumanMachineToken m_machineData;

         private RectTransform rectTransform;

        public HumanMachineToken MachineData
        {
            get { return m_machineData; }
            set
            {
                m_machineData = value;
                if (!value) return;
                if (cardArt) cardArt.sprite = value.machineIcon;
                if(machineNameText) machineNameText.text = value.machineName.GetLocalizedString();
            }
        }

         void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            machineNameText = machineName.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        }

         public void OnPointerEnter()
        {
            if(!machineName || !rectTransform) return; 

            rectTransform.DOScale(new Vector3(1.2f,1.2f,1.2f),0.3f);
            machineName.gameObject.SetActive(true);
        }

        public void OnPointerExit()
        {
            if(!machineName || !rectTransform) return; 
            
            rectTransform.DOScale(new Vector3(1f,1f,1f),0.3f);
            machineName.gameObject.SetActive(false);

        }




    }
}
