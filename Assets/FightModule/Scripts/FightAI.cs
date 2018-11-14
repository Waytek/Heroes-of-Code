using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public static class FightAI {
    static int waitTimeMs = 1000;
	
    public static void MoveAI(FightUnitPoint selectedUnit, List<FightUnitPoint> enemyList, System.Action onMove)
    {
        System.Action waitMove = () =>
        {
            Thread.Sleep(waitTimeMs);
            Threading.Execute(delegate
            {
                selectedUnit.TryAction(enemyList.Find((FightUnitPoint unit) => unit.Squad != null));
                onMove.Invoke();                
            });
            
        };
        Thread waitThread = new Thread(new ThreadStart(waitMove));
        waitThread.Start();
        
    }
}
