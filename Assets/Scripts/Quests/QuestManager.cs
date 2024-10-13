using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private Dictionary<string, Quest> QuestMap;
    public QuestEvents QE;

    private void Awake()
    {
        QuestMap =  CreateQuestMap();
        QE = GetComponent<QuestEvents>();
    }

    private void OnEnable()
    {
        QuestEvents.OnStartQuest += StartQuest;
        QuestEvents.OnAdvanceQuest += AdvanceQuest;
        QuestEvents.OnFinishQuest += FinishQuest;
    }
    private void OnDisable()
    {
        QuestEvents.OnStartQuest -= StartQuest;
        QuestEvents.OnAdvanceQuest -= AdvanceQuest;
        QuestEvents.OnFinishQuest -= FinishQuest;
    }

    private void Start()
    {
        foreach(Quest quest in QuestMap.Values)
        {
            QE.QuestStateChange(quest);
        }
    }

    private void ChangeQuestState(string ID, QuestState QS)
    {
        Quest quest = GetQuestByID(ID);
        quest.state = QS;
        QE.QuestStateChange(quest);
    }
    private void StartQuest(string ID)
    {

    }
    private void AdvanceQuest(string ID)
    {

    }
    private void FinishQuest(string ID)
    {

    }
    private Dictionary<string, Quest> CreateQuestMap()
    {
        QuestInfoSO[] AllQuests = Resources.LoadAll<QuestInfoSO>("Quests");

        Dictionary<string, Quest> IDToQuestMap = new Dictionary<string, Quest>();
        foreach(QuestInfoSO quest in AllQuests)
        {
            if(IDToQuestMap.ContainsKey(quest.ID))
            {
                Debug.Log("Duplicate ID Found: " + quest.ID);
            }
            IDToQuestMap.Add(quest.ID, new Quest(quest));
        }

        return IDToQuestMap;
    }

    private Quest GetQuestByID(string QuestID)
    {
        Quest quest = QuestMap[QuestID];
        if(quest == null)
        {
            Debug.Log("ID not Found: " + QuestID);
        }
        return quest;
    }
}
