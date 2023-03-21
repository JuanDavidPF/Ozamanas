using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.Board;
using DG.Tweening;

namespace Ozamanas.Energy
{
    public class EnergyFlower : MonoBehaviour
    {

        private Animator animator;


        void Awake()
        {
            animator=GetComponent<Animator>();
        }
        public void ShowOrHideFlower(bool show)
        {
          if(show) animator.SetTrigger("Show");
          else animator.SetTrigger("Hide");
        }
    }

}
