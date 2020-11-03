using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    private new SpriteRenderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        renderer = gameObject.GetComponent<SpriteRenderer>();
        StartCoroutine("HitText");
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Fades the text and scrolls it upwards
    IEnumerator HitText()
    {
        yield return new WaitForSeconds(0.25f);
        for (float ft = 1; ft > 0; ft -= 0.025f)
        {
            Color c = renderer.color;
            c.a = ft;
            renderer.material.color = c;
            gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + 0.02f, transform.position.z);
            yield return null;
        }
        Destroy(gameObject);
    }
}
