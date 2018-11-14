using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroView : baseView<HeroView> {
    [SerializeField]
    private Image icon;
    [SerializeField]
    private Text heroName;
    [SerializeField]
    private List<SquadImage> squadList = new List<SquadImage>();
    [SerializeField]
    private Button createArmyBtn;

    public Hero curHero { get; private set; }

    private HeroArmyRecrutingView recrutingView;

    

    public override void Activate(object controller)
    {
        gameObject.SetActive(true);
        curHero = controller as Hero;
        icon.sprite = curHero.Icon;
        heroName.text = curHero.HeroName;
        UpdateHeroArmy();
        createArmyBtn.onClick.RemoveAllListeners();
        createArmyBtn.onClick.AddListener(delegate { OnCreateArmyClick(curHero); });
    }
    public override void Deactivate(object controller)
    {
        if(recrutingView)
            recrutingView.Deactivate(null);
        gameObject.SetActive(false);
        
    }
    void OnCreateArmyClick(Hero hero)
    {
       
        if (!recrutingView)
            recrutingView = HeroArmyRecrutingView.Create(ViewStructHelper.Instance.basePanel);
        recrutingView.Activate(this);
    }
    public void UpdateHeroArmy()
    {
        for (int i = 0; i < squadList.Count; i++)
        {
            if (i <= curHero.GetArmy().Count - 1)
            {
                squadList[i].Activate(curHero.GetArmy()[i]);
            }
            else
            {
                squadList[i].Deactivate();
            }
        }
    }
}
