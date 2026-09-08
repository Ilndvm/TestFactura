using UnityEngine;
using UnityEngine.InputSystem;

public class TurretController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform turretPivot;

    [Header("Aiming")]
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float maxRotationAngle = 70f;

    [Header("Aiming Visual")]
    [SerializeField] private LineRenderer aimLine;
    [SerializeField] private Transform muzzle;
    [SerializeField] private float aimLineLength = 25f;

    private PlayerInputActions inputActions;

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
        AimTurret();
        UpdateAimLine();
    }

    private void AimTurret()
    {
        Vector2 mousePosition = inputActions.Player.Aim.ReadValue<Vector2>();

        Ray ray = playerCamera.ScreenPointToRay(mousePosition);

        Plane groundPlane = new Plane(
            Vector3.up,
            Vector3.zero
        );

        if (!groundPlane.Raycast(ray, out float distance))
            return;

        Vector3 targetPoint = ray.GetPoint(distance);

        Vector3 direction = targetPoint - turretPivot.position;

        // We only want horizontal rotation.
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
            return;

        direction.Normalize();

        // Calculate angle relative to the car's forward direction.
        float angle = Vector3.SignedAngle(
            transform.forward,
            direction,
            Vector3.up
        );

        // Limit turret rotation.
        angle = Mathf.Clamp(
            angle,
            -maxRotationAngle,
            maxRotationAngle
        );

        Vector3 limitedDirection =
            Quaternion.Euler(0f, angle, 0f) *
            transform.forward;

        Quaternion targetRotation =
            Quaternion.LookRotation(limitedDirection);

        turretPivot.rotation = Quaternion.Slerp(
            turretPivot.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    private void UpdateAimLine()
    {
        if (aimLine == null || muzzle == null)
            return;

        Vector3 startPosition = muzzle.position;
        Vector3 direction = turretPivot.forward;

        Vector3 endPosition =
            startPosition + direction * aimLineLength;

        aimLine.SetPosition(0, startPosition);
        aimLine.SetPosition(1, endPosition);
    }
}