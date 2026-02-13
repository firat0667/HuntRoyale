using Firat0667.WesternRoyaleLib.Patterns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonExample : FoundationSingleton<SingletonExample>, IFoundationSingleton
{
    public bool Initialized { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    private void Start()
    {
       
    }
    private void Update()
    {
        
    }
}
