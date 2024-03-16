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
    
    public MeshRenderer _meshRenderer;
    public Material basicMaterial;
    public Material selectMaterial;

    public void Awake()
    {
        Free();

        _meshRenderer = gameObject.GetComponent<MeshRenderer>();
        basicMaterial = Resources.Load(Config.resources["materials"] + Type + "/Basic", typeof(Material)) as Material;
        selectMaterial = Resources.Load(Config.resources["materials"] + Type + "/Select", typeof(Material)) as Material;
        
        
        position = new Vector2Int((int)transform.position.x, (int)transform.position.z);
    }


	public void Start()
    {
        sceneState = GameObject.FindWithTag("State").GetComponent<SceneState>();
        
        stateMachine.Initialize(sceneState.GetCurrentState());
        
        //OnChangeStateEvent
        EventMaster.current.StateChanged += OnChangeState;
    }


    public override void OnChangeState(State newState)
    {
        stateMachine.ChangeState(newState);
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
    
    public void renderSwitch(bool render)
    {
        _meshRenderer.enabled = render;
    }
    public override void OnFocus()
    {
        stateMachine.currentState.OnCellMouseEnter(this);
    }

    public override void OnDefocus()
    {
        stateMachine.currentState.OnCellMouseExit(this);
    }

    public override void OnClick()
    {
        Debug.Log("Clicked on Cell!");
        stateMachine.currentState.OnCellClick(this);
    }
}
