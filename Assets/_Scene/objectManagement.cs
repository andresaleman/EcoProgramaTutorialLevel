using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectManagement : MonoBehaviour {

    Vector3 tempPos;
    float firstx;
    float firsty;

	// Use this for initialization
	void Start () {
        firstx = transform.position.x;
        firsty = transform.position.y;
       
    }

    // Update is called once per frame
    void Update () {
        objectToObjectMovement(this.gameObject, GameObject.Find("dibujo"), 1704f, 87f, 400f);
        //float speed = 400f;
        //GameObject dibujo = GameObject.Find("dibujo");
        //Vector3 dibujoPos = dibujo.transform.position;
        //tempPos = transform.position;
        //if (Vector3.Distance((Vector3)tempPos, dibujoPos) < 3f)
        //{
        //    print("Ya");
        //    transform.Translate(0, 0, 0, Camera.main.transform);
        //    transform.position = new Vector2(1704f, 87f);
        //    print("Postiion x: " + tempPos.x + "  Position y: " + tempPos.y);
        //}
        //else
        //{
        //    Vector3 travel = dibujoPos - (Vector3)tempPos;
        //    travel.Normalize();
        //    //transform.position = tempPos;
        //    transform.Translate(travel.x * speed * Time.deltaTime, travel.y * speed * Time.deltaTime, 0, Camera.main.transform);
        //    print("Postiion x: " + tempPos.x + "  Position y: " + tempPos.y);

        //}
        //print("Postiion x: " + tempPos.x + "  Position y: " + tempPos.y);
        //if(tempPos.x<=0f | tempPos.y >= 1000f)
        //{
        //    print("Start again");
        //    tempPos.x = firstx;
        //    tempPos.y = firsty;
        //}
        //else
        //{
        //    tempPos.x -= 5f;
        //    tempPos.y += 5f;
        //}


        //dibujo.transform.Translate(-5, 5, 0, Camera.main.transform);

    }

    public void objectToObjectMovement(GameObject from, GameObject to, float startX, float startY, float speed)
    {
        Vector3 fromVector = from.transform.position;
        Vector3 toVector = to.transform.position;

        if (Vector3.Distance(fromVector, toVector) < 3f)
        {
            transform.Translate(0, 0, 0, Camera.main.transform);
            transform.position = new Vector2(startX, startY);
        }
        else
        {
            Vector3 travel = toVector - fromVector;
            travel.Normalize();
            transform.Translate(travel.x * speed * Time.deltaTime, travel.y * speed * Time.deltaTime, 0, Camera.main.transform);
        }
    }
}
