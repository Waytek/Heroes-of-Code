using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroViewMovable : baseView<HeroViewMovable> {

    [SerializeField]
    private Image icon;
    [SerializeField]
    private Text unitName;
    [SerializeField]
    private List<SquadImage> squadList = new List<SquadImage>();

    private RectTransform rectTransform;
    private UnitOnMap curUnit;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public override void Activate(object controller)
    {
        if(controller is Hero)
        {
            return;
        }
        gameObject.SetActive(true);
        curUnit = controller as UnitOnMap;
        icon.sprite = curUnit.GetArmy()[0].unitInfo.unitIcon;
        unitName.text = curUnit.GetArmy()[0].unitInfo.unitName;
        UpdateHeroArmy();
    }
    public override void Deactivate(object controller)
    {
        gameObject.SetActive(false);
    }
    void Update()
    {
        if(Input.mousePosition.x < Screen.width / 2)
        {
            if (Input.mousePosition.y < Screen.height / 2)
            {
                rectTransform.pivot = new Vector2(0, 0);
            }
            else
            {
                rectTransform.pivot = new Vector2(0, 1);
            }
                
        }
        else
        {
            if (Input.mousePosition.y < Screen.height / 2)
            {
                rectTransform.pivot = new Vector2(1, 0);
            }
            else
            {
                rectTransform.pivot = new Vector2(1, 1);
            }
        }

        rectTransform.position = Input.mousePosition;
    }
    public void UpdateHeroArmy()
    {
        for (int i = 0; i < squadList.Count; i++)
        {
            if (i <= curUnit.GetArmy().Count - 1)
            {
                squadList[i].Activate(curUnit.GetArmy()[i]);
            }
            else
            {
                squadList[i].Deactivate();
            }
        }
    }
}
