using System;
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
            StateMachine = new StateMachine<PlayerState>(this);
            StateMachine.Add(PlayerState.Idle, new PlayerIdleState(this.gameObject));
            StateMachine.Add(PlayerState.Move, new PlayerMoveState(this.gameObject));

            StateMachine.AddTransition(PlayerState.Idle, PlayerState.Move,
                async () =>
                {
                    await UniTask.Delay(5000);
                    return true;
                }); 
            StateMachine.AddTransition(PlayerState.Move, PlayerState.Idle, 
                async () => Input.GetKeyUp(KeyCode.Space)); 

            StateMachine.InitializeAndStartLoopAsync(PlayerState.Idle);
        }

        public void Update()
        { 
        //    Debug.Log("On update Simple FSM Test " + this.gameObject.name);
        }

        public void OnGUI()
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Change State To Move"))
            {
                StateMachine.ChangeStateAsync(PlayerState.Move).Forget();
            }

            if (GUILayout.Button("Change State To Idle"))
            {

                StateMachine.ChangeStateAsync(PlayerState.Idle).Forget();
            } 
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
            await UniTask.Delay(2000);
            
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
             Debug.Log("Enter");
        }

        public void OnStateUpdate()
        {
            go.transform.position += -UnityEngine.Vector3.left * Time.deltaTime;
        }

        public async UniTask OnStateExit()
        {   
            
            Debug.Log("Exit");
        }
    }
}