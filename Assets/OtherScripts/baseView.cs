using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class baseView<T> : MonoBehaviour {

    static T view;

    void Start()
    {
        
    }

    private static void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
    {
        view = default(T);
    }

    public static T Create(Transform parent)
    {
       
        if (view == null)
        {
            GameObject newObject = Instantiate(Resources.Load("Prefabs/View/" + typeof(T)),parent) as GameObject;
            view = newObject.GetComponent<T>();
            SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
        }
        else
        {            
            (view as baseView<T>).GetComponent<Transform>().transform.parent = parent;
        }        
        return view;
    }
    public abstract void Activate(object controller);
    public abstract void Deactivate(object controller);

}
