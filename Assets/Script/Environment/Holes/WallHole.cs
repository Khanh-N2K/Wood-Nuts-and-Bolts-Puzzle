using UnityEngine;

public class WallHole : MonoBehaviour
{
    public Bolt Bolt { get; private set; }
    [SerializeField] private float standardRadius;
    public float StandardRadius { get => standardRadius; }

    public virtual void PinBolt(Bolt bolt)
    {
        this.Bolt = bolt;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, this.standardRadius);
    }
}
