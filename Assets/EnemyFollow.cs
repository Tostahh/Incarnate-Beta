using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    [SerializeField] private NavMeshAgent Agent;

    private Transform target;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 6)
        {
            if(other.gameObject.tag == "Player")
            {
                target = other.gameObject.transform;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            if (other.gameObject.tag == "Player")
            {
                target = null;
            }
        }
    }

    private void Update()
    {
        if (target)
        {
            Agent.SetDestination(target.position);
        }
        else
        {
            Agent.SetDestination(transform.position);
        }
    }


}
