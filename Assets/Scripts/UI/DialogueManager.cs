using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI actorName;
    public TextMeshProUGUI messageText;
    public RectTransform backgroundBox;

    private Message[] currentMessages;
    private Actor[] currentActors;
    private int activeMessage = 0;
    public static bool isActive = false;
    public static DialogueManager instance;



    private void Start()
    {
        instance = this;
        ToggleDisplay(false);
    }



    //Method called by Dialogue trigger to begin conversation
    public void OpenDialogue(Message[] messages, Actor[] actors)
    {
        ToggleDisplay(true);
        currentMessages = messages;
        currentActors = actors;
        activeMessage = 0;
        isActive = true;

        DisplayMessage();
        Debug.Log("Started conversation! loaded messages: " + messages.Length);
    }



    //Updates UI elements with conversation variables
    void DisplayMessage()
    {
        Message messageToDisplay = currentMessages[activeMessage];
        messageText.text = messageToDisplay.message;

        Actor actorToDisplay = currentActors[messageToDisplay.actorID];
        actorName.text = actorToDisplay.name;
    }



    public void NextMessage()
    {
        activeMessage++;
        if(activeMessage < currentMessages.Length)
        {
            DisplayMessage();
        }
        else
        {
            Debug.Log("Conversation Ended!");
            isActive = false;
            ToggleDisplay(false);
        }
    }


    //Toggles all relevant UI components
    private void ToggleDisplay(bool b)
    {
        int children = transform.childCount;
        for(int i = 0; i < children; i++)
        {
            transform.GetChild(i).gameObject.SetActive(b);
        }
        GetComponent<Image>().enabled = b;
    }
}
