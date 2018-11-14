using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewStructHelper : MonoBehaviour {
    public static ViewStructHelper Instance;

    public Transform basePanel;
    public Transform rightPanel;

	void Awake () {
        Instance = this;
    }

}
