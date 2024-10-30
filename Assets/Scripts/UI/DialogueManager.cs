using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;

public class DialogueManager : MonoBehaviour
{
    public static Action PauseGame = delegate { };

    public TextMeshProUGUI actorName;
    public TextMeshProUGUI messageText;
    public RectTransform backgroundBox;

    private List<Message> currentMessages = new List<Message>();
    private int activeMessage = 0;
    public static bool isActive = false;
    public static DialogueManager instance;



    private void Start()
    {
        instance = this;
        transform.GetChild(0).gameObject.SetActive(false);
    }



    //Method called by Dialogue trigger to begin conversation
    public void OpenDialogue(List<Message> messages)
    {
        PauseGame();
        transform.GetChild(0).gameObject.SetActive(true);
        currentMessages = messages;
        activeMessage = 0;
        isActive = true;

        DisplayMessage();
        Debug.Log("Started conversation! loaded messages: " + messages.Count);
    }



    //Updates UI elements with conversation variables
    void DisplayMessage()
    {
        Message messageToDisplay = currentMessages[activeMessage];
        messageText.text = messageToDisplay.message;
        actorName.text = messageToDisplay.actorName;
    }



    public void NextMessage()
    {
        activeMessage++;
        if(activeMessage < currentMessages.Count)
        {
            DisplayMessage();
        }
        else
        {
            PauseGame();
            Debug.Log("Conversation Ended!");
            isActive = false;
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
