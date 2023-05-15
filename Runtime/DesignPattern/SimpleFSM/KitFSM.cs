using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.LowLevel;

namespace Kit
{

    public class StateMachine<TStateKey> : IDisposable, Kit.IStateMachine<TStateKey> where TStateKey : struct
    { 
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
        public StateMachine(GameObject mLifeObject)
        {
            this.m_lifeObject = mLifeObject;
        }
        public StateMachine()
        {
                
        }
        
        
        private CancellationTokenSource m_managedTokenSource;
        private GameObject m_lifeObject;
        
        
        
        
        
        
        private IState m_currentState; 
        private IState CurrentState
        {
            get => m_currentState;
            set
            {
                if (m_currentState != null)
                {
                    m_currentState?.OnStateExit();
                }
                m_currentState = value;
                m_currentState?.OnStateEnter();
            }
        }


        private TStateKey _mCurState;
        
        /// <summary>
        /// 상태를 변경합니다.
        /// </summary>
        public TStateKey CurState
        {
            get => _mCurState;
            set
            {
                if (_mCurState.Equals(value))
                    return;

                m_currentState?.OnStateExit();
                m_currentState = States[value];
                _mCurState = value;
                m_currentState?.OnStateEnter(); 
            }
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
        public void InitializeAndStartLoop(TStateKey defaultState)
        {
         
            if (States.ContainsKey(defaultState) == false)
            {
                throw new Exception($"등록된 State {defaultState.ToString()} 을(를) 찾을 수 없습니다.");
            }
            this.CurrentState = States[defaultState];
 
            if (m_lifeObject != null)
            {
                var token = m_lifeObject.GetCancellationTokenOnDestroy(); 
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
        } 
        
        public void Remove(TStateKey key)
        {
            if(this.States.ContainsKey(key)) 
                this.States.Remove(key);
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
        public async UniTask ChangeState(TStateKey key)
        {
            if (_mCurState.Equals(key))
                return;

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

            this.CurState = key;
        }
        
        public async UniTask UpdateAsync()
        {
            await UpdateTransitionAsync(); 
           
            UpdateState();
        }


        private async UniTask UpdateTransitionAsync()
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
                        await ChangeState(link.To);
                        return;
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
            while (true)
            {
                if (token.IsCancellationRequested) 
                    return;
                
                await UniTask.Yield(PlayerLoopTiming.Update, token);
                await UpdateAsync();
            }
        }
          

        public void Dispose()
        {
            this.m_managedTokenSource?.Cancel(); 
            this.m_managedTokenSource = null;
        }
    }
}
