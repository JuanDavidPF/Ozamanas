using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Ozamanas.Energy
{
    public class EnergyOrb : MonoBehaviour
    {
        [SerializeField] private bool goToCounter;
        [SerializeField] private float movementSpeed;

        [SerializeField] private float initDelay;

        [SerializeField] private Vector3 initPosition;

        public Vector2 leftTopPosition ;
        private  EnergyAbsorber absorber;

        private Sequence mySequence;

        public EnergyAbsorber Absorber { get => absorber; set => absorber = value; }

        private bool isDestroyed = false;

        public void GoToEnergyAbsorber()
        {
            if (!goToCounter || !Absorber) return;

             mySequence = DOTween.Sequence();

            Vector3 initPos = transform.position + initPosition;

             mySequence.Append(transform.DOMove(initPos, movementSpeed).SetSpeedBased(true));

            mySequence.Append(transform.DOMove(leftTopPosition, movementSpeed).SetDelay(initDelay).SetSpeedBased(true));

            mySequence.OnComplete(() =>
            {
                DestroyOrb();
            });
        }//Closes Start Method

        void OnTriggerEnter2D(Collider2D col)
        {

            DestroyOrb();
        }

        private void DestroyOrb()
        {
            if(isDestroyed) return;
            mySequence.Kill();
            isDestroyed = true;
            if(Absorber) Absorber.AddEnergyAmount();
            Destroy(gameObject); 
        }

        private void OnDisable()
        {
            mySequence.Kill();
        }//Closes OnDisable method

    }//Closes EnergyOrb class
}//Closes Namespace declaration