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
    [SerializeField] private float    _skillCooldown;
    [SerializeField] private float    _skillCost;
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
    public float SkillCooldown => _skillCooldown;
    public float SkillCost => _skillCost;
    public GameObject SkillPrefab => _skillPrefab;
    public int SkillId => _skillId;
    public SkillType Skill => _skillType;


}
