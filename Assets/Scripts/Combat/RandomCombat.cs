using System;
using UnityEngine;

public class RandomCombat : MonoBehaviour
{
    public GameObject[] Enemies;

    public float MaxTimeBetweenEncounters;
    public float MinTimeBetweenEncounters;

    private float EncounterTimer;

    private PlayerActions PA;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            EncounterTimer = 0;
            PA = other.gameObject.GetComponentInChildren<PlayerActions>();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (PA.Moving)
            {
                EncounterTimer += Time.deltaTime;
                if (EncounterTimer >= UnityEngine.Random.Range(MinTimeBetweenEncounters, MaxTimeBetweenEncounters))
                {
                    Instantiate(Enemies[UnityEngine.Random.Range(0, Enemies.Length)], PA.gameObject.transform.position, Quaternion.identity);
                    EncounterTimer = 0;
                }
            }
        }
    }
}
