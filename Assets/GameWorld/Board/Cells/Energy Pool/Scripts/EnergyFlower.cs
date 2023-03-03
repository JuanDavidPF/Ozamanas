using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.Board;
using DG.Tweening;

namespace Ozamanas.Energy
{
    public class EnergyFlower : MonoBehaviour
    {
        public void ShowOrHideFlower(bool show)
        {
            float temp = 0f;
            if(show) temp=1f;
            transform.DOScaleY(temp,0.1f);
        }
    }

}
