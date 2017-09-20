using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour 
{
  Rigidbody2D _rb;

  void Awake()
  {
    _rb = GetComponent<Rigidbody2D>();
  }

  void Update()
  {
    if (Input.GetKey(KeyCode.Space))
    {
      _rb.AddForce(Vector2.right, ForceMode2D.Impulse);
    }
  }

  void FixedUpdate()
  {
    /*
    if (Input.GetKey(KeyCode.Space))
    {
      _rb.AddForce(Vector2.right, ForceMode2D.Impulse);
    }
    */
  }
}
