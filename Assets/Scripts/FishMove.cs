using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class FishMove : MonoBehaviour
{

    private FishSpawner m_FishSpawner;

    private bool m_hasTarget = false;
    private bool m_isTurning;

    private Vector3 m_waypoint;
    private Vector3 m_lastwaypoint = new Vector3 (0f,0f,0f);

    private Animator m_animator;
    private float m_speed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_FishSpawner = transform.parent.GetComponentInParent<FishSpawner>();
        m_animator = GetComponent<Animator>();

        SetUpFish();
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_hasTarget)
        {
            m_hasTarget = CanFindTarget();
        }
        else
        {
            RotateFish(m_waypoint, m_speed);
            transform.position = Vector3.MoveTowards(transform.position, m_waypoint, m_speed * Time.deltaTime);
        }

        if (transform.position == m_waypoint)
        {
            m_hasTarget = false;
        }
    }

    void SetUpFish()
    {
        // randomly scale each fish
        float m_scale = Random.Range(0.5f, 1.5f);
        transform.localScale = new Vector3(m_scale, m_scale, m_scale);
    }

    bool CanFindTarget()
    {
        m_waypoint = m_FishSpawner.RandomWaypoint();
        if (m_lastwaypoint == m_waypoint)
        {
            m_waypoint = m_FishSpawner.RandomWaypoint();
            return false;
        }
        else
        {
            m_lastwaypoint = m_waypoint;
            m_speed = Random.Range(0.5f, 2f);
            m_animator.speed = m_speed;
            return true;
        }
    }

    void RotateFish(Vector3 target, float speed)
    {
        float TurnSpeed = speed * Random.Range(1f,3f);
        Vector3 direction = target - this.transform.position;
        Quaternion toRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, TurnSpeed * Time.deltaTime);
    }
}
