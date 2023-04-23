using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float turnSpeed = 20f;

    private Animator m_Animator;
    private Rigidbody m_Rigidbody;
    private Vector3 m_Moviment;
    private Quaternion m_Rotation = Quaternion.identity;
    private AudioSource m_AudioSource;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_AudioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        GetMoviment();
        GetRotation();
    }

    private void OnAnimatorMove()
    {
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Moviment * m_Animator.deltaPosition.magnitude);
        m_Rigidbody.MoveRotation(m_Rotation);
    }

    private void GetRotation()
    {
        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Moviment, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation(desiredForward);
    }

    private void GetMoviment()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        m_Moviment.Set(horizontal, 0f, vertical);
        m_Moviment.Normalize();

        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        
        StartAnimation(isWalking);
        PlayAudioSource(isWalking);
    }

    private void StartAnimation(bool isWalking)
    {
        m_Animator.SetBool("IsWalking", isWalking);
    }

    private void PlayAudioSource(bool isWalking)
    {
        if (isWalking && !m_AudioSource.isPlaying)
        {
            m_AudioSource.Play();
        }
        else
        {
            m_AudioSource.Stop();

        }
    }
}
