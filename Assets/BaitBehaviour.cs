using System;
using Unity.Mathematics;
using UnityEngine;

public class BaitBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject m_rod;
    [SerializeField] private float miny = 0f;
    private bool landed = true;
    
    private static readonly System.Int32 frameDelay = 12;
    private Vector3[] previousPosition = new Vector3[frameDelay];
    private float minMagnitude = 0.5f;
    private int previousPositionIdx = 0;
    public bool followingRodToggle = false;
    public bool m_followingRod = true;
    private Vector3 targetOffset;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetOffset = transform.position - m_rod.transform.position;
        GetComponent<Collider>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
            
        if (transform.position.y < miny)
        {
            GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            transform.position -= new Vector3(0, transform.position.y - miny, 0);
            landed = true;
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
        Vector3 direction = transform.position - previousPosition[(previousPositionIdx + 1) % frameDelay];
        if (direction.sqrMagnitude > minMagnitude * minMagnitude) {
            GetComponent<Rigidbody>().AddForce(direction.normalized * 13.5f, ForceMode.Impulse);
            landed = false;
        }
    }

    public void DoReel() {
        if (!landed) return;
         
        // followingRodToggle = true;
        // m_followingRod = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            miny = other.transform.position.y;
        }
    }
}
