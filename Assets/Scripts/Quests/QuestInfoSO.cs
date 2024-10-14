using UnityEngine;

[CreateAssetMenu(fileName = "QuestInfoSO", menuName = "Scriptable Objects/QuestInfoSO")]
public class QuestInfoSO : ScriptableObject
{
    [SerializeField] public string ID { get; private set; }

    [Header("Genral")]
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
    private void OnValidate()
    {
        #if UNITY_EDITOR
        ID = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
        #endif
    }
}
