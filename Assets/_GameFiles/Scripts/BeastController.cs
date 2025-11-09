using UnityEngine;
using System;

public class BeastController : MonoBehaviour
{
    // Event that fires when Beast collides with a solid object
    public static event Action<GameObject> OnBeastCollision;

    [Header("Movement Settings")]
    [SerializeField] private float speed = 3f;

    [Header("Custom Physics")]
    [SerializeField] private Vector2 velocity;
    private Vector2 position;
    private int direction = 1; // 1 = right, -1 = left

    [Header("Collision Settings")]
    [SerializeField] private Vector2 colliderSize = new Vector2(1f, 1f);
    [SerializeField] private Vector2 colliderOffset = Vector2.zero;

    private SpriteRenderer spriteRenderer;
    GameObject[] obstacles;

    void Start()
    {
        // Get all objects with "Solid" tag
        obstacles = GameObject.FindGameObjectsWithTag("Solid");
       
        spriteRenderer = GetComponent<SpriteRenderer>();
        position = transform.position;
        velocity = new Vector2(speed * direction, 0);

        // Auto-calculate collider size from sprite if available
        if (spriteRenderer != null && spriteRenderer.sprite != null)
        {
            colliderSize = spriteRenderer.bounds.size;
        }
    }

    void Update()
    {
        // Apply custom physics movement
        ApplyMovement();

        // Check for collisions with obstacles
        CheckCollisions();
    }

    private void ApplyMovement()
    {
        // Update position using velocity and delta time
        position.x += velocity.x * Time.deltaTime;
        position.y += velocity.y * Time.deltaTime;

        // Apply position to transform
        transform.position = position;
    }

    private void CheckCollisions()
    {
        foreach (GameObject obstacle in obstacles)
        {
            float penetrationDepth = GetPenetrationDepth(obstacle);

            if (penetrationDepth > 0)
            {
                // Push the Beast out of the obstacle completely
                position.x -= penetrationDepth * direction;
                transform.position = position;

                // Fire the collision event, passing the obstacle that was hit
                OnBeastCollision?.Invoke(obstacle);

                // Now reverse direction
                ReverseDirection();

                break; // Only process one collision per frame
            }
        }
    }

    private float GetPenetrationDepth(GameObject other)
    {
        // Get the bounds of this Beast
        Vector2 beastCenter = (Vector2)transform.position + colliderOffset;
        Vector2 beastMin = beastCenter - colliderSize / 2f;
        Vector2 beastMax = beastCenter + colliderSize / 2f;

        // Get the bounds of the other object
        SpriteRenderer otherRenderer = other.GetComponent<SpriteRenderer>();
        if (otherRenderer == null) return 0;

        Bounds otherBounds = otherRenderer.bounds;
        Vector2 otherMin = otherBounds.min;
        Vector2 otherMax = otherBounds.max;

        // AABB (Axis-Aligned Bounding Box) collision detection
        bool collisionX = beastMax.x >= otherMin.x && beastMin.x <= otherMax.x;
        bool collisionY = beastMax.y >= otherMin.y && beastMin.y <= otherMax.y;

        if (!collisionX || !collisionY)
            return 0;

        // Calculate penetration depth on X axis
        float penetration;
        if (direction > 0) // Moving right
        {
            penetration = beastMax.x - otherMin.x;
        }
        else // Moving left
        {
            penetration = otherMax.x - beastMin.x;
        }

        return penetration;
    }

    private void ReverseDirection()
    {
        // Flip the direction
        direction *= -1;

        // Update velocity
        velocity.x = speed * direction;

        // Flip the sprite to face the new direction by scaling on X axis
        // This will flip all child sprites as well
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        transform.localScale = scale;
    }

    // Visualize the collision bounds in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector2 center = (Vector2)transform.position + colliderOffset;
        Gizmos.DrawWireCube(center, colliderSize);
    }
}
