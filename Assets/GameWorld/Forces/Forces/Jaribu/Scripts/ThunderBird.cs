using UnityEngine;
using DG.Tweening;

namespace Ozamanas.Forces
{
    public class ThunderBird : MonoBehaviour
    {
        [SerializeField] private ParticleSystem thunder;
        private Tween birdTween;
        [SerializeField] private float thunderSpeed = 2f;

        public void Start()
        {
            birdTween = transform.DOMoveY(0, thunderSpeed, false).SetSpeedBased();
            thunder.Play();
            birdTween.OnComplete(() =>
            {
                Destroy(gameObject);
            });
        }




    }
}
