using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactable : MonoBehaviour
{
    public bool PlayerInRange;
    public PlayerControl PC;

    public GameObject InteractableUI;

    public GameObject InstanceUI;
    public virtual void Awake()
    {
        PC = new PlayerControl();
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
            InstanceUI = Instantiate(InteractableUI, transform.position, Camera.main.transform.rotation);
        }
    }

    public virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInRange = false;
            Destroy(InstanceUI);
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
