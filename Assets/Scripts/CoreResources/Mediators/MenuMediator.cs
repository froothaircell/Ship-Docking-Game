using System;
using System.Collections.Generic;
using CoreResources.Handlers.EventHandler;
using CoreResources.Pool;
using CoreResources.StateMachine;
using CoreResources.Utils;
using CoreResources.Utils.Disposables;

namespace CoreResources.Mediators
{
    public abstract class MenuMediator<TMenuMediator, TMenuStateMachine, TMenuView> : StateMachineMediator<TMenuMediator, TMenuStateMachine> 
        where TMenuMediator : MenuMediator<TMenuMediator, TMenuStateMachine, TMenuView>
        where TMenuStateMachine : StateMachineHistory, new()
        where TMenuView : MenuView<TMenuView>
    {
        public TMenuView View;
        public PooledList<IDisposable> _disposables;

        public virtual void OnEnter(REvent evt)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            OnEnterMenu();
        }

        public virtual void OnExit(REvent evt)
        {
            OnExitMenu();
            transform.GetChild(0).gameObject.SetActive(false);
        }

        public abstract void SubscribeToEvents();
        
        public abstract void OnEnterMenu();
        public abstract void OnExitMenu();
        
    }
}