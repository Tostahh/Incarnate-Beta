using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Song : MonoBehaviour
{
    public Vector2[] notes;
    [SerializeField] private GameObject NotePrefab;

    [Header("SongInformation")]
    public AudioClip SongMusic;
    public float SongBPM;
    public float FirstBeatOffset;
    public float BeatsShownInAdvance;

    private int nextIndex = 0;
    private Transform MiddleRow;
    private Transform LeftRow;
    private Transform RightRow;

    private void OnEnable()
    {
        Conductor.SpawnNote += SpawnNote;
    }

    private void OnDisable()
    {
        Conductor.SpawnNote -= SpawnNote;
    }
    private void Start()
    {
        MiddleRow = FindObjectOfType<Conductor>().MiddleRow;
        LeftRow = FindObjectOfType<Conductor>().LeftRow;
        RightRow = FindObjectOfType<Conductor>().RightRow;
    }
    private void SpawnNote(float Beat)
    {
        if(nextIndex < notes.Length && notes[nextIndex].x < Beat)
        {
            if(notes[nextIndex].y == 0)
            {
                Note note = Instantiate(NotePrefab, LeftRow).GetComponent<Note>();
                note.Set(notes[nextIndex].x, LeftRow, LeftRow.transform.GetChild(0));
                note.gameObject.transform.parent = null;
            }
            if(notes[nextIndex].y == 1)
            {
                Note note = Instantiate(NotePrefab, MiddleRow).GetComponent<Note>(); ;
                note.Set(notes[nextIndex].x, MiddleRow, MiddleRow.transform.GetChild(0));
                note.gameObject.transform.parent = null;
            }
            if(notes[nextIndex].y == 2)
            {
                Note note = Instantiate(NotePrefab, RightRow).GetComponent<Note>();
                note.Set(notes[nextIndex].x, RightRow, RightRow.transform.GetChild(0));
                note.gameObject.transform.parent = null;
            }
            nextIndex++;
        }
    }
}
