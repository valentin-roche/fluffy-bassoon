using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyGroup", menuName = "ScriptableObjects/EnemyGroup", order = 1)]
public class EnemyGroupSO : ScriptableObject
{
    public EnemySO[] Enemies;

    public int groupValue;
}
