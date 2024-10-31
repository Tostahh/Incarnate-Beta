using UnityEngine;
using UnityEngine.InputSystem;

public class CameraActions : MonoBehaviour
{
    public GameObject LockedUI;
    public GameObject LockTarget;
    public Animator Anim;
    public bool LockedOn;
    public float LockDistance = 13f;

    private PlayerControl PC;
    private Transform Player;
    private void Awake()
    {
        PC = new PlayerControl();
        PC.Enable();
        Player = FindObjectOfType<PlayerActions>().gameObject.transform;
    }
    private void OnEnable()
    {
        PC.Player.LockCam.performed += LockCam;
    }
    private void OnDisable()
    {
        PC.Player.LockCam.performed -= LockCam;
    }

    private void Update()
    {
        if(!LockedOn)
        {
            LockTarget.transform.position = Player.position;
        }
        else
        {
            if (InRange())
            {
                GameObject target = FindTarget();
                LockTarget.transform.position = target.transform.position;
            }
            else
            {
                if (LockedOn)
                {
                    LockedOn = false;
                    Anim.SetBool("LockedOn", LockedOn);
                    LockedUI.SetActive(false);
                }
            }
        }
    }
    public void LockCam(InputAction.CallbackContext jump)
    {
        if(LockedOn)
        {
            LockedOn = false;
            Anim.SetBool("LockedOn", LockedOn);
            LockedUI.SetActive(false);
        }
        else
        {
            if (InRange())
            {
                LockedOn = true;
                Anim.SetBool("LockedOn", LockedOn);
                LockedUI.SetActive(true);
            }
        }
    }
    public GameObject FindTarget()
    {
        CamLockTarget[] targetes = FindObjectsOfType<CamLockTarget>();
        GameObject Target = gameObject;

        if (InRange())
        {
            for (int i = 0; i < targetes.Length; i++)
            {
                if (i == 0)
                {
                    Target = targetes[i].gameObject;
                }
                else
                {
                    if (Vector3.Distance(Target.transform.position, Player.position) > Vector3.Distance(targetes[i].transform.position, Player.position))
                    {
                        Target = targetes[i].gameObject;
                    }
                }
            }
        }

        return Target;
    }
    public bool InRange()
    {
        CamLockTarget[] targetes = FindObjectsOfType<CamLockTarget>();
        bool go = false;

        foreach (CamLockTarget t in targetes)
        {
            if (Vector3.Distance(t.transform.position, Player.position) <= LockDistance)
            {
                go = true;
            }
        }
        return go;
    }
}
