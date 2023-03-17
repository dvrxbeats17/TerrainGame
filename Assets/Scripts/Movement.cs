using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] private InputAction movement;
    [SerializeField] private InputAction laser;
    [SerializeField] private float speed = 5f;

    [SerializeField] private float maxPosX;
    [SerializeField] private float maxPosY;
    [SerializeField] private float minPosY;

    [SerializeField] private float pitchFactor = -5f;
    [SerializeField] private float controlPitchFactor = 5f;
    [SerializeField] private float yawFactor = 5f;
    [SerializeField] private float controlRollFactor = 5f;

    [SerializeField] private GameObject[] lasers;

    private Vector3 shipLocalPos;



    private float _horizontalMovement;
    private float _verticalMovement;


    private void OnEnable()
    {
        movement.Enable();
        laser.Enable();
    }
    private void OnDisable()
    {
        movement.Disable();
        laser.Disable();
    }
    private void Update()
    {
        ProcessThrow();
        ProcessRotation();
        ProccesLaser();
    }

    private void ProcessRotation()
    {
        float pitch = transform.localPosition.y * pitchFactor + _verticalMovement * controlPitchFactor;
        float yaw = transform.localPosition.x * yawFactor;
        float roll = _horizontalMovement * controlRollFactor;

        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }

    private void ProcessThrow()
    {
        _horizontalMovement = movement.ReadValue<Vector2>().x;
        _verticalMovement = movement.ReadValue<Vector2>().y;

        shipLocalPos.x = Mathf.Clamp(transform.localPosition.x + _horizontalMovement * Time.deltaTime * speed, -maxPosX, maxPosX);
        shipLocalPos.y = Mathf.Clamp(transform.localPosition.y + _verticalMovement * Time.deltaTime * speed, minPosY, maxPosY);

        transform.localPosition = shipLocalPos;
    }

    private void ProccesLaser()
    {
        if (laser.ReadValue<float>() > 0.5f)
        {
            foreach (var laser in lasers)
            {
                laser.gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (var laser in lasers)
            {
                laser.gameObject.SetActive(false);
            }
        }
    }
}
