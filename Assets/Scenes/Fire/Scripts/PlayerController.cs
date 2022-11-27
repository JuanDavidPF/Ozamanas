using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Ozamanas.Tags;

public class PlayerController : MonoBehaviour
{
    
     private Tween playerTween;

    private Animator animator;

    private Vector3 currentDestination = new Vector3(0f,-10f,0f);

    private List<Vector3> playerPath = new List<Vector3>();

    [SerializeField] List<float> playerJumps = new List<float>();
    
    [SerializeField] float speed = 2f;

     [SerializeField]  private PlayerState playerState = PlayerState.Idling;

    private bool onLevelTriggerEnter = false;
    public PlayerState PlayerState { get => playerState; set => playerState = value; }

    public void Awake()
    {
        animator = GetComponent<Animator>();
    }
    
    public void MoveToDestination(Vector3 destination)
    {
        if(destination == transform.position) return;

        if(currentDestination != destination) currentDestination = destination;

        foreach(float jump in playerJumps)
        {
            Vector3 temp = Vector3.Lerp(transform.position,currentDestination,jump);
            playerPath.Add(temp);
        }

        gameObject.transform.LookAt(destination);
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

     private void OnTriggerStay(Collider other)
    {
         if (other.tag != "Level") return;

         if(transform.position != currentDestination ) return;

         if(onLevelTriggerEnter) return;
         
         if(other.TryGetComponent<LevelHandler>(out LevelHandler level))
         {
            onLevelTriggerEnter = true;     
            PlayerState = PlayerState.Idling;
            animator.SetTrigger("Idle");
            level.PlayLevel();
         }
    }

}
