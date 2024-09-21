using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SmallMonster : CometMonster
{
    [Header("SupportFormInformation")]
    [SerializeField] private float BuffCooldown;
    private bool Buffing;
    private float cooldownTimer;

    private void Start()
    {
        if (inventory.SmallMonsterSlotFull)
        {
            inventory.SmallMonsterSlotFull = false;
            Destroy(inventory.SmallMonsterSlot.gameObject);
        }
        inventory.SmallMonsterSlot = gameObject;
        inventory.SmallMonsterSlotFull = true;
    }
    public override void OnEnable()
    {
        base.OnEnable();
        PlayerActions.CallSmall += PowerUp;
    }
    public override void OnDisable()
    {
        base.OnDisable();
        PlayerActions.CallSmall -= PowerUp;
    }

    private void Update()
    {
        if (!Player)
        {
            Player = FindObjectOfType<PlayerActions>().gameObject.transform;
        }

        if (!InBattle)
        {
            Battling = false;
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
            Battling = true;
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

            if(Battling)
            {
                Agent.SetDestination(Player.position);
            }
        }
        cooldownTimer -= Time.deltaTime;

        Anim.SetBool("Running", Agent.velocity.magnitude > 0.01f);
    }

    private Transform FindTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length > 0)
        {
            GameObject Target = enemies[0];
            Transform target;
            for (int i = 0; i < enemies.Length; i++)
            {
                if (Vector3.Distance(transform.position, enemies[i].transform.position) < Vector3.Distance(transform.position, Target.transform.position))
                {
                    Target = enemies[i];
                }
            }

            target = Target.transform;

            return target;
        }
        else
        {
            return Player;
        }
    }

    private void PowerUp()
    {
        if (!Buffing && cooldownTimer <= 0f)
        {
            Agent.Warp(Player.transform.position + new Vector3(Random.Range(-2,2), 0, Random.Range(-2, 2)));
            Buffing = true;
            Agent.velocity = Vector3.zero;
            Agent.speed = 0;
            Anim.SetTrigger("Buff");
            cooldownTimer = BuffCooldown;
            Invoke("PowerUpReset", 1.5f);
        }
    }

    private void PowerUpReset()
    {
        Buffing = false;
        Agent.speed = 3;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && Battling)
        {
            Debug.Log("Hit");
        }
    }
}
