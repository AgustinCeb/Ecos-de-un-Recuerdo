using UnityEngine;

public class AnimationEventRelay : MonoBehaviour
{
    [SerializeField] private Attack _playerAttack;
    [SerializeField] private SkillSlot _playerSkills;

    public void OnAttack()
    {
        _playerAttack.PlayerAttack();
    }

    public void OnSkill1()
    {
        _playerSkills.Skill1();
    }

    public void Onskill2()
    {
        _playerSkills.Skill2();
    }

    public void OnSkill3()
    {
        _playerSkills.Skill3();
    }

}
