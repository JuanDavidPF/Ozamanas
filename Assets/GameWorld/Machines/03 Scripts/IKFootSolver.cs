using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Ozamanas.Machines
{
    public class IKFootSolver : MonoBehaviour
    {
   
    [SerializeField] private float stepTime = 1f;
    [SerializeField] private float stepHeight = 1f;
    [SerializeField] private float trackDistance = 0.2f;
    [SerializeField] private Transform body;
    [SerializeField] private List<Transform> targets;
    [SerializeField] private List<Transform> IK_targets;
    private List<Vector3> current_Positions = new List<Vector3>();

    private bool goLeftLegs = true;

    private Vector3 currentPosition = new Vector3();




    private void Start()
    {
         if(targets.Count != IK_targets.Count) return;

         for (int i =0; i<IK_targets.Count;i++)
         {
            IK_targets[i].position = targets[i].position;
            current_Positions.Add(targets[i].position);
         }

         currentPosition = body.position;
    }

    // Update is called once per frame

    void Update()
    {
        for (int i =0; i<IK_targets.Count;i++)
         {
            IK_targets[i].position = current_Positions[i];
         }
        

        if(!CheckDistanceWalked()) return;

        DOTween.KillAll();

       // GoLeft();

        currentPosition = body.position;
      
    }

   
    private void GoLeft()
    {
        current_Positions[0] = targets[0].position;
        IK_targets[0].transform.DOJump(targets[0].position,stepHeight,1,stepTime,false);
    }

     private bool CheckDistanceWalked()
    {
            var distanceWalked = Vector3.Distance(currentPosition, new Vector3(body.position.x,0,body.position.z));
            return distanceWalked >= trackDistance;
          
    }

    

    }
}
