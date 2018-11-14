using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Squad {

    public UnitInfo unitInfo;
    [SerializeField]
    private int _count;
    public int Count
    {
        get
        {
            return _count;
        }
        set
        {
            
            if (value > _count)
            {
                _count = value;
                CurrentHp = value * unitInfo.unitHp;
            }
            else
            {
                _count = value;
            }
            
        }
    }
    private int _currentHp;
    public int CurrentHp
    {
        get
        {
            return _currentHp;
        }
        set
        {
            if (value < Count * unitInfo.unitHp)
            {
                if (value < _currentHp)
                {
                    if (value <= 0)
                    {
                        OnDeath.Invoke(this);
                    }
                    else
                    {
                        Count = (int)Mathf.Ceil((float)value / (float)unitInfo.unitHp);
                    }
                }
                _currentHp = value;
            }
            else
            {
                _currentHp = Count * unitInfo.unitHp;   //не даёт прибавлять кол-во при лечении
            }
            
        }
    }
    public int Attack
    {
        get
        {
            return unitInfo.unitAttack * Count;
        }
    }
    public System.Action<Squad> OnDeath = delegate { };

    public Squad(UnitInfo unitInfo, int count)
    {
        this.unitInfo = unitInfo;
        this.Count = count;
    }
}
