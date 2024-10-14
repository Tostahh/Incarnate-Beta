using UnityEditor;
using UnityEngine;

public class QuestIcon : MonoBehaviour
{
    [Header("Icons")]
    [SerializeField] private GameObject NotReady;
    [SerializeField] private GameObject CanStart;
    [SerializeField] private GameObject InProgress;
    [SerializeField] private GameObject CanTurnIn;

    public void SetState(QuestState newState, bool StartPoint, bool EndPoint)
    {
        Debug.Log("SettingStateIcon");


        NotReady.SetActive(false);
        CanStart.SetActive(false);
        InProgress.SetActive(false);
        CanTurnIn.SetActive(false);

        switch(newState)
        {
            case QuestState.REQUIREMENTS_NOT_MET:
                if(StartPoint)
                {
                    NotReady.SetActive(true);
                }
                break;
            case QuestState.CAN_START:
                if (StartPoint)
                {
                    CanStart.SetActive(true);
                }
                break;
            case QuestState.IN_PROGRESS:
                if (EndPoint)
                {
                    InProgress.SetActive(true);
                }
                break;
            case QuestState.CAN_FINSIH:
                if (EndPoint)
                {
                    CanTurnIn.SetActive(true);
                }
                break;
            case QuestState.FINSIED:
                break;
            default:
                Debug.Log("Non Valid QuestState");
                break;
        }
    }
}
