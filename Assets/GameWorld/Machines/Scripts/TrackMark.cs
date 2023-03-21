using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ozamanas.Machines
{
    public class TrackMark : MonoBehaviour
    {
        private float yOffset = 0;
          private Vector3 newPosition;

        void Start()
        {
            yOffset = Random.Range(0f, .01f);
        }

       void OnEnable()
        {
            newPosition = transform.position;
            newPosition.y = yOffset;
            transform.position = newPosition;
        }
    }
}
