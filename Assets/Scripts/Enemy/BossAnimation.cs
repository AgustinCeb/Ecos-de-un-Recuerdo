using UnityEngine;

public class BossAnimation : MonoBehaviour
{
    [SerializeField] Animator animator;

    public void AnimAttack(int type)
    {
        animator.SetTrigger("Attack");
    }
    public void SetState(int state)
    {
        animator.SetInteger("State", state);
    }
    public void AnimDeath()
    {
        animator.SetTrigger("Death");
    }
}
