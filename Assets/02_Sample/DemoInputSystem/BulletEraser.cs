using UnityEngine;

public class BulletEraser : MonoBehaviour
{
    // ---------------------------- SerializeField


    // ---------------------------- Field



    // ---------------------------- UnityMessage
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(TagName.Bullet))
        {
            Destroy(collision.gameObject);
        }
    }



    // ---------------------------- PublicMethod





    // ---------------------------- PrivateMethod





}
