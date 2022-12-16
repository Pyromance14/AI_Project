using System.Linq;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public Team Team => _team;
    [SerializeField] private Team _team;
    [SerializeField] private LayerMask _layerMask;

    private float _attackRange = 3.0f;
    private float _rayDistance = 5.0f;
    private float _stoppingDistance = 5.0f;

    private Vector3 _destination;
    private Quaternion _desiredRotation;
    private Vector3 _direction;
    private StateMachine _target;
    private EnemyState _currentState;

    private void Update()
    {
        switch (_currentState)
        {
            case EnemyState.Wander:
                {
                    // Checks to see if destination is needed then gets it.
                    if (NeedsDestination())
                    {
                        GetDestination();
                    }

                    transform.rotation = _desiredRotation;

                    transform.Translate(translation: Vector3.forward * Time.deltaTime * 5.0f);

                    //Checks to see if the path is blocked turning red, base ray colour is green.
                    var rayColor = IsPathBlocked() ? Color.red : Color.green;
                    Debug.DrawRay(start: transform.position, dir: _direction * _rayDistance, rayColor);

                    while (IsPathBlocked())
                    {
                        Debug.Log(message: "Path Blocked");
                        GetDestination();
                    }

                    var targetToAggro = CheckForAggro();
                    if(targetToAggro != null)
                    {
                        _target = targetToAggro.GetComponent<StateMachine>();
                        _currentState = EnemyState.Chase;
                    }

                    break;
                }

            case EnemyState.Chase:
                {
                    // When enemy can't find target when chasing them, it goes vback into a wandering state.
                    if(_target == null)
                    {
                        _currentState = EnemyState.Wander;
                         
                        return;
                    }

                    transform.LookAt(_target.transform);
                    transform.Translate(translation: Vector3.forward * Time.deltaTime * 5.0f);

                    // Checks to see if in attack range than enters into attack state.
                    if(Vector3.Distance(a: transform.position, b:_target.transform.position) < _attackRange)
                    {
                        _currentState = EnemyState.Attack;
                    }
                    break;
                }
            case EnemyState.Attack:
                {
                    // If target is found it is destroyed by enemy.
                    if(_target != null)
                    {
                        Destroy(_target.gameObject);
                    }

                    _currentState = EnemyState.Wander;
                    break;
                    
                }
        }
    }

    private bool IsPathBlocked()
    {
        Ray ray = new Ray(origin: transform.position, _direction);
        var hitSomething = Physics.RaycastAll(ray, _rayDistance, _layerMask);
        return hitSomething.Any();
    }

    private void GetDestination()
    {
        Vector3 testPosition = (transform.position + (transform.forward * 4.0f)) +
            new Vector3(x: UnityEngine.Random.Range(-4.5f, 4.5f), y: 0.0f,
            z: UnityEngine.Random.Range(-4.5f, 4.5f));

        _destination = new Vector3(testPosition.x, y: 1.0f, testPosition.z);

        _direction = Vector3.Normalize(_destination - transform.position);
        _direction = new Vector3(_direction.x, y: 0.0f, _direction.z);
        _desiredRotation = Quaternion.LookRotation(_direction);
    }


   private bool NeedsDestination()
    {
        if (_destination == Vector3.zero)
            return true;

        var distance = Vector3.Distance(a: transform.position, b: _destination);
        if(distance < _stoppingDistance)
        {
            return true;
        }

        return false;
    }

    Quaternion startingAngle = Quaternion.AngleAxis(angle: -60, Vector3.up);
    Quaternion stepAngle = Quaternion.AngleAxis(angle: 5, Vector3.up);

    private Transform CheckForAggro()
    {
        float aggroRadius = 5.0f;

        RaycastHit hit;
        var angle = transform.rotation * startingAngle;
        var direction = angle * Vector3.forward;
        var pos = transform.position;
        for(var i = 0; i < 24; i++)

        {
            if(Physics.Raycast(origin: pos, direction, out hit, aggroRadius))
            {
                var enemy = hit.collider.GetComponent<StateMachine>();
                if(enemy != null && enemy.Team != gameObject.GetComponent<StateMachine>().Team)
                {
                    Debug.DrawRay(start: pos, dir: direction * hit.distance, Color.red);
                    return enemy.transform;
                }
                else
                {
                    Debug.DrawRay(start: pos, dir: direction * hit.distance, Color.yellow);
                }
            }
            else
            {
                Debug.DrawRay(start: pos, dir: direction * aggroRadius, Color.white);
            }

            direction = stepAngle * direction;
        }

        return null;
    }   
}

public enum Team
{
    Gold,
    Silver
}

public enum EnemyState
{
    Wander,
    Chase,
    Attack
}
