using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class CoreComponent : MonoBehaviour, ILogicUpdate
{
    protected Subsystem core;

    protected virtual void Awake()
    {
        core = transform.parent.GetComponent<Subsystem>();

        if (core == null) { Debug.LogError("There is no Core on the parent"); }
        core.AddComponent(this);
    }

    public virtual void LogicUpdate() { }

}
