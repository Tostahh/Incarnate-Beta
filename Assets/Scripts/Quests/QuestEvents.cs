using System;
using UnityEngine;

public class QuestEvents : MonoBehaviour
{
    public static Action<string> OnStartQuest = delegate { };
    public static Action<string> OnAdvanceQuest = delegate { };
    public static Action<string> OnFinishQuest = delegate { };
    public static Action<Quest> OnQuestStateChange = delegate { };

    public void StartQuest(string ID)
    {
        if(OnStartQuest != null)
        {
            OnStartQuest(ID);
        }
    }
    public void AdvanceQuest(string ID)
    {
        if (OnAdvanceQuest != null)
        {
            OnAdvanceQuest(ID);
        }
    }
    public void FinishQuest(string ID)
    {
        if (OnFinishQuest != null)
        {
            OnFinishQuest(ID);
        }
    }
    public void QuestStateChange(Quest quest)
    {
        if (OnQuestStateChange != null)
        {
            OnQuestStateChange(quest);
        }
    }
}
