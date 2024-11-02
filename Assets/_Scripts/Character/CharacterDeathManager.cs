using Unity.Netcode;
using UnityEngine;

namespace Core
{
    public class CharacterDeathManager : NetworkBehaviour
    {

        [field: SerializeField] public bool IsDead { get; set; } = false;


        protected void Die()
        {
            IsDead = true;
        }

        public void CheckDeath(float previousHealth, float newHealth)
        {
            if(newHealth <= 0) Die();
        }
    }
}