using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    Collider[] enemySight;
    [SerializeField] float sightRange;
    GameObject playerObjective;
    [SerializeField] float Speed;
    Collider[] attackSight;
    [SerializeField] float attackRange;
    bool playerInSight;
    bool playerInAttack;


    private void Awake()
    {

    }

    private void Update()
    {
        enemySight = Physics.OverlapSphere(transform.position, sightRange);
        playerInSight=false;
        foreach (Collider col in enemySight)
        {
            if(col.CompareTag("Player"))
            {
                playerObjective = col.gameObject;
                playerInSight = true;
            }
        }
        if(!playerInSight) playerObjective = null;
        playerInAttack = false;

        attackSight = Physics.OverlapSphere(transform.position, attackRange);
        foreach (Collider col in attackSight)
        {
            if (col.gameObject == playerObjective) playerInAttack = true;
        }
        if(!playerInAttack) MoveTowardPlayer();
    }
    private void MoveTowardPlayer()
    {
        if (playerObjective != null)
        {
            Vector3 direction = playerObjective.transform.position - transform.position;
            direction.y = 0;
            transform.rotation = Quaternion.LookRotation(direction);
            transform.position += transform.forward * Speed * Time.deltaTime;
        }
    }
}
