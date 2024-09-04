using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grader : MonoBehaviour
{
    public static Action LoseFossil = delegate { };
    public static Action WinFossil = delegate { };

    [Header("NoteInformation")]
    public float HitNotes;
    public float MissedNotes;
    public float PlayedNotes;

    [Header("ProgressVisuals")]
    [SerializeField] private Image ProgressBar;
    [SerializeField] private Image MissBar;

    private float TotalNotes;
    private Song CurrentSong;
    private Conductor Conductor;

    private void Start()
    {
        Conductor = GetComponent<Conductor>();
        CurrentSong = FindObjectOfType<Song>();
        TotalNotes = CurrentSong.notes.Length;
    }
    private void OnEnable()
    {
        Note.NoteHit += HitNote;
        Note.NoteMiss += MissNote;
        Note.NotePlayed += PlayedNote;
    }
    private void OnDisable()
    {
        Note.NoteHit -= HitNote;
        Note.NoteMiss -= MissNote;
        Note.NotePlayed -= PlayedNote;
    }

    private void Update()
    {
        ProgressBar.fillAmount = HitNotes /TotalNotes;
        MissBar.fillAmount = MissedNotes / 3f;

        if(MissedNotes >= 3)
        {
            LoseFossil();
        }

        if(PlayedNotes >= TotalNotes)
        {
            if(Conductor.SongDone)
            {
                WinFossil();
            }
        }
    }

    public void HitNote()
    {
        HitNotes++;
    }
    public void MissNote()
    {
        MissedNotes++;
    }

    public void PlayedNote()
    {
        PlayedNotes++;
    }
}
