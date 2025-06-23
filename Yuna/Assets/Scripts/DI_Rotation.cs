using UnityEngine;

public class DI_Rotation : MonoBehaviour
{

    public float velocity = 50.0f;
    public float smooth = 0.005f;
    private bool col = false;
    private int h = 0;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * velocity);

        if (col)
        {
           if(h < 25)
           {
                h = h + 1;
                transform.Translate(0, smooth, 0);
           }
        }
        else
        {
            if (h > 0)
            {
                h = h - 1;
                transform.Translate(0, -smooth, 0);
            }
        }
    }

    private void OnTriggerEnter(Collider colision)
    {
        if(colision.tag == "Player")
        {
            col = true;
        }

    }

    private void OnTriggerExit(Collider colision)
    {
        if (colision.tag == "Player")
        {
            col = false;
        }
    }


}
