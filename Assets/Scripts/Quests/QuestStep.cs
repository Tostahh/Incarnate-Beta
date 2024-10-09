using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    private bool IsFinished = false;

    protected void FinishQuestStep()
    {
        if(!IsFinished)
        {
            IsFinished = true;

            //send out advance quest event

            Destroy(this.gameObject);
        }
    }
}
