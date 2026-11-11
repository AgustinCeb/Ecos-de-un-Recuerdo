using UnityEngine;

public class BossAI : MonoBehaviour
{
    Collider[] enemySight;
    [SerializeField] float sightRange;
    GameObject playerObjective;
    [SerializeField] float Speed;
    [SerializeField] float meleeRange;
    [SerializeField] float rangedRange;
    IEnemyAttack meleeAttack;
    IEnemyAttack rangedAttack;
    bool playerInSight;
    float attackCD;
    [SerializeField] float attackMaxCD;
    [SerializeField] float attackInitCD;
    [SerializeField] BossAnimation animController;


    private void Awake()
    {
        attackCD = attackInitCD;

        IEnemyAttack[] attacks = GetComponents<IEnemyAttack>();

        meleeAttack = attacks[0];
        rangedAttack = attacks[1];
    }

    private void Update()
    {
        sightCheck();

        if (!playerInSight)
            playerObjective = null;

        handleAttack();

        if (playerObjective != null)
        {
            float distance = Vector3.Distance(
                transform.position,
                playerObjective.transform.position
            );

            if (distance > meleeRange)
                MoveTowardPlayer();
        }
    }
    private void MoveTowardPlayer()
    {
        if (playerObjective != null)
        {
            animController.SetState(1);
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
    void handleAttack()
    {
    if (playerObjective == null) return;

    Vector3 direction = playerObjective.transform.position - transform.position;
    direction.y = 0;

    transform.rotation = Quaternion.LookRotation(direction);

    float distance = direction.magnitude;

    // Fuera del rango de ataque
    if (distance > rangedRange)
    {
        attackCD = attackInitCD;
        return;
    }

    animController.SetState(0);

    attackCD -= Time.deltaTime;

    if (attackCD <= 0)
    {
        if (distance <= meleeRange)
        {
            meleeAttack.EnemyAttack();
            animController.AnimAttack(0);
            }
        else
        {
            rangedAttack.EnemyAttack();
                animController.AnimAttack(1);
            }


        attackCD = attackMaxCD;
    }
    }
}
