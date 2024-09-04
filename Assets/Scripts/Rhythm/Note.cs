using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Note : MonoBehaviour
{
    public static Action NoteHit = delegate { };
    public static Action NoteMiss = delegate { };
    public static Action NotePlayed = delegate { };

    public bool Hit;
    public bool Miss;

    private float BeatOfNote;
    private Transform StartPoint;
    private Transform HitPoint;
    private Conductor Conductor;
    private void Awake()
    {
        Conductor = FindObjectOfType<Conductor>();
    }
    private void Start()
    {
        NotePlayed();
    }
    public void Set(float _BeatOfNote, Transform _StartPoint, Transform _HitPoint)
    {
        BeatOfNote = _BeatOfNote;
        StartPoint = _StartPoint;
        HitPoint = _HitPoint;
    }
    private void Update()
    {
        if (!Hit)
        {
            if (!Miss)
            {
                if (StartPoint && HitPoint)
                {
                    transform.position = Vector3.Lerp(StartPoint.position, HitPoint.position,
                    (Conductor.BeatsShownInAdvance - (BeatOfNote - Conductor.SongPosInBeat)) / Conductor.BeatsShownInAdvance);
                }
            }
        }

        if (transform.position == HitPoint.position)
        {
            if(!Hit)
            GetComponentInChildren<MeshRenderer>().material.color = Color.red;
            NoteMiss(); // Link to Bad Animation && Sound
            Miss = true;
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("NoteHit"))
        {
            GetComponentInChildren<MeshRenderer>().material.color = Color.green;
            Debug.Log("hit");
            NoteHit(); // Link to Good Animation && Sound
            Hit = true;
            Destroy(gameObject);
        }
    }
}
