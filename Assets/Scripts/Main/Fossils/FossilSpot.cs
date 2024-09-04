using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FossilSpot : Interactable
{
    [SerializeField] private GameObject FossilSprite;

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
            Destroy(gameObject);
        }
    }
}
