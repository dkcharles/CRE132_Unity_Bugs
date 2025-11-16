using UnityEngine;

public enum BigBossState
{
    Idle,
    Attacking

}

public class BigBoss_AI : MonoBehaviour
{
    public Transform playerPosition;
    int hysteresis = 2;
    SpriteRenderer spriteRenderer;
    public int attackDistance; 
    // Current state of the Big Boss
    public BigBossState currentState = BigBossState.Idle;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        // get distance to player
        float distanceToPlayer = Vector2.Distance(transform.position, playerPosition.position);
        switch (currentState)
        {
            case BigBossState.Idle:
                if (distanceToPlayer < attackDistance)
                {
                    currentState = BigBossState.Attacking;
                    spriteRenderer.color = Color.red;
                }
                break;

            case BigBossState.Attacking:
                if (distanceToPlayer > attackDistance + hysteresis)
                {
                    currentState = BigBossState.Idle;
                    spriteRenderer.color = Color.white;
                }
                break;
        }
    }
}
