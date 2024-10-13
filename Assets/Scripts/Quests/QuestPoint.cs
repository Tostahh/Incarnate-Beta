using UnityEngine;
using UnityEngine.InputSystem;

public class QuestPoint : Interactable
{
    [Header("Quest")]
    [SerializeField] private QuestInfoSO Info;

    [Header("Settings")]
    [SerializeField] private bool StartPoint = true;
    [SerializeField] private bool EndPoint = true;

    private string ID;
    private QuestState CurrentQuestState;
    private QuestManager QM;

    public override void OnEnable()
    {
        base.OnEnable();
        QuestEvents.OnQuestStateChange += QuestStateChange;
        QM = FindFirstObjectByType<QuestManager>();
    }
    public override void OnDisable()
    {
        base.OnDisable();
        QuestEvents.OnQuestStateChange -= QuestStateChange;
    }

    private void QuestStateChange(Quest quest)
    {
        if(quest.info.ID == ID)
        {
            CurrentQuestState = quest.state;

            // Add Visual Update When this is called
        }
    }

    public override void Interact(InputAction.CallbackContext Shift)
    {
        if (PlayerInRange)
        {
            if(CurrentQuestState == QuestState.CAN_START && StartPoint)
            {
                QM.QE.StartQuest(ID);
            }
            else if(CurrentQuestState == QuestState.CAN_FINSIH && EndPoint)
            {
                QM.QE.FinishQuest(ID);
            }
        }
    }
}
