using UnityEngine;

public class BossAnimation : MonoBehaviour
{
    [SerializeField] Animator animator;

    public void AnimAttack(int type)
    {
        if (type == 0) animator.SetTrigger("MeleeAttack");
        if (type == 1) animator.SetTrigger("RangedAttack");
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
