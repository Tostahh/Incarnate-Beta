using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    [SerializeField] private Animator Anim;
    [SerializeField] private NavMeshAgent Agent;

    [SerializeField] private float KnockbackAmount;
    [SerializeField] private float Dmg;

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
        if (PlayerTarget.GetComponent<PlayerActions>().SmallMon)
        {
            SmallTarget = FindObjectOfType<SmallMonster>().transform;
        }
        if (PlayerTarget.GetComponent<PlayerActions>().BigMon)
        {
            BigTarget = FindObjectOfType<BigMonster>().transform;
        }
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Hitting Player");

            other.GetComponentInChildren<Heath>().UpdateHeath(-Dmg);

            other.GetComponentInChildren<PlayerActions>().PlayerHit();

            Vector3 Direction = (other.transform.position - transform.position).normalized;
            KnockBack(other.GetComponentInChildren<Rigidbody>(), Direction, KnockbackAmount, 0.5f);
        }

        if (other.gameObject.CompareTag("CometMonster"))
        {
            Debug.Log("Hitting Mon");

            other.GetComponentInChildren<Heath>().UpdateHeath(-Dmg);

            other.GetComponentInChildren<CometMonster>().MonHit();
        }
    }

    private IEnumerator KnockBack(Rigidbody rb, Vector3 Direction, float Power, float time)
    {
        float timer = 0;
        while (timer <= time)
        {
            rb.AddForce(Direction.normalized * Power);
            yield return new WaitForFixedUpdate();
            timer += Time.deltaTime;
        }
    }

    public void Stun()
    {
        Agent.speed = 0;
        StartCoroutine(StunC());
    }
    private IEnumerator StunC()
    {
        yield return new WaitForSeconds(0.1f);
        Agent.speed = 2.5f;
    }
}
