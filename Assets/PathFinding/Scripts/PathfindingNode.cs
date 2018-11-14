
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PathfindingNode : MonoBehaviour {

	public int X { get; set; }
	public int Y { get; set; }
	public int Cost { get; set; }
	public bool IsLock { get; set; }
    public UnitOnMap Unit { get; set; }

    public MeshRenderer Mesh; // указываем меш клетки
    public BoxCollider BoxColider;

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
        if (PathfindingField.Instance.HasPath && PathfindingField.Instance.EndNode == this)
        {
            PathfindingField.Instance.StartMove();
        }
        else
        {
            PathfindingField.Instance.FindPath(this);
        }
       
        
    }

}
