using System;
using Unity.Mathematics;
using UnityEngine;

public class BaitBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject m_rod;
    
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
        if (followingRodToggle)
        {
            if (transform.position.y < 0)
            {
                transform.position -= new Vector3(0, transform.position.y, 0);
            }
        }

        if (m_followingRod)
        {
            transform.position = (m_rod.transform.position) + targetOffset.magnitude * m_rod.transform.up;
        }
    }
}
