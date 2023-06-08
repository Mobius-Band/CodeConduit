using UnityEngine;

namespace HackNSlash.Scripts.Combat
{
    public class Knockback : MonoBehaviour
    {
        public void ApplyKnockback(Transform hitObject, int amount)
        {
            transform.GetComponent<Rigidbody>().AddForce(hitObject.forward * amount, ForceMode.Acceleration);
        }
    }
}
