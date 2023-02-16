using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Ozamanas.Tags;

namespace Ozamanas.GameScenes
{

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform cameraAnchor;

     private Tween playerTween;

    private Animator animator;

    private Transform currentDestination;

    private List<Vector3> playerPath = new List<Vector3>();

    [SerializeField] float jumpDistance ;
    
    [SerializeField] float speed = 2f;

     [SerializeField]  private PlayerState playerState = PlayerState.Idling;

    public PlayerState PlayerState { get => playerState; set => playerState = value; }

    public void Awake()
    {
        animator = GetComponent<Animator>();
    }

    
    public void MoveToDestination(Transform destination)
    {
        if(currentDestination != destination) currentDestination = destination;

        float distance =Vector3.Distance(gameObject.transform.position,destination.position );

        float jumps = distance / jumpDistance;

        for(int i = 1; i <= jumps + 1 ; i++)
        {
            float value = Mathf.Clamp(((i*jumpDistance)/distance),0f,1f);
            Vector3 temp = Vector3.Lerp(transform.position,currentDestination.transform.position ,value);
            playerPath.Add(temp);
        }

        gameObject.transform.DOLookAt(destination.position ,0.5f);
        
        animator.SetTrigger("Jump");

        PlayerState = PlayerState.Running;

    }

    public void Jump()
    {
       if (playerPath.Count == 0) return;

        if (playerTween != null) playerTween.Kill();

        playerTween = transform.DOMove(playerPath[0], speed, false).SetSpeedBased();

        playerTween.OnComplete(() =>
        {
            playerPath.RemoveAt(0);
           
        });

    }

    public void CheckNextJump()
    {
        if (playerPath.Count > 0) return;
        
        animator.SetTrigger("Idle");
        PlayerState = PlayerState.Waiting;
    }



}
}