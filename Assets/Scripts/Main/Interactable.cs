using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactable : MonoBehaviour
{
    public bool PlayerInRange;
    public PlayerControls PC;

    public virtual void Awake()
    {
        PC = new PlayerControls();
        PC.Enable();
    }
    public virtual void OnEnable()
    {
        PC.Player.Interact.performed += Interact;
    }
    public virtual void OnDisable()
    {
        PC.Player.Interact.performed -= Interact;
    }
    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInRange = true;
        }
    }

    public virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInRange = false;
        }
    }

    public virtual void Interact(InputAction.CallbackContext Shift)
    {
        if (PlayerInRange)
        {
            //Do Thing
        }
    }
}
