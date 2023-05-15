using UnityEngine;

namespace HackNSlash.Scripts.Combat
{
    public class Knockback : MonoBehaviour
    {
        [SerializeField] private float _knockbackForce = 50;
        
        public void ApplyKnockback(Transform hitObject, int amount)
        {
            transform.GetComponent<Rigidbody>().AddForce(hitObject.forward * amount * _knockbackForce, ForceMode.Acceleration);
        }
    }
}
