using UnityEditorInternal;
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
            StateMachine = new StateMachine<PlayerState>();
            StateMachine.Add(PlayerState.Idle, new PlayerIdleState(this.gameObject));
            StateMachine.Add(PlayerState.Move, new PlayerMoveState(this.gameObject));

            StateMachine.AddTransition(PlayerState.Idle, PlayerState.Move, () => Input.GetKeyDown(KeyCode.Space));
            StateMachine.AddTransition(PlayerState.Move, PlayerState.Idle, () => Input.GetKeyUp(KeyCode.Space));
            StateMachine.Initialize(PlayerState.Idle);
        }

        public void Update()
        {
            StateMachine.Update(Time.deltaTime);
        }

        public void OnGUI()
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Change State To Move"))
                StateMachine.CurState = PlayerState.Move;

            if (GUILayout.Button("Change State To Idle"))
                StateMachine.CurState = PlayerState.Idle;
            GUILayout.EndHorizontal();
        }
    }

     internal class PlayerIdleState : IState
    {
        public GameObject go;
        public PlayerIdleState(GameObject obj)
        {
            this.go = obj;
        }
        public void OnStateEnter()
        {
            Debug.Log($"{this.GetType().Name} OnStateEnter");
        }

        public void OnStateUpdate()
        {
            Debug.Log($"{this.GetType().Name} OnStateUpdate");
        }

        public void OnStateExit()
        {
            Debug.Log($"{this.GetType().Name} OnStateExit");
        }
    }


     internal class PlayerMoveState : IState
    {
        public GameObject go;
        public PlayerMoveState(GameObject obj)
        {
            this.go = obj;
        }
        public void OnStateEnter()
        {
            Debug.Log($"{this.GetType().Name} OnStateEnter");
        }

        public void OnStateUpdate()
        {
            Debug.Log($"{this.GetType().Name} OnStateUpdate");
            go.transform.position += new Vector3(1, 0, 0) * Time.deltaTime;
        }

        public void OnStateExit()
        {
            Debug.Log($"{this.GetType().Name} OnStateExit");
        }
    }
}