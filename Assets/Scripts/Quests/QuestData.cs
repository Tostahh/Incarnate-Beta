using UnityEngine;

[System.Serializable]
public class QuestData
{
    public QuestState state;

    public int Index;
    public QuestStepState[] questStepStates;

    public QuestData(QuestState state, int index, QuestStepState[] questStepState)
    {
        this.state = state;
        this.Index = index;
        this.questStepStates = questStepState;
    }
}
