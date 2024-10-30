using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SetCombat : MonoBehaviour
{
    public static Action TriggerCombat = delegate { };

    [SerializeField] private int SetCombatID;

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
        var saveData = FindObjectOfType<SaveLoadJson>().GiveSaveData();

        if (saveData.SetCombats.Length > SetCombatID)
        {
            if (saveData.SetCombats[SetCombatID].Defeated)
            {
                Defeated = saveData.SetCombats[SetCombatID].Defeated;
            }
        }
    }
    private void OnEnable()
    {
        var saveData = FindObjectOfType<SaveLoadJson>().GiveSaveData();

        if (saveData.SetCombats.Length > SetCombatID)
        {
            if (saveData.SetCombats[SetCombatID].Defeated)
            {
                Defeated = saveData.SetCombats[SetCombatID].Defeated;
            }
        }

        if (Defeated)
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
                var saveData = FindObjectOfType<SaveLoadJson>().GiveSaveData();
                var combats = saveData.SetCombats;

                if (combats.Length > SetCombatID)
                {
                    if (!combats[SetCombatID].Defeated)
                    {
                        Defeated = true;
                        combats[SetCombatID].Defeated = Defeated;
                        FindObjectOfType<SaveLoadJson>().SaveGame();
                    }
                }
                else
                {
                    Array.Resize(ref combats, SetCombatID + 1);

                    for (int i = 0; i < combats.Length; i++)
                    {
                        if (combats[i].CombatID == 0 && !combats[i].Defeated)
                        {
                            combats[i] = new CombatInfo { CombatID = i, Defeated = false };
                        }
                    }

                    Defeated = true;
                    combats[SetCombatID] = new CombatInfo { CombatID = SetCombatID, Defeated = Defeated };

                    Array.Sort(combats, (a, b) => a.CombatID.CompareTo(b.CombatID));

                    saveData.SetCombats = combats;
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

[System.Serializable]
public struct CombatInfo
{
    public int CombatID;
    public bool Defeated;
}
