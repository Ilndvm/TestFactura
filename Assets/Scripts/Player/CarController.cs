using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float finishDistance = 200f;

    private bool isMoving = true;

    public float DistanceTravelled { get; private set; }
    public bool HasFinished { get; private set; }

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        if (!isMoving || HasFinished)
            return;

        float movement = moveSpeed * Time.deltaTime;

        transform.position += Vector3.forward * movement;

        DistanceTravelled = transform.position.z - startPosition.z;

        if (DistanceTravelled >= finishDistance)
        {
            FinishLevel();
        }
    }

    private void FinishLevel()
    {
        HasFinished = true;
        isMoving = false;

        Debug.Log("LEVEL COMPLETE!");
    }

    public void StopCar()
    {
        isMoving = false;
    }
}