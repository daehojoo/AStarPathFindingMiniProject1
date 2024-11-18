using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ontrigger : MonoBehaviour
{
    SpriteRenderer image;
    private void Start()
    {
        image = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("123");
        
        if (collision.gameObject.CompareTag("PlayerUnit"))
        { 
            
            image.color = Color.red;
        }
        
            

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        image.color = Color.green;
    }
}
