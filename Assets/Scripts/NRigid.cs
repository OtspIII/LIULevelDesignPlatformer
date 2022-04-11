using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(NetworkTransform))]
    public class NRigid : NetworkBehaviour
    {
        public Rigidbody m_Rigidbody;
        public NetworkTransform m_NetworkTransform;

        public bool m_OriginalKinematic;
        public RigidbodyInterpolation m_OriginalInterpolation;

        // Used to cache the authority state of this rigidbody during the last frame
        public bool m_IsAuthority;

        /// <summary>
        /// Gets a bool value indicating whether this <see cref="NetworkRigidbody"/> on this peer currently holds authority.
        /// </summary>
        private bool HasAuthority => m_NetworkTransform.CanCommitToTransform;

        private void Awake()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
            m_NetworkTransform = GetComponent<NetworkTransform>();
        }

        private void FixedUpdate()
        {
            if (NetworkManager.IsListening)
            {
                if (HasAuthority != m_IsAuthority)
                {
                    m_IsAuthority = HasAuthority;
                    UpdateRigidbodyKinematicMode();
                }
            }
        }

        // Puts the rigidbody in a kinematic non-interpolated mode on everyone but the server.
        private void UpdateRigidbodyKinematicMode()
        {
            if (m_IsAuthority == false)
            {
                m_OriginalKinematic = m_Rigidbody.isKinematic;
                m_Rigidbody.isKinematic = true;

                m_OriginalInterpolation = m_Rigidbody.interpolation;
                // Set interpolation to none, the NetworkTransform component interpolates the position of the object.
                m_Rigidbody.interpolation = RigidbodyInterpolation.None;
            }
            else
            {
                // Resets the rigidbody back to it's non replication only state. Happens on shutdown and when authority is lost
                m_Rigidbody.isKinematic = m_OriginalKinematic;
                m_Rigidbody.interpolation = m_OriginalInterpolation;
            }
        }

        /// <inheritdoc />
        public override void OnNetworkSpawn()
        {
            m_IsAuthority = HasAuthority;
            m_OriginalKinematic = m_Rigidbody.isKinematic;
            m_OriginalInterpolation = m_Rigidbody.interpolation;
            UpdateRigidbodyKinematicMode();
        }

        /// <inheritdoc />
        public override void OnNetworkDespawn()
        {
            UpdateRigidbodyKinematicMode();
        }
    }