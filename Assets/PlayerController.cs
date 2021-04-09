using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float distance = 1f;
    private bool readyToMove = true;

    private RaycastHit2D rightHit;
    private RaycastHit2D forwardHit;
    private RaycastHit2D leftHit;

    public GameObject panel;

    private void CastRays()
    {
        LayerMask layerMask = LayerMask.GetMask("Maze");

        rightHit = Physics2D.Raycast(transform.position, transform.right, distance, layerMask);
        forwardHit = Physics2D.Raycast(transform.position, transform.up, distance, layerMask);
        leftHit = Physics2D.Raycast(transform.position, -transform.right, distance, layerMask);

        Debug.Log(rightHit.collider);
        Debug.Log(forwardHit.collider);
        Debug.Log(leftHit.collider);
    }

    private void Update()
    {
        if (readyToMove)
            StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        /*
        * # Wall follower, Right-hand Rule
        * While Not target_point
        *  If right is open Then
        *      turn_right
        *  Else If front is open Then
        *      go_forward
        *  Else If left is open Then
        *      turn_left
        *  Else
        *      Turn_around
        * Loop
        */

        readyToMove = false;

        CastRays();

        if (rightHit.collider == null)
        {
            transform.Rotate(0.0f, 0.0f, -90.0f, Space.Self);
            print("Turning Right");

            yield return new WaitForSeconds(1);

            CastRays();

            if (forwardHit.collider == null)
            {
                transform.Translate(Vector2.up);
                print("Going Forward");
            }
        }
        else if (forwardHit.collider == null)
        {
            transform.Translate(Vector2.up);
            print("Going Forward");
        }
        else if (leftHit.collider == null)
        {
            transform.Rotate(0.0f, 0.0f, 90.0f, Space.Self);
            Debug.Log("Turning Left");

            yield return new WaitForSeconds(1);

            CastRays();

            if (forwardHit.collider == null)
            {
                transform.Translate(Vector2.up);
                print("Going Forward");
            }
        }
        else
        {
            transform.Rotate(0.0f, 0.0f, 180.0f, Space.Self);
            print("Turning Around");

            yield return new WaitForSeconds(1);

            CastRays();

            if (forwardHit.collider == null)
            {
                transform.Translate(Vector2.up);
                print("Going Forward");
            }
        }

        yield return new WaitForSeconds(1);

        readyToMove = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<BoxCollider2D>() != null)
        {
            panel.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
