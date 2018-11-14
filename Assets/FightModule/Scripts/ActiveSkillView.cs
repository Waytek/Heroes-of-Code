using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActiveSkillView : baseView<ActiveSkillView>
{
    [SerializeField]
    private Button skillBtn;
    [SerializeField]
    private GameObject skillInfoObj;
    [SerializeField]
    private Text skillInfoLable;
    // Use this for initialization
    private FightUnitPoint currFightUnit;

    public override void Activate(object controller)
    {
        gameObject.SetActive(true);
        currFightUnit = (FightUnitPoint)controller;
        skillBtn.interactable = !currFightUnit.SkillUsed;
        skillBtn.image.sprite = currFightUnit.Squad.unitInfo.activeSkillIcon;
        skillBtn.onClick.RemoveAllListeners();
        skillBtn.onClick.AddListener(OnSkillBtnClick);
        skillInfoLable.text = GetSkillInfo();
        skillInfoObj.SetActive(false);
    }

    public override void Deactivate(object controller)
    {
        gameObject.SetActive(false);
        if (currFightUnit)
        {
            currFightUnit.IsActiveSkillSelected = false;
        }        
        skillInfoObj.SetActive(false);
        currFightUnit = null;
    }
    void OnSkillBtnClick()
    {
        currFightUnit.IsActiveSkillSelected = !currFightUnit.IsActiveSkillSelected;
    }    
    public void OnMouseEnter()
    {
        skillInfoObj.SetActive(true);
    }
    public void OnMouseExit()
    {
        skillInfoObj.SetActive(false);
    }
    string GetSkillInfo()
    {
        switch (currFightUnit.Squad.unitInfo.activeSkill)
        {
            case UnitInfo.ActiveSkill.Heal:
                return string.Format("Подлечить выбранный дружественный отряд \n на {0} процентов от максимального значения \n здоровья юнита, из которого состоит отряд.", currFightUnit.Squad.unitInfo.healPercent);
            case UnitInfo.ActiveSkill.Damage:
                return string.Format("Нанести урон выбранному вражескому \n отряду в размере {0}", currFightUnit.Squad.unitInfo.damageConst + currFightUnit.Squad.unitInfo.damageKoef * currFightUnit.DamageDone);
            case UnitInfo.ActiveSkill.AoE:
                return string.Format("Нанести {0} урона всем \n (как вражеским, так и дружественным) отрядам", currFightUnit.Squad.unitInfo.AoEDamage);
            default:
                return "ErrorDefault";
        }
    }
}
