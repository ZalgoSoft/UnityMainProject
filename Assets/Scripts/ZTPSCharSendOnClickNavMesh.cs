using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
public class ZCharSendOnClickNavMesh : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    float maxDistance = 100f;
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    void Update()
    {
        //auto check
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        { //animation stop
        }
        else
        { //animation walk
        }
        //on click on 3d world
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, maxDistance))
            {
                //if destination successfull
                if (navMeshAgent.SetDestination(hit.point))
                {
                    //animator.SetTrigger("Run");
                }
            }
        }
    }
}