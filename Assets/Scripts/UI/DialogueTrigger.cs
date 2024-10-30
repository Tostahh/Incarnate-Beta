using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueTrigger : Interactable
{
    [SerializeField] private bool triggerImmediately;
    [SerializeField] private TextAsset TextFileAsset;

    private List<Message> messages = new List<Message>();
    private string name, text;
    private int counter;



    public override void Awake()
    {
        base.Awake();
    }



    private void Start()
    {
        ReadTextFile();
    }



    public override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInRange = true;
            if (triggerImmediately)
            {
                TriggerDialogue();
            }
            else
            {
                InstanceUI = Instantiate(InteractableUI, transform.position, Camera.main.transform.rotation);
            }
        }
    }



    public override void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!triggerImmediately)
            {
                if (other.CompareTag("Player"))
                {
                    PlayerInRange = false;
                    Destroy(InstanceUI);
                }
            }
        }
    }



    public override void Interact(InputAction.CallbackContext Shift)
    {
        if (PlayerInRange)
        {
            TriggerDialogue();
        }
    }



    private void TriggerDialogue()
    {
        DialogueManager dManager = FindAnyObjectByType<DialogueManager>();
        if (!DialogueManager.isActive)
        {
            dManager.OpenDialogue(messages);
        }
        else
        {
            dManager.NextMessage();
        }
    }



    private void ReadTextFile()
    {
        string txt = TextFileAsset.text;

        string[] lines = txt.Split(System.Environment.NewLine.ToCharArray()); // Split dialogue lines by newline

        counter = 0;
        foreach (string line in lines) // for every line of dialogue
        {
            if (!string.IsNullOrEmpty(line))// ignore empty lines of dialogue
            {
                if (line.StartsWith("[")) // e.g [NAME=Michael] Hello, my name is Michael
                {
                    name = line.Substring(6, line.IndexOf(']') - 6); // special = [NAME=Michael]
                    text = line.Substring(line.IndexOf(']') + 1); // curr = Hello, ...
                    
                }
                else
                {
                    text = line;
                }



                Message m = new Message();
                m.actorName = name;
                m.message = text;
                messages.Add(m);
                counter++;
            }
        }
    }
}



[System.Serializable]
public class Message
{
    public string actorName;
    public string message;
}