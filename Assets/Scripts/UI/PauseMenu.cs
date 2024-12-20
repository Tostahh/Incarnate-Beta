using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu main;
    public bool Paused;
    private bool NoUI;

    [SerializeField] private GameObject PauseMenuUI;

    private PlayerControl PC;
    private void Awake()
    {
        main = this;
        PC = new PlayerControl();
        PC.Enable();
    }
    private void OnEnable()
    {
        PC.Player.Pause.performed += PauseStateGame;
        SetSelectedButton.PauseGame += PauseWithOutUI;
        DialogueManager.PauseGame += PauseWithOutUI;
        SceneManagment.NewSceneLoaded += NewScene;
    }
    private void OnDisable()
    {
        PC.Player.Pause.performed -= PauseStateGame;
        SetSelectedButton.PauseGame -= PauseWithOutUI;
        DialogueManager.PauseGame -= PauseWithOutUI;
        SceneManagment.NewSceneLoaded -= NewScene;
    }
    private void PauseStateGame(InputAction.CallbackContext jump)
    {
        if(NoUI)
        {
            return;
        }

        if(Paused)
        {
            PauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            Paused = false;
        }
        else
        {
            PauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            Paused = true;
        }
    }

    public void PauseWithOutUI()
    {
        if (Paused)
        {
            Paused = false;
            NoUI = false;
            Time.timeScale = 1f;
        }
        else
        {
            Paused = true;
            NoUI = true;
            Time.timeScale = 0f;
        }
    }

    public void NewScene()
    {
        if (Paused)
        {
            Paused = false;
            Time.timeScale = 1f;
        }

        if(NoUI)
        {
            NoUI = false;
        }
    }
}
