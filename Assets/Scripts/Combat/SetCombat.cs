using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SetCombat : MonoBehaviour
{
    public static Action TriggerCombat = delegate { };

    [SerializeField] private int AreaNumb;
    [SerializeField] private int EncounterNumb;

    public bool Active;
    public bool Defeated;

    [SerializeField] private GameObject[] EnemiesInCombat;
    [SerializeField] private float ArenaSize;
    [SerializeField] private GameObject Walls;

    [SerializeField] private Transform RewardSpawn;
    [SerializeField] private GameObject Reward;

    private GameObject[] enemies;
    private int EnemiesDefeated;

    private void Awake()
    {
        if (AreaNumb == 0)
        {
            if (FindObjectOfType<SaveLoadJson>().GiveSaveData().PlanetAreaSetCombats[EncounterNumb])
            {
                Defeated = FindObjectOfType<SaveLoadJson>().GiveSaveData().PlanetAreaSetCombats[EncounterNumb];
            }
        }
    }
    private void OnEnable()
    {
        if (AreaNumb == 0)
        {
            if (FindObjectOfType<SaveLoadJson>().GiveSaveData().PlanetAreaSetCombats[EncounterNumb])
            {
                Defeated = FindObjectOfType<SaveLoadJson>().GiveSaveData().PlanetAreaSetCombats[EncounterNumb];
            }
        }

        if(Defeated)
        {
            GetComponent<Collider>().enabled = false;
        }

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
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i])
            {
                Destroy(enemies[i]);
            }
        }
        EnemiesDefeated = 0;
        Active = false;
    }

    public void EnemyCounter()
    {
        if (Active)
        {
            EnemiesDefeated++;
            if (EnemiesDefeated >= EnemiesInCombat.Length)
            {
                EnemiesDead();
            }
        }
    }
    public void EnemiesDead()
    {
        if (Active)
        {
            if (!Defeated)
            {
                Instantiate(Reward, RewardSpawn.transform.position, Quaternion.identity);

                TriggerCombat();
                GetComponent<Collider>().enabled = false;
                Walls.SetActive(false);

                if (!FindObjectOfType<SaveLoadJson>().GiveSaveData().PlanetAreaSetCombats[EncounterNumb])
                {
                    Defeated = true;
                    FindObjectOfType<SaveLoadJson>().GiveSaveData().PlanetAreaSetCombats[EncounterNumb] = Defeated;
                    FindObjectOfType<SaveLoadJson>().SaveGame();
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!Defeated)
        {
            if (other.CompareTag("Player"))
            {
                enemies = new GameObject[EnemiesInCombat.Length];

                GetComponent<Collider>().enabled = false;

                Walls.SetActive(true);

                TriggerCombat();

                for (int i = 0; i < EnemiesInCombat.Length; i++)
                {
                    Vector3 SpawnTransform = new Vector3(transform.position.x + UnityEngine.Random.Range(-ArenaSize, ArenaSize), transform.position.y, transform.position.z + UnityEngine.Random.Range(-ArenaSize, ArenaSize));

                    enemies[i] = Instantiate(EnemiesInCombat[i], SpawnTransform, Quaternion.identity);
                }
                Active = true;
            }
        }
    }
}
