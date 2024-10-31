using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NoteInteractions : MonoBehaviour
{
    [SerializeField] private Collider Collider;
    [SerializeField] private NotePlacement NotePlace;

    private PlayerControl PC;

    private void Awake()
    {
        PC = new PlayerControl();
        PC.Enable();
        Collider = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        if (NotePlace == NotePlacement.Left)
        {
            PC.Rhythm.LeftNote.performed += ButtonPressed;
            //PC.Rhythm.LeftNote.canceled += ButtonReleased;
        }
        else if (NotePlace == NotePlacement.Middle)
        {
            PC.Rhythm.MiddleNote.performed += ButtonPressed;
            //PC.Rhythm.MiddleNote.canceled += ButtonReleased;
        }
        else if (NotePlace == NotePlacement.Right)
        {
            PC.Rhythm.RightNote.performed += ButtonPressed;
            //PC.Rhythm.RightNote.canceled += ButtonReleased;
        }
    }
    private void OnDisable()
    {
        if (NotePlace == NotePlacement.Left)
        {
            PC.Rhythm.LeftNote.performed -= ButtonPressed;
            //PC.Rhythm.LeftNote.canceled -= ButtonReleased;
        }
        else if (NotePlace == NotePlacement.Middle)
        {
            PC.Rhythm.MiddleNote.performed -= ButtonPressed;
            //PC.Rhythm.MiddleNote.canceled -= ButtonReleased;
        }
        else if (NotePlace == NotePlacement.Right)
        {
            PC.Rhythm.RightNote.performed -= ButtonPressed;
            //PC.Rhythm.RightNote.canceled -= ButtonReleased;
        }
    }

    private void ButtonPressed(InputAction.CallbackContext Pressed)
    {
        StartCoroutine(EnableCollider());
    }

    /*private void ButtonReleased(InputAction.CallbackContext Released)
    {
        //Visual Something or another or held notes
    }*/

    private IEnumerator EnableCollider()
    {
        Collider.enabled = true;
        yield return new WaitForSeconds(0.1f);
        Collider.enabled = false;
    }
}

[System.Serializable]
public enum NotePlacement
{
    Left,
    Middle,
    Right
};
