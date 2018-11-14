using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightModule : MonoBehaviour {
    public static FightModule Instance;
    [SerializeField]
    private Camera fightModuleCamera;
    [SerializeField]
    private List<FightUnitPoint> heroArmyPoints = new List<FightUnitPoint>();
    [SerializeField]
    private List<FightUnitPoint> enemyArmyPoints = new List<FightUnitPoint>();
    [SerializeField]
    private Color defaultColor; // цвет клетки по умолчанию
    [SerializeField]
    private Color selectedColor; // цвет клетки с выбраным юнитом

    private Hero hero;
    private UnitOnMap enemy;
    private Camera camBeforeFight;
    private bool isHeroTurn;
    private FightUnitPoint curUnit;
    private ActiveSkillView view;

    public System.Action OnEndFight = delegate { };

    void Awake()
    {
        Instance = this;
    }
    
    public void LoadFightModule(Hero hero, UnitOnMap enemy)
    {
        this.hero = hero;
        this.enemy = enemy;
        OnEndFight += hero.OnEndFight;
        OnEndFight += enemy.OnEndFight;
        hero.UnselectHero();
        
        for(int i = 0; i < hero.GetArmy().Count; i++)
        {
            heroArmyPoints[i].SpawnSquad(hero.GetArmy()[i],true);
            hero.GetArmy()[i].OnDeath += OnAnyDead;
            heroArmyPoints[i].onMouseDownUnit += OnUnitClick;
            heroArmyPoints[i].onMouseOverUnit += OnMoseOverUnit;
        }
        for (int i = 0; i < enemy.GetArmy().Count; i++)
        {
            enemyArmyPoints[i].SpawnSquad(enemy.GetArmy()[i],false);
            enemy.GetArmy()[i].OnDeath += OnAnyDead;
            enemyArmyPoints[i].onMouseDownUnit += OnUnitClick;
            enemyArmyPoints[i].onMouseOverUnit += OnMoseOverUnit;
        }
        isHeroTurn = true;
        List<Camera> cameras =new List<Camera>(FindObjectsOfType<Camera>());
        camBeforeFight = cameras.Find((Camera cam) => cam.gameObject.activeInHierarchy);
        camBeforeFight.gameObject.SetActive(false);
        fightModuleCamera.gameObject.SetActive(true);
        SelectUnit(GetCurrentMoveUnit(isHeroTurn));
    }
    FightUnitPoint GetCurrentMoveUnit(bool isHeroTurn)
    {

        if (isHeroTurn)
        {
            return heroArmyPoints.Find((FightUnitPoint unitPoint) => !unitPoint.IsMoveOnRound);
        }
        else
        {
            return enemyArmyPoints.Find((FightUnitPoint unitPoint) => !unitPoint.IsMoveOnRound);
        }

    }
    void ChangeTurn()
    {
        if (curUnit)
        {
            curUnit.IsMoveOnRound = true;
        }
        
        isHeroTurn = !isHeroTurn;
        FightUnitPoint tempUnit = GetCurrentMoveUnit(isHeroTurn);
        if(tempUnit != null)
        {
            SelectUnit(tempUnit);            
        }
        else
        {
            if (CheckEndRound())
            {
                ChangeRound();
                SelectUnit(GetCurrentMoveUnit(isHeroTurn));
            }
            else
            {
                ChangeTurn();
            }
        }
    }
    void SelectUnit(FightUnitPoint unitPoint)
    {
        if (!unitPoint)
        {
            return;
        }
        if (curUnit)
        {
            if (curUnit.IsHeroUnit)
            {
                if (view)
                {
                    view.Deactivate(unitPoint);
                }
                    
            }            
            curUnit.Mesh.material.color = defaultColor;
        }
                
        curUnit = unitPoint;

        curUnit.Mesh.material.color = selectedColor;
        if (curUnit.IsHeroUnit)
        {
            if (curUnit.Squad.unitInfo.activeSkill != UnitInfo.ActiveSkill.None)
            {
                if (!view)
                {
                    view = ActiveSkillView.Create(ViewStructHelper.Instance.basePanel);
                }

                view.Activate(unitPoint);
            }
           
        }
        else
        {
            FightAI.MoveAI(curUnit, heroArmyPoints, ChangeTurn);
        }       
    }
    void ChangeRound()
    {
        heroArmyPoints.ForEach((FightUnitPoint unit) => unit.IsMoveOnRound = false);
        enemyArmyPoints.ForEach((FightUnitPoint unit) => unit.IsMoveOnRound = false);        
    }
    bool CheckEndRound()
    {
        bool heroCanMove = heroArmyPoints.Exists((FightUnitPoint unit) => !unit.IsMoveOnRound);
        bool enemyCanMove = enemyArmyPoints.Exists((FightUnitPoint unit) => !unit.IsMoveOnRound);
        if(!heroCanMove && !enemyCanMove)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    bool CheckEndFight(bool isHeroTurn)
    {
        if (isHeroTurn)
        {
            return !enemyArmyPoints.Exists((FightUnitPoint unit) => unit.Squad != null);
        }
        else
        {
            return !heroArmyPoints.Exists((FightUnitPoint unit) => unit.Squad != null);
        }
    }
    void EndFight()
    {
        camBeforeFight.gameObject.SetActive(true);
        fightModuleCamera.gameObject.SetActive(false);
        heroArmyPoints.ForEach((FightUnitPoint point) => point.ClearPoint());
        enemyArmyPoints.ForEach((FightUnitPoint point) => point.ClearPoint());
        if (view)
        {
            view.Deactivate(this);
        }
        OnEndFight.Invoke();        
        for (int i = 0; i < hero.GetArmy().Count; i++)
        {            
            hero.GetArmy()[i].OnDeath -= OnAnyDead;
            heroArmyPoints[i].onMouseDownUnit -= OnUnitClick;
            heroArmyPoints[i].onMouseOverUnit -= OnMoseOverUnit;
        }
        for (int i = 0; i < enemy.GetArmy().Count; i++)
        {
            enemy.GetArmy()[i].OnDeath -= OnAnyDead;
            enemyArmyPoints[i].onMouseDownUnit -= OnUnitClick;
            enemyArmyPoints[i].onMouseOverUnit -= OnMoseOverUnit;
        }
        OnEndFight -= hero.OnEndFight;
        OnEndFight -= enemy.OnEndFight;
    }
    void OnMoseOverUnit(FightUnitPoint unit)
    {
        //тут планируется менять указатель на "меч" при наведении на врага


    }
    void OnUnitClick(FightUnitPoint unit)
    {
        if(curUnit == null)
        {
            return;
        }
        if (!curUnit.IsHeroUnit)
        {
            return;
        }
        if(curUnit.TryAction(unit))
        {
            ChangeTurn();
        }
    }
    public List<FightUnitPoint> GetAllUnit()
    {
        List<FightUnitPoint> allUnit = new List<FightUnitPoint>();
        allUnit.AddRange(heroArmyPoints);
        allUnit.AddRange(enemyArmyPoints);
        return allUnit;
    }
    void OnAnyDead(Squad squad)
    {
        if (CheckEndFight(isHeroTurn))
        {
            EndFight();
        }
    }

}
