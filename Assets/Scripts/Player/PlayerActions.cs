using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
public class PlayerActions : MonoBehaviour
{
    public static Action CallSmall = delegate{};

    public bool Moving;

    [SerializeField] public PlayerStats PS;
    [SerializeField] public BigMonster BigMon;
    [SerializeField] public SmallMonster SmallMon;

    [SerializeField] private Animator Anim;
    [SerializeField] private Light HandLight;
    [SerializeField] private Collider SwordHitBox;
    [SerializeField] private GameObject PlayerModel;
    [SerializeField] private GameObject SearchFlyPrefab;

    [SerializeField] private Rigidbody RB;

    [SerializeField] private float JumpForce;
    [SerializeField] private float Speed;
    [SerializeField] private float SpinSpeed;

    [SerializeField] private bool Transformed;

    [SerializeField] private float KnockBackAmount;

    [SerializeField] private float fallThreshold;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float fallMultiplier;
    [SerializeField] private float maxFallSpeed;

    private bool Tcooldown;

    private PlayerControl PC;
    private Vector2 InputDirection;
    private Vector3 AppliedDirection;
    private Vector3 CamMovement;

    private bool Grounded = true;
    private bool Attacking;

    private int ComboCounter;
    private float ComboTimer;
    private bool Falling = false;

    private void Awake()
    {
        PS = GetComponentInChildren<PlayerStats>();
        SetMons();
        Anim = GetComponentInChildren<Animator>();
        RB = GetComponentInChildren<Rigidbody>();
        HandLight = GetComponentInChildren<Light>();

        // Set collision detection mode to Continuous
        RB.collisionDetectionMode = CollisionDetectionMode.Continuous;

        PC = new PlayerControl();
        PC.Enable();
    }

    private void OnEnable()
    {
        PC.Player.Jump.performed += Jump;
        PC.Player.LightAttack.performed += LightAttack;
        PC.Player.SupportCall.performed += SupportCall;
        PC.Player.BattleIncarnate.performed += BattleIncarnate;
        PC.Player.FiesSearch.performed += Search;
        SceneManagment.NewSceneLoaded += SetMons;
    }
    private void OnDisable()
    {
        PC.Player.Jump.performed -= Jump;
        PC.Player.LightAttack.performed -= LightAttack;
        PC.Player.SupportCall.performed -= SupportCall;
        PC.Player.BattleIncarnate.performed -= BattleIncarnate;
        PC.Player.FiesSearch.performed -= Search;
        SceneManagment.NewSceneLoaded += SetMons;
    }

