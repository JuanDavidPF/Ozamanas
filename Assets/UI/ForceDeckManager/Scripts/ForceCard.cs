using System.Collections;
using System.Collections.Generic;

using DG.Tweening;
using JuanPayan.References;
using Ozamanas.Forces;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.InputSystem;

namespace Ozamanas.UI
{
    public class ForceCard : MonoBehaviour
    {

        [Space(15)]
        [Header("Events")]
        [SerializeField] private UnityEvent OnIsNotAffordable;
        [SerializeField] private UnityEvent OnIsOnCollDown;
        [SerializeField] private UnityEvent OnForceFailedPlacement;

         [Space(15)]
        [Header("Card Setup")]
        [SerializeField] private ForceData m_forceData;
        [SerializeField] private IntegerVariable energyCounter;



        public ForceData forceData
        {
            get { return m_forceData; }
            set
            {

                m_forceData = value;
                if (!value) return;

                if (cardArt) cardArt.sprite = value.forceIcon;
                if (priceLabel) priceLabel.text = value.price.value.ToString();
                if(forceNameText) forceNameText.text = value.forceName.GetLocalizedString();
            }
        }



        [Space(10)]
        [Header("Card components")]

        [SerializeField] private Image cardArt;

        [SerializeField] private Image selectedImage;
        [SerializeField] private TextMeshProUGUI priceLabel;
        [SerializeField] private Image cooldownProgress;

        private RectTransform rectTransform;

        [SerializeField] private RectTransform forceName;
         private TextMeshProUGUI forceNameText;




        public static AncientForce m_forceBeingPlaced;

        public static AncientForce forceBeingPlaced
        {
            get { return m_forceBeingPlaced; }
            set
            {
                if (m_forceBeingPlaced)
                {
                    m_forceBeingPlaced.DestroyForce();
                }
                m_forceBeingPlaced = value;


            }
        }


        private bool isOnCooldown;
        private bool isAffordable = true;

        void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            forceNameText = forceName.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        }


        public void SelectForceCard(bool active)
        {
            if(!selectedImage) return;
            selectedImage.gameObject.SetActive(active);
        }

       

        public void OnPlayerEnergyChanged(int energyAmount)
        {
            if (!forceData) return;
            isAffordable = energyAmount >= forceData.price.value;
        }//Closes OnPlayerEnergyChanged method

        private bool IsCardAvailable()
        {
            if(!isAffordable) OnIsNotAffordable?.Invoke();

            if(isOnCooldown) OnIsOnCollDown?.Invoke();

            return !isOnCooldown && isAffordable;
        }//Closes IsCardAvailable method


        #region Drag and Drop functionality

        public void LaunchForce(InputAction.CallbackContext context)
        {
            if(!context.performed) return;

            InstantiateForce();
        }

        public void OnPointerClick()
        {
            
             InstantiateForce();
        }

        private void InstantiateForce()
        {
             if (!forceData || !forceData.force || !IsCardAvailable()) return;
            forceBeingPlaced = Instantiate(forceData.force, Vector3.zero, Quaternion.identity);
            forceBeingPlaced.OnSuccesfulPlacement.AddListener(OnSuccesfulPlacement);
            forceBeingPlaced.OnFailedPlacement.AddListener(OnFailedPlacement);
        }

        private void Update()
        {
             if (!forceBeingPlaced || forceBeingPlaced.data != forceData) return;
            forceBeingPlaced.Drag();
        }

         public void FireForce(InputAction.CallbackContext context)
        {
            if(!context.performed) return;

           if (!forceBeingPlaced || forceBeingPlaced.data != forceData) return;


            if(forceBeingPlaced.Placements==0) forceBeingPlaced.FirstPlacement();
            else if(forceBeingPlaced.Placements==1) forceBeingPlaced.SecondPlacement();
            else if(forceBeingPlaced.Placements==2) forceBeingPlaced.ThirdPlacement();
        }

         public void CancelLaunch(InputAction.CallbackContext context)
        {
            if(!context.performed) return;

            if(!forceBeingPlaced) return;

           
            forceBeingPlaced.OnFailedPlacement.RemoveListener(OnFailedPlacement);
            forceBeingPlaced.OnSuccesfulPlacement.RemoveListener(OnSuccesfulPlacement);
            forceBeingPlaced = null;
        }
       

        #endregion


        #region Placement callbacks
        private void OnFailedPlacement(AncientForce force)
        {
            if (!force || !force.data) return;
            force.OnFailedPlacement.RemoveListener(OnFailedPlacement);
            force.OnSuccesfulPlacement.RemoveListener(OnSuccesfulPlacement);
            if (forceBeingPlaced == force) forceBeingPlaced = null;
            OnForceFailedPlacement?.Invoke();

        }//Closes OnCancelledPlacement method
        private void OnSuccesfulPlacement(AncientForce force)
        {

            if (!force || !force.data) return;
            force.OnFailedPlacement.RemoveListener(OnFailedPlacement);
            force.OnSuccesfulPlacement.RemoveListener(OnSuccesfulPlacement);

            if (force == forceBeingPlaced) m_forceBeingPlaced = null;

            if (energyCounter) energyCounter.value -= force.data.price.value;

            isOnCooldown = true;

            if (cooldownProgress)
            {
                cooldownProgress.DOFillAmount(0, force.data.cooldown.value).From(1f).OnComplete(() =>
                {
                    isOnCooldown = false;
                });
            }

        }//Closes OnSuccesfulPlacement method


        #endregion


        #region Editor method
        [ContextMenu("Reload")]
        public void ReloadCard()
        {
            forceData = m_forceData;
        }//Closes ReloadData method

        public void OnPointerEnter()
        {
            if(!forceName || !rectTransform) return; 

            rectTransform.DOScale(new Vector3(1.2f,1.2f,1.2f),0.3f);
            forceName.gameObject.SetActive(true);
        }

        public void OnPointerExit()
        {
            if(!forceName || !rectTransform) return; 
            
            rectTransform.DOScale(new Vector3(1f,1f,1f),0.3f);
            forceName.gameObject.SetActive(false);

        }



        #endregion
    }//Closes ForceCard class
}//Closes Namespace declaration
