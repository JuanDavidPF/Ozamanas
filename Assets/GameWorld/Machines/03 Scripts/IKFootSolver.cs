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


    private Vector3 newPosition,currentPosition,targetOffset = new Vector3(0,0,0);

    private void Start()
    {
        /*footSpacing = transform.localPosition.x;
        currentPosition = newPosition = oldPosition = transform.position;
        currentNormal = newNormal = oldNormal = transform.up;
        lerp = 1;*/
        targetOffset =  transform.position - new Vector3(body.position.x,0,body.position.z);
        transform.position = new Vector3(0,0,0);
    }

    // Update is called once per frame

    void Update()
    {
        transform.position = currentPosition;

        if(!CheckDistanceWalked()) return;


        currentPosition = new Vector3(body.position.x,0,body.position.z)+ targetOffset;

        transform.DOJump(currentPosition,stepHeight,1,stepTime,false);

       /* transform.position = currentPosition;
        transform.up = currentNormal;*/


       // 
        /*

        Ray ray = new Ray(body.position + (body.right * footSpacing), Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit info, 10, terrainLayer.value))
        {
            if (Vector3.Distance(newPosition, info.point) > stepDistance && !otherFoot.IsMoving() && lerp >= 1)
            {
                lerp = 0;
                int direction = body.InverseTransformPoint(info.point).z > body.InverseTransformPoint(newPosition).z ? 1 : -1;
                newPosition = info.point + (body.forward * stepLength * direction) + footOffset;
                newNormal = info.normal;
            }
        }

        if (lerp < 1)
        {
            Vector3 tempPosition = Vector3.Lerp(oldPosition, newPosition, lerp);
            tempPosition.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;

            currentPosition = tempPosition;
            currentNormal = Vector3.Lerp(oldNormal, newNormal, lerp);
            lerp += Time.deltaTime * speed;
        }
        else
        {
            oldPosition = newPosition;
            oldNormal = newNormal;
        }*/
    }

    private void OnDrawGizmos()
    {
        newPosition = body.position - new Vector3(0,body.position.y,0);

        Gizmos.color = Color.red;
       Gizmos.DrawSphere(newPosition, 0.5f);
    }


     private bool CheckDistanceWalked()
    {
            var distanceWalked = Vector3.Distance(new Vector3(transform.position.x,0,transform.position.z), new Vector3(body.position.x,0,body.position.z));

            return distanceWalked >= trackDistance;
    }

    

    }
}
