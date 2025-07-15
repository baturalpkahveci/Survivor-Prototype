using UnityEngine;

public class HomingProjectile : ProjectileBase
{
    public Transform target;
    public float rotateSpeed = 5f;

    protected override void InitMovement()
    {
        // Velocity initialized once, then direction is updated in Update
        rb.linearVelocity = transform.forward * speed;
    }

    void Update()
    {
        if (target == null) return;

        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * rotateSpeed);

        rb.linearVelocity = transform.forward * speed;
    }
}
