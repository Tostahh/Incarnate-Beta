using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SmallMonster : MonoBehaviour
{
    [SerializeField] private string Name;
    public int MonsterNum;

    [SerializeField] private Animator Anim;
    [SerializeField] private Collider ShroomHitBox;

    [SerializeField] private NavMeshAgent Agent;
    [SerializeField] private Transform Player;
    [SerializeField] private float RoamRange;
    [SerializeField] private float PlayerFollowRange;
    [SerializeField] private float BuffCooldown;

    [SerializeField] private bool InBattle;

    private bool Following;
    private bool Roaming;
    private bool Buffing;
    private bool Battling;

    private float RoamTimer;
    private float cooldownTimer;
    private bool Attacking;

    private Inventory inventory;
    private void Awake()
    {
        DontDestroyOnLoad(this);

        inventory = FindObjectOfType<Inventory>();
    }
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
    private void OnEnable()
    {
        PlayerActions.CallSmall += PowerUp;
        SceneManagment.NewSceneLoaded += SetPos;
        SetCombat.TriggerCombat += CombatStance;
    }
    private void OnDisable()
    {
        PlayerActions.CallSmall -= PowerUp;
        SceneManagment.NewSceneLoaded -= SetPos;
        SetCombat.TriggerCombat -= CombatStance;
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

                if (Agent.remainingDistance <= Agent.stoppingDistance && !Attacking)
                {
                    Attacking = true;
                    Agent.speed = 0;
                    StartCoroutine(Attack());
                }
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
    private void Rotate()
    {
        Vector3 lookPos = Agent.destination - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 12f);
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
    public string GiveName()
    {
        return Name;
    }
    public void SetPos()
    {
        Agent.enabled = false;
        Debug.Log("Called Pos");
        Player = FindObjectOfType<PlayerActions>().gameObject.transform;
        transform.position = Player.transform.position;
        Agent.enabled = true;
    }
    public void CombatStance()
    {
        if (!InBattle)
        {
            InBattle = true;
        }
        else
        {
            InBattle = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && Battling)
        {
            Debug.Log("Hit");
        }
    }

    private IEnumerator Attack()
    {
        ShroomHitBox.enabled = true;
        Anim.SetTrigger("Attack");
        yield return new WaitForSeconds(1f);
        ShroomHitBox.enabled = false;
        Agent.speed = 3;
        Attacking = false;
    }
}
