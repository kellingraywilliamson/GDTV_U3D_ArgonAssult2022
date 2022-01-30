using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("General Settings")] [Tooltip("How fast the ship moves up and down.")] [SerializeField]
    private float controlSpeed = 20;

    [Tooltip("The horizontal range of the screen the player can move across.")] [SerializeField]
    private float xRange = 3.5f;

    [Tooltip("The vertical range of the screen the player can move across.")] [SerializeField]
    private float yRange = 2f;

    [SerializeField] private float positionPitchFactor = -15f;
    [SerializeField] private float controlPitchFactor = -10f;
    [SerializeField] private float positionYawFactor = 2f;
    [SerializeField] private float controlRollFactor = -20f;

    [Header("Projectile Settings")]
    [Tooltip("References to the laser GameObjects that will be activated/deactivated when the player fires.")]
    [SerializeField] private GameObject[] lasers;

    private Vector2 _movementInputValue;
    private Vector3 _currentLocalPosition;
    private bool _isFiring = false;

    // Update is called once per frame
    private void Update()
    {
        _currentLocalPosition = transform.localPosition;
    }

    private void FixedUpdate()
    {
        ProcessTranslation();
        ProcessRotation();
        ProcessFiring();
    }

    public void Move(InputAction.CallbackContext context)
    {
        _movementInputValue = context.ReadValue<Vector2>();
    }

    public void Fire(InputAction.CallbackContext context)
    {
        _isFiring = context.performed;
    }

    private void ProcessFiring()
    {
        foreach (var laser in lasers)
        {
            var em = laser.GetComponent<ParticleSystem>().emission;
            em.enabled = _isFiring;
        }
    }


    private void ProcessRotation()
    {
        var pitchDueToPosition = _currentLocalPosition.y * positionPitchFactor;
        var pitchDueToControlThrow = _movementInputValue.y * controlPitchFactor;

        var pitch = pitchDueToPosition + pitchDueToControlThrow;
        var yaw = _currentLocalPosition.x * positionYawFactor;
        var roll = _movementInputValue.x * controlRollFactor;
        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }

    private void ProcessTranslation()
    {
        var xOffset = _movementInputValue.x * controlSpeed * Time.deltaTime;
        var yOffset = _movementInputValue.y * controlSpeed * Time.deltaTime;

        var rawXPos = _currentLocalPosition.x + xOffset;
        var rawYPos = _currentLocalPosition.y + yOffset;
        var clampedXPos = Mathf.Clamp(rawXPos, -xRange, xRange);
        var clampedYPos = Mathf.Clamp(rawYPos, -yRange, yRange);

        transform.localPosition = new Vector3(clampedXPos, clampedYPos, _currentLocalPosition.z);
    }
}
