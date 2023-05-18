using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks; 
using Cysharp.Threading.Tasks.Linq; 
using UnityEngine;
using UnityEngine.Profiling;

namespace Kit
{

    public class StateMachine<TStateKey> : IDisposable, Kit.IStateMachine<TStateKey> where TStateKey : struct
    { 
        
        /// <summary>
        /// 트랜지션끼리 연결시키기 위해 사용
        /// </summary>
        public class TransitionLink
        {
            public TransitionLink(TStateKey from, TStateKey to)
            {
                this.From = from;
                this.To = to;
                this.Callbacks = new List<ITransition>();
            }

            private TStateKey from;
            private TStateKey to;
            private List<ITransition> callbacks;

            public TStateKey From
            {
                get => from;
                set => from = value;
            }

            public TStateKey To
            {
                get => to;
                set => to = value;
            }

            public List<ITransition> Callbacks
            {
                get => callbacks;
                set => callbacks = value;
            } 
        }
        
        /// <summary>
        /// 스테이트 머신 생성자입니다. 라이프사이클 오브젝트는 스테이트 머신의 생명주기 관리를 위해 필요합니다.
        /// </summary>
        /// <param name="lifecycleObjectForCancel"></param>
        public StateMachine(MonoBehaviour lifecycleObjectForCancel)
        {
            this._mLifeObjectForCancel = lifecycleObjectForCancel; 
        } 
         
        private CancellationTokenSource m_managedTokenSource;
        private MonoBehaviour _mLifeObjectForCancel;
       




        public enum StateWorkingStep : byte
        {
            ENTER,
            UPDATING,
            EXIT
        }

        
        
        
        private IState m_currentState; 
        private IState CurrentState
        {
            get => m_currentState;
        }


        private TStateKey _mCurState;
        
        /// <summary>
        /// 상태를 변경합니다.
        /// </summary>
        public TStateKey CurState
        {
            get => _mCurState;
        } 


        private Dictionary<TStateKey, IState> m_States = new Dictionary<TStateKey, IState>();
        public Dictionary<TStateKey, IState> States
        {
            get => m_States;
            set => m_States = value;
        }


        public Dictionary<TStateKey, TransitionLink> Transitions
        {
            get;
            set;
        }
        
        /// <summary>
        /// Awake, 생성자 등에서 호출
        /// </summary>
        /// <param name="defaultState">기본 상태</param> 
        /// <exception cref="Exception"></exception>
        public async UniTask InitializeAndStartLoopAsync(TStateKey defaultState)
        {
          
            if (States.ContainsKey(defaultState) == false) 
                throw new Exception($"등록된 State {defaultState.ToString()} 을(를) 찾을 수 없습니다.");

            var changeStateTask =  this.ChangeStateAsync(defaultState)
                .AttachExternalCancellation(this._mLifeObjectForCancel.GetCancellationTokenOnDestroy());

            
            
            await changeStateTask;
                
            if (_mLifeObjectForCancel != null)
            {
                var token = _mLifeObjectForCancel.GetCancellationTokenOnDestroy(); 
                this.StartUpdateLoopAsync(token).Forget();
            }
            else
            { 
                m_managedTokenSource?.Cancel();
                m_managedTokenSource = null; 
                m_managedTokenSource = new CancellationTokenSource();
                this.StartUpdateLoopAsync(m_managedTokenSource.Token).Forget();
            }  
        }
        
        public void Add(TStateKey key, IState state)
        {
            this.States.TryAdd(key, state);
            state.StateMachine = this; 
        } 
        
        public void Remove(TStateKey key)
        {
            if (this.States.ContainsKey(key))
            { 
                var state = this.States[key];
                if (m_currentState.Equals(state))
                {
                    throw new NotImplementedException(
                        $"[구현필요]삭제 하려는 스테이트가 currentState({m_currentState.GetType().Name}) 과 같은 경우 이 작업을 수행할 수 없습니다.");
                }
                state.StateMachine = null;
                this.States.Remove(key);
            } 
        }

        
        private bool IsKeyValid(TStateKey key) => m_States.ContainsKey(key) == true;
        

