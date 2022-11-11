using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

namespace Ozamanas.Forces
{
    public class ThunderBird : MonoBehaviour
    {
        public UnityEvent OnRelease;
        public void Start()
        {
            transform.position = new Vector3(transform.position.x,0,transform.position.z);   
            OnRelease?.Invoke();
            Destroy(gameObject,1f);
        }

   
    }
}
