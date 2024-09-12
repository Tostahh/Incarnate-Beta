using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu main;
    public bool Paused;

    [SerializeField] private GameObject PauseMenuUI;

    private PlayerControls PC;
    private void Awake()
    {
        main = this;
        PC = new PlayerControls();
        PC.Enable();
    }
    private void OnEnable()
    {
        PC.Player.Pause.performed += PauseStateGame;
        SetSelectedButton.PauseGame += PauseWithOutUI;
        SceneManagment.NewSceneLoaded += NewScene;
    }
    private void OnDisable()
    {
        PC.Player.Pause.performed -= PauseStateGame;
        SetSelectedButton.PauseGame -= PauseWithOutUI;
        SceneManagment.NewSceneLoaded -= NewScene;
    }
    public void PauseStateGame(InputAction.CallbackContext jump)
    {
        if(Paused)
        {
            Paused = false;
            PauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
        }
        else
        {
            Paused = true;
            PauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void PauseWithOutUI()
    {
        if (Paused)
        {
            Paused = false;
            Time.timeScale = 1f;
        }
        else
        {
            Paused = true;
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
    }
}
