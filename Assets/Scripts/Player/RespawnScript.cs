using UnityEngine;
using System;
public class RespawnScript : MonoBehaviour
{
    public static Action RespawnCall = delegate { };
    private void OnEnable()
    {
        Heath.PlayerDefeated += Respawn;
        DeathZone.PlayerReset += Respawn;
    }
    private void OnDisable()
    {
        Heath.PlayerDefeated -= Respawn;
        DeathZone.PlayerReset -= Respawn;
    }
    public void Respawn()
    {
        RespawnCall();
    }
}
