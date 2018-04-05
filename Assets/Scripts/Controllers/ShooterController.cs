using Global;
using System.Collections.Generic;
using UnityEngine;

public class ShooterController : MonoBehaviour
{
    private Queue<GameObject> bullets = new Queue<GameObject>();
    private float _time = 0.0f;

    public InputMapping.PlayerTag playerTag;
    public int bulletCount = 10;
    public float bulletSpeed = 300.0f;
    public float cooldown = 0.25f;

    void Update()
    {
        if(_time > 0.0f)
        {
            _time -= Time.deltaTime;
        }
        else if(Input.GetAxisRaw(InputMapping.GetInputName(playerTag, InputMapping.Input.RT)) > 0.5)
        {
            _time = cooldown;
            Shoot();
        }
    }

    private void Shoot()
    {
        GameObject bullet;
        if (bullets.Count < bulletCount)
        {
            bullet = Instantiate(Resources.Load("Bullet", typeof(GameObject))) as GameObject;
        }
        else
        {
            bullet = bullets.Dequeue();
            if(bullet == null)
            {
                bullet = Instantiate(Resources.Load("Bullet", typeof(GameObject))) as GameObject;
            }
        }

        bullet.transform.position = transform.position + transform.forward + transform.up;
        Rigidbody bulletBody = bullet.GetComponent<Rigidbody>();
        bulletBody.velocity = Vector3.zero;
        bulletBody.AddForce(transform.forward * bulletSpeed);
        bullets.Enqueue(bullet);
    }
}
