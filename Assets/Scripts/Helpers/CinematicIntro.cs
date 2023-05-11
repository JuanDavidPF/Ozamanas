using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace JuanPayan.Helpers
{
    public class CinematicIntro : MonobehaviourEvents
    {
        private Tween anchorTween;
        private CameraAnchor cameraAnchor;
        [SerializeField] private float focusSpeed = 0.2f;
        public override void Behaviour()
        {
             cameraAnchor = FindObjectOfType<CameraAnchor>();
             anchorTween = cameraAnchor.transform.DOMove(transform.position,focusSpeed,false);
        }

     
    }
}
