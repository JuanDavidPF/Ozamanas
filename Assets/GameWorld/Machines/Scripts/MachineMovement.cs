using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MachineMovement : MonoBehaviour
{

    public Camera cam;
    private NavMeshAgent machine;

    public Vector3 current_destination;

    // Start is called before the first frame update
    void Start()
    {
         machine = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if( Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray,out hit))
            {
                SetDestination(hit.point);
            }
         }
    }

    public void SetDestination(Vector3 destination)
    {
        current_destination = destination;
    }

    public bool GoToDestination()
    {
        if (current_destination==null) return false;

        return machine.SetDestination(current_destination);

    }

    public NavMeshPathStatus CalculatePath()
    {
        NavMeshPath path = new NavMeshPath();
        machine.CalculatePath(current_destination, path);
        return path.status;
    }

    public bool CheckIfReachDestination()
    {

        if(machine.pathPending) return false; 
        if (machine.remainingDistance > machine.stoppingDistance) return false;
        if (machine.hasPath || machine.velocity.sqrMagnitude != 0f) return false;
        return true;
    }

}
