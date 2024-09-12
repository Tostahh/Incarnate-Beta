using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SetCombat : MonoBehaviour
{
    public static Action TriggerCombat = delegate { };

    [SerializeField] private string EncounterName;

    [SerializeField] private GameObject[] EnemiesInCombat;
    [SerializeField] private float ArenaSize;
    [SerializeField] private GameObject Walls;

    [SerializeField] private Transform RewardSpawn;
    [SerializeField] private GameObject Reward;

    private int EnemiesDefeated;

    private void OnEnable()
    {
        Heath.PlayerDefeated += PlayerDead;
        Heath.EnemyDefeated += EnemyCounter;
    }
    private void OnDisable()
    {
        Heath.PlayerDefeated -= PlayerDead;
        Heath.EnemyDefeated -= EnemyCounter;
    }
    public void PlayerDead()
    {
        TriggerCombat();
        GetComponent<Collider>().enabled = true;
        Walls.SetActive(false);
    }

    public void EnemyCounter()
    {
        EnemiesDefeated++;
        if(EnemiesDefeated >= EnemiesInCombat.Length)
        {
            EnemiesDead();
        }
    }
    public void EnemiesDead()
    {
        Instantiate(Reward, RewardSpawn.transform.position, Quaternion.identity);

        TriggerCombat();
        Walls.SetActive(false);

        // Save that Combat was Won using Name of Encounter
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<Collider>().enabled = false;

            Walls.SetActive(true);

            TriggerCombat();

            for (int i = 0; i < EnemiesInCombat.Length; i++)
            {
                Vector3 SpawnTransform = new Vector3(transform.position.x + UnityEngine.Random.Range(-ArenaSize, ArenaSize), transform.position.y, transform.position.z + UnityEngine.Random.Range(-ArenaSize, ArenaSize));

                Instantiate(EnemiesInCombat[i], SpawnTransform, Quaternion.identity);
            }
        }
    }
}
