using System;
using System.IO;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    private static Action _resetAllNPCsAction;
    private static Action _saveAllNPCsAction;

    public static void ResetAllNPCs() => _resetAllNPCsAction?.Invoke();
    public static void SaveAllNPCs() => _saveAllNPCsAction?.Invoke();

    private NPCData _npcData;
    private string _npcDir, _npcDataFilePath;
    private static readonly string NPC_DATA_FILE = "npcData.json";

    private NPCState _currentState;

    // Shared components
    [field: SerializeField] public NPCInterestPoints NPCInterestPoints { get; private set; }
    public NavMeshAgent NavAgent { get; private set; }
    public EnemyHealth EnemyHealth { get; private set; }
    public Animator Animator { get; private set; }
    public AudioSource AudioSource { get; private set; }
    // ...

    // States
    [field: Header("States")]
    [field: SerializeField] public NPCWanderState WanderState { get; private set; }
    [field: SerializeField] public NPCDeadState DeadState { get; private set; }
    // ...

    private static readonly string SPEED_ANIMATOR_PARAMETER = "Speed";
    private static readonly string MOTION_SPEED_ANIMATOR_PARAMETER = "MotionSpeed";

    [Header("Sound Settings")]
    [SerializeField] private AudioClip[] _footstepClips;


    private void Awake()
    {
        // Search for components
        if (NPCInterestPoints == null) NPCInterestPoints = GetComponentInChildren<NPCInterestPoints>();
        NavAgent = GetComponentInChildren<NavMeshAgent>();
        EnemyHealth = GetComponentInChildren<EnemyHealth>();
        Animator = GetComponentInChildren<Animator>();
        AudioSource = GetComponentInChildren<AudioSource>();

        // Check for missing components or "invalid states" + initialization
        if (NPCInterestPoints == null) throw new Exception($"NPCInterestPoints is missing in {name}");
        NPCInterestPoints.Init();

        if (NavAgent == null) throw new Exception($"NavMeshAgent is missing in {name}");
        else if (!NavAgent.isOnNavMesh) throw new Exception($"NavMeshAgent is not on the NavMesh in {name}");

        if (EnemyHealth == null) throw new Exception($"EnemyHealth is missing in {name}");
        else EnemyHealth.OnDeath += OnDeath;

        // Initialize states
        InitializeStates();

        // Setup save/load + Initial load
        _npcDir = Path.Combine(Application.persistentDataPath, "NPCs", name);
        _npcDataFilePath = Path.Combine(_npcDir, NPC_DATA_FILE);
        ResetNPC();

        _resetAllNPCsAction += ResetNPC;
        _saveAllNPCsAction += SaveNPC;
    }


    private void Start() => TransitionToState(WanderState);
    private void OnDeath() => TransitionToState(DeadState);

    private void OnDestroy()
    {
        _resetAllNPCsAction -= ResetNPC;
        _saveAllNPCsAction -= SaveNPC;
        EnemyHealth.OnDeath -= OnDeath;
    }


    public void TransitionToState(NPCState state)
    {
        if (_currentState != null) _currentState.ExitState(this);
        _currentState = state;
        if (_currentState != null) _currentState.EnterState(this);
    }


    private void InitializeStates()
    {
        WanderState = InitializeState(WanderState);
        DeadState = InitializeState(DeadState);
        // ...
    }

    private T InitializeState<T>(T state) where T : NPCState
    {
        if (state == null) throw new Exception($"{typeof(T)} is missing in {name}");

        T newState = Instantiate(state);
        newState.name = $"{state.name} - {name}";
        newState.InitState(this);
        return newState;
    }


    private void Update()
    {
        if (_currentState != null) _currentState.UpdateState(this);
    }

    private void FixedUpdate()
    {
        UpdateAnimator();
        if (_currentState != null) _currentState.FixedUpdateState(this);
    }


    public void ResetNPC()
    {
        LoadNPCData();

        // Reset position and rotation
        transform.SetPositionAndRotation(_npcData.Position, _npcData.Rotation);
        if (NavAgent.isOnNavMesh) NavAgent.ResetPath();
        NavAgent.Warp(_npcData.Position);

        // Reset Health and current state
        EnemyHealth.ResetHealth(_npcDir);
        TransitionToState(WanderState);
    }

    private void LoadNPCData()
    {
        // Already loaded
        if (_npcData != null) return;

        try
        {
            // Read the JSON file and deserialize it + Set the data variable
            string json = File.ReadAllText(_npcDataFilePath);
            _npcData = JsonUtility.FromJson<NPCData>(json) ?? throw new Exception($"Failed to parse npc data from {_npcDataFilePath}");
        }
        catch (Exception e)
        {
            Debug.Log($"Failed to load npc data for {name}: {e.Message}");

            // Initialize with default values if load fails
            _npcData = new NPCData
            {
                Position = transform.position,
                Rotation = transform.rotation
            };
        }
    }


    public void SaveNPC()
    {
        // Save data variable
        _npcData ??= new NPCData();
        _npcData.Position = transform.position;
        _npcData.Rotation = transform.rotation;

        try
        {
            // Convert the data to JSON
            string json = JsonUtility.ToJson(_npcData);

            // Create the directory if it doesn't exist
            if (!Directory.Exists(_npcDir)) Directory.CreateDirectory(_npcDir);

            // Save the JSON to a file
            File.WriteAllText(_npcDataFilePath, json);
        }
        catch (Exception e)
        {
            Debug.Log($"Failed to save npc data for {name}: {e.Message}");
        }
    }


    private void UpdateAnimator()
    {
        if (Animator == null) return;

        Animator.SetFloat(SPEED_ANIMATOR_PARAMETER, NavAgent.velocity.magnitude);
        Animator.SetFloat(MOTION_SPEED_ANIMATOR_PARAMETER, NavAgent.velocity.magnitude / NavAgent.speed);
    }


    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
    private void OnFootstep(AnimationEvent animationEvent)
    {
        if (_footstepClips.Length == 0) return;
        if (animationEvent.animatorClipInfo.weight >= 0.5f) return;


        int index = UnityEngine.Random.Range(0, _footstepClips.Length);
        AudioSource.clip = _footstepClips[index];
        AudioSource.Play();
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
    private void OnLand(AnimationEvent _) { }


    [Serializable]
    private class NPCData
    {
        public Vector3 Position;
        public Quaternion Rotation;
    }
}