        /// <summary>
        /// 트랜지션 조건을 추가합니다.
        /// 트랜지션이 2개이상 중첩된경우 And 연산됩니다.
        /// </summary> 
        public void AddTransition(TStateKey from, TStateKey to, ITransition transition)
        {
            Transitions ??= new();
            if (!IsKeyValid(from) || IsKeyValid(to)) 
                throw new Exception($"{from} or {to} 트랜지션 키를 찾을 수 없습니다. 먼저 키를 등록해주세요.");

            Transitions.TryGetValue(from, out var linkData);
            if(linkData == null) 
                Transitions.Add(from, new TransitionLink(from, to));
            
            Transitions[from].Callbacks.Add(transition);
        }

        /// <summary>
        /// 트랜지션 조건을 Func Predicate로 추가합니다.
        /// 트랜지션이 2개이상 중첩된경우 And 연산됩니다.
        /// </summary> 
        public void AddTransition(TStateKey from, TStateKey to, Func<UniTask<bool>> shouldTransitionPredicate)
        { 
            Transitions ??= new();
            if (!IsKeyValid(from) || !IsKeyValid(to)) 
                throw new Exception($"{from} or {to} 트랜지션 키를 찾을 수 없습니다. 먼저 키를 등록해주세요.");
            
            Transitions.TryGetValue(from, out var linkData);
            if(linkData == null) 
                Transitions.Add(from, new TransitionLink(from, to));

            Transitions[from].Callbacks.Add(new TransitionFunctor(shouldTransitionPredicate));
        }

        
        
        /// <summary>
        /// 이 함수는 CurState에 직접 값을 대입하는것과 동일한 동작을 합니다.
        /// </summary>
        public async UniTask ChangeStateAsync(TStateKey key)
        {
            if (_mCurState.Equals(key)) 
                return;


            if (m_currentState != null)
            {
                var task = m_currentState.OnStateExit();
                task.GetAwaiter();
                
            }

            m_currentState = States[key];
            _mCurState = key;

            if (m_currentState != null)
            {
                var task = m_currentState.OnStateEnter();
                 
                await task;
            }

            this._mCurState = key;
        }
        
        /// <summary>
        /// 이 함수는 CurState에 직접 값을 대입하는것과 동일한 동작을 합니다.
        /// </summary>
        public async UniTask ChangeStateImediate(TStateKey key)
        {
            if (_mCurState.Equals(key))
            {  
                return;   
            }
                
            if (m_currentState != null)
            {
                await m_currentState.OnStateExit();
            }

            m_currentState = States[key];
            _mCurState = key;

            if (m_currentState != null)
            {
                await m_currentState.OnStateEnter();
            }

            this._mCurState = key;
        }
        async UniTask LogicAsync()
        {
            // 트랜지션이 완료되기 전까지 State 업데이트를 별도로 실행하지 않습니다.
            await UpdateTransitionAsync(); 
            // 오해 없게 메모, 이건 동기 로직임.
            // 업데이트에서는 task 처리가 불가능하게 만듬.
            UpdateState();
        }


        async UniTask UpdateTransitionAsync()
        { 
            if (Transitions == null) return;
            if (this.Transitions.ContainsKey(this.CurState))
            {
                var link = this.Transitions[CurState];
                for (var index = 0; index < link.Callbacks.Count; index++)
                {
                    var callback = link.Callbacks[index];
                    var shouldTransition = await callback.ShouldTransition();
                    if (shouldTransition)
                    {
                        // 현재는 트랜지션이 가능한 상태더라도 현재 state의 task가 종료 되어야만 호출 됨.  
                        await ChangeStateAsync(link.To);
                    }
                }
            }
        }

        private void UpdateState()
        { 
            if (States != null && CurrentState != null)
            {
                 CurrentState.OnStateUpdate();  
            } 
        }
        
        
        /// <summary>
        /// FSM 로직을 업데이트 타이밍에 비동기로 처리합니다.
        /// </summary>
        private async UniTaskVoid StartUpdateLoopAsync(CancellationToken token)
        {
            await foreach (var _ in UniTaskAsyncEnumerable.EveryUpdate().WithCancellation(token))
            { 
                if (_mLifeObjectForCancel.gameObject.activeSelf && _mLifeObjectForCancel.enabled)
                {
                    await LogicAsync().SuppressCancellationThrow();
                }
            } 
        }
          

        public void Dispose()
        {
            this.m_managedTokenSource?.Cancel(); 
            this.m_managedTokenSource = null;
        }
    }
}
