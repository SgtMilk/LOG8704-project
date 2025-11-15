using System;
using UnityEngine;

public class BaitBehaviour : MonoBehaviour
{
    [SerializeField]
    private Transform m_FishingRod;
    
    private bool followingRod = true;
    private Vector3 targetOffset;
    private Quaternion rotationOffset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetOffset = transform.position - m_FishingRod.transform.position;
        rotationOffset = m_FishingRod.transform.rotation * Quaternion.Inverse(transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        if (followingRod)
        {
            // transform.position = m_FishingRod.transform.position + targetOffset;
            // transform.rotation = rotationOffset * m_FishingRod.transform.rotation;
        }
    }
}
