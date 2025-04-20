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
    [SerializeField] private bool isMoving;

    private Vector3 moveInput;
    private CharacterController controller;
    private InputSystem_Actions controls;

    public static bool wpnChanged = false;

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
    }

    private void OnChangeWpn(InputAction.CallbackContext context)
    {
        wpnChanged = true;
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
        Debug.Log("Fire");
        float fireRate = Player.instance.currentWeapon.fireRate;
        int bulletsPerShot = Player.instance.currentWeapon.bulletsPerShot;
       
        if (fireCooldown > 0) return;
        else
        {
            StartCoroutine(FireBurst(bulletsPerShot));

            //Vector3 UpdatedPos = new Vector3(transform.position.x, 2, transform.position.z);
            //for (int i = 0; i < bulletsPerShot; i++) 
            //{
            //    Debug.Log("Shot");
            //    Instantiate(bulletPrefab, UpdatedPos, transform.rotation);
            //    Player.instance.currentAmmo--;
            //}
            fireCooldown = 1f / fireRate;
        }
    }

    private IEnumerator FireBurst(int bulletsPerShot)
    {
        float burstDelay = 0.1f; // time between each shot in the burst

        for (int i = 0; i < bulletsPerShot; i++)
        {
            if (Player.instance.currentAmmo <= 0) yield break;

            Vector3 UpdatedPos = new Vector3(transform.position.x, 2, transform.position.z);

            Instantiate(bulletPrefab, UpdatedPos, transform.rotation);
            Player.instance.currentAmmo--;

            Debug.Log("Shot " + (i + 1));

            yield return new WaitForSeconds(burstDelay);
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
            moveSpeed *= 2;
        }
        else if (context.canceled)
        {
            isSprinting = false;
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

}
