using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCollisionScript : MonoBehaviour
{
    public AudioClip audioClip;
    private AudioSource drumAudio;

    // Start is called before the first frame update
    void Start()
    {
        GameObject drum = this.gameObject;
        //Debug.Log(this.gameObject);
        drumAudio = drum.AddComponent<AudioSource>();
        drumAudio.clip = audioClip;
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "DrumStick")
        {
            Debug.Log("You hit the " + collision.gameObject.name);
            drumAudio.Play(0);
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
