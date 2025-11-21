using System;
using Unity.Mathematics;
using UnityEngine;

public class BaitBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject m_rod;
    [SerializeField] private float miny = 0f;
    private bool landed = false;
    
    private static readonly System.Int32 frameDelay = 12;
    private Vector3[] previousPosition = new Vector3[frameDelay];
    private float minMagnitude = 0.5f;
    private int previousPositionIdx = 0;
    public bool followingRodToggle = false;
    public bool m_followingRod = true;
    private Vector3 targetOffset;

    private bool outofwater = false;
    private bool hasFish = true; // TODO change for fish catch
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetOffset = transform.position - m_rod.transform.position;
        GetComponent<Collider>().enabled = false;
        miny = GameObject.Find("Plane").transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
            
        if (transform.position.y < miny)
        {
            GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            transform.position -= new Vector3(0, transform.position.y - miny, 0);
            if (!landed) landed = true;
        }
        
        if (m_followingRod)
        {
            GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            transform.position = Vector3.Lerp(transform.position, m_rod.transform.position + targetOffset.magnitude * -m_rod.transform.forward, 0.5f);
        }
        previousPosition[previousPositionIdx] = transform.position;
        previousPositionIdx = (previousPositionIdx + 1) % frameDelay;

    }

    void FixedUpdate()
    {
        GetComponent<Rigidbody>().linearVelocity = GetComponent<Rigidbody>().linearVelocity * 0.99f;
    }

    public void ExecuteThrow()
    {
        GetComponent<Rigidbody>().useGravity = true;
        m_followingRod = false;
        Vector3 direction = transform.position - previousPosition[(previousPositionIdx + 1) % frameDelay];
        if (direction.sqrMagnitude > minMagnitude * minMagnitude) {
            GetComponent<Rigidbody>().AddForce(direction.normalized * 13.5f, ForceMode.Impulse);
            landed = false;
            outofwater = false;
        }
    }

    public void DoReel() {
        if (!landed) return;

        GetComponent<Rigidbody>().useGravity = false;
        Vector3 target = m_rod.transform.position + targetOffset.magnitude * -m_rod.transform.forward;
        target.y = transform.position.y;

        // TODO Replace with filet catch
        if ((target-transform.position).sqrMagnitude <= minMagnitude * minMagnitude )
        {
            followingRodToggle = true;
            m_followingRod = true;
        }

        
        // TODO replace const with var for fish weight
        if (!outofwater) {
            transform.position = (target - transform.position).normalized * 0.0625f + transform.position;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // TODO how tf do I check body overlap without the plane being a trigger?
        Debug.Log("Bump");
        if (other.gameObject.CompareTag("Floor") || other.gameObject.CompareTag("Water"))
        {
            GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        } else if (other.gameObject.CompareTag("Floor") && !other.gameObject.CompareTag("Water"))
        {
            Debug.Log("On floor not water");
            outofwater = true;
        } else if (other.gameObject.CompareTag("Net") && hasFish)
        {
            followingRodToggle = true;
            m_followingRod = true;
        }
        
    }
}
