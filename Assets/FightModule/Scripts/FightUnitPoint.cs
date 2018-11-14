using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FightUnitPoint : MonoBehaviour {

    [SerializeField]
    private Text countUnit;
    

    public Squad Squad { get; private set; }
    public bool IsHeroUnit { get; private set; }
    public bool IsActiveSkillSelected { get; set; }
    public int DamageDone { get; private set; }
    public bool SkillUsed { get; private set; }

    private MeshRenderer mesh;
    private Canvas countCanvas;

    public MeshRenderer Mesh
    {
        get
        {
            if (!mesh)
            {
                mesh = GetComponent<MeshRenderer>();
            }
            return mesh;
        }
    }

    private GameObject model;

    
    private bool isMoveOnRound;
    

    public bool IsMoveOnRound
    {
        get
        {
            if(Squad == null)
            {

                return true;
            }
            else
            {
                return isMoveOnRound;
            }
        }
        set
        {
            isMoveOnRound = value;
        }
    }
    public System.Action<FightUnitPoint> onMouseOverUnit = delegate { };
    public System.Action<FightUnitPoint> onMouseDownUnit = delegate {  };


    void Start()
    {
        countCanvas = GetComponentInChildren<Canvas>();
        countCanvas.gameObject.SetActive(false);
        mesh = GetComponent<MeshRenderer>();
    }

    public void SpawnSquad(Squad squad, bool isHeroUnit)
    {
        Squad = squad;
        IsHeroUnit = isHeroUnit;
        model = Instantiate(squad.unitInfo.unitModele, transform);
        countUnit.text = squad.Count.ToString();
        countCanvas.gameObject.SetActive(true);

        squad.OnDeath += OnDeathSquad;
        

    }
    public bool TryAction(FightUnitPoint unitToAction)
    {
        if(unitToAction.Squad == null)
        {
            return false;
        }
        if (IsActiveSkillSelected)
        {
            return TryUseSkill(unitToAction);
        }
        if (unitToAction.IsHeroUnit != IsHeroUnit)
        {
            if (unitToAction == this)
            {;
                return false;
            }
            Attack(unitToAction);
            return true;
        }
        return false;
    }
    bool TryUseSkill(FightUnitPoint targetUnit)
    {
        switch (Squad.unitInfo.activeSkill)
        {
            case UnitInfo.ActiveSkill.Heal:
                if(targetUnit.IsHeroUnit == IsHeroUnit)
                {
                    if (targetUnit == this)
                    {
                        return false;
                    }
                    targetUnit.Squad.CurrentHp += (targetUnit.Squad.unitInfo.unitHp * Squad.unitInfo.healPercent)/100;
                    SkillUsed = true;
                    return true;
                }
                break;
            case UnitInfo.ActiveSkill.Damage:
                if (targetUnit.IsHeroUnit != IsHeroUnit)
                {
                    if (targetUnit == this)
                    {
                        return false;
                    }
                    targetUnit.SetDamage(Squad.unitInfo.damageConst + Squad.unitInfo.damageKoef*DamageDone);
                    SkillUsed = true;
                    return true;
                }
                break;
            case UnitInfo.ActiveSkill.AoE:
                int damage = Squad.unitInfo.AoEDamage;
                FightModule.Instance.GetAllUnit().ForEach((FightUnitPoint unit) =>
                {
                    if (unit.Squad != null)
                    {
                         //на случай если умирает юнит использующий скилл
                        unit.SetDamage(damage);
                    }
                });
                SkillUsed = true;
                return true;
            default:
                Debug.LogError("Что-то пошло не так");
                return false;
        }
        return false;
    }
    void Attack(FightUnitPoint unitToAttack)
    {        
        DamageDone += Squad.Attack;
        unitToAttack.SetDamage(Squad.Attack);
    }
    public void SetDamage(int damage)
    {
        Squad.CurrentHp -= damage;
        if (Squad != null)
        {
            countUnit.text = Squad.Count.ToString();
        }
        
    }
    void OnDeathSquad(Squad squad)
    {
        ClearPoint();
    }
    public void ClearPoint()
    {
        if (model)
        {
            Destroy(model);
        }
        if (Squad!= null)
        {            
            Squad.CurrentHp = Squad.Count * Squad.unitInfo.unitHp;
                      
            Squad.OnDeath -= OnDeathSquad;
            Squad = null;
        }       
        onMouseDownUnit = delegate { };
        DamageDone = 0;
        SkillUsed = false;
        countCanvas.gameObject.SetActive(false);
    }
    void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject(0))
        {
            return;
        }
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        onMouseDownUnit.Invoke(this);
    }
    void OnMouseOver()
    {
        if (EventSystem.current.IsPointerOverGameObject(0))
        {
            return;
        }
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        onMouseOverUnit.Invoke(this);
    }
}
