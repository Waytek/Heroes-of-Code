using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroArmyRecrutingView : baseView<HeroArmyRecrutingView> {

    [SerializeField]
    private List<SquadAddView> sqadAddViewList = new List<SquadAddView>();
    [SerializeField]
    private Button close;
    //public List<Button>
    HeroView curHeroView;

    void Awake()
    {
        sqadAddViewList.ForEach((SquadAddView squadAddView) => squadAddView.OnAddSquadClick += AddSquad);
        close.onClick.AddListener(() => Deactivate(null));
    }
    public override void  Activate(object controller)
    {
        curHeroView = (HeroView)controller;
        gameObject.SetActive(true);
        sqadAddViewList.ForEach((SquadAddView squadAddView) => squadAddView.UpdateAcces(curHeroView.curHero.GetArmy()));
    }
    public override void  Deactivate(object controller)
    {
        gameObject.SetActive(false);
    }
    void AddSquad(UnitInfo unitInfo, int count)
    {

        Squad squad = curHeroView.curHero.GetArmy().Find((Squad heroSquad) => heroSquad.unitInfo == unitInfo);
        if (squad!=null)
        {
            squad.Count += count;
        }
        else
        {
            squad = new Squad(unitInfo, count);
            curHeroView.curHero.AddSquad(squad);
        }
        sqadAddViewList.ForEach((SquadAddView squadAddView) => squadAddView.UpdateAcces(curHeroView.curHero.GetArmy()));
        curHeroView.UpdateHeroArmy();
    }
    
}
