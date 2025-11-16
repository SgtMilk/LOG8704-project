using System;
using Unity.Mathematics;
using UnityEngine;

public class BaitBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject m_rod;
    
    private Vector3 previousPosition = Vector3.zero;
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
        if (m_followingRod)
        {
            transform.position = (m_rod.transform.position) + targetOffset.magnitude * m_rod.transform.up;
        }
        else
        {
            
            if (transform.position.y < 0)
            {
                transform.position -= new Vector3(0, transform.position.y - (float) 0.1, 0);
            }
        }
    }

    private void FixedUpdate()
    {
        previousPosition = transform.position;
    }

    public void ExecuteThrow()
    {
        GetComponent<Rigidbody>().AddForce((previousPosition-transform.position).normalized * 5, ForceMode.Impulse);
    }
}
