using UnityEngine;

[CreateAssetMenu(fileName = "QuestInfoSO", menuName = "Scriptable Objects/QuestInfoSO")]
public class QuestInfoSO : ScriptableObject
{
    [SerializeField] public string ID { get; private set; }

    [Header("Genral")]
    public string DisplayedName;

    [Header("Requirements")]
    public QuestInfoSO[] QuestPrereqs;

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
