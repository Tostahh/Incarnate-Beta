using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueTrigger : Interactable
{
    public Message[] messages;
    public Actor[] actors;



    public override void Awake()
    {
        base.Awake();
        InteractableUI = GameObject.Find("InteractableUI");
    }



    public override void Interact(InputAction.CallbackContext Shift)
    {
        if (PlayerInRange)
        {
            DialogueManager dManager = FindAnyObjectByType<DialogueManager>();
            if (!DialogueManager.isActive)
            {
                dManager.OpenDialogue(messages, actors);
            }
            else
            {
                dManager.NextMessage();
            }
        }
    }
}



[System.Serializable]
public class Message
{
    public int actorID;
    public string message;
}




[System.Serializable]
public class Actor
{
    public string name;
}