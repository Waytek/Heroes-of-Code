using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquadAddView : MonoBehaviour {
    [SerializeField]
    private UnitInfo unitInfo;
    [SerializeField]
    private Image unitInfoImg;
    [SerializeField]
    private InputField countField;
    [SerializeField]
    private Button countIncrement;
    [SerializeField]
    private Button countDecrement;
    [SerializeField]
    private Button addSqad;

    public System.Action<UnitInfo, int> OnAddSquadClick = delegate { };

    private int count = 1;
    

    void Awake()
    {
        if(unitInfo == null)
        {
            gameObject.SetActive(false);
            return;
        }
        unitInfoImg.sprite = unitInfo.unitIcon;
        countField.keyboardType = TouchScreenKeyboardType.NumberPad;
        countField.text = count.ToString();
        countField.onEndEdit.AddListener((string countStr) => count = int.Parse(countStr));
        countIncrement.onClick.AddListener(IncrementCount);
        countDecrement.onClick.AddListener(DecrementCount);
        addSqad.onClick.AddListener(AddSquad);

    }
    public void UpdateAcces(List<Squad> heroArmy)
    {
        Squad squadToAdd = heroArmy.Find((Squad squad) => squad.unitInfo == unitInfo);
        if (squadToAdd!= null)
        {
            addSqad.interactable = true;
            return;
        }
        if(heroArmy.Count < Hero.MaxArmyCount)
        {
            addSqad.interactable = true;
            return;
        }
        addSqad.interactable = false;
    }
    void IncrementCount()
    {
        count++;
        countField.text = count.ToString();
    }
    void DecrementCount()
    {
        count--;
        countField.text = count.ToString();
    }
    void AddSquad()
    {
        OnAddSquadClick.Invoke(unitInfo, count);
    }

}
