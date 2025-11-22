// using System;
// using Unity.Mathematics;
using UnityEngine;
using System.Collections;

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

    private bool startedfish = false;

    private bool outofwater = true;

    public GameObject fishy;
    private GameObject fishyInstance = null;

    Rigidbody rb;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        targetOffset = transform.position - m_rod.transform.position;
        GetComponent<Collider>().enabled = true;
        rb.useGravity = false;
        miny = GameObject.Find("Plane").transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
            
        if (transform.position.y < miny)
        {
            rb.linearVelocity = Vector3.zero;
            transform.position -= new Vector3(0, transform.position.y - miny, 0);
            outofwater = false;
            StartCoroutine(StartTimer());
            if (!landed) landed = true;
        }
        
        if (m_followingRod)
        {
            rb.linearVelocity = Vector3.zero;
            transform.position = Vector3.Lerp(transform.position, m_rod.transform.position + targetOffset.magnitude * 0.5f * -m_rod.transform.forward, 0.5f);
        }
        previousPosition[previousPositionIdx] = transform.position;
        previousPositionIdx = (previousPositionIdx + 1) % frameDelay;

    }

    void FixedUpdate()
    {
        rb.linearVelocity = rb.linearVelocity * 0.99f;
    }

    public void ExecuteThrow()
    {
        Vector3 direction = transform.position - previousPosition[(previousPositionIdx + 1) % frameDelay];
        if (direction.sqrMagnitude > minMagnitude * minMagnitude) {
            rb.useGravity = true;
            m_followingRod = false;

            rb.AddForce(direction.normalized * 13.5f, ForceMode.Impulse);
            landed = false;
            outofwater = true;
            startedfish = false;
        }
    }

    public void DoReel() {
        if (!landed) return;

        rb.useGravity = false;
        Vector3 target = m_rod.transform.position + targetOffset.magnitude * -m_rod.transform.forward;
        target.y = transform.position.y;

        // TODO Replace with filet catch
        if (outofwater || ((target-transform.position).sqrMagnitude <= minMagnitude * minMagnitude))
        {
            if (fishyInstance == null)
            {
                TrackRod();
            }
        }

        if (outofwater) return;
        // TODO replace const with var for fish weight
        transform.position = (target - transform.position).normalized * 0.0625f + transform.position;
    }

    private void OnCollisionEnter(Collision other)
    {
        // TODO how tf do I check body overlap without the plane being a trigger?
        Debug.Log("Bump");

        if (other.gameObject.CompareTag("Water"))
        {
            outofwater = false;
            startedfish = false;
        }

        if (other.gameObject.CompareTag("Floor") || other.gameObject.CompareTag("Water"))
        {
            rb.linearVelocity = Vector3.zero;
            landed = true;
        }
        
        if (other.gameObject.CompareTag("Floor") && !other.gameObject.CompareTag("Water"))
        {
            Debug.Log("On floor not water");
            outofwater = true;
        }
        
        if (other.gameObject.CompareTag("Net"))
        {
            TrackRod();

            if (fishyInstance != null)
            {
                Destroy(fishyInstance);
                fishyInstance = null;
            }
        }

        
        if (!outofwater)
        {
            StartCoroutine(StartTimer());
        }
        
    }

    void TrackRod()
    {
        landed = false;
        followingRodToggle = true;
        m_followingRod = true;
        outofwater = true;

        if (fishyInstance != null)
        {
            Destroy(fishyInstance);
            fishyInstance = null;
        }
    }

    IEnumerator StartTimer()
    {   
        Debug.Log("Timer started!");
        startedfish = true;
        int duration = Random.Range(3, 5); 
        yield return new WaitForSeconds(duration);
        Debug.Log("Timer finished!");

        if (startedfish && !outofwater && fishyInstance == null)
        {
            fishyInstance = Instantiate(fishy, transform);
        }
    }
}
