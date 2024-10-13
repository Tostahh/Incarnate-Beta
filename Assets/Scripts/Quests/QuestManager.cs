using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private Dictionary<string, Quest> QuestMap;

    private void Awake()
    {
        QuestMap =  CreateQuestMap();

        Quest quest = GetQuestByID("CollectStarbits");
        Debug.Log(quest.info.name);
        Debug.Log(quest.info.StarBits);
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
