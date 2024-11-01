using UnityEngine;

[CreateAssetMenu(fileName = "QuestInfoSO", menuName = "Scriptable Objects/QuestInfoSO")]
public class QuestInfoSO : ScriptableObject
{
    [SerializeField] private string id;

    public string ID
    {
        get => id;
        private set => id = value;
    }

    [Header("General")]
    public string DisplayedName;

    [Header("Requirements")]
    public QuestInfoSO[] QuestPreRecs;
    public int SpearDmg;
    public int AxeDmg;
    public int SlingDmg;
    public bool Axe;
    public bool Sling;
    public bool Lantern;
    public int InventorySlots;
    public bool Mount;
    public bool DoubleJump;

    [Header("Steps")]
    public GameObject[] QuestStepPrefabs;

    [Header("Rewards")]
    public int StarBits;
    public int PearlStone;
    public int RootStem;
    public int DeepfrostOre;
    public int DarkDisasterKey;
    public GameObject[] Fossils;

    private void OnEnable()
    {
        if (string.IsNullOrEmpty(ID))
        {
            ID = name;
        }
    }

    private void OnValidate()
    {
#if UNITY_EDITOR
        if (string.IsNullOrEmpty(id))
        {
            id = name;
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
    public void SetID()
    {
        if (string.IsNullOrEmpty(ID))
        {
            ID = name;  // Set ID based on the asset's name if not already set
        }
    }
}

