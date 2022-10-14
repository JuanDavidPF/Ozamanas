using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ozamanas.Forces
{
    public class SnakeController : MonoBehaviour
    {
        Transform biteObjective;


        public void Bite(Transform transform)
        {
            biteObjective = transform;
        }//Closes Bite method

        // Update is called once per frame
        void Update()
        {
            if (!biteObjective) return;

            transform.position = biteObjective.position;
        }
    }
}
