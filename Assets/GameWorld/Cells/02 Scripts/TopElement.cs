using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ozamanas
{
    public class TopElement : MonoBehaviour
    {
       
        void Start()
        {
            RotateTopElement();
        }

        void RotateTopElement()
        {
            int y = 0;

            y += Random.Range(0,6) * 60;

            transform.Rotate(0,y,0,Space.Self);
        }



       
    }
}
