using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Hero : UnitOnMap {

    public static readonly int MaxArmyCount = 5;
    public Sprite Icon;
    public string HeroName;    

    HeroView view;

    void Start()
    {
        StayBesideAction += (UnitOnMap besideUnit) => { FightModule.Instance.LoadFightModule(this, besideUnit); };        
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
        if (PathfindingField.Instance.IsMove)
        {
            return;
        }
        SelectHero();
    }
    void OpenHeroMiniView()
    {
        if (!view)
        {
            view = HeroView.Create(ViewStructHelper.Instance.rightPanel);
        }        
        view.Activate(this);        
    }
    void CloseHeroMiniView()
    {
        if (view)
        {
            view.Deactivate(this);
        }
    }
    void SelectHero()
    {
        if (PathfindingField.Instance.selectedUnit)
        {
            PathfindingField.Instance.selectedUnit.UnselectHero();
        }
        OpenHeroMiniView();
        PathfindingField.Instance.selectedUnit = this;
    }
    public void UnselectHero()
    {
        CloseHeroMiniView();
        PathfindingField.Instance.selectedUnit = null;
    }

}
