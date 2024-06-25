using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerActions : MonoBehaviour
{
    public static Action CallSmall = delegate{};

    [SerializeField] private Animator Anim;
    [SerializeField] private Light HandLight;
    [SerializeField] private Collider SwordHitBox;

    [SerializeField] private Rigidbody RB;

    [SerializeField] private float JumpForce;
    [SerializeField] private float Speed;
    [SerializeField] private float SpinSpeed;

    private PlayerControls PC;
    private Vector2 InputDirection;
    private Vector3 AppliedDirection;
    private Vector3 CamMovement;

    private bool Grounded = true;
    private bool Attacking;

    private int ComboCounter;
    private float ComboTimer;

    private void Awake()
    {
        Anim = GetComponentInChildren<Animator>();
        RB = GetComponentInChildren<Rigidbody>();
        HandLight = GetComponentInChildren<Light>();

        PC = new PlayerControls();
        PC.Enable();
    }

    private void OnEnable()
    {
        PC.Player.Jump.performed += Jump;
        PC.Player.Attack.performed += Attack;
        PC.Player.Call.performed += Call;
    }
    private void OnDisable()
    {
        PC.Player.Jump.performed -= Jump;
        PC.Player.Attack.performed -= Attack;
        PC.Player.Call.performed -= Call;
    }

    private void Update()
    {
        InputDirection.x = PC.Player.Movement.ReadValue<Vector2>().x;
        InputDirection.y = PC.Player.Movement.ReadValue<Vector2>().y;

        if (!Attacking)
        {
            RotatePlayer();
        }

        if (InputDirection.x != 0 || InputDirection.y != 0)
        {
            Anim.SetBool("Running", true);
        }
        else
        {
            Anim.SetBool("Running", false);
        }

        ComboTimer -= Time.deltaTime;

        if (ComboTimer < 0)
        {
            ComboCounter = 0;
            ComboTimer = 0;
        }
    }

    private void FixedUpdate()
    {
        if (!Attacking)
        {
            AppliedDirection = new Vector3(InputDirection.x, 0, InputDirection.y);
            CamMovement = ConvertToCamSpace(AppliedDirection);
            RB.velocity = new Vector3(CamMovement.x * Speed * Time.deltaTime, RB.velocity.y, CamMovement.z * Speed * Time.deltaTime);
        }
    }

    private Vector3 ConvertToCamSpace(Vector3 VectorToSpin)
    {
        float currentYValue = VectorToSpin.y;
        //Camera Forward and Right;
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;

        //Flatening the Vector
        camForward.y = 0;
        camRight.y = 0;
            
        //Normalizing
        camForward = camForward.normalized;
        camRight = camRight.normalized;
        
        //Setting Player input to Camera Vector
        Vector3 camForwardZProduct = VectorToSpin.z * camForward;
        Vector3 camRightXProduct = VectorToSpin.x * camRight;

        //Returning
        Vector3 CameraSpaceVector = camForwardZProduct + camRightXProduct;
        CameraSpaceVector.y = currentYValue;
        return CameraSpaceVector;
    }

    private void RotatePlayer()
    {
        Vector3 posToLookAt;

        posToLookAt.x = CamMovement.x;
        posToLookAt.y = 0;
        posToLookAt.z = CamMovement.z;

        Quaternion CurrentRotation = transform.rotation;
        if(InputDirection.x != 0 || InputDirection.y != 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(posToLookAt);

            transform.rotation = Quaternion.Slerp(CurrentRotation, targetRotation, SpinSpeed * Time.deltaTime);
        }
        else
        {
            RB.angularVelocity = Vector3.zero;
        }
    }

    private void Jump(InputAction.CallbackContext jump)
    {
        if (!Attacking)
        {
            if (Grounded)
            {
                Anim.SetTrigger("Jump");
                RB.AddForce(new Vector3(0, JumpForce, 0));
                Invoke("Fall", 0.8f);
            }
        }
        Grounded = false;
    }

    private void Fall()
    {
        Anim.SetTrigger("Down");
    }

    private void Attack(InputAction.CallbackContext swing)
    {
        if(!Attacking && ComboCounter == 0)
        {
            if (Grounded)
            {
                Attacking = true;
                StartCoroutine(Swing1());
            }
        }

        if (!Attacking && ComboCounter == 1)
        {
            if (Grounded)
            {
                Attacking = true;
                StartCoroutine(Swing2());
            }
        }
    }

    private void Call(InputAction.CallbackContext swing)
    {
        if(!Attacking)
        {
            if(Grounded)
            {
                Attacking = true;
                StartCoroutine(Call());
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            if (!Grounded)
            {
                Anim.SetTrigger("Land");
                Grounded = true;
            }
        }

        if (other.gameObject.tag == "Enemy" && Attacking)
        {
            Debug.Log("Hit");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            Grounded = false;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.layer == 3)
        {
            Grounded = true;
        }
    }

    IEnumerator Swing1()
    {
        RB.velocity = Vector3.zero;
        SwordHitBox.enabled = true;
        Anim.SetTrigger("Attack");
        yield return new WaitForSeconds(0.35f);
        Attacking = false;
        SwordHitBox.enabled = false;
        ComboCounter = 1;
        ComboTimer = 0.5f;
    }

    IEnumerator Swing2() 
    {
        RB.velocity = Vector3.zero;
        SwordHitBox.enabled = true;
        Anim.SetTrigger("Attack2");
        yield return new WaitForSeconds(0.45f);
        Attacking = false;
        SwordHitBox.enabled = false;
        ComboCounter = 0;
        ComboTimer = 0;
    }

    IEnumerator Call()
    {
        RB.velocity = Vector3.zero;
        Anim.SetTrigger("Wave");
        HandLight.enabled = true;
        CallSmall();
        yield return new WaitForSeconds(0.8f);
        HandLight.enabled = false;
        Attacking = false;
    }
}
