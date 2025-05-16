using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject bulletPrefab;
    private bool isSprinting = false;
    private float fireCooldown = 0;
    float burstDelay = 0.1f; // time between each shot in the burst
    [SerializeField] private bool isMoving;

    private Vector3 moveInput;
    private CharacterController controller;
    private InputSystem_Actions controls;

    public static bool wpnChanged = false;

    public AudioSource emptyClip;

    public Transform bulletSpawn;

    Animator animator;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        controls = new InputSystem_Actions();

        controls.ZombieAttack.Move.performed += OnMove;
        controls.ZombieAttack.Move.canceled += OnMove;
        controls.ZombieAttack.Sprint.performed += OnSprint;
        controls.ZombieAttack.Sprint.canceled += OnSprint;

        controls.ZombieAttack.Fire.performed += OnFire;

        controls.ZombieAttack.Reload.performed += OnReload;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    void OnEnable()
    {
        controls.ZombieAttack.Enable();
    }

    void OnDisable()
    {
        controls.ZombieAttack.Disable();
    }

    void Update()
    {
        fireCooldown -= Time.deltaTime;

        //movement mngment
        isMoving = moveInput.sqrMagnitude > 0.01f;
        CameraPositionUpdate();
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        animator.SetBool("IsWalking", isMoving);

        controller.Move(move * moveSpeed * Time.deltaTime);

        if (!isSprinting)
        {
            RotateTowardsMouse();
        }
        else
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime); //sletp makes it smooth
        }

        if (GameManager.instance.gameLost) gameObject.SetActive(false);

    }

    private void OnFire(InputAction.CallbackContext context)
    {
        float fireRate = Player.instance.currentWeapon.fireRate;
        int bulletsPerShot = Player.instance.currentWeapon.bulletsPerShot;

        if (fireCooldown > 0) return; //not ready to shoot yet
        else if (Player.instance.currentAmmo == 0)// if no ammo
        {
            Debug.Log("Empty Gun!");
            emptyClip.Play();
            if (Player.instance.currentWeapon.name != "1911")
            {
                Player.instance.audioSources[1].Play();
                Player.instance.EquipWeapon("1911");
            }
        }
        else //if ammo 
        {
            Player.instance.audioSources[0].Play();
            StartCoroutine(FireBurst(bulletsPerShot));
            fireCooldown = 1f / fireRate;
        }
    }
    private IEnumerator FireBurst(int bulletsPerShot)
    {
        for (int i = 0; i < bulletsPerShot; i++)
        {
            if(Player.instance.currentAmmo != 0) //this control doesn't seem to work properly
            {
                Player.instance.audioSources[0].Play();
                Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
                Player.instance.currentAmmo--;
                yield return new WaitForSeconds(burstDelay);
            }
        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isSprinting = true;
            animator.SetBool("IsSprinting", true);
            moveSpeed *= 2;
        }
        else if (context.canceled)
        {
            isSprinting = false;
            animator.SetBool("IsSprinting", false);
            moveSpeed /= 2;
        }
    }

    private void OnReload(InputAction.CallbackContext context)
    {
        if (Player.instance.currentWeapon.name != "1911") return; //weapons are disposable, don't reload unless 1911.

        //if conditions are met
        if (!Player.instance.isReloading && Player.instance.currentAmmo != Player.instance.currentWeapon.maxAmmo)
        {
            Player.instance.isReloading = true;
            Player.instance.audioSources[1].Play();
            StartCoroutine(ReloadTime());
        }

    }
    private IEnumerator ReloadTime()
    {
        yield return new WaitForSeconds(0.6f);
        Player.instance.currentAmmo = Player.instance.currentWeapon.maxAmmo;
        Player.instance.isReloading = false;
    }

    private void RotateTowardsMouse()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hitInfo, 100f))
        {
            Vector3 targetPosition = hitInfo.point;
            Vector3 direction = targetPosition - transform.position;
            direction.y = 0f; // ignore vertical difference

            if (direction.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = targetRotation;
            }
        }
    }
    private void CameraPositionUpdate()
    {
        Vector3 currentPlayerPos = new Vector3(transform.position.x, 40, transform.position.z);
        mainCamera.transform.position = currentPlayerPos;
    }


}
