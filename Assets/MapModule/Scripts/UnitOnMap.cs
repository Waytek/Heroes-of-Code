using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitOnMap : MonoBehaviour {
    [SerializeField]
    private List<Squad> army = new List<Squad>();

    public System.Action<UnitOnMap> StayBesideAction = delegate { };
    public System.Action<UnitOnMap> OnDead = delegate { };

    private HeroViewMovable heroViewMovable;

    public PathfindingNode CurrentNode {
        get {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up * 10, Vector3.down, out hit, Mathf.Infinity, ~ignoreLayerMask))
            {
                return hit.transform.GetComponent<PathfindingNode>();
            }            
            return null;            
        }
    }

    [SerializeField]
    private LayerMask ignoreLayerMask;

    void Awake()
    {
        army.ForEach((Squad squad) => {

            squad.CurrentHp = squad.Count * squad.unitInfo.unitHp;
            squad.OnDeath += OnDeadSquad;
        });
    }
    public bool OnNode(PathfindingNode node)
    {
        if(transform.position.x == node.transform.position.x && transform.position.z == node.transform.position.z)
        {
            
            node.Unit = this;
            return true;
        }
        return false;
    }
    public List<Squad> GetArmy()
    {
        return army;
    }
    public void AddSquad(Squad squad)
    {
        army.Add(squad);
        squad.OnDeath += OnDeadSquad;
    }
    void OnDeadSquad(Squad squad)
    {
        army.Remove(squad);
        if(army.Count == 0)
        {
            OnDead.Invoke(this);
            Destroy(gameObject);
        }
    }
    public void OnEndFight()
    {
        army.ForEach((Squad squad) => squad.CurrentHp = squad.Count * squad.unitInfo.unitHp);
    }
    void OnMouseEnter()
    {
        if (!heroViewMovable)
        {
            heroViewMovable = HeroViewMovable.Create(ViewStructHelper.Instance.basePanel);
        }
        heroViewMovable.Activate(this);
    }
    void OnMouseExit()
    {
        if (heroViewMovable)
        {
            heroViewMovable.Deactivate(this);
        }
    }
}
