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

namespace Ozamanas.UI.ForceDeck
{
    public class ForceCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
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

                if (cardArt) cardArt.sprite = value.forceCard;
                if (priceLabel) priceLabel.text = value.price.value.ToString();
            }
        }



        [Space(10)]
        [Header("Card components")]

        [SerializeField] private Image cardArt;
        [SerializeField] private TextMeshProUGUI priceLabel;
        [SerializeField] private Image cooldownProgress;






        public static AncientForce m_forceBeingPlaced;

        public static AncientForce forceBeingPlaced
        {
            get { return m_forceBeingPlaced; }
            set
            {
                if (m_forceBeingPlaced)
                {
                    Destroy(m_forceBeingPlaced.gameObject);
                }
                m_forceBeingPlaced = value;


            }
        }





        private bool isOnCooldown;
        private bool isAffordable = true;



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

        public void OnBeginDrag(PointerEventData eventData)
        {

            if (!forceData || !forceData.force || !IsCardAvailable()) return;


            forceBeingPlaced = Instantiate(forceData.force, Vector3.zero, Quaternion.identity);
            forceBeingPlaced.OnSuccesfulPlacement.AddListener(OnSuccesfulPlacement);
            forceBeingPlaced.OnFailedPlacement.AddListener(OnFailedPlacement);
        }//Closes OnBeginDrag method
        public void OnDrag(PointerEventData eventData)
        {
            if (!forceBeingPlaced || forceBeingPlaced.data != forceData) return;
            forceBeingPlaced.Drag();

        }//Closes OnDrag method
        public void OnEndDrag(PointerEventData eventData)
        {
            if (!forceBeingPlaced || forceBeingPlaced.data != forceData) return;
            forceBeingPlaced.FirstPlacement();
        }//Closes OnEndDrag method

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

        #endregion
    }//Closes ForceCard class
}//Closes Namespace declaration
