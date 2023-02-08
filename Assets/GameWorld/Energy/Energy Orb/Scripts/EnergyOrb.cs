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
        private Tweener tweener;
        private void Start()
        {

            Transform absorber = EnergyAbsorber.mainAbsorber;

            if (!goToCounter || !absorber) return;
            
            tweener = transform.DOMove(absorber.position, movementSpeed)
            .SetSpeedBased(true);


            tweener.OnUpdate(() =>
            {
                float distance = Mathf.Min(Vector3.Distance(transform.position, absorber.position) /4, 1f);
                transform.localScale = new Vector3(distance, distance, distance);
                tweener.ChangeEndValue(absorber.position, true);
            }
            );

        }//Closes Start Method

        private void OnDisable()
        {
            tweener.Kill();
        }//Closes OnDisable method

    }//Closes EnergyOrb class
}//Closes Namespace declaration