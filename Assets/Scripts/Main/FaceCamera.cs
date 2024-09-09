using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    public Camera Camera;
    public Transform ObjToRotate;

    private void OnEnable()
    {
        Camera = Camera.main;
        ObjToRotate = transform;
    }
    private void Update()
    {
        ObjToRotate.rotation = Quaternion.Slerp(ObjToRotate.rotation, Camera.transform.rotation, 20f * Time.deltaTime);
    }
}
