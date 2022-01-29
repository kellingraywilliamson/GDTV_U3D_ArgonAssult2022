using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float controlSpeed = 20;
    [SerializeField] private float xRange = 3.5f;
    [SerializeField] private float yRange = 2f;
    [SerializeField] private float positionPitchFactor = -15f;
    [SerializeField] private float controlPitchFactor = -10f;
    [SerializeField] private float positionYawFactor = 2f;
    [SerializeField] private float controlRollFactor = -20f;

    private Vector2 _movementInputValue;
    private Vector3 _currentLocalPosition;

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
        if (context.performed)
        {
            print("firing!");
        }
    }
    private void ProcessFiring()
    {
        // Debug.Log(fire.ReadValue<float>());
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
        var xOffset = _movementInputValue.x * controlSpeed;
        var yOffset = _movementInputValue.y * controlSpeed;

        var rawXPos = _currentLocalPosition.x + (xOffset * Time.deltaTime);
        var rawYPos = _currentLocalPosition.y + (yOffset * Time.deltaTime);
        var clampedXPos = Mathf.Clamp(rawXPos, -xRange, xRange);
        var clampedYPos = Mathf.Clamp(rawYPos, -yRange, yRange);

        transform.localPosition = new Vector3(clampedXPos, clampedYPos, _currentLocalPosition.z);
    }
}
