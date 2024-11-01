using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class QuestManager : MonoBehaviour
{
    public Dictionary<string, Quest> QuestMap;
    public QuestEvents QE;

    private bool Axe;
    private bool Sling;
    private int SpearDmg;
    private int AxeDmg;
    private int SlingDmg;

    private int InventorySlots;

    private bool Lantern;
    private bool Mount;
    private bool DoubleJump;

    private void Awake()
    {
        QuestMap = CreateQuestMap();
        QE = GetComponent<QuestEvents>();
    }

    private void OnEnable()
    {
        SceneManagment.NewSceneLoaded += OnNewScene;

        QuestEvents.OnStartQuest += StartQuest;
        QuestEvents.OnAdvanceQuest += AdvanceQuest;
        QuestEvents.OnFinishQuest += FinishQuest;

        QuestEvents.OnQuestStepStateChange += QuestStepChange;

        MarketStation.Upgrade += PlayerUpgrade;
        // Set up another event for playerAdvancement
    }
    private void OnDisable()
    {
        SceneManagment.NewSceneLoaded -= OnNewScene;

        QuestEvents.OnStartQuest -= StartQuest;
        QuestEvents.OnAdvanceQuest -= AdvanceQuest;
        QuestEvents.OnFinishQuest -= FinishQuest;

        QuestEvents.OnQuestStepStateChange -= QuestStepChange;

        MarketStation.Upgrade -= PlayerUpgrade;
    }
    private void Start()
    {
        foreach(Quest quest in QuestMap.Values)
        {
            if(quest.state == QuestState.IN_PROGRESS)
            {
                quest.MakeCurrentQuestStep(this.transform);
            }
            QE.QuestStateChange(quest);
        }

        FindFirstObjectByType<QuestTracker>().LoadQuestDisplay();
    }

    private void OnNewScene()
    {
        foreach(Quest quest in QuestMap.Values)
        {
            ChangeQuestState(quest.info.ID, quest.state);
        }
    }

    private void Update()
    {
        if (QuestMap.Values.Count > 0)
        {
            foreach (Quest quest in QuestMap.Values)
            {
;               if (quest.state == QuestState.REQUIREMENTS_NOT_MET && CheckIfRequirmentsMet(quest))
                {
                    ChangeQuestState(quest.info.ID, QuestState.CAN_START);
                }
            }
        }
    }

    private void ChangeQuestState(string ID, QuestState QS)
    {
        Quest quest = GetQuestByID(ID);
        quest.state = QS;
        QE.QuestStateChange(quest);
    }
    private bool CheckIfRequirmentsMet(Quest quest)
    {
        Debug.Log(quest.info.name + " " + quest.state);

        bool RequirementsMet = true;

        if (Axe != quest.info.Axe)
        {
            RequirementsMet = false;
        }
        if (Sling != quest.info.Sling)
        {
            RequirementsMet = false;
        }
        if (SpearDmg < quest.info.SpearDmg)
        {
            RequirementsMet = false;
        }
        if (AxeDmg < quest.info.AxeDmg)
        {
            RequirementsMet = false;
        }
        if (SlingDmg < quest.info.SlingDmg)
        {
            RequirementsMet = false;
        }

        if (InventorySlots < quest.info.InventorySlots)
        {
            RequirementsMet = false;
        }

        if (Lantern != quest.info.Lantern)
        {
            RequirementsMet = false;
        }
        if (Mount != quest.info.Mount)
        {
            RequirementsMet = false;
        }
        if (DoubleJump != quest.info.DoubleJump)
        {
            RequirementsMet = false;
        }

        foreach (QuestInfoSO PreRec in quest.info.QuestPreRecs)
        {
            if(GetQuestByID(PreRec.ID).state != QuestState.FINSIED)
            {
                RequirementsMet = false;
                break;
            }
        }

        return RequirementsMet;
    }
    private void StartQuest(string ID)
    {
        Quest quest = GetQuestByID(ID);
        quest.MakeCurrentQuestStep(transform);
        ChangeQuestState(quest.info.ID, QuestState.IN_PROGRESS);

        Debug.Log(quest.info.ID + " " + quest.info.name + " " + quest.state);
    }
    private void AdvanceQuest(string ID)
    {
        Quest quest = GetQuestByID(ID);

        quest.MoveToNextStep();

        if(quest.CurrentStepExists())
        {
            quest.MakeCurrentQuestStep(this.transform);
        }
        else
        {
            ChangeQuestState(quest.info.ID, QuestState.CAN_FINSIH);
        }

        Debug.Log(quest.info.ID + " " + quest.info.name + " " + quest.state);
    }
    private void FinishQuest(string ID)
    {
        Quest quest = GetQuestByID(ID);
        ClaimRewards(quest);
        ChangeQuestState(quest.info.ID, QuestState.FINSIED);

        Debug.Log(quest.info.ID + " " + quest.info.name + " " + quest.state);
    }
    private void ClaimRewards(Quest quest)
    {
        Inventory I = FindFirstObjectByType<Inventory>();

        if (quest.info.StarBits > 0)
        {
            I.Starbits += quest.info.StarBits;
            Debug.Log("Player rewarded with " + quest.info.StarBits + " StarBits.");
        }
        if (quest.info.RootStem > 0)
        {
            I.RootStem += quest.info.RootStem;
            Debug.Log("Player rewarded with " + quest.info.RootStem + " RootStem.");
        }
        if (quest.info.DeepfrostOre > 0)
        {
            I.DeepfrostOre += quest.info.DeepfrostOre;
            Debug.Log("Player rewarded with " + quest.info.DeepfrostOre + " DeepfrostOre.");
        }
        if (quest.info.DarkDisasterKey > 0)
        {
            I.DarkDisasterKey += quest.info.DarkDisasterKey;
            Debug.Log("Player rewarded with " + quest.info.DarkDisasterKey + " Dark Disaster Key.");
        }

        if (quest.info.Fossils.Length > 0)
        {
            foreach (GameObject Fossil in quest.info.Fossils)
            {
                if (I.InvetoryIsFull())
                {
                    FindObjectOfType<InventoryUIManager>().InventoryPopUp(Fossil);
                }
                else
                {
                    for (int i = 0; i < I.SlotIsFull.Length; i++)
                    {
                        if (I.SlotIsFull[i] == false)
                        {
                            I.SlotIsFull[i] = true;
                            I.Slots[i] = Instantiate(Fossil);
                            I.Slots[i].SetActive(false);
                            break;
                        }
                    }
                }
            }
        }
    }

    private void QuestStepChange(string ID, int Index, QuestStepState state)
    {
        Quest quest = GetQuestByID(ID);
        quest.StoreQuestStepState(state, Index);
        ChangeQuestState(ID, quest.state);
    }
    private Dictionary<string, Quest> CreateQuestMap()
    {
        QuestInfoSO[] AllQuests = Resources.LoadAll<QuestInfoSO>("Quests");
        Dictionary<string, Quest> IDToQuestMap = new Dictionary<string, Quest>();

        if (AllQuests == null || AllQuests.Length == 0)
        {
            foreach (QuestInfoSO quest in AllQuests)
            {
                quest.SetID();  // Ensure ID is set

                // Further processing of quests
                Debug.Log($"Quest {quest.ID} has been loaded.");
            }

            Debug.LogWarning("No quests found in the 'Quests' folder. Ensure assets are correctly placed.");
            return IDToQuestMap;
        }



        foreach (QuestInfoSO quest in AllQuests)
        {
            if (quest == null)
            {
                Debug.LogError("QuestInfoSO is null in the Quests folder. Please check all assets.");
                continue;
            }

            if (string.IsNullOrEmpty(quest.ID))
            {
                Debug.LogError($"QuestInfoSO '{quest.name}' has an empty ID.");
                continue;
            }

            if (IDToQuestMap.ContainsKey(quest.ID))
            {
                Debug.LogError($"Duplicate Quest ID '{quest.ID}' found for '{quest.name}'.");
                continue;
            }

            Quest loadedQuest = LoadQuests(quest);
            if (loadedQuest != null)
            {
                IDToQuestMap.Add(quest.ID, loadedQuest);
            }
        }

        return IDToQuestMap;
    }



    public Quest GetQuestByID(string QuestID)
    {
        Quest quest = QuestMap[QuestID];
        if(quest == null)
        {
            Debug.Log("ID not Found: " + QuestID);
        }
        return quest;
    }

    //UpdateInfoEvents
    private void PlayerUpgrade(PlayerStats PS, Inventory I)
    {
        Axe = PS.Axe;
        Sling = PS.Sling;
        SpearDmg = PS.SpearDmg;
        AxeDmg = PS.AxeDmg;
        SlingDmg = PS.SlingDmg;

        InventorySlots = I.FossilSlots;

        Lantern = PS.Lantern;
        Mount = PS.Mount;
        DoubleJump = PS.DoubleJump;
    }
    public void ClearQuestSteps()
    {
        QuestStep[] questSteps = FindObjectsOfType<QuestStep>();

        foreach (QuestStep step in questSteps)
        {
            Destroy(step.gameObject);
        }
    }

    public void SaveAll()
    {
        foreach(Quest quest in QuestMap.Values)
        {
            SaveQuests(quest);
        }
    }
    public void SaveQuests(Quest quest)
    {
        try
        {
            QuestData data = quest.GetData();

            string DataToJson = JsonUtility.ToJson(data);

            FindFirstObjectByType<SaveLoadJson>().GiveSaveData().QuestSaveStates[quest.info.ID] = DataToJson;

            Debug.Log(DataToJson);
        }
        catch (System.Exception E)
        {
            Debug.Log("Issue Saving Quest :" + quest.info.name);
        }
    }

    public Quest LoadQuests(QuestInfoSO info)
    {
        // Check for a null QuestInfoSO before proceeding
        if (info == null || string.IsNullOrEmpty(info.ID))
        {
            Debug.LogError("QuestInfoSO is null or has an empty ID. Ensure all Quest assets have valid IDs.");
            return null;  // Early exit if the quest info is invalid
        }

        Quest quest = null;
        try
        {
            // Check if the quest data exists in QuestSaveStates
            var saveData = FindFirstObjectByType<SaveLoadJson>().GiveSaveData().QuestSaveStates;
            if (saveData.ContainsKey(info.ID))
            {
                // Retrieve JSON data for the quest and deserialize it
                string data = saveData[info.ID];
                QuestData questData = JsonUtility.FromJson<QuestData>(data);

                // Create a Quest instance with the loaded data
                quest = new Quest(info, questData.state, questData.Index, questData.questStepStates);
            }
            else
            {
                // If the quest has no saved data, initialize it as a new quest
                quest = new Quest(info);
            }
        }
        catch (System.Exception E)
        {
            Debug.LogError($"Issue loading save for quest '{info.name}': {E.Message}");
        }

        return quest;
    }

}
