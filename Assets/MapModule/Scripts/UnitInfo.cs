using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUnit", menuName = "Army/Create Unit", order = 1)]
public class UnitInfo : ScriptableObject {

    public Sprite unitIcon;
    public GameObject unitModele;
    public string unitName;
    public int unitHp;
    public int unitAttack;
    public enum ActiveSkill
    {
        None,
        Heal,
        Damage,
        AoE
    }
    public ActiveSkill activeSkill;
    public Sprite activeSkillIcon;

    #region HealSkill
    public int healPercent = 100;
    #endregion

    #region DamageSkill
    public int damageConst = 1;
    public int damageKoef = 1;
    #endregion

    #region AoE Skill
    public int AoEDamage = 10;
    #endregion
    
}
