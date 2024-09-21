using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CometMonster : MonoBehaviour
{
    [Header("MonsterInformation")]
    public string Name;
    public int MonsterNum;

    [Header("RoamBehavior")]
    public float RoamTimer;
    public float RoamRange;
    public float PlayerFollowRange;

    [Header("States")]
    public bool InBattle;
    public bool Following;
    public bool Roaming;
    public bool Battling;

    [Header("Refs")]
    public Transform Player;
    public Inventory inventory;
    public Animator Anim;
    public NavMeshAgent Agent;

    public virtual void Awake()
    {
        DontDestroyOnLoad(this);

        inventory = FindObjectOfType<Inventory>();
    }
    public virtual void OnEnable()
    {
        SceneManagment.NewSceneLoaded += SetPos;
        SetCombat.TriggerCombat += CombatStance;
    }
    public virtual void OnDisable()
    {
        SceneManagment.NewSceneLoaded -= SetPos;
        SetCombat.TriggerCombat -= CombatStance;
    }

    public virtual bool RandomRoam(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }
    public virtual void Rotate()
    {
        Vector3 lookPos = Agent.destination - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 12f);
    }
    public virtual string GiveName()
    {
        return Name;
    }
    public virtual void SetPos()
    {
        StartCoroutine(Set());
    }
    public virtual IEnumerator Set()
    {
        if (!Player)
        {
            yield return new WaitForSeconds(0.1f);
            Agent.isStopped = true;
            Player = FindObjectOfType<PlayerActions>().gameObject.transform;
            Agent.velocity = Vector3.zero;
            Agent.Warp(Player.position);
            Agent.isStopped = false;
        }
        else
        {
            yield return null;
        }
    }
    public virtual void CombatStance()
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
    public virtual void MonHit()
    {
        Anim.SetTrigger("Hit");
    }
}
