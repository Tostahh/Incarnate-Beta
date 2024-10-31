using System.ComponentModel;
using UnityEngine;

public class CollectStarbitsStep : QuestStep
{
    [SerializeField] private int StarbitsToCollect;

    private int StarbitsCollected;

    private void OnEnable()
    {
        this.DisplayName = "Collect Starbits";
        this.Requirement = "Left to Get: " + (StarbitsToCollect - StarbitsCollected).ToString();
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
            Requirement = "Left to Get: " + (StarbitsToCollect - StarbitsCollected).ToString();
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

    protected override void SetQuestStepState(string state)
    {
        this.StarbitsCollected = System.Int32.Parse(state);
        UpdateStep();
    }
}
