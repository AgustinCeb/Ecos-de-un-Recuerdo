using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SkillData", menuName = "Skills/NewSkill")]

public class Skilldata : ScriptableObject
{
    
    public enum SkillType
    {
        Attack,
        Buff,
        Ultimate,

    }
    //Base Info
    [SerializeField] private string _skillName;
    [SerializeField] private Sprite _skillIcon;
    [SerializeField] private string _skillDescription;
    //Damage,cooldown y Mana
    [SerializeField] private int    _skillDamage;
    [SerializeField] private int    _skillCooldown;
    [SerializeField] private int    _skillCost;
    //Prefab
    [SerializeField] private GameObject _skillPrefab;
    //Id
    [SerializeField] private int _skillId;
    //Type
    [SerializeField] private SkillType _skillType;

    public string SkillName => _skillName;
    public Sprite SkillIcon => _skillIcon;
    public string SkillDescription => _skillDescription;
    public int SkillDamage => _skillDamage;
    public int SkillCooldown => _skillCooldown;
    public int SkillCost => _skillCost;
    public GameObject SkillPrefab => _skillPrefab;
    public int SkillId => _skillId;
    public SkillType Skill => _skillType;


}
