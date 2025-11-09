using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class ObstacleLightController : MonoBehaviour
{
    [Header("Light Settings")]
    [SerializeField] private float lightDuration = 2f;
    [SerializeField] private bool debugMode = false;

    private Light2D[] childLights;
    private Coroutine lightCoroutine;

    void Start()
    {
        // Find all Light2D components in child objects
        childLights = GetComponentsInChildren<Light2D>();

        if (childLights.Length == 0)
        {
            Debug.LogWarning($"No Light2D components found on {gameObject.name} or its children!");
        }
        else
        {
            if (debugMode)
                Debug.Log($"Found {childLights.Length} Light2D component(s) on {gameObject.name}");

            // Make sure lights start OFF
            SetLightsActive(false);
        }

        // Subscribe to the collision event
        BeastController.OnBeastCollision += HandleBeastCollision;
    }

    void OnDestroy()
    {
        // Unsubscribe from the event to prevent memory leaks
        BeastController.OnBeastCollision -= HandleBeastCollision;
    }

    private void HandleBeastCollision(GameObject hitObstacle)
    {
        // ALL obstacles respond to ANY collision
        if (debugMode)
        {
            if (hitObstacle == gameObject)
                Debug.Log($"{gameObject.name} was hit directly! Activating lights.");
            else
                Debug.Log($"{gameObject.name} responding to collision with {hitObstacle.name}");
        }

        // If a light coroutine is already running, stop it
        if (lightCoroutine != null)
        {
            StopCoroutine(lightCoroutine);
        }

        // Start the light activation coroutine
        lightCoroutine = StartCoroutine(ActivateLightsTemporarily());
    }

    private IEnumerator ActivateLightsTemporarily()
    {
        // Turn lights ON
        SetLightsActive(true);

        // Wait for the specified duration
        yield return new WaitForSeconds(lightDuration);

        // Turn lights OFF
        SetLightsActive(false);

        lightCoroutine = null;
    }

    private void SetLightsActive(bool active)
    {
        foreach (Light2D light in childLights)
        {
            if (light != null)
            {
                light.enabled = active;
            }
        }
    }

    // Optional: Method to test the light effect in the editor
    [ContextMenu("Test Light Effect")]
    private void TestLightEffect()
    {
        if (Application.isPlaying)
        {
            HandleBeastCollision(gameObject);
        }
        else
        {
            Debug.LogWarning("Test Light Effect can only be run in Play Mode");
        }
    }
}