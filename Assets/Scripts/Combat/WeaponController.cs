using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform muzzle;
    [SerializeField] private GameObject projectilePrefab;

    [Header("Weapon")]
    [SerializeField] private float fireRate = 5f;

    private PlayerInputActions inputActions;

    private float nextFireTime;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }

    private void Update()
    {
        if (inputActions.Player.Fire.IsPressed())
        {
            TryFire();
        }
    }

    private void TryFire()
    {
        if (Time.time < nextFireTime)
            return;

        nextFireTime = Time.time + 1f / fireRate;

        Fire();
    }

    private void Fire()
    {
        if (projectilePrefab == null || muzzle == null)
            return;

        Instantiate(
            projectilePrefab,
            muzzle.position,
            muzzle.rotation
        );
    }
}