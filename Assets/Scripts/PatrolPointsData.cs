using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PatrolPoints", menuName = "Enemy/PatrolPoints", order = 1)]
public class PatrolPointsData : ScriptableObject
{
    public Vector3[] points;
}

