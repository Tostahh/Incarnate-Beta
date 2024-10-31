using UnityEngine;

public class KillEnemiesStep : QuestStep
{
    [SerializeField] private int EnemiesToKill;

    private int EnemiesKilled;
    private void OnEnable()
    {
        this.DisplayName = "Kill Enemies";
        this.Requirement = "Left to Get: " + (EnemiesToKill - EnemiesKilled).ToString();
        Heath.EnemyDefeated += UpdateKills;
    }
    private void OnDisable()
    {
        Heath.EnemyDefeated -= UpdateKills;
    }
    private void UpdateKills()
    {
        if (EnemiesKilled < EnemiesToKill)
        {
            EnemiesKilled++;
            Requirement = "Left to Get: " + (EnemiesToKill - EnemiesKilled).ToString();
            UpdateStep();
        }

        if (EnemiesKilled >= EnemiesToKill)
        {
            FinishQuestStep();
        }
    }
    private void UpdateStep()
    {
        string state = EnemiesKilled.ToString();
        ChangeQuestStepState(state);
    }

    protected override void SetQuestStepState(string state)
    {
        this.EnemiesKilled = System.Int32.Parse(state);
        UpdateStep();
    }
}
