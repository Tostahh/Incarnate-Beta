using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Conductor : MonoBehaviour
{
    public static Action<float> SpawnNote = delegate { };

    [Header("ConductorInformation")]
    public bool SongPlaying;
    public bool SongDone;
    public Transform MiddleRow;
    public Transform LeftRow;
    public Transform RightRow;

    [Header("SongInformation")]
    public float SecPerBeat;
    public float SongPosInSec;
    public float SongPosInBeat;
    public float SongDSPStartTime;
    public float BeatsShownInAdvance;

    private PlayerControls PC;
    private AudioSource MusicSource;
    private Song CurrentSong;
    private float SongBPM;
    private float FirstBeatOffset;
    private float SongLength;

    private void Awake()
    {
        Instantiate(FindObjectOfType<Fossil>().SongPrefab);
        PC = new PlayerControls();
        PC.Enable();
        MusicSource = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        PC.Rhythm.StartSong.performed += StartSong;
        Grader.WinFossil += Win;
        Grader.LoseFossil += LoseSong;
    }
    private void OnDisable()
    {
        PC.Rhythm.StartSong.performed -= StartSong;
        Grader.WinFossil -= Win;
        Grader.LoseFossil -= LoseSong;
    }
    private void Start()
    {
        CurrentSong = FindObjectOfType<Song>();
        SongBPM = CurrentSong.SongBPM;
        FirstBeatOffset = CurrentSong.FirstBeatOffset;
        BeatsShownInAdvance = CurrentSong.BeatsShownInAdvance;
        MusicSource.clip = CurrentSong.SongMusic;
        SongLength = MusicSource.clip.length;
    }
    private void StartSong(InputAction.CallbackContext Pressed)
    {
        if (!SongPlaying)
        {
            SecPerBeat = 60f / SongBPM;
            SongDSPStartTime = (float)AudioSettings.dspTime;
            MusicSource.Play();
            SongPlaying = true;
        }
    }
    private void EndSong()
    {
        MusicSource.Stop();
        SongPlaying = false;
        SongDone = true;
    }

    private void Win()
    {
        FindObjectOfType<Fossil>().SongCompleted();
        FindObjectOfType<SceneManagment>().ChangeScene("SpaceHQ");
    }
    private void Update()
    {
        if (SongPlaying)
        {
            SongPosInSec = ((float)AudioSettings.dspTime - SongDSPStartTime - FirstBeatOffset);
            SongPosInBeat = SongPosInSec / SecPerBeat;

            SpawnNote(SongPosInBeat + BeatsShownInAdvance);
            if(SongPosInSec >= SongLength)
            {
                EndSong();
            }
        }
    }

    private void LoseSong()
    {
        MusicSource.Stop();
        SongPlaying = false;
        FindObjectOfType<Fossil>().SongLost();
        FindObjectOfType<SceneManagment>().ChangeScene("SampleScene");
    }
}
