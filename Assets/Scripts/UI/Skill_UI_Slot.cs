using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Skill_UI_Slot : MonoBehaviour
{
    [SerializeField] private Image _skillIcon;
    [SerializeField] private TextMeshProUGUI _manaCostText;

    public void SetSkill(Skilldata skilldata)
    {
        _skillIcon.sprite = skilldata.SkillIcon;
        _manaCostText.text = skilldata.SkillCost.ToString();

    }

}
