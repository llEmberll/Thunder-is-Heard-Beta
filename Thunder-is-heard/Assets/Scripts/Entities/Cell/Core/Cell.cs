using UnityEngine;

public class Cell : Interactable
{
	public override string Type {
	get
        {
            return "Cell";
        }
	}

    public StateMachine stateMachine = new StateMachine();
    
    public SceneState sceneState;
    
    public Vector2Int position;
    public bool occupied;
    public bool visible;
    
    public MeshRenderer _meshRenderer;
    public Material basicMaterial;
    public Material selectMaterial;

    public ISubsituableCellBehaviour _behaviour;

    public void Awake()
    {
        Free();

        _meshRenderer = gameObject.GetComponent<MeshRenderer>();
        basicMaterial = Resources.Load(Config.resources["defaultCellMaterial"], typeof(Material)) as Material;
        selectMaterial = Resources.Load(Config.resources["selectCellMaterial"], typeof(Material)) as Material;
        
        
        position = new Vector2Int((int)transform.position.x, (int)transform.position.z);
    }


	public void Start()
    {
        sceneState = GameObject.FindWithTag(Tags.state).GetComponent<SceneState>();
        
        stateMachine.Initialize(sceneState.GetCurrentState());
        
        EventMaster.current.StateChanged += OnChangeState;

        RenderSwitch(stateMachine.currentState.IsCellMustBeVisible(this));

        ChangeBehaviour();
    }


    public override void OnChangeState(State newState)
    {
        stateMachine.ChangeState(newState);
        RenderSwitch(stateMachine.currentState.IsCellMustBeVisible(this));
    }
    
    
    
    public void Occupy()
    {
        occupied = true;
    }

    public void Free()
    {
        occupied = false;
    }
    
    
    public void ChangeMaterial(Material mat)
    {
        _meshRenderer.material = mat;
    }
    
    public void RenderSwitch(bool render)
    {
        _meshRenderer.enabled = render;
        visible = _meshRenderer.enabled;
    }

    public override void OnFocus()
    {
        _behaviour.OnFocus(this);
    }

    public override void OnDefocus()
    {
        _behaviour.OnDefocus(this);
    }

    public override void OnClick()
    {
        _behaviour.OnClick(this);
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        Cell other = (Cell)obj;
        return position == other.position;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + position.GetHashCode();
            return hash;
        }
    }

    public void OnSomeComponentChangeBehaviour(string componentName, string behaviourName)
    {
        if (componentName != Type) return;
        ChangeBehaviour(behaviourName);
    }

    public void OnResetBehaviour()
    {
        ChangeBehaviour();
    }

    public void ChangeBehaviour(string name = "Base")
    {
        _behaviour = SubsituableCellFactory.GetBehaviourById(name);
        _behaviour.Init(this);
    }
}
