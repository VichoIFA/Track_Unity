using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GunControlGrabManager : MonoBehaviour
{
    [SerializeField] private GrabInteractable grabbable;
    [SerializeField] private Transform leftController;
    [SerializeField] private Transform rightController;

    private AudioSource shootSoundSource;

    private bool isLeftHand = false;

    private bool grabbedZapper = false;

    private void Start()
    {
        shootSoundSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(grabbable.State == InteractableState.Select && !grabbedZapper)
        {
            Debug.Log(grabbable.State);

            grabbedZapper = true;

            if(Vector3.Distance(leftController.position, this.transform.position) < Vector3.Distance(rightController.position, this.transform.position))
            {
                this.transform.parent = leftController;
                this.transform.rotation = leftController.rotation * Quaternion.Euler(25, 0, 0);
                this.transform.localPosition = Vector3.zero + Vector3.down * 0.1f;

                isLeftHand = true;
            }

            else
            {
                this.transform.parent = rightController;
                this.transform.rotation = rightController.rotation * Quaternion.Euler(25, 0, 0);
                this.transform.localPosition = Vector3.zero + Vector3.down * 0.1f;

                isLeftHand = false;
            }
        }

        else if(grabbable.State == InteractableState.Normal && grabbedZapper)
        {
            this.transform.parent = null;
            grabbedZapper = false;
        }

        if(OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) && isLeftHand && grabbedZapper || 
          (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger) && !isLeftHand && grabbedZapper  && !shootSoundSource.isPlaying))
        {
            shootSoundSource.Play();
        }
    }
}
