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

    public GameObject StartUI;
    public GameObject LoseUI;
    public GameObject WinUI;

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

        if(StartUI)
        {
            StartUI.SetActive(true);
        }
    }
    private void OnEnable()
    {
        PC.Rhythm.StartSong.performed += StartSong;
        Grader.WinFossil += Win;
        Grader.LoseFossil += LoseSong;

        CometMonster[] CMS = FindObjectsOfType<CometMonster>();
        foreach(CometMonster CM in CMS)
        {
            CM.SetVisuals();
        }
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
    private void StartSong(InputAction.CallbackContext Pressed)
    {
        if (!SongPlaying)
        {
            if (StartUI)
            {
                StartUI.SetActive(false);
            }
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
        WinUI.SetActive(true);
        FindObjectOfType<Fossil>().SongCompleted();
    }
    private void LoseSong()
    {
        LoseUI.SetActive(true);
        MusicSource.Stop();
        SongPlaying = false;
        FindObjectOfType<Fossil>().SongLost();
    }

    public void BackToHQ()
    {
        CometMonster[] CMS = FindObjectsOfType<CometMonster>();
        foreach (CometMonster CM in CMS)
        {
            CM.SetVisuals();
        }
        FindObjectOfType<SceneManagment>().ChangeScene("SpaceHQ");
    }
}
