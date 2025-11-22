using UnityEngine;
using System.Collections;

public class BirdSoundPlayer : MonoBehaviour
{
    
    private bool waitingonsound = false;
    private AudioSource audioSource = null;

    private int duration = 0;
    private float currTime = 0;

    [SerializeField]
    private int mintime = 15;

    [SerializeField]
    private int maxtime = 45;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (audioSource != null && !waitingonsound)
        {
            duration = Random.Range(mintime, maxtime);
            waitingonsound = true;
            currTime = 0;
        } else if (audioSource != null && waitingonsound)
        {
            currTime += Time.deltaTime;
            if (currTime >= duration)
            {
                audioSource.PlayOneShot(audioSource.clip);
                waitingonsound = false;
            }
        }
    }
}
