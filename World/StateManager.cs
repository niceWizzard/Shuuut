﻿using System;
using System.Collections.Generic;
using Godot;

namespace Shuuut.World;

public class StateManager<T, K> where T : struct, Enum where K : Node
{
    
    
    
    private Dictionary<T, BaseState<T, K>> allStates;
    public readonly K Parent;

    public BaseState<T, K> CurrentState { get; private set; }
    public BaseState<T, K> PreviousState { get; private set; }

    public T CurrentStateEnum{ get; private set; }
    public T? PreviousStateEnum { get; private set; }
    
    public StateManager(Dictionary<T, BaseState<T, K> > states, K parent)
    {
        Parent = parent;

        allStates = states;
        int index = 0;
        foreach (var h in allStates)
        {
                
            h.Value?.Register(this);
            if (index == 0)
            {
                ChangeState(h.Key);
                index++;
            }
        }
        
    }

    public void ChangeState(T newState)
    {
        PreviousStateEnum = CurrentStateEnum;
        CurrentStateEnum = newState;
        PreviousState = CurrentState;
        CurrentState = allStates[newState];
        CurrentState?.OnEnter();
        PreviousState?.OnExit();
    }

    public void Ready()
    {
        foreach (var v in allStates)
        {
            v.Value?.Ready();
        }
    }

    public void Process(float delta)
    {
        CurrentState?.Process(delta);
    }

    public void PhysicsProcess(float delta)
    {
        CurrentState?.PhysicsProcess(delta);
    }
    

}
