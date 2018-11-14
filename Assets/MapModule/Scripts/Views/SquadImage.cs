using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquadImage : MonoBehaviour
{
    [SerializeField]
    private Text nameLable;
    [SerializeField]
    private Image icon;
    [SerializeField]
    private Text count;

    private Squad curSquad;

    public void Activate(Squad squad)
    {
        curSquad = squad;
        nameLable.gameObject.SetActive(true);
        icon.gameObject.SetActive(true);
        count.gameObject.SetActive(true);
        nameLable.text = curSquad.unitInfo.unitName;
        icon.sprite = curSquad.unitInfo.unitIcon;
        count.text = curSquad.Count.ToString(); 
    }
    public void Deactivate()
    {
        nameLable.gameObject.SetActive(false);
        icon.gameObject.SetActive(false);
        count.gameObject.SetActive(false);
    }
}
