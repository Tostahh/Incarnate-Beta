using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SmallMonster : MonoBehaviour
{
    [SerializeField] private Animator Anim;

    [SerializeField] private NavMeshAgent Agent;
    [SerializeField] private Transform Player;
    [SerializeField] private float RoamRange;
    [SerializeField] private float PlayerFollowRange;
    [SerializeField] private float BuffCooldown;

    [SerializeField] private bool InBattle;

    private bool Following;
    private bool Roaming;
    private bool Buffing;
    private bool Attacking;

    private float RoamTimer;
    private float cooldownTimer;

    private void OnEnable()
    {
        PlayerActions.CallSmall += PowerUp;
    }

    private void Update()
    {
        if (!InBattle)
        {
            if (Vector3.Distance(Player.position, transform.position) > PlayerFollowRange)
            {
                Following = true;
                Roaming = false;
            }
            else
            {
                Following = false;
                Roaming = true;
            }
        }
        else
        {
            Attacking = true;
        }

        if (!Buffing)
        {
            if (Following)
            {
                Agent.SetDestination(Player.position);
            }

            if (Roaming)
            {
                RoamTimer -= Time.deltaTime;

                if (Agent.remainingDistance <= Agent.stoppingDistance && RoamTimer <= 0)
                {
                    Vector3 point;
                    if (RandomRoam(Player.position, RoamRange, out point))
                    {
                        Debug.DrawRay(point, Vector3.up, Color.white, 1.0f);
                        Agent.SetDestination(point);
                        RoamTimer = Random.Range(2f, 6f);
                    }
                }
            }

            if(Attacking)
            {


                //Agent.SetDestination(FindTarget().position);
            }
        }
        cooldownTimer -= Time.deltaTime;

        Anim.SetBool("Running", Agent.velocity.magnitude > 0.01f);
    }

    private void FindTarget()
    {

    }

    private void PowerUp()
    {
        if (!Buffing && cooldownTimer <= 0f)
        {
            Buffing = true;
            Agent.velocity = Vector3.zero;
            Anim.SetTrigger("Buff");
            cooldownTimer = BuffCooldown;
            Invoke("PowerUpReset", 1.5f);
        }
    }

    private void PowerUpReset()
    {
        Buffing = false;
        Agent.SetDestination(Player.position);
    }

    private bool RandomRoam(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        NavMeshHit hit;

        if(NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.5f);
    }
}
