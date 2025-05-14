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

        controls.TowerDefense.Move.performed += OnMove;
        controls.TowerDefense.Move.canceled += OnMove;
        controls.TowerDefense.Sprint.performed += OnSprint;
        controls.TowerDefense.Sprint.canceled += OnSprint;

        controls.TowerDefense.Fire.performed += OnFire;

        controls.TowerDefense.ChangeWpn.performed += OnChangeWpn;

        controls.TowerDefense.Reload.performed += OnReload;
    }

   

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    void OnEnable()
    {
        controls.TowerDefense.Enable();
    }

    void OnDisable()
    {
        controls.TowerDefense.Disable();
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
        
    }

    private void OnFire(InputAction.CallbackContext context)
    {
        float fireRate = Player.instance.currentWeapon.fireRate;
        int bulletsPerShot = Player.instance.currentWeapon.bulletsPerShot;

        if (fireCooldown > 0 || Player.instance.currentAmmo <= 0) return;
        else if (Player.instance.currentAmmo == 0)
        {
            emptyClip.Play();
        }
        else
        {
            StartCoroutine(FireBurst(bulletsPerShot));
            fireCooldown = 1f / fireRate;
        }
    }

    private IEnumerator FireBurst(int bulletsPerShot)
    {
        for (int i = 0; i < bulletsPerShot; i++)
        {
            if (Player.instance.currentAmmo > 0)
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

    private void OnChangeWpn(InputAction.CallbackContext context)
    {
        wpnChanged = true;
    }

    private void OnReload(InputAction.CallbackContext context)
    {
        if (!Player.instance.isReloading && Player.instance.currentAmmo != Player.instance.currentWeapon.maxAmmo)
        {
            Player.instance.isReloading = true;
            Player.instance.audioSources[1].Play();
            StartCoroutine(ReloadTime());
        }
    }

    private IEnumerator ReloadTime()
    {
        yield return new WaitForSeconds(Player.instance.currentWeapon.reloadTime);
        Player.instance.currentAmmo = Player.instance.currentWeapon.maxAmmo;
        Player.instance.isReloading = false;
    }
}
