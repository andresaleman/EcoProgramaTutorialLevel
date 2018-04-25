using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{

    Vector2 tempPos;
    float firstx;
    float firsty;
    static int prevStep;
    public GameObject row1A;
    public GameObject row1D;
    public GameObject row1Q;
    public GameObject row2A;
    public GameObject row2D;
    public GameObject row2Q;
    public GameObject row3A;
    public GameObject row3D;
    public GameObject row3Q;
    public GameObject row4A;
    public GameObject row4D;
    public GameObject row4Q;
    public GameObject row5A;
    public GameObject row5D;
    public GameObject row5Q;
    public GameObject row6A;
    public GameObject row6D;
    public GameObject row6Q;
    public GameObject row7A;
    public GameObject buttonPlay;


    // Use this for initialization
    void Start()
    {
        firstx = 588;
        firsty = 128;
        // firstx = transform.position.x;
        // firsty = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        tempPos = transform.position;
        print("Postiion x: " + tempPos.x + "  Position y: " + tempPos.y);
        if(DragAndDropCell.Tutorialstep == 1)
        {
            objectToObjectMovement(this.gameObject, row1A, 588f, 128f, 400f); //swim
        }
        if (DragAndDropCell.Tutorialstep == 2)
        {
            objectToObjectMovement(this.gameObject, row1D, 1347f, 32f, 400f); //right
        }
        if(DragAndDropCell.Tutorialstep == 3)
        {
            objectToObjectMovement(this.gameObject, row1Q, 1592f, 6f, 400f); //4
        }
        if (DragAndDropCell.Tutorialstep == 4)
        {
            objectToObjectMovement(this.gameObject, row2A, 588f, 38f, 400f); //pick up
        }
        if (DragAndDropCell.Tutorialstep == 5)
        {
            objectToObjectMovement(this.gameObject, row3A, 588f, 128f, 400f);//swim
        }
        if (DragAndDropCell.Tutorialstep == 6)
        {
            objectToObjectMovement(this.gameObject, row3D, 1338f, 122f, 400f); //down
        }
        if (DragAndDropCell.Tutorialstep == 7)
        {
            objectToObjectMovement(this.gameObject, row3Q, 1592f, 6f, 400f); //3
        }
        if (DragAndDropCell.Tutorialstep == 8)
        {
            objectToObjectMovement(this.gameObject, row4A, 837f, 36f, 400f);  //drop
        }
        if (DragAndDropCell.Tutorialstep == 9) //waiting for play button
        {
            objectToObjectMovement(this.gameObject, buttonPlay, 433f, 975f, 0f);
        }
        if (DragAndDropCell.Tutorialstep == 10)
        {
            objectToObjectMovement(this.gameObject, row1A, 588f, 128f, 400f); //swim
        }
        if (DragAndDropCell.Tutorialstep == 11)
        {
            objectToObjectMovement(this.gameObject, row1D, 1144f, 5f, 400f); //left
        }
        if (DragAndDropCell.Tutorialstep == 12)
        {
            objectToObjectMovement(this.gameObject, row1Q, 1588f, 125.5f, 400f); //One
        }
        if (DragAndDropCell.Tutorialstep == 13)
        {
            objectToObjectMovement(this.gameObject, row2A, 626f, 44f, 400f); //pickup
        }
        if (DragAndDropCell.Tutorialstep == 14) //Second Part
        {
            //this.gameObject.SetActive(true);
            objectToObjectMovement(this.gameObject, row3A, 588f, 128f, 400f); //swim
        }
        if (DragAndDropCell.Tutorialstep == 15)
        {
            objectToObjectMovement(this.gameObject, row3D, 1161f, 153f, 400f); //up
        }
        if (DragAndDropCell.Tutorialstep == 16)
        {
            objectToObjectMovement(this.gameObject, row3Q, 1734f, 124.5f, 400f); //two
        }
        if (DragAndDropCell.Tutorialstep == 17)
        {
            objectToObjectMovement(this.gameObject, row4A, 597f, 121f, 400f); //swim
        }
        if (DragAndDropCell.Tutorialstep == 18)
        {
            objectToObjectMovement(this.gameObject, row4D, 1349f, 40f, 400f); //right
        }
        if (DragAndDropCell.Tutorialstep == 19)
        {
            objectToObjectMovement(this.gameObject, row4Q, 1734f, 124.5f, 400f); //2
        }
        if (DragAndDropCell.Tutorialstep == 20)
        {
            objectToObjectMovement(this.gameObject, row5A, 626f, 44f, 400f); //pickup
        }
        if (DragAndDropCell.Tutorialstep == 21)
        {
            objectToObjectMovement(this.gameObject, row6A, 797f, 119f, 400f); //jump
        }
        if (DragAndDropCell.Tutorialstep == 22)
        {
            objectToObjectMovement(this.gameObject, row6D, 1339f, 151f, 400f); //down
        }
        if (DragAndDropCell.Tutorialstep == 23)
        {
            objectToObjectMovement(this.gameObject, row7A, 832f, 45f, 400f); //drop
        }
        if(DragAndDropCell.Tutorialstep == 24)
        {
            objectToObjectMovement(this.gameObject, buttonPlay, 433f, 975f, 0f); //play button
        }

        
    }

    public void objectToObjectMovement(GameObject from, GameObject to, float startX, float startY, float speed)
    {
        if (prevStep < DragAndDropCell.Tutorialstep)
        {
            //set start position
            tempPos.x = startX;
            tempPos.y = startY;
            prevStep = DragAndDropCell.Tutorialstep;
            transform.position = tempPos;
        }
        else
        {
            Vector3 fromVector = from.transform.position;
            Vector3 toVector = to.transform.position;
            toVector.x = toVector.x + 50;
            toVector.y = toVector.y - 50;
            if (Vector3.Distance(fromVector, toVector) < 10f)
            {
                transform.Translate(0, 0, 0, Camera.main.transform);
                transform.position = new Vector2(startX, startY);
            }
            else
            {
                Vector3 travel = toVector - fromVector;
                print("Aqui estoy");
                print("From " + fromVector.x + ", " + fromVector.y);
                print("To: " + toVector.x + ", " + toVector.y);
                travel.Normalize();
                transform.Translate(travel.x * speed * Time.deltaTime, travel.y * speed * Time.deltaTime, 0, Camera.main.transform);
                print("Se movio");
            }
        }
       
    }


    public static void ObjectMovementSlope2(GameObject toObject, Vector2 tempPos, float firstx, float firsty, float speed)
    {
        Vector3 toVector = toObject.transform.position;
        
        if (prevStep < DragAndDropCell.Tutorialstep)
        {
            //set start position
            tempPos.x = firstx;
            tempPos.y = firsty;
            prevStep = DragAndDropCell.Tutorialstep;
        }
        else
        {
            if (tempPos.x <= toVector.x | tempPos.y >= toVector.y)
            {
                print("Start again");
                tempPos.x = firstx;
                tempPos.y = firsty;
            }
            else
            {
                tempPos.x -= 1f * speed;
                tempPos.y += .41f * speed;

            }
        }
    }


}
