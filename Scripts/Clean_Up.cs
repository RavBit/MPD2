using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clean_Up : MonoBehaviour {
    public float maxTime;
    public float minSwipeDist;
    public Animator RemoveStain;
    public Animator BirdWing;

    float startTime;
    float endTime;

    Vector3 startPos;
    Vector3 endPos;

    Vector2 firstPressPos;
    Vector2 secondPressPos;
    Vector2 currentSwipe;

    float swipeDistance;
    float swipeTime;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_STANDALONE || UNITY_WEBGL || UNITY_EDITOR
        SwipeMouse();
#endif
#if UNITY_ANDROID
        SwipeAndroid();
#endif

    }
    void SwipeAndroid()
    {
        if (Input.touchCount > 0)
        {
            Debug.Log("Touch");
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                startTime = Time.time;
                startPos = touch.position;

            }
            else if (touch.phase == TouchPhase.Ended)
            {
                endTime = Time.time;
                endPos = touch.position;

                swipeDistance = (endPos - startPos).magnitude;
                swipeTime = endTime - startTime;
            }

            if (swipeTime < maxTime && swipeDistance > minSwipeDist)
                swipeFunction();
        }
    }
    void SwipeMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            firstPressPos = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            currentSwipe = (Vector2)Input.mousePosition - firstPressPos;
            if(currentSwipe.normalized.x > 0)
            {
                removeStain();
            }
        }
    }
    void swipeFunction()
    {
        Vector2 distance = endPos - startPos;
        if (Mathf.Abs(distance.x) > Mathf.Abs(distance.y))
            removeStain();
    }

    public void removeStain()
    {
        RemoveStain.SetBool("clean", true);
        BirdWing.SetTrigger("trigger");
        Invoke("PlayerFinished", 3);
    }
    void PlayerFinished()
    {
        GetComponentInParent<Player>().CmdMiniGameFinished();
    }
}
