using UnityEngine;

public class Quest
{
    public QuestInfoSO info;
    public QuestState state;
    private int CurrentQuestStepIndex;

    public Quest(QuestInfoSO QuestInfo)
    {
        this.info = QuestInfo;
        this.state = QuestState.REQUIREMENTS_NOT_MET;
        this.CurrentQuestStepIndex = 0;
    }

    public void MoveToNextStep()
    {
        CurrentQuestStepIndex++;
    }

    public bool CurrentStepExists()
    {
        return (CurrentQuestStepIndex < info.QuestStepPrefabs.Length);
    }

    public void MakeCurrentQuestStep(Transform _parentTransform)
    {
        GameObject QuestStepPrefab = GetCurrentQuestStepPrefab();
        if(QuestStepPrefab != null)
        {
            Object.Instantiate(QuestStepPrefab, _parentTransform);
        }
    }

    private GameObject GetCurrentQuestStepPrefab()
    {
        GameObject questStep = null;
        if (CurrentQuestStepIndex < info.QuestStepPrefabs.Length)
        {
            questStep = info.QuestStepPrefabs[CurrentQuestStepIndex];
        }
        else
        {
            Debug.Log("Can not Get quest step prefab, stepIndex was out of range. Current Quest ID = " + info.ID);
        }
        return questStep;
    }
}

public enum QuestState
{
    REQUIREMENTS_NOT_MET,
    CAN_START,
    IN_PROGRESS,
    CAN_FINSIH,
    FINSIED
}
