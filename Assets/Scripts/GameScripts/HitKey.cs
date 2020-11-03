using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitKey : MonoBehaviour
{
    public new Camera camera;

    private ParticleSystem particle;
    // Start is called before the first frame update
    void Start()
    {
        particle = this.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch;
            Touch[] touches = Input.touches;

            for (int i = 0; i < touches.Length; i++)
            {
                touch = touches[i];
                Vector2 touchPos = camera.ScreenToWorldPoint(touch.position);
                if (this.GetComponent<BoxCollider2D>().bounds.Contains(touchPos))
                {
                    particle.Play();
                }
            }
        }
    }
}
