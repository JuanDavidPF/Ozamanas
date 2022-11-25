using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    
     private Tween playerTween;

    private Animator animator;

    private Vector3 currentDestination;

    private List<Vector3> playerPath = new List<Vector3>();

    [SerializeField] List<float> playerJumps = new List<float>();
    
    [SerializeField] float speed = 2f;
    public void MoveToDestination(Vector3 destination)
    {
        if(currentDestination != destination) currentDestination = destination;

        foreach(float jump in playerJumps)
        {
            Vector3 temp = Vector3.Lerp(transform.position,currentDestination,jump);
            playerPath.Add(temp);
        }

        gameObject.transform.LookAt(destination);
        animator.SetTrigger("Jump");
        Jump();


    }

    private void Jump()
    {
        if (playerPath.Count == 0) return;

        if (playerTween != null) playerTween.Kill();

        playerTween = transform.DOMove(playerPath[0], speed, false).SetSpeedBased();

        playerTween.OnComplete(() =>
        {
                playerPath.RemoveAt(0);
                Jump();     
        });     
    }

     private void OnTriggerStay(Collider other)
    {
         if (other.tag != "Level") return;

         if(transform.position != currentDestination ) return;

         if(other.TryGetComponent<LevelHandler>(out LevelHandler level))
         {
            level.PlayLevel();
         }
    }

}
