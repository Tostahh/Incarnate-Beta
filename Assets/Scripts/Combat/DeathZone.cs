using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            ReturnStation[] Stations = FindObjectsOfType<ReturnStation>();

            foreach(ReturnStation station in Stations)
            {
                // Intranstition

                station.SetPlayerPos();

                // Outtransition
            }
        }
    }
}
