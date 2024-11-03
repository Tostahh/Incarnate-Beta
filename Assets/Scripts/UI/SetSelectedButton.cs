using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SetSelectedButton : MonoBehaviour
{
    public static Action PauseGame = delegate { };

    EventSystem myEventSystem;

    [SerializeField] private GameObject SelectedButton;
    [SerializeField] private bool DontPause;

    private void Awake()
    {
        myEventSystem = FindObjectOfType<EventSystem>();
        myEventSystem.SetSelectedGameObject(null);
        myEventSystem.SetSelectedGameObject(SelectedButton);
    }

    private void OnEnable()
    {
        myEventSystem = FindObjectOfType<EventSystem>();
        myEventSystem.SetSelectedGameObject(null);
        myEventSystem.SetSelectedGameObject(SelectedButton);
        if (!DontPause)
        {
            PauseGame();
        }
    }

    private void OnDisable()
    {
        myEventSystem.SetSelectedGameObject(null);
        if (!DontPause)
        {
            PauseGame();
        }
    }
}
