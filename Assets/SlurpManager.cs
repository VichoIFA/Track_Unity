using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlurpManager : MonoBehaviour
{
    [SerializeField] private GrabInteractable grabbable;

    [SerializeField] private AudioSource audioSource;

    private bool destroying = false;

    private void Update()
    {
        if (grabbable.State == InteractableState.Select &&
            Vector3.Distance(this.transform.position, Camera.main.transform.position) < .25f && !destroying)
        {
            destroying = true;

            audioSource.Play();

            Destroy(gameObject, 1f);
        }
    }
}
