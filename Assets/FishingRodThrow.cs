using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class FishingRodThrow : MonoBehaviour
{
    
    
    [SerializeField]
    InputActionReference m_throwAction;
    
    [SerializeField]
    InputActionReference m_reelAction;

    [SerializeField] private GameObject m_bait;
    private InputAction reelBait = null;

    
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
        
        reelBait = GetInputAction(m_reelAction);
        if (reelBait != null)
        {
            reelBait.performed += OnReelDemanded;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (reelBait != null)
        {
            if (reelBait.ReadValueAsObject() != null)
            {
                m_bait.GetComponent<BaitBehaviour>().DoReel();
            }
        }
    }
    
    static InputAction GetInputAction(InputActionReference actionReference)
    {
#pragma warning disable IDE0031 // Use null propagation -- Do not use for UnityEngine.Object types
        return actionReference != null ? actionReference.action : null;
#pragma warning restore IDE0031
    }

    private void OnThrowDemanded(InputAction.CallbackContext ctx)
    {
        m_bait.GetComponent<BaitBehaviour>().ExecuteThrow();
    }
    
    private void OnReelDemanded(InputAction.CallbackContext ctx)
    {
        m_bait.GetComponent<BaitBehaviour>().DoReel();
    }
    
    
}
