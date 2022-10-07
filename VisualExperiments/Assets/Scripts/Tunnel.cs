using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Tunnel : MonoBehaviour
{
    [SerializeField]
    TunnelEffect vfx;
    [SerializeField]
    float speed;
    [SerializeField]
    float radius;
    [SerializeField]
    float distance;
    [SerializeField]
    float interval;

    int amount => (int)((distance / speed) / interval);

    float timer = 0;


    Queue<GameObject> queue = new Queue<GameObject>();

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            timer = 0;
            if (queue.Count >= amount)
            {
                Destroy(queue.Dequeue());
            }
            Vector3 circlePos = (Vector3)Random.insideUnitCircle.normalized;
            Vector3 position =  circlePos * radius + new Vector3(0, 0, distance);
            var rotation = Quaternion.LookRotation(Vector3.forward, -circlePos);
            var effect = Instantiate(vfx, transform.position + position, rotation, transform);
            effect.speed = speed;
            effect.direction = Vector3.back;
            effect.center = Vector3.zero;
            queue.Enqueue(effect.gameObject);

        }

    }

}
