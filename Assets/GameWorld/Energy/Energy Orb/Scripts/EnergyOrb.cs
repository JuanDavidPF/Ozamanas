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
        [SerializeField] private Ease movementEase;
        private Tweener tweener;



        
        private void Start()
        {

            Transform absorber = EnergyAbsorber.mainAbsorber;

            if (!goToCounter || !absorber) return;
            tweener = transform.DOMove(absorber.position, movementSpeed)
            .SetSpeedBased(true)
            .SetEase(movementEase);

            tweener.OnUpdate(() =>
                tweener.ChangeEndValue(absorber.position, true)
            );

        }//Closes Start Method

        private void OnDisable()
        {
            tweener.Kill();
        }//Closes OnDisable method

    }//Closes EnergyOrb class
}//Closes Namespace declaration