using Cysharp.Threading.Tasks; 
using UnityEngine;

namespace Kit.Test
{
     internal class SimpleFSMTest : MonoBehaviour
    {
     
        public enum PlayerState
        {
            Idle,
            Move
        }
        public StateMachine<PlayerState> StateMachine { get; set; }

        public void Awake()
        {
            StateMachine = new StateMachine<PlayerState>(this.gameObject);
            StateMachine.Add(PlayerState.Idle, new PlayerIdleState(this.gameObject));
            StateMachine.Add(PlayerState.Move, new PlayerMoveState(this.gameObject));

            StateMachine.AddTransition(PlayerState.Idle, PlayerState.Move, async () => Input.GetKeyDown(KeyCode.Space)); 
            StateMachine.InitializeAndStartLoop(PlayerState.Idle);
        }

        public void OnGUI()
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Change State To Move"))
                StateMachine.CurState = PlayerState.Move;

            if (GUILayout.Button("Change State To Idle"))
                StateMachine.CurState = PlayerState.Idle;
            GUILayout.EndHorizontal();
            
            GUILayout.Label("Current State:" + StateMachine.CurState.ToString());
        }
    }

 
    internal class PlayerIdleState : IState
    {
        public GameObject go;
        public PlayerIdleState(GameObject obj)
        {
            this.go = obj;
        }

        public IStateMachine StateMachine { get; set; }
        public async UniTask OnStateEnter()
        {
            await UniTask.Yield();
        }

        public void OnStateUpdate()
        {
           
        }

        public async UniTask OnStateExit()
        {
            await UniTask.Yield();
        }
    }


     internal class PlayerMoveState : IState
    {
        public GameObject go;
        public PlayerMoveState(GameObject obj)
        {
            this.go = obj;
        }

        public IStateMachine StateMachine { get; set; }
        public async UniTask OnStateEnter()
        {
            await UniTask.Delay(1000); 
        }

        public void OnStateUpdate()
        {
            Debug.Log("Called Move");
        }

        public async UniTask OnStateExit()
        { 
            await UniTask.Delay(1000); 
        }
    }
}