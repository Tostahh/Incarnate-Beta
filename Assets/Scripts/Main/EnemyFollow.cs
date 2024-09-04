using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    [SerializeField] private Animator Anim;
    [SerializeField] private NavMeshAgent Agent;

    private Transform Target;

    private Transform PlayerTarget;
    private Transform SmallTarget;
    private Transform BigTarget;

    private void Awake()
    {
        Anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        PlayerTarget = FindObjectOfType<PlayerActions>().transform;
        SmallTarget = FindObjectOfType<SmallMonster>().transform;
        BigTarget = FindObjectOfType<BigMonster>().transform;
    }

    private void Update()
    {
        if (!Target)
        {
            if (PlayerTarget && BigTarget)
            {
                if (Vector3.Distance(transform.position, PlayerTarget.position) < Vector3.Distance(transform.position, BigTarget.position))
                {
                    Target = PlayerTarget;
                    Agent.SetDestination(Target.position);
                }
                else
                {
                    Target = BigTarget;
                    Agent.SetDestination(Target.position);
                }
            }
            else if (PlayerTarget && SmallTarget)
            {
                if (Vector3.Distance(transform.position, PlayerTarget.position) < Vector3.Distance(transform.position, SmallTarget.position))
                {
                    Target = PlayerTarget;
                    Agent.SetDestination(Target.position);
                }
                else
                {
                    Target = SmallTarget;
                    Agent.SetDestination(Target.position);
                }
            }
            else
            {
                Target = PlayerTarget;
                Agent.SetDestination(Target.position);
            }
        }
        else
        {
            Agent.SetDestination(Target.position);
        }

        Anim.SetBool("Running", Agent.velocity.magnitude > 0.01f);
    }
}
