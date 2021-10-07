using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] float loseWait = 1f;
    float startingWait;
    bool playable = false;

    private Vector3 fp;
    private Vector3 lp;
    private float dragDistance;

    List<Vector3> pathPositions;
    Rigidbody2D myRigidbody;
    Animator myAnimator;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 startPos = new Vector3(FindObjectOfType<PathGenerator>().GetStartingX() + 1, 1, 0);
        transform.position = startPos;

        dragDistance = Screen.width / 10;

        pathPositions = new List<Vector3>();
        FillPathPositions();

        startingWait = FindObjectOfType<PathGenerator>().GetShowTime();
        StartCoroutine(WaitAtStart());

        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    IEnumerator WaitAtStart()
    {
        yield return new WaitForSeconds(startingWait);
        playable = true;
    }

    void Update()
    {
        if (Input.touchCount == 1 && playable) // user is touching the screen with a single touch
        {
            Touch touch = Input.GetTouch(0); // get the touch
            if (touch.phase == TouchPhase.Began) //check for the first touch
            {
                fp = touch.position;
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved) // update the last position based on where they moved
            {
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended) //check if the finger is removed from the screen
            {
                lp = touch.position;  //last touch position. Ommitted if you use list

                //Check if drag distance is greater than 20% of the screen height
                if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
                {//It's a drag
                 //check if the drag is vertical or horizontal
                    if (Mathf.Abs(lp.x - fp.x) > Mathf.Abs(lp.y - fp.y))
                    {   //If the horizontal movement is greater than the vertical movement...
                        if ((lp.x > fp.x))  //If the movement was to the right)
                        {   //Right swipe
                            MoveRight();
                        }
                        else
                        {   //Left swipe
                            MoveLeft();
                        }
                    }
                    else
                    {   //the vertical movement is greater than the horizontal movement
                        if (lp.y > fp.y)  //If the movement was up
                        {   //Up swipe
                            MoveUp();
                        }
                        else
                        {   //Down swipe
                            MoveDown();
                        }
                    }
                }
                else
                {   //It's a tap as the drag distance is less than 20% of the screen height
                    //Debug.Log("Tap");
                }
            }
        }
    }

    private void MoveDown()
    {
        myAnimator.SetBool("Idle", false);
        myAnimator.SetTrigger("Down");
        myRigidbody.velocity = new Vector2(0, -2);
        StartCoroutine(WaitForMove());
    }

    private void MoveUp()
    {
        myAnimator.SetBool("Idle", false);
        myAnimator.SetTrigger("Up");
        myRigidbody.velocity = new Vector2(0, 2);
        StartCoroutine(WaitForMove());
    }

    private void MoveLeft()
    {
        myAnimator.SetBool("Idle", false);
        myAnimator.SetTrigger("Left");
        myRigidbody.velocity = new Vector2(-2, 0);
        StartCoroutine(WaitForMove());
    }

    private void MoveRight()
    {
        myAnimator.SetBool("Idle", false);
        myAnimator.SetTrigger("Right");
        myRigidbody.velocity = new Vector2(2,0);
        StartCoroutine(WaitForMove());
    }

    IEnumerator WaitForMove()
    {
        yield return new WaitForSeconds(0.5f);
        Stop();
        CheckPos();
    }

    private void Stop()
    {
        myRigidbody.velocity = new Vector2(0,0);
        transform.position = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), 0f);
        myAnimator.SetBool("Idle", true);
    }

    private void CheckPos()
    {
        bool onPath = false;
        foreach(Vector3 pos in pathPositions)
        {
            if (pos.x == transform.position.x && pos.y == transform.position.y)
            {
                onPath = true;
            }
        }
        if (!onPath)
        {
            Die();
        }
        else 
        {
            FindObjectOfType<PathGenerator>().ShowTile(transform.position);
            if (transform.position.y == 8)
            {
                FindObjectOfType<GameSession>().Win();
                Destroy(gameObject);
            }   
        }
    }

    private void FillPathPositions()
    {
        Tile[] tiles = FindObjectsOfType<Tile>();
        foreach(Tile tile in tiles)
        {
            pathPositions.Add(tile.gameObject.transform.position);
        }
    }

    private void Die()
    {
        StartCoroutine(ShowPathAndLose());
    }

    IEnumerator ShowPathAndLose()
    {
        myAnimator.SetTrigger("Fall");
        FindObjectOfType<PathGenerator>().ShowAllTiles();
        yield return new WaitForSeconds(loseWait);
        FindObjectOfType<GameSession>().Lose();
        Destroy(gameObject);
    }
}
