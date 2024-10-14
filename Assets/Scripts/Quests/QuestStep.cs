using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    private bool IsFinished = false;
    private string ID;
    private int StepIndex;
    private QuestManager QM;

    public void StartQuestStep(string id, int Index)
    {
        QM = FindFirstObjectByType<QuestManager>();
        ID = id;
        StepIndex = Index;
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
}
