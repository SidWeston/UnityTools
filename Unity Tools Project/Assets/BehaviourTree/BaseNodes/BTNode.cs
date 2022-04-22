using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BTNode : ScriptableObject
{

    public enum State
    {
        Running, 
        Failure,
        Success
    }

    [HideInInspector] public State state = State.Running;
    [HideInInspector] public bool started = false;
    [HideInInspector] public string guid;
    [HideInInspector] public Vector2 position;
    [HideInInspector] public Blackboard blackboard;
    [HideInInspector] public AIController controller;
    [HideInInspector] public int parentIndex;
    [TextArea] public string description;

    public State Update()
    {
        if(!started)
        {
            OnStart();
            started = true;
        }

        state = OnUpdate();

        if(state == State.Failure || state == State.Success)
        {
            OnStop();
            started = false;
        }

        return state;
    }

    public virtual BTNode Clone()
    {
        return Instantiate(this);
    }

    protected abstract void OnStart();
    protected abstract void OnStop();
    protected abstract State OnUpdate();

}
