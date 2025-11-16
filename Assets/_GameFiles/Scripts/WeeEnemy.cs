using UnityEngine;

public class WeeEnemy : MonoBehaviour
{
    public Transform playerPosition;
    Vector2 directionToPlayer;
    public int EnemySpeed = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    { }

    // Update is called once per frame
    void Update()
    {
        // get direction to player
        if (playerPosition != null)
        {
            // normalize the direction to player so we get a unit vector, i.e. a vector with a magnitude of 1
            directionToPlayer = (playerPosition.position - transform.position).normalized;
        }

        if (playerPosition.position - transform.position != Vector3.zero) MoveToPlayer(EnemySpeed);

    }

    public void MoveToPlayer(float speed)
    {
        // move the enemy towards the player by the speed and the direction to the player
        Vector3 movement = directionToPlayer * speed * Time.deltaTime;
        // we need to use a 3D vector to move the enemy towards the player since transform.position is a 3D vector 
        // and we need to move in the z axis as well so we need to add the movement to the x and y axes and the z axis is 0 since we are not moving in the z axis
        transform.position = new Vector3(transform.position.x + movement.x, transform.position.y + movement.y, 0);
        
    }
}
