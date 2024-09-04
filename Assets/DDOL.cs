using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DDOL : MonoBehaviour
{
    public static DDOL MainDDOL;
    private void Awake()
    {
        if (MainDDOL == null)
        {
            MainDDOL = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
