using System;
using UnityEngine;
using UnityEngine.Networking;

namespace Network
{
#pragma warning disable 618
    public abstract class NetworkMovableObject : NetworkBehaviour
#pragma warning restore 618
    {
        protected abstract float _speed { get; }
        protected Action _onUpdateAction { get; set; }
        protected Action _onFixedUpdateAction { get; set; }
        protected Action _onLateUpdateAction { get; set; }
        protected Action _onPreRenderActionAction { get; set; }
        protected Action _onPostRenderAction { get; set; }

        private UpdatePhase _updatePhase;

#pragma warning disable 618
        [SyncVar] protected Vector3 _serverPosition;
        [SyncVar] protected Vector3 _serverEuler;
#pragma warning restore 618
        public override void OnStartAuthority()
        {
            Initiate();
        }
        protected virtual void Initiate(UpdatePhase updatePhase = UpdatePhase.Update)
        {
            switch (updatePhase)
            {
                case UpdatePhase.Update:
                    _onUpdateAction += Movement;
                    break;
                case UpdatePhase.FixedUpdate:
                    _onFixedUpdateAction += Movement;
                    break;
                case UpdatePhase.LateUpdate:
                    _onLateUpdateAction += Movement;
                    break;
                case UpdatePhase.PostRender:
                    _onPostRenderAction += Movement;
                    break;
                case UpdatePhase.PreRender:
                    _onPreRenderActionAction += Movement;
                    break;
            }
        }
        private void Update()
        {
            _updatePhase = UpdatePhase.Update;
            _onUpdateAction?.Invoke();
        }
        private void LateUpdate()
        {
            _updatePhase = UpdatePhase.LateUpdate;
            _onLateUpdateAction?.Invoke();
        }
        private void FixedUpdate()
        {
            _updatePhase = UpdatePhase.FixedUpdate;
            _onFixedUpdateAction?.Invoke();
        }
        private void OnPreRender()
        {
            _updatePhase = UpdatePhase.PreRender;
            _onPreRenderActionAction?.Invoke();
        }
        private void OnPostRender()
        {
            _updatePhase = UpdatePhase.PostRender;
            _onPostRenderAction?.Invoke();
        }
        protected virtual void Movement()
        {
            if (hasAuthority)
            {
                HasAuthorityMovement();
            }
            else
            {
                FromServerUpdate();
            }
        }
        protected abstract void HasAuthorityMovement();
        protected abstract void FromServerUpdate();
        protected abstract void SendToServer();
    }
}