
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingField : MonoBehaviour {
    public static PathfindingField Instance;
	[SerializeField] private Color defaultColor; // цвет клетки по умолчанию
	[SerializeField] private Color pathColor; // подсветка пути
	[SerializeField] private Color cursorColor; // подсветка указателя
	[SerializeField] private LayerMask layerMask; // маска клетки
	[SerializeField] private int width;
	[SerializeField] private int height;
	[SerializeField] [Range(1f, 10f)] private float moveSpeed = 1;
	[SerializeField] [Range(0.1f, 1f)] private float rotationSpeed = 0.25f;
	[SerializeField] private PathfindingNode[] grid;

	public bool IsMove { get; set; }
    public bool HasPath { get; set; }
    public PathfindingNode EndNode { get; set; }

    private bool breakMove;
	private PathfindingNode[,] map;
    private List<PathfindingNode> tempNodeList = new List<PathfindingNode>();
    private Hero _selectedUnit;



    public Hero selectedUnit {
        get
        {
            return _selectedUnit;
        }
        set
        {
            if (IsMove) return;

            HasPath = false;
            EndNode = null;
            FieldUpdate();
            _selectedUnit = value;
        }
    }


    void Awake()
	{
		Instance = this;
		BuildMap();
	}

	// обновление состояния клеток поля, эту функцию нужно вызывать, если на поле появляются новые объекты или уничтожаются имеющиеся
	public static void UpdateNodeState()
	{
		Instance.UpdateNodeState_inst();
	}

	void BuildMap() // инициализация двумерного массива
	{
		map = new PathfindingNode[width, height];
		int i = 0;
		for(int y = 0; y < height; y++)
		{
			for(int x = 0; x < width; x++)
			{
				grid[i].X = x;
				grid[i].Y = y;
				map[x,y] = grid[i];
				i++;
			}
		}

		UpdateNodeState_inst();
	}

	void UpdateNodeState_inst() // обновления поля, после совершения действия
	{
		for(int i = 0; i < grid.Length; i++)
		{
			RaycastHit hit; // пускаем луч сверху на клетку, проверяем занята она или нет
            //Physics.BoxCast(grid[i].transform.position + Vector3.up * 100f, grid[i].boxColider.bounds.size/2, Vector3.down, out hit,Quaternion.identity, 100f, ~layerMask);
			Physics.Raycast(grid[i].transform.position + Vector3.up * 100f, Vector3.down, out hit, 100f, ~layerMask);

			if(hit.collider == null) // пустая клетка
			{
				grid[i].IsLock = false;
				grid[i].Cost = -1; // свободное место
			}
			else if(hit.collider.tag == "Player") // найден юнит
			{
				//grid[i].target = hit.transform;
				grid[i].IsLock = true;
                grid[i].Unit = hit.collider.GetComponent<UnitOnMap>();
				grid[i].Cost = -2; // препятствие
            }
            else if (hit.collider.tag == "UnitOnMap") // найден юнит
            {
                //grid[i].target = hit.transform;
                grid[i].IsLock = true;
                grid[i].Unit = hit.collider.GetComponent<UnitOnMap>();
                grid[i].Cost = -2; // препятствие

            }
            else // любой другой объект/препятствие
			{                
				grid[i].IsLock = true;
				grid[i].Cost = -2;
			}

			grid[i].Mesh.material.color = defaultColor;
		}
	}

	void FieldUpdate() // обновление поля, перед подсветкой пути
	{
		for(int i = 0; i < grid.Length; i++)
		{
			if(grid[i].IsLock)
			{
				grid[i].Cost = -2;
				grid[i].Mesh.material.color = defaultColor;
			}
			else
			{
				grid[i].Mesh.material.color = defaultColor;
				grid[i].Cost = -1;
			}
		}
	}
    public void FindPath(PathfindingNode endNode)
    {
        this.EndNode = endNode;
        if (selectedUnit)
        {
            selectedUnit.CurrentNode.Cost = -1;
            tempNodeList = Pathfinding.Find(selectedUnit.CurrentNode, endNode, map, width, height);
            if (tempNodeList != null)
            {
                FieldUpdate();
                selectedUnit.CurrentNode.Mesh.material.color = pathColor;
                tempNodeList.ForEach((PathfindingNode pathNode) => pathNode.Mesh.material.color = pathColor);
                HasPath = true;
            }
            else
            {
                HasPath = false;
            }
        }
        else
        {
            HasPath = false;
        }
    }
    public void StartMove()
    {
        if (HasPath)
        {
            if (Input.GetMouseButtonDown(0))
            {
                selectedUnit.CurrentNode.Mesh.material.color = defaultColor;
                StartCoroutine(Move(tempNodeList));
                HasPath = false;
            }
        }
    }
    void Update()
    {
        if (IsMove)
        {            
            return;
        }
        if (Input.GetMouseButtonDown(1))
        {
            if(selectedUnit)
                selectedUnit.UnselectHero();
        }
               
    }
    IEnumerator Move(List<PathfindingNode> path)
    {
        IsMove = true;
        for (int step = 0; step < path.Count; step++)
        {
            selectedUnit.transform.LookAt(new Vector3(path[step].transform.position.x, selectedUnit.transform.position.y, path[step].transform.position.z));
            
            if (path[step].Unit || path[step].Cost == -2) break;
            selectedUnit.CurrentNode.Unit = null;
            path[step].Unit = null;            
            while (!selectedUnit.OnNode(path[step]))
            {
                Vector3 newPos = Vector3.MoveTowards(selectedUnit.transform.position, path[step].transform.position, moveSpeed*Time.deltaTime);
                selectedUnit.transform.position = new Vector3(newPos.x, selectedUnit.transform.position.y, newPos.z);
                yield return null;
            }
            PathfindingNode actionNode =  Pathfinding.GetSurroundNodes(path[step], map, width, height).Find((PathfindingNode node) => node.Unit != null );
            if (actionNode)
            {
                selectedUnit.StayBesideAction.Invoke(actionNode.Unit);
                breakMove = true;
            }
            path[step].Mesh.material.color = defaultColor;

            if (breakMove)
                break;
        }
        breakMove = false;
        IsMove = false;
        //FieldUpdate();
        UpdateNodeState_inst();
    }
	#if UNITY_EDITOR
	[SerializeField] private PathfindingNode sample; // шаблон клетки
	[SerializeField] private float sampleSize = 1; // размер клетки
	public void CreateGrid()
	{
		for(int i = 0; i < grid.Length; i++)
		{
			if(grid[i] != null) DestroyImmediate(grid[i].gameObject);
		}

		grid = new PathfindingNode[width * height];

		float posX = -sampleSize * width / 2f - sampleSize / 2f;
		float posY = sampleSize * height / 2f - sampleSize / 2f;
		float Xreset = posX;
		int z = 0;
		for(int y = 0; y < height; y++)
		{
			posY -= sampleSize;
			for(int x = 0; x < width; x++)
			{
				posX += sampleSize;
				PathfindingNode clone = Instantiate(sample, new Vector3(posX, 0, posY), Quaternion.identity, transform) as PathfindingNode;
				clone.transform.name = "Node-" + z;
				grid[z] = clone;
				z++;
			}
			posX = Xreset;
		}
	}
	#endif
}
