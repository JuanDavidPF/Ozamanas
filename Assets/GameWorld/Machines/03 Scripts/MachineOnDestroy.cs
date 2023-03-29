using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Ozamanas
{
    public class MachineOnDestroy : MonoBehaviour
    {

        [SerializeField] public UnityEvent OnMachineDamaged;
        public void DestructableSetup(float time)
        {
            StartCoroutine(RemoveColliders(time));
            OnMachineDamaged?.Invoke();
            Destroy(gameObject,time+2f);
        }

        IEnumerator RemoveColliders(float time)
        {
            yield return new WaitForSeconds(time);
            MeshCollider[] colliders = gameObject.GetComponentsInChildren<MeshCollider>();
            foreach (MeshCollider collider in colliders)
            {
                collider.isTrigger = true;
            }

        }

        
    }
}
