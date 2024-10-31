using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System.Collections.Generic;

public class QuestTracker : MonoBehaviour
{
    [SerializeField] private GameObject QuestPrefabUI; // Prefab for each quest's UI display
    [SerializeField] private Transform QuestUIContainer; // Container for the quest UI elements

    private QuestManager QM;
    private Dictionary<string, GameObject> activeQuests = new Dictionary<string, GameObject>(); // Track active quests by ID

    private void OnEnable()
    {
        QM = FindFirstObjectByType<QuestManager>();
        QuestEvents.OnStartQuest += MakeQuestDisplay;
        QuestEvents.OnAdvanceQuest += UpdateQuestDisplay;
        QuestEvents.OnQuestStepStateChange += UpdateQuestDisplayR;
        QuestEvents.OnFinishQuest += DestroyQuestDisplay;
        QuestEvents.OnQuestStepInitialized += OnQuestStepInitialized;


    }

    private void OnDisable()
    {
        QuestEvents.OnStartQuest -= MakeQuestDisplay;
        QuestEvents.OnAdvanceQuest -= UpdateQuestDisplay;
        QuestEvents.OnQuestStepStateChange -= UpdateQuestDisplayR;
        QuestEvents.OnFinishQuest -= DestroyQuestDisplay;
        QuestEvents.OnQuestStepInitialized -= OnQuestStepInitialized;
    }

    private void MakeQuestDisplay(string ID)
    {
        // Check if the quest is already displayed
        if (activeQuests.ContainsKey(ID))
            return;

        // Instantiate and configure the quest UI display
        GameObject QP = Instantiate(QuestPrefabUI, QuestUIContainer);
        QP.name = ID;

        // Fetch the text components for the quest display
        TextMeshProUGUI Name = QP.transform.Find("QuestName").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI Step = QP.transform.Find("QuestStep").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI Requirements = QP.transform.Find("QuestStepRequirements").GetComponent<TextMeshProUGUI>();

        // Get the quest data by ID and populate UI text
        Quest Q = QM.GetQuestByID(ID);
        if (Q != null)
        {
            Name.text = Q.info.DisplayedName;
            QuestStep currentStep = FindActiveQuestStep(ID, Q.CurrentQuestStepIndex);
            if (currentStep != null)
            {
                Debug.Log("Quest Step is :" + currentStep.name);
                Step.text = currentStep.DisplayName;
                Requirements.text = currentStep.Requirement;
            }
        }

        // Add this quest UI element to the dictionary
        activeQuests[ID] = QP;

        UpdateQuestDisplay(ID);
    }
    private void OnQuestStepInitialized(string ID, int stepIndex)
    {
        UpdateQuestDisplay(ID);
    }
    private void UpdateQuestDisplayR(string ID, int StepIndex, QuestStepState questStepState)
    {
        UpdateQuestDisplay(ID);
    }
    private void UpdateQuestDisplay(string ID)
    {
        // Check if the quest display exists in the dictionary
        if (!activeQuests.ContainsKey(ID))
            return;

        // Retrieve the existing quest display
        GameObject QP = activeQuests[ID];

        // Update the quest step and requirements in the UI
        TextMeshProUGUI Step = QP.transform.Find("QuestStep").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI Requirements = QP.transform.Find("QuestStepRequirements").GetComponent<TextMeshProUGUI>();

        // Get the quest data by ID and update UI text
        Quest Q = QM.GetQuestByID(ID);
        if (Q != null && Q.CurrentStepExists())
        {
            QuestStep currentStep = FindActiveQuestStep(ID, Q.CurrentQuestStepIndex);
            if (currentStep != null)
            {
                Step.text = currentStep.DisplayName;
                Requirements.text = currentStep.Requirement;
            }
        }
    }
    private void DestroyQuestDisplay(string ID)
    {
        // Check if the quest display exists in the dictionary
        if (!activeQuests.ContainsKey(ID))
            return;

        // Remove and destroy the quest UI element
        GameObject QP = activeQuests[ID];
        Destroy(QP);
        activeQuests.Remove(ID);
    }

    private QuestStep FindActiveQuestStep(string questID, int stepIndex)
    {
        // Search for all active QuestStep instances in the scene
        QuestStep[] questSteps = FindObjectsOfType<QuestStep>();

        foreach (QuestStep step in questSteps)
        {
            if (step.ID == questID && step.StepIndex == stepIndex)  // Assuming QuestStep has ID and StepIndex properties
            {
                return step;
            }
        }
        return null;
    }

}

