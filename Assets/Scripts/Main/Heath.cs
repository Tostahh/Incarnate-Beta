using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Heath : MonoBehaviour
{
    public static Action EnemyDefeated = delegate { };
    public static Action PlayerDefeated = delegate { };

    public float MaxHeath;
    public float CurrentHeath;

    [SerializeField] private Image HeathBar;

    public bool Enemy;
    public bool CometMonster;
    public bool Player;

    private void Awake()
    {
        if (!HeathBar)
        {
            HeathBar = GetComponentInChildren<Image>();
        }
        CurrentHeath = MaxHeath;
        if (HeathBar)
        {
            HeathBar.fillAmount = CurrentHeath / MaxHeath;
        }
    }

    private void Update()
    {
        if (HeathBar)
        {
            HeathBar.fillAmount = CurrentHeath / MaxHeath;
        }

        if(CurrentHeath <= 0)
        {
            Dead();
        }
    }

    public void UpdateHeath(float AmountOfChange)
    {
        CurrentHeath += AmountOfChange;
    }

    public void Dead()
    {
        if (Enemy)
        {
            Destroy(gameObject);
            EnemyDefeated();
        }

        if(Player)
        {
            PlayerDefeated();
        }
    }
}
