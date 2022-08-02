using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private Animator BlockAnimator;

    [SerializeField] private AudioSource _audio;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("line"))
        {           
                    _audio.Play();
                    Debug.Log("collision");
                    BlockAnimator.enabled = true;
                    BlockAnimator.SetBool("reached", true);
                    StartCoroutine(WaitAndDestroy());
        }
                
    }

    IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }
}
