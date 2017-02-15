using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Shelter for enemy to cover behind
/// </summary>
public class Shelter : MonoBehaviour
{
    private static List<Shelter> _Shelters = new List<Shelter>();
    public static List<Shelter> Shelters { get { return _Shelters; } }

    /// <summary>
    /// Current covered enemy
    /// </summary>
    public EnemyAIController Controller { get; set; }
    
    void Start()
    {
        enabled = false;
        _Shelters.Add(this);
    }


    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "Shelter.png");
    }
}
