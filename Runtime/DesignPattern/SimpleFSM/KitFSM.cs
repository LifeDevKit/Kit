using System;
using System.Collections.Generic; 

namespace Kit
{

 
    
    public class StateMachine<TStateKey> : Kit.IStateMachine<TStateKey> where TStateKey : struct
    {    
        public class TransitionLink
        {
            public TransitionLink(TStateKey from, TStateKey to)
            {
                this.from = from;
                this.to = to;
                this.callbacks = new List<ITransition>();
            }
            public TStateKey from;
            public TStateKey to;
            public List<ITransition> callbacks;
        }
        
        
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
            get
            {
                return _mCurState;
            }
            set
            {
                if (m_currentState != null)
                {
                    m_currentState?.OnStateExit();
                }     
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
        /// State 추가를 완료한 후 초기화 호출
        /// </summary>
        /// <param name="defaultState"></param>
        public void Initialize(TStateKey defaultState)
        {
            if (States.ContainsKey(defaultState) == false)
            {
                throw new Exception($"등록된 State {defaultState.ToString()} 을(를) 찾을 수 없습니다.");
            }
            this.CurrentState = States[defaultState];
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
            
            Transitions[from].callbacks.Add(transition);
        }

        /// <summary>
        /// 트랜지션 조건을 Func Predicate로 추가합니다.
        /// 트랜지션이 2개이상 중첩된경우 And 연산됩니다.
        /// </summary> 
        public void AddTransition(TStateKey from, TStateKey to, Func<bool> shouldTransitionPredicate)
        {
            Transitions ??= new();
            if (!IsKeyValid(from) || IsKeyValid(to)) 
                throw new Exception($"{from} or {to} 트랜지션 키를 찾을 수 없습니다. 먼저 키를 등록해주세요.");
            
            Transitions.TryGetValue(from, out var linkData);
            if(linkData == null) 
                Transitions.Add(from, new TransitionLink(from, to));

            Transitions[from].callbacks.Add(new TransitionFunctor(shouldTransitionPredicate));
        }


        private void UpdateTransition(float dt)
        {
            if (this.Transitions.ContainsKey(this.CurState))
            {
                var link = this.Transitions[CurState];
                foreach (var callback in link.callbacks)
                {
                    var shouldTransition = callback.ShouldTransition();
                    if (shouldTransition)
                    {
                        CurState = link.to;
                        return;
                    }
                }
            }
        }

        private void UpdateState(float dt)
        {
            if (States != null && CurrentState != null)
            {
                CurrentState?.OnStateUpdate();
            } 
        }
        
        
        public void Update(float dt)
        {
            UpdateTransition(dt); 
            UpdateState(dt);
        } 
    }
}
