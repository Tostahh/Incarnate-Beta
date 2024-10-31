using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    public string DisplayName;
    public string Requirement;

    public string ID;
    public int StepIndex;

    private bool IsFinished = false;
    private QuestManager QM;

    public void StartQuestStep(string id, int Index, string stepstate)
    {
        QM = FindFirstObjectByType<QuestManager>();
        ID = id;
        StepIndex = Index;
        if(stepstate != null && stepstate != "")
        {
            SetQuestStepState(stepstate);
        }

        QM.QE.QuestStepInitialized(ID, StepIndex);
    }
    protected void FinishQuestStep()
    {
        if(!IsFinished)
        {
            IsFinished = true;

            QM.QE.AdvanceQuest(ID);

            Destroy(this.gameObject);
        }
    }
    protected void ChangeQuestStepState(string NewState)
    {
        QM.QE.QuestStepStateChange(ID, StepIndex, new QuestStepState(NewState));
    }

    protected abstract void SetQuestStepState(string state);
}
