using UnityEngine;

public class BombExplosion : MonoBehaviour
{
    private BombManager bombManager;

    [SerializeField] private float force;
    [SerializeField] private float radius;
    public float Radius { get => radius; }

    private Vector3 originalPosition;

    private void Awake()
    {
        this.bombManager = GetComponent<BombManager>();

        transform.position = new Vector3(transform.position.x, transform.position.y, -7f);
        this.originalPosition = transform.position;
    }

    public void Explode()
    {
        Handheld.Vibrate();
        SoundManager.Instance.PlaySFX(SoundEffect.Bomb);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, this.Radius, Config.Instance.PlatesLayerMask);

        Plate plate;
        Rigidbody2D rigidbody;
        foreach (Collider2D collider in colliders)
        {
            plate = collider.GetComponent<Plate>();
            if (plate == null)
                return;

            rigidbody = plate.RigidBody;
            if (plate.PinnedBoltCount <= 1)
            {
                Vector2 direction = (rigidbody.position - (Vector2)transform.position).normalized;
                rigidbody.AddForce(direction * this.force, ForceMode2D.Impulse);
                plate.RemoveSelf();
            }
        }

        transform.position = originalPosition;
        this.bombManager.DeactiveBomb();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, this.Radius);
    }
}
