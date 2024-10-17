using Unity.Netcode.Components;
using UnityEngine;

namespace Core
{
    public class ClientNetworkAnimator : NetworkAnimator
    {
        protected override bool OnIsServerAuthoritative()
        {
            return false;
        }
    }
}
