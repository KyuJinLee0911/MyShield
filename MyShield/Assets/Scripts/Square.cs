using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.gameObject.CompareTag("Balloon"))
            GameManager.instance.GameOver();
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < -6)
        {
            GameManager.ReturnObject(this);
        }
            
    }
}
