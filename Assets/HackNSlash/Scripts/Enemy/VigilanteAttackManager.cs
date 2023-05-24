using Combat;
using UnityEngine;

public class VigilanteAttackManager : MonoBehaviour
{
    [SerializeField] private ProjectileAttack projectileAttack;
    [SerializeField] private Attack bodyLaunchAttack;
    [SerializeField] private LayerMask attackedMask;

    //Should be called on an Animation Event
    public void Shoot()
    {
        projectileAttack.Execute(transform, attackedMask);
    }

    public void ThrowSelf()
    {
        // bodyLaunchAttack.
    }

}
