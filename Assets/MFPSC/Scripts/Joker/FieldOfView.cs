using System;
using System.Linq;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] private float _viewRadius;
    [SerializeField] private float _viewAngle;
    [SerializeField] private float _nearestDistanceToFind;
    [SerializeField] private LayerMask _targetMask;
    [SerializeField] private LayerMask _obstacleMask;

    private Color _fovColor = Color.white;

    public Transform Target { get; private set; }

    public Action<Transform> TargetFinded;

    private void Update()
    {

        if (TryFindTarget())
        {
            TargetFinded?.Invoke(Target);
        }
    }


    private bool TryFindTarget()
    {
        var objects = FindObjects();

        if (objects == null) return false;

        var nearest = objects.ToList().FirstOrDefault(obj => CheckNear(obj.transform));
        
        if (nearest != null)
        {
            Target = nearest.transform;
            return true;
        }

        Target = CheckObjects(objects);

        return Target != null;
    }
    

    private Collider[] FindObjects()
    {
        var findObjects = Physics.OverlapSphere(transform.position, _viewRadius, _targetMask);

        return findObjects;
    }

    private bool CheckNear(Transform target)
    {
        if (Vector3.Distance(transform.position, target.position) > _nearestDistanceToFind)
        {
            return false;
        }

        Vector3 directionToTarget = (target.position - transform.position).normalized;

        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, directionToTarget, out hitInfo, _viewRadius, _obstacleMask))
        {
            return false;
        }

        return true;
    }

    private Transform CheckObjects(Collider[] objects)
    {
        for (int i = 0; i < objects.Count(); i++)
        {
            var obj = objects[i].transform;

            if (CheckFront(obj))
            {
                return obj.transform;
            }
        }

        return null;
    }

    private bool CheckFront(Transform target)
    {
        Vector3 directionToTarget = (target.position - transform.position).normalized;

        if (Vector3.Angle(transform.forward, directionToTarget) > _viewAngle / 2)
        {
            return false;
        }

        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, directionToTarget, out hitInfo, _viewRadius, _obstacleMask))
        {
            return false;
        }

        return true;
    }

    public void SetColor(Color color)
    {
        _fovColor = color;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = _fovColor;
        Gizmos.DrawWireSphere(transform.position, _viewRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _nearestDistanceToFind);

        Vector3 viewAngleA = DirFromAngle(-_viewAngle / 2, false);
        Vector3 viewAngleB = DirFromAngle(_viewAngle / 2, false);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + viewAngleA * _viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + viewAngleB * _viewRadius);
    }

    private Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
