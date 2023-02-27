using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ozamanas.Machines
{
    public class PathManager : MonoBehaviour
    {
       [SerializeField] private GameObject pathRenderer;

       private TrailRenderer currentTrail;

       private MachinePhysicsManager physicsManager;

       void Awake()
       {
         physicsManager = GetComponentInParent<MachinePhysicsManager>();
       }

       void Start()
       {
           AttachNewTrail();
       }

       public void DetachTrail()
       {
            if(!currentTrail) return;

            currentTrail.emitting = false;

            currentTrail = null;
       }

       public void AttachNewTrail()
       {
            if(!pathRenderer) return;

            if(currentTrail) return;
           
           currentTrail = Instantiate(pathRenderer,transform).GetComponent<TrailRenderer>();
       }


    }
}
