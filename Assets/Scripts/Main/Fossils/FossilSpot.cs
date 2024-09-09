using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FossilSpot : Interactable
{
    [SerializeField] private GameObject FossilSprite;

    public bool Spotted;

    public override void Interact(InputAction.CallbackContext Shift)
    {
        if(PlayerInRange)
        {
            FossilSprite.SetActive(true);
        }
    }

    private void Update()
    {
        if(FossilSprite == null)
        {
            if(InstanceUI)
            {
                Destroy(InstanceUI);
            }
            Destroy(gameObject);
        }
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInRange = true;
            if (Spotted)
            {
                InstanceUI = Instantiate(InteractableUI, transform.position, Camera.main.transform.rotation);
            }
        }
    }
}
