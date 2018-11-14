using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinLooseModule : MonoBehaviour {
    [SerializeField]
    private GameObject winWindows;
    [SerializeField]
    private GameObject looseWindows;

    public List<UnitOnMap> AllUnits = new List<UnitOnMap>();
	
	void Start () {
        AllUnits.AddRange(FindObjectsOfType<UnitOnMap>());
        AllUnits.ForEach((UnitOnMap unit) => unit.OnDead += OnAnyUnitDead);
    }
    void OnAnyUnitDead(UnitOnMap deadUnit)
    {
        AllUnits.Remove(deadUnit);
        StartCoroutine(CheckEndGame());
    }
    IEnumerator CheckEndGame()
    {
        if (AllUnits.Find((UnitOnMap unit) => unit is Hero) == null)
        {
            yield return new WaitForSeconds(1);
            looseWindows.SetActive(true);
        }
        else
        {
            if (AllUnits.Find((UnitOnMap unit) => !(unit is Hero)) == null)
            {
                yield return new WaitForSeconds(1);
                winWindows.SetActive(true);
            }
        }
    }    
    public void RestartGame()
    {
        SceneManager.LoadScene(0);
        
    }
}
