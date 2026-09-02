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
    IEnemyAttack myAttack;
    float attackCD;
    [SerializeField]float attackMaxCD;
    [SerializeField]float attackInitCD;
    [SerializeField] bool isRangedEnemy = false;


    private void Awake()
    {
        attackCD = attackInitCD;
        myAttack = GetComponent<IEnemyAttack>();
    }

    private void Update()
    {
        sightCheck();
        if(!playerInSight) playerObjective = null;
        playerInAttack = false;
        inAttackCheck();
        handleAttack();
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
    void sightCheck()
    {
        enemySight = Physics.OverlapSphere(transform.position, sightRange);
        playerInSight = false;
        foreach (Collider col in enemySight)
        {
            if (col.CompareTag("Player"))
            {
                playerObjective = col.gameObject;
                playerInSight = true;
            }
        }
    }
    void inAttackCheck()
    {
        attackSight = Physics.OverlapSphere(transform.position, attackRange);
        foreach (Collider col in attackSight)
        {
            if (col.gameObject == playerObjective) playerInAttack = true;
        }
    }
    void handleAttack()
    {
        if (playerObjective == null) return;
        Vector3 direction = playerObjective.transform.position - transform.position;
        direction.y = 0;
        transform.rotation = Quaternion.LookRotation(direction);
        if (!playerInAttack) attackCD = attackInitCD;
        attackCD -= Time.deltaTime;
        if (attackCD <= 0)
        {
            if (isRangedEnemy) GetComponent<RangedAttack>().setTarget(playerObjective.transform.position);
            myAttack.EnemyAttack();
            attackCD = attackMaxCD;
        }
    }
}
