using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Chaser : MonoBehaviour
{
    public Vector3 target;
    public bool Paused { set { _agent.isStopped = value; } }

    private Vector3 _movementPoint;
    private NavMeshAgent _agent;

    // Start is called before the first frame update
    void Start()
    {
        if (!TryGetComponent(out _agent))
            print("Agent does not exist on this GameObject");

        _movementPoint = target;
        _agent.isStopped = true;

        InvokeRepeating(nameof(FindPath), 0.25f, 0.25f);
    }

    // Update path to target. If path becomes invalid, find a new target
    void FindPath()
	{
        NavMeshPath path = new NavMeshPath();
        _agent.CalculatePath(target, path);

        if (path.status != NavMeshPathStatus.PathComplete)
        {
            _movementPoint = FindBlockingPoint(path);
            _agent.CalculatePath(_movementPoint, path);
        }

        _agent.path = path;
        _agent.SetPath(path);
    }

    Vector3 FindBlockingPoint(NavMeshPath path)
	{
        return path.corners[path.corners.Length - 1];
	}
}
