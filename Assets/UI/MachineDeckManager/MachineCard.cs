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
        [SerializeField] private TextMeshProUGUI machineNameText;
        [SerializeField] private Image selectedImage;
        [SerializeField] private RectTransform machineName;


         [Header("Card Setup")]
        [SerializeField] private HumanMachineToken m_machineData;

        private CodexHandler codexHandler;

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
            codexHandler = FindObjectOfType<CodexHandler>();
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

        public void SelectMachineCard(bool active)
        {
            if(!selectedImage) return;

            selectedImage.gameObject.SetActive(active);

        }

        public void OnPointerClick()
        {

             if(!selectedImage) return;

             if(!m_machineData) return;

            if(!codexHandler) return;

            codexHandler.UnSelectCards();

            selectedImage.gameObject.SetActive(true);

            codexHandler.OnObjectClicked(m_machineData);
             
        }

    }
}
