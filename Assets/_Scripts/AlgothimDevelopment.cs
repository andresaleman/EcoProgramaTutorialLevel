using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlgothimDevelopment : MonoBehaviour {
   internal static int size = 1;
       
    //rows,columns
   public static string [,] cellArray = new string[50,3];

    static Vector2  SheetStartposition;
  	// Use this for initialization
	void Start () {
        SheetStartposition = GameObject.Find("Sheet").transform.position;
    }
   


    public static void addCellArray(string name, string pname, int type) {

        //Get the number of the row by trimming the name of row (x). 
        int num = (int)char.GetNumericValue(pname.Substring(pname.LastIndexOf('(') + 1).ToCharArray()[0]);
           
         //Double Digits
        if ((pname.LastIndexOf('(') + 1) != (pname.LastIndexOf(')') - 1))
            {
                int num2 = (int)char.GetNumericValue(pname.Substring(pname.LastIndexOf(')') - 1).ToCharArray()[0]);
                num = num * 10 + num2;
            }
                
                cellArray[num - 1, type] = name;      
     
        if (cellArray[num - 1, 0] == "Swim" && cellArray[num - 1, 1] != null && cellArray[num - 1, 2] != null) {
            //No previous action bloock on cell
            if (num == size && size <=50)
            {
                size++; 
                GameObject.Find("Content").GetComponent<RectTransform>().sizeDelta += new Vector2(0, 125);
                GameObject.Find("Sheet").transform.position += new Vector3(0, 60, 0);
            }
            GameObject newRow = GameObject.Find("row (" + size + ")");
            newRow.transform.GetChild(0).gameObject.SetActive(true);
            newRow.transform.GetChild(1).gameObject.SetActive(true);
            newRow.transform.GetChild(2).gameObject.SetActive(true);

        }

        else if ( cellArray[num - 1, 0] == "Jump" && cellArray[num - 1, 1] != null)
        {
            if (num == size && size <= 50)
            {
                size++; 
                GameObject.Find("Content").GetComponent<RectTransform>().sizeDelta += new Vector2(0, 125);
                GameObject.Find("Sheet").transform.position += new Vector3(0, 60, 0);
            }
            GameObject newRow = GameObject.Find("row (" + size + ")");
            newRow.transform.GetChild(0).gameObject.SetActive(true);
            newRow.transform.GetChild(1).gameObject.SetActive(true);
            newRow.transform.GetChild(2).gameObject.SetActive(true);
        }

         else if (cellArray[num - 1, 0] == "PickUp" || cellArray[num - 1, 0] == "Drop")
            {
            if (num == size && size <= 50)
            {
                size++;
                GameObject.Find("Content").GetComponent<RectTransform>().sizeDelta += new Vector2(0, 125);
                GameObject.Find("Sheet").transform.position += new Vector3(0, 60,0);
            }
            GameObject newRow = GameObject.Find("row (" + size + ")");
            newRow.transform.GetChild(0).gameObject.SetActive(true);
            newRow.transform.GetChild(1).gameObject.SetActive(true);
            newRow.transform.GetChild(2).gameObject.SetActive(true);
             
            }  
       
    }

    public static void deleteCellArray( string pname, int type)
    {
        //Get the number of the row by trimming the name of row (x). 
        int num = (int)char.GetNumericValue(pname.Substring(pname.LastIndexOf('(') + 1).ToCharArray()[0]);

        //Double Digits
        if ((pname.LastIndexOf('(') + 1) != (pname.LastIndexOf(')') - 1))
        {
            int num2 = (int)char.GetNumericValue(pname.Substring(pname.LastIndexOf(')') - 1).ToCharArray()[0]);
            num = num * 10 + num2;
        }
        cellArray[num-1, type] = null;
    }

    //Turn on the Toggle
    public static void turnONToggle() {

        for (int i = 1; i <= size; i++)
        {
           
            GameObject newRow = GameObject.Find("row (" + i + ")");
            if (newRow != null)
            newRow.transform.GetChild(3).gameObject.SetActive(true);          
        }

        if (cellArray[size, 0] == null)
            GameObject.Find("row (" + (size) + ")").transform.GetChild(3).gameObject.SetActive(false);
    }

    //Turn Off the Toggle   
    public static void turnOFFToggle()
    {
        for (int i = 1; i <= size; i++)
        {

            GameObject newRow = GameObject.Find("row (" + i + ")");
            if (newRow != null)
                newRow.transform.GetChild(3).gameObject.SetActive(false);
        }       
    }

    public static void deleteSelected() {
        for (int i = 1; i <= size; i++)
        {
            GameObject newRow = GameObject.Find("row (" + i + ")");

            //The Toggle of the Row if OFF
            if (newRow != null && !newRow.transform.GetChild(3).GetComponent<Toggle>().isOn) {
                //Destroy the item on it
                newRow.transform.GetChild(0).gameObject.GetComponent<DragAndDropCell>().DestroyItem();
                newRow.transform.GetChild(1).gameObject.GetComponent<DragAndDropCell>().DestroyItem();
                newRow.transform.GetChild(2).gameObject.GetComponent<DragAndDropCell>().DestroyItem();
                //Disable Row Cells
                newRow.transform.GetChild(0).gameObject.SetActive(false);
                newRow.transform.GetChild(1).gameObject.SetActive(false);
                newRow.transform.GetChild(2).gameObject.SetActive(false);
                //Make Toggle True
                newRow.transform.GetChild(3).GetComponent<Toggle>().isOn = true;
                //Disable Toggle
                newRow.transform.GetChild(3).gameObject.SetActive(false);

                //Change to Null array in that index
                cellArray[i-1, 0] = cellArray[i-1, 1] = cellArray[i-1, 2] = null;

                //Disable Row for Life
                newRow.SetActive(false);
            }           
        }


    }

    public static void resetCell()
    {
        //Delete Item in First Row
        GameObject Sheet = GameObject.Find("Sheet");
        Sheet.transform.GetChild(0).gameObject.SetActive(true);
        GameObject newRow = GameObject.Find("row (1)");
        newRow.transform.GetChild(0).gameObject.GetComponent<DragAndDropCell>().DestroyItem();
        newRow.transform.GetChild(1).gameObject.GetComponent<DragAndDropCell>().DestroyItem();
        newRow.transform.GetChild(2).gameObject.GetComponent<DragAndDropCell>().DestroyItem();
        newRow.transform.GetChild(3).gameObject.SetActive(false); // Disable Toggle

        //Delete Item and Disable the Other rows
        for (int i = 2; i <= size; i++)
        {
            Sheet.transform.GetChild(i-1).gameObject.SetActive(true);
            newRow = GameObject.Find("row (" + i + ")");            
                newRow.transform.GetChild(0).gameObject.GetComponent<DragAndDropCell>().DestroyItem();
                newRow.transform.GetChild(1).gameObject.GetComponent<DragAndDropCell>().DestroyItem();
                newRow.transform.GetChild(2).gameObject.GetComponent<DragAndDropCell>().DestroyItem();
                newRow.transform.GetChild(0).gameObject.SetActive(false);
                newRow.transform.GetChild(1).gameObject.SetActive(false);
                newRow.transform.GetChild(2).gameObject.SetActive(false);           
        }
        //Make all elements in Array null
        cellArray = new string[50, 3];
        //Reset Contenet Block Size
        GameObject.Find("Content").GetComponent<RectTransform>().sizeDelta = new Vector2(0, 125);
        //Reset Sheet Position   
        GameObject.Find("Sheet").GetComponent<RectTransform>().transform.position = new Vector3(120, 849.9f, 0);
        //Reset Size to 1
        size = 1;
    }


}
