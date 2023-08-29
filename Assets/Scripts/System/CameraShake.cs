using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;
    [SerializeField] Transform cameraTransform; // The camera's transform component
    [SerializeField] float shakeDuration = 0.5f; // Duration of the shake
    [SerializeField] float shakeMagnitude = 0.1f; // Intensity of the shake

    private Vector3 originalPosition;
    private float remainingShakeDuration = 0f;

    void Awake()
    {
        instance=this;
    }

    void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }

        originalPosition = cameraTransform.localPosition;
    }

    void Update()
    {
        if (remainingShakeDuration > 0)
        {
            // Generate a random offset within the specified magnitude
            Vector3 randomOffset = Random.insideUnitSphere * shakeMagnitude;

            // Apply the offset to the camera's local position
            cameraTransform.localPosition = originalPosition + randomOffset;

            remainingShakeDuration -= Time.deltaTime;
        }
        else
        {
            // Reset the camera's position
            cameraTransform.localPosition = originalPosition;
        }
    }

    public void Shake()
    {
        remainingShakeDuration = shakeDuration;
    }
    public void Shake(float duration,float intensity)
    {

        remainingShakeDuration = duration;
        shakeMagnitude=intensity;
    }
}

