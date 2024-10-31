using UnityEngine;

public class Quest
{
    public QuestInfoSO info;
    public QuestState state;
    public int CurrentQuestStepIndex;
    private QuestStepState[] QueststepsStates;

    public Quest(QuestInfoSO QuestInfo)
    {
        this.info = QuestInfo;
        this.state = QuestState.REQUIREMENTS_NOT_MET;
        this.CurrentQuestStepIndex = 0;
        this.QueststepsStates = new QuestStepState[info.QuestStepPrefabs.Length];
        for (int i = 0; i < QueststepsStates.Length; i++)
        {
            QueststepsStates[i] = new QuestStepState();
        }
    }

    public Quest(QuestInfoSO info, QuestState state, int currentQuestStepIndex, QuestStepState[] queststepsStates)
    {
        this.info = info;
        this.state = state;
        CurrentQuestStepIndex = currentQuestStepIndex;
        QueststepsStates = queststepsStates;

        if(this.QueststepsStates.Length != this.info.QuestStepPrefabs.Length)
        {
            Debug.LogWarning("Reset Save Data Project has been Updated");
        }
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
        Debug.Log("making Quest Step");

        GameObject QuestStepPrefab = GetCurrentQuestStepPrefab();
        if(QuestStepPrefab != null)
        {
            QuestStep Step = Object.Instantiate(QuestStepPrefab, _parentTransform).GetComponent<QuestStep>();
            Step.StartQuestStep(info.ID, CurrentQuestStepIndex, QueststepsStates[CurrentQuestStepIndex].state);
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

    public void StoreQuestStepState(QuestStepState questStepState,int Index)
    {
        if(Index < QueststepsStates.Length)
        {
            QueststepsStates[Index].state = questStepState.state;
        }
        else
        {
            Debug.Log("Can not access quest step state data, index is out of range" + info.ID);
        }
    }

    public QuestData GetData()
    {
        return new QuestData(state, CurrentQuestStepIndex, QueststepsStates);
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
