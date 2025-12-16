using UnityEngine;

public interface ICharacterInputProvider
{
    Vector3 MoveWorld { get; }    
    Vector3 AimWorld { get; }   
    bool AttackPressed { get; }  
}