    private void Update()
    {
        InputDirection.x = PC.Player.Movement.ReadValue<Vector2>().x;
        InputDirection.y = PC.Player.Movement.ReadValue<Vector2>().y;

        Moving = InputDirection != Vector2.zero;

        GroundCheck();
        FallCheck();

        if (!Attacking)
        {
            RotatePlayer();
        }

        Anim.SetBool("Running", Moving);

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
            ApplyMovement();
        }
    }
    private void ApplyMovement()
    {
        AppliedDirection = new Vector3(InputDirection.x, 0, InputDirection.y);
        CamMovement = ConvertToCamSpace(AppliedDirection);

        Vector3 DesiredVelocity = CamMovement * Speed * Time.fixedDeltaTime;

        // Ensure smoother movement using MovePosition
        Vector3 newPosition = RB.position + new Vector3(DesiredVelocity.x, 0, DesiredVelocity.z);
        RB.MovePosition(newPosition);

        if (Falling)
        {
            // Apply faster falling by multiplying the y-velocity with fallMultiplier
            RB.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

        // Optional: Limit the max fall speed
        if (RB.linearVelocity.y < maxFallSpeed)
        {
            RB.linearVelocity = new Vector3(RB.linearVelocity.x, maxFallSpeed, RB.linearVelocity.z);
        }

        RB.linearVelocity = DesiredVelocity + new Vector3(0, RB.linearVelocity.y, 0);
    }
    private void GroundCheck()
    {
        // Perform a raycast slightly below the player to check if grounded
        Vector3 rayOrigin = transform.position + Vector3.up * 0.1f; // Starting point slightly above the player's feet
        RaycastHit hit;

        // Cast a ray downwards to check for the ground
        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, groundCheckDistance, groundLayer))
        {
            Grounded = true;
            if (Falling)
            {
                Anim.SetTrigger("Land");
                Anim.ResetTrigger("Jump");
                Anim.ResetTrigger("Down");
            }
        }
        else
        {
            Grounded = false;
        }
    }
    private void FallCheck()
    {
        // Check if the player is moving downward and not grounded
        if (RB.linearVelocity.y < fallThreshold && !Grounded)
        {
            if (!Falling)
            {
                Falling = true;
                Anim.SetTrigger("Down");  // Trigger fall animation (optional)
            }
        }
        else if (Grounded && Falling)
        {
            // Reset falling state upon landing
            Falling = false;
        }
    }



    private Vector3 ConvertToCamSpace(Vector3 VectorToSpin)
    {
        float currentYValue = VectorToSpin.y;
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward = camForward.normalized;
        camRight = camRight.normalized;

        Vector3 camForwardZProduct = VectorToSpin.z * camForward;
        Vector3 camRightXProduct = VectorToSpin.x * camRight;

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
        if (InputDirection.x != 0 || InputDirection.y != 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(posToLookAt);
            transform.rotation = Quaternion.Slerp(CurrentRotation, targetRotation, SpinSpeed * Time.deltaTime);
        }
        else
        {
            RB.angularVelocity = Vector3.zero;
        }
    }
    private void PlayerAutoTarget()
    {
        var lookPos = FindObjectOfType<CameraActions>().LockTarget.transform.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 1000f);
    }

    private void Jump(InputAction.CallbackContext jump)
    {
        if (!Transformed && !Attacking && Grounded)
        {
            Anim.ResetTrigger("Land");
            Anim.SetTrigger("Jump");

            // Detect if the player is in contact with an object
            if (IsTouchingWall())
            {
                // Apply a slightly stronger jump force if colliding with an object
                RB.linearVelocity = new Vector3(RB.linearVelocity.x, 0, RB.linearVelocity.z);
                RB.AddForce(Vector3.up * (JumpForce * 1.3f), ForceMode.Impulse);  // Increase the jump force by 20%
            }
            else
            {
                // Normal jump when not touching objects
                RB.linearVelocity = new Vector3(RB.linearVelocity.x, 0, RB.linearVelocity.z);
                RB.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
            }
        }
    }

    private bool IsTouchingWall()
    {
        // Raycast in the forward direction to check if the player is touching a wall or object
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 0.5f))  // Adjust distance as necessary
        {
            return true;
        }
        return false;
    }

    private void LightAttack(InputAction.CallbackContext swing)
    {
        if (FindObjectOfType<CameraActions>().LockedOn)
        {
            PlayerAutoTarget();
        }
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

    private void SupportCall(InputAction.CallbackContext swing)
    {
        if (SmallMon)
        {
            if (!Transformed)
            {
                if (!Attacking)
                {
                    if (Grounded)
                    {
                        Attacking = true;
                        StartCoroutine(Call());
                    }
                }
            }
        }
    }

    private void BattleIncarnate(InputAction.CallbackContext Shift)
    {
        if (BigMon)
        {
            if (!Attacking)
            {
                if (Grounded)
                {
                    if (!Transformed && !Tcooldown)
                    {
                        Transformed = true;
                        gameObject.transform.position = Vector3.Lerp(transform.position, BigMon.gameObject.transform.position, Speed / Time.deltaTime);
                        Anim = BigMon.GetComponentInChildren<Animator>();
                        BigMon.enabled = false;
                        PlayerModel.SetActive(false);
                        StartCoroutine(IncarnateCoolDown());
                    }
                }
            }
        }
    }

    private void Search(InputAction.CallbackContext Search)
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, PS.SearchRange, Vector3.up);
        foreach(RaycastHit hit in hits)
        {
            if(hit.transform.gameObject.CompareTag("Fossil"))
            {
                GameObject tmp = Instantiate(SearchFlyPrefab, hit.transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
                hit.transform.gameObject.GetComponent<FossilSpot>().Spotted = true;
                StartCoroutine(DestroyTMP(tmp));
            }
        }
    }
    private void SetMons()
    {
        BigMon = FindObjectOfType<BigMonster>();
        SmallMon = FindObjectOfType<SmallMonster>();
    }

    public void PlayerHit()
    {
        Anim.SetTrigger("Hit");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            if (!Grounded)
            {
                Grounded = true;
                Anim.ResetTrigger("Jump");
                Anim.ResetTrigger("Down");
                Anim.SetTrigger("Land");
            }
        }

        if (other.gameObject.tag == "Enemy" && Attacking)
        {
            other.GetComponentInChildren<Heath>().UpdateHeath(-PS.SpearDmg);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            Grounded = false;
        }
    }
    IEnumerator Swing1()
    {
        if (!Transformed)
        {
            RB.linearVelocity = Vector3.zero;
            SwordHitBox.enabled = true;
            Anim.SetTrigger("Attack1");
            yield return new WaitForSeconds(0.35f);
            Attacking = false;
            SwordHitBox.enabled = false;
            ComboCounter = 1;
            ComboTimer = 0.5f;
        }
        else
        {
            RB.linearVelocity = Vector3.zero;
            //SwordHitBox.enabled = true;
            Collider[] HitBoxes = GetComponentsInChildren<SphereCollider>();
            foreach(Collider S in HitBoxes)
            {
                S.enabled = true;
            }
            Anim.SetTrigger("Attack1");
            yield return new WaitForSeconds(BigMon.Attack1HitCoolDown);
            Attacking = false;
            //SwordHitBox.enabled = false;
            foreach (Collider S in HitBoxes)
            {
                S.enabled = false;
            }
            ComboCounter = 1;
            ComboTimer = BigMon.Attack1HitCoolDown*2f;
        }
    }

    IEnumerator Swing2() 
    {
        if (!Transformed)
        {
            RB.linearVelocity = Vector3.zero;
            SwordHitBox.enabled = true;
            Anim.SetTrigger("Attack2");
            yield return new WaitForSeconds(0.45f);
            Attacking = false;
            SwordHitBox.enabled = false;
            ComboCounter = 0;
            ComboTimer = 0;
        }
        else
        {
            RB.linearVelocity = Vector3.zero;
            //SwordHitBox.enabled = true;
            Collider[] HitBoxes = GetComponentsInChildren<SphereCollider>();
            foreach (Collider S in HitBoxes)
            {
                S.enabled = true;
            }
            Anim.SetTrigger("Attack2");
            yield return new WaitForSeconds(BigMon.Attack2HitCoolDown);
            Attacking = false;
            //SwordHitBox.enabled = false;
            foreach (Collider S in HitBoxes)
            {
                S.enabled = false;
            }
            ComboCounter = 0;
            ComboTimer = 0;
        }
    }

    IEnumerator Call()
    {
        RB.linearVelocity = Vector3.zero;
        Anim.SetTrigger("Wave");
        HandLight.enabled = true;
        CallSmall();
        yield return new WaitForSeconds(0.8f);
        HandLight.enabled = false;
        Attacking = false;
    }

    IEnumerator IncarnateCoolDown()
    {
        while (Tcooldown)
        {
            BigMon.transform.position = transform.position;
        }
        yield return new WaitForSeconds(0);

        Transformed = false;
        PlayerModel.SetActive(true);
        Anim = GetComponentInChildren<Animator>();
        BigMon.enabled = true;
    }
    private IEnumerator DestroyTMP(GameObject GO)
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(GO);
    }
}
