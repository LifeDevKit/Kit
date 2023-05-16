# Kit Simple FSM

```csharp
StateMachine = new StateMachine<PlayerState>(this);
StateMachine.Add(PlayerState.Idle, new PlayerIdleState(this.gameObject));
StateMachine.Add(PlayerState.Move, new PlayerMoveState(this.gameObject));

StateMachine.AddTransition(PlayerState.Idle, PlayerState.Move,
    async () =>
    { 
        return Input.GetKeyDown(KeyCode.Space);
    }); 
StateMachine.AddTransition(PlayerState.Move, PlayerState.Idle, 
                async () => Input.GetKeyUp(KeyCode.Space)); 

StateMachine.InitializeAndStartLoop(PlayerState.Idle);
```


