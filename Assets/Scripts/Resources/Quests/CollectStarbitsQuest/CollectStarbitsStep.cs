using UnityEngine;

public class CollectStarbitsStep : QuestStep
{
    [SerializeField] private int StarbitsToCollect;

    private int StarbitsCollected;

    private void OnEnable()
    {
        CurrencyPickUp.StarbitsPickedUp += UpdateStarbitCount;
    }
    private void OnDisable()
    {
        CurrencyPickUp.StarbitsPickedUp -= UpdateStarbitCount;
    }

    private void UpdateStarbitCount(int StarBits)
    {
        if(StarbitsCollected < StarbitsToCollect)
        {
            StarbitsCollected += StarBits;
            UpdateStep();
        }

        if(StarbitsCollected >= StarbitsToCollect)
        {
            FinishQuestStep();
        }
    }

    private void UpdateStep()
    {
        string state = StarbitsCollected.ToString();
        ChangeQuestStepState(state);
    }
}
