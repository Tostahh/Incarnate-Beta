using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BattleForm : CometMonster
{
    [Header("BattleFormInformation")]
    [SerializeField] private Collider FistHitBox;
    [SerializeField] private Collider HeadHitBox;
    public float Attack1HitCoolDown;
    public float Attack2HitCoolDown;
    [SerializeField] private float Dmg;
    private bool Attacking;
    private int ComboCounter = 0;

    private void Start()
    {
        if (inventory.BigMonsterSlotFull)
        {
            inventory.BigMonsterSlotFull = false;
            Destroy(inventory.BigMonsterSlot.gameObject);
        }
        inventory.BigMonsterSlot = gameObject;
        inventory.BigMonsterSlotFull = true;
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

        if (Battling)
        {
            Agent.SetDestination(FindTarget().position);

            if (Agent.remainingDistance <= Agent.stoppingDistance)
            {
                Agent.updateRotation = false;
                Rotate();
            }
            else
            {
                Agent.updateRotation = true;
            }

            if (Agent.remainingDistance <= Agent.stoppingDistance && !Attacking && ComboCounter < 3)
            {
                Attacking = true;
                Agent.speed = 0;
                StartCoroutine(Attack());
            }
            else if(Agent.remainingDistance <= Agent.stoppingDistance && !Attacking)
            {
                Attacking = true;
                Agent.speed = 0;
                StartCoroutine(Attack1());
            }
        }

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
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && Battling)
        {
            Debug.Log("Hit");

            other.GetComponentInChildren<Heath>().UpdateHeath(-Dmg);
        }
    }

    private IEnumerator Attack()
    {
        FistHitBox.enabled = true;
        Anim.SetTrigger("Attack1");
        yield return new WaitForSeconds(Attack1HitCoolDown);
        FistHitBox.enabled = false;
        Agent.speed = 3;
        Attacking = false;
        ComboCounter++;
    }

    private IEnumerator Attack1()
    {
        FistHitBox.enabled = true;
        Anim.SetTrigger("Attack2");
        yield return new WaitForSeconds(Attack2HitCoolDown);
        FistHitBox.enabled = false;
        Agent.speed = 3;
        Attacking = false;
        ComboCounter = 0;
    }
}
