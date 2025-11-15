using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class FishingRodThrow : MonoBehaviour
{
    private Vector3 previousPosition = Vector3.zero;
    
    
    [SerializeField]
    InputActionReference m_throwAction;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnEnable()
    {
        var throwBait = GetInputAction(m_throwAction);
        if (throwBait != null)
        {
            throwBait.performed += OnThrowDemanded;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
        previousPosition = transform.position;

    }
    
    static InputAction GetInputAction(InputActionReference actionReference)
    {
#pragma warning disable IDE0031 // Use null propagation -- Do not use for UnityEngine.Object types
        return actionReference != null ? actionReference.action : null;
#pragma warning restore IDE0031
    }

    private void OnThrowDemanded(InputAction.CallbackContext ctx)
    {
        Debug.Log(previousPosition-transform.position);
    }
}
