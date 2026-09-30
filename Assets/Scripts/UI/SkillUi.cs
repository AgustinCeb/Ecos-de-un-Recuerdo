using UnityEngine;

public class SkillUi : MonoBehaviour
{
    
    [SerializeField] private Skill_UI_Slot _skillCont1;
    [SerializeField] private Skill_UI_Slot _skillCont2;
    [SerializeField] private Skill_UI_Slot _skillCont3;

    
    public void SetSkill(Skilldata skill1, Skilldata skill2, Skilldata skill3)
    {
        _skillCont1.SetSkill(skill1);
        _skillCont2.SetSkill(skill2);
        _skillCont3.SetSkill(skill3);

    }

    public void SetSkill1Unlocked(bool unlocked)
    {
        _skillCont1.SetSkillUnlocked(unlocked);
    }

    public void SetSkill2Unlocked(bool unlocked)
    {
        _skillCont2.SetSkillUnlocked(unlocked);
    }

    public void SetSkill3Unlocked(bool unlocked)
    {
        _skillCont3.SetSkillUnlocked(unlocked);
    }

}
