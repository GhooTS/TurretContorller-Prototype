using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    public float speed;
    public float damaged;
    public Rigidbody2D rb2D;
    public ReactionQueue reactionQueue;

    private void OnEnable()
    {
        rb2D = GetComponent<Rigidbody2D>();
        reactionQueue.AddReaction(new WaitForObjectDestroyReaction(gameObject));
    }


    public void Shoot(Vector2 direction)
    {
        rb2D.AddForce(direction * speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(this.gameObject);
    }
}
