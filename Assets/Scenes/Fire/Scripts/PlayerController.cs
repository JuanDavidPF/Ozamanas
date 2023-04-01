using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Ozamanas.Tags;

namespace Ozamanas.GameScenes
{

public class PlayerController : MonoBehaviour
{
     private Tween playerTween;

    private Animator animator;

    private Vector3 currentDestination;

    private List<Vector3> playerPath = new List<Vector3>();

    public Vector3 playerOffset = new Vector3(0f,0.35f,0f);
    [SerializeField] float jumpDistance ;
    
    [SerializeField] float speed = 2f;

     [SerializeField]  private PlayerState playerState = PlayerState.Idling;

    public PlayerState PlayerState { get => playerState; set => playerState = value; }

    public void Awake()
    {
        animator = GetComponent<Animator>();
    }

    
    public void MoveToDestination(Vector3 destination,  List<Vector3> path)
    {
        if(currentDestination != destination) currentDestination = destination;

        if(path == null || path.Count == 0) return;

        playerPath = path;

        gameObject.transform.DOLookAt(destination ,0.5f);
        
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