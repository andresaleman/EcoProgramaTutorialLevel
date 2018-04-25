using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

/// <summary>
/// Every item's cell must contain this script
/// </summary>
[RequireComponent(typeof(Image))]
public class DragAndDropCell : MonoBehaviour, IDropHandler
{
    public enum CellType                                                    // Cell types
    {
        Swap,                                                               // Items will be swapped between any cells
        DropOnly,                                                           // Item will be dropped into cell
        DragOnly                                                            // Item will be dragged from this cell
    }

    public bool Action;
    public bool Direction;
    public bool Quantiy;
    public bool canDrop = true;
    //tutorial variables ---------------------- Tutorial -------------------------------------
    public bool Tutorial;
    public Text Dialogue;
    public static int Tutorialstep = 1;
    // ---------------------------------------- Tutorial -------------------------------------


    public enum TriggerType                                                 // Types of drag and drop events
    {
        DropRequest,                                                        // Request for item drop from one cell to another
        DropEventEnd,                                                       // Drop event completed
        ItemAdded,                                                          // Item manualy added into cell
        ItemWillBeDestroyed                                                 // Called just before item will be destroyed
    }

    public class DropEventDescriptor                                        // Info about item's drop event
    {
        public TriggerType triggerType;                                     // Type of drag and drop trigger
        public DragAndDropCell sourceCell;                                  // From this cell item was dragged
        public DragAndDropCell destinationCell;                             // Into this cell item was dropped
        public DragAndDropItem item;                                        // Dropped item
        public bool permission = true;                                             // Decision need to be made on request
    }

	[Tooltip("Functional type of this cell")]
    public CellType cellType = CellType.Swap;                               // Special type of this cell
	[Tooltip("Sprite color for empty cell")]
    public Color empty = new Color();                                       // Sprite color for empty cell
	[Tooltip("Sprite color for filled cell")]
    public Color full = new Color();                                        // Sprite color for filled cell
	[Tooltip("This cell has unlimited amount of items")]
    public bool unlimitedSource = false;                                    // Item from this cell will be cloned on drag start

	private DragAndDropItem myDadItem;										// Item of this DaD cell

    void OnEnable()
    {
        DragAndDropItem.OnItemDragStartEvent += OnAnyItemDragStart;         // Handle any item drag start
        DragAndDropItem.OnItemDragEndEvent += OnAnyItemDragEnd;             // Handle any item drag end
		UpdateMyItem();
		UpdateBackgroundState();
    }

    void OnDisable()
    {
        DragAndDropItem.OnItemDragStartEvent -= OnAnyItemDragStart;
        DragAndDropItem.OnItemDragEndEvent -= OnAnyItemDragEnd;
        StopAllCoroutines();                                                // Stop all coroutines if there is any
    }
      void Start()
    {


    }

    /// <summary>
    /// On any item drag start need to disable all items raycast for correct drop operation
    /// </summary>
    /// <param name="item"> dragged item </param>
    private void OnAnyItemDragStart(DragAndDropItem item)
    {
		UpdateMyItem();
		if (myDadItem != null)
        {
			myDadItem.MakeRaycast(false);                                  	// Disable item's raycast for correct drop handling
			if (myDadItem == item)                                         	// If item dragged from this cell
            {
                // Check cell's type
                switch (cellType)
                {
                    case CellType.DropOnly:
                        DragAndDropItem.icon.SetActive(false);              // Item can not be dragged. Hide icon
                        break;
                }
            }
        }
    }

    /// <summary>
    /// On any item drag end enable all items raycast
    /// </summary>
    /// <param name="item"> dragged item </param>
    private void OnAnyItemDragEnd(DragAndDropItem item)
    {
		UpdateMyItem();
		if (myDadItem != null)
        {
			myDadItem.MakeRaycast(true);                                  	// Enable item's raycast
        }
		UpdateBackgroundState();
    }

    /// <summary>
    /// Item is dropped in this cell
    /// </summary>
    /// <param name="data"></param>
    public void OnDrop(PointerEventData data)
    {
        if (DragAndDropItem.icon != null)
        {
            DragAndDropItem item = DragAndDropItem.draggedItem;
            DragAndDropCell sourceCell = DragAndDropItem.sourceCell;
            if (DragAndDropItem.icon.activeSelf == true)                    // If icon inactive do not need to drop item into cell
            {
                if ((item != null) && (sourceCell != this))
                {
                    DropEventDescriptor desc = new DropEventDescriptor();
                    switch (cellType)                                       // Check this cell's type
                    {
                        case CellType.Swap:                                 // Item in destination cell can be swapped
							UpdateMyItem();
                            switch (sourceCell.cellType)
                            {
                                case CellType.Swap:                         // Item in source cell can be swapped
                                    // Fill event descriptor
                                    desc.item = item;
                                    desc.sourceCell = sourceCell;
                                    desc.destinationCell = this;
                                    SendRequest(desc);                      // Send drop request
                                    StartCoroutine(NotifyOnDragEnd(desc));  // Send notification after drop will be finished
                                    if (desc.permission == true)            // If drop permitted by application
                                    {
										if (myDadItem != null)            // If destination cell has item
                                        {
                                            // Fill event descriptor
                                            DropEventDescriptor descAutoswap = new DropEventDescriptor();
											descAutoswap.item = myDadItem;
                                            descAutoswap.sourceCell = this;
                                            descAutoswap.destinationCell = sourceCell;
                                            SendRequest(descAutoswap);                      // Send drop request
                                            StartCoroutine(NotifyOnDragEnd(descAutoswap));  // Send notification after drop will be finished
                                            if (descAutoswap.permission == true)            // If drop permitted by application
                                            {
                                                SwapItems(sourceCell, this);                // Swap items between cells
                                            }
                                            else
                                            {
												PlaceItem(item);            // Delete old item and place dropped item into this cell
                                            }
                                        }
                                        else
                                        {
											PlaceItem(item);                // Place dropped item into this empty cell
                                        }
                                    }
                                    break;
                                default:                                    // Item in source cell can not be swapped
                                    // Fill event descriptor
                                    desc.item = item;
                                    desc.sourceCell = sourceCell;
                                    desc.destinationCell = this;
                                    SendRequest(desc);                      // Send drop request
                                    StartCoroutine(NotifyOnDragEnd(desc));  // Send notification after drop will be finished
                                    if (desc.permission == true)            // If drop permitted by application
                                    {
										PlaceItem(item);                    // Place dropped item into this cell
                                    }
                                    break;
                            }
                            break;
                        case CellType.DropOnly:                             // Item only can be dropped into destination cell
                            // Fill event descriptor
                            desc.item = item;
                            desc.sourceCell = sourceCell;
                            desc.destinationCell = this;
                            SendRequest(desc);                              // Send drop request
                            StartCoroutine(NotifyOnDragEnd(desc));          // Send notification after drop will be finished

                            //Object Parent 
                            GameObject parent = this.transform.parent.gameObject;

                            if (!Tutorial)
                            {


                                if (this.Action && item.Action && desc.permission == true)
                                {
                                    PlaceItem(item);
                                    AlgothimDevelopment.addCellArray(item.name, parent.name, 0);
                                    switch (item.name)
                                    {
                                        case ("Jump"):

                                            DisableDropOtherblocks(2);
                                            break;
                                        case ("PickUp"):
                                        case ("Drop"):
                                            DisableDropOtherblocks(1);
                                            break;
                                        default:
                                            DisableDropOtherblocks(0);
                                            break;
                                    }
                                }
                                else if (this.Quantiy && item.Quantiy && desc.permission == true && this.canDrop == true)
                                {
                                    AlgothimDevelopment.addCellArray(item.name, parent.name, 2);
                                    PlaceItem(item);
                                }
                                else if (this.Direction && item.Direction && desc.permission == true && this.canDrop == true)
                                {
                                    AlgothimDevelopment.addCellArray(item.name, parent.name, 1);
                                    PlaceItem(item);
                                }
                            }
                            else //Tutorial Enable ------------------------------------------------------------------------------------
                            {
                                //Swim
                                if (this.Action && item.Action && desc.permission == true && item.name.Equals("Swim") && Tutorialstep == 1)
                                {
                                    PlaceItem(item);
                                    AlgothimDevelopment.addCellArray(item.name, parent.name, 0);
                                    Dialogue.text = "Dirígeme hacia dónde debo nadar.";
                                    Tutorialstep++;
                                }
                                //Right
                                else if (this.Direction && item.Direction && desc.permission == true && this.canDrop == true && item.name.Equals("Right") && Tutorialstep == 2)
                                {
                                    AlgothimDevelopment.addCellArray(item.name, parent.name, 1);
                                    PlaceItem(item);
                                    Dialogue.text = "Indícame cuantos espacios debo nadar.";
                                    Tutorialstep++;
                                }
                                //4
                                else if (this.Quantiy && item.Quantiy && desc.permission == true && this.canDrop == true && item.name.Equals("4") && Tutorialstep == 3)
                                {
                                    AlgothimDevelopment.addCellArray(item.name, parent.name, 2);
                                    PlaceItem(item);
                                    Dialogue.text = "Ayúdame a saber qué hacer con el aluminio.";
                                    Tutorialstep++;
                                }
                                //Pick Up
                                else if (this.Action && item.Action && desc.permission == true && item.name.Equals("PickUp") && Tutorialstep == 4 && item.name.Equals("PickUp"))
                                {
                                    AlgothimDevelopment.addCellArray(item.name, parent.name, 0);
                                    PlaceItem(item);
                                    Dialogue.text = "Ayúdame a llevar la botella sobre el contenedor de reciclaje de aluminio.";
                                    Tutorialstep++;
                                    DisableDropOtherblocks(1);
                                }
                                //Swim
                                else if (this.Action && item.Action && desc.permission == true && item.name.Equals("Swim") && Tutorialstep == 5)
                                {
                                    AlgothimDevelopment.addCellArray(item.name, parent.name, 0);
                                    PlaceItem(item);
                                    Dialogue.text = "Ayúdame a llevar la botella sobre el contenedor de reciclaje de aluminio.";
                                    Tutorialstep++;
                                }
                                //Down
                                else if (this.Direction && item.Direction && desc.permission == true && this.canDrop == true && item.name.Equals("Down") && Tutorialstep == 6)
                                {
                                    AlgothimDevelopment.addCellArray(item.name, parent.name, 1);
                                    PlaceItem(item);
                                    Dialogue.text = "Ayúdame a llevar la botella sobre el contenedor de reciclaje de aluminio.";
                                    Tutorialstep++;
                                }
                                //4
                                else if (this.Quantiy && item.Quantiy && desc.permission == true && this.canDrop == true && item.name.Equals("4") && Tutorialstep == 7)
                                {
                                    AlgothimDevelopment.addCellArray(item.name, parent.name, 2);
                                    PlaceItem(item);
                                    Dialogue.text = "Ayúdame a saber qué hacer cuando este sobre el contenedor.";
                                    Tutorialstep++;
                                }
                                //Drop
                                else if (this.Action && item.Action && desc.permission == true && item.name.Equals("Drop") && Tutorialstep == 8 && item.name.Equals("Drop"))
                                {
                                    AlgothimDevelopment.addCellArray(item.name, parent.name, 0);
                                    PlaceItem(item);
                                    Dialogue.text = "Presiona Play para correr el algoritmo de recoger y desechar la lata de aluminio.";
                                    Tutorialstep++;
                                    DisableDropOtherblocks(1);
                                }
                                //Wait for play button 
                                else if (Tutorialstep==9){ // wait for play button 
                                    Dialogue.text = "Dirígeme hacia dónde debo nadar para alcanzar el cristal.";
                                }
                                //Secon Part -----------------------
                                //swim
                                else if (this.Action && item.Action && desc.permission == true && item.name.Equals("Swim") && Tutorialstep == 10)
                                {
                                    AlgothimDevelopment.addCellArray(item.name, parent.name, 0);
                                    PlaceItem(item);
                                    Dialogue.text = "Dirígeme hacia dónde debo nadar para alcanzar el cristal.";
                                    Tutorialstep++;
                                }
                                //left
                                else if (this.Direction && item.Direction && desc.permission == true && this.canDrop == true && item.name.Equals("Left") && Tutorialstep == 11)
                                {
                                    AlgothimDevelopment.addCellArray(item.name, parent.name, 1);
                                    PlaceItem(item);
                                    Dialogue.text = "Dirígeme hacia dónde debo nadar para alcanzar el cristal.";
                                    Tutorialstep++;
                                }
                                //1
                                else if (this.Quantiy && item.Quantiy && desc.permission == true && this.canDrop == true && item.name.Equals("1") && Tutorialstep == 12)
                                {
                                    AlgothimDevelopment.addCellArray(item.name, parent.name, 2);
                                    PlaceItem(item);
                                    Dialogue.text = "Ayúdame a recoger la botella de cristal.";
                                    Tutorialstep++;
                                }
                                //pick up
                                else if (this.Action && item.Action && desc.permission == true && Tutorialstep == 13 && item.name.Equals("PickUp"))
                                {
                                    AlgothimDevelopment.addCellArray(item.name, parent.name, 0);
                                    PlaceItem(item);
                                    Dialogue.text = "Dirígeme hacia dónde debo nadar para alcanzar el cristal.";
                                    Tutorialstep++;
                                    DisableDropOtherblocks(1);
                                }
                                //Swim
                                else if (this.Action && item.Action && desc.permission == true && item.name.Equals("Swim") && Tutorialstep == 14) 
                                {
                                    AlgothimDevelopment.addCellArray(item.name, parent.name, 0);
                                    PlaceItem(item);
                                    Dialogue.text = "Dirígeme hacia dónde debo nadar para alcanzar el cristal.";
                                    Tutorialstep++;
                                }
                                //Up
                                else if (this.Direction && item.Direction && desc.permission == true && this.canDrop == true && item.name.Equals("Up") && Tutorialstep == 15)
                                {
                                    AlgothimDevelopment.addCellArray(item.name, parent.name, 1);
                                    PlaceItem(item);
                                    Dialogue.text = "Dirígeme hacia dónde debo nadar para alcanzar el cristal.";
                                    Tutorialstep++;
                                }
                                //2
                                else if (this.Quantiy && item.Quantiy && desc.permission == true && this.canDrop == true && item.name.Equals("2") && Tutorialstep == 16)
                                {
                                    AlgothimDevelopment.addCellArray(item.name, parent.name, 2);
                                    PlaceItem(item);
                                    Dialogue.text = "Dirígeme hacia dónde debo nadar para alcanzar el cristal.";
                                    Tutorialstep++;
                                }
                                //Swim
                                else if (this.Action && item.Action && desc.permission == true && item.name.Equals("Swim") && Tutorialstep == 17)
                                {
                                    AlgothimDevelopment.addCellArray(item.name, parent.name, 0);
                                    PlaceItem(item);
                                    Dialogue.text = "Dirígeme hacia dónde debo nadar para alcanzar el cristal.";
                                    Tutorialstep++;
                                }
                                //Right
                                else if (this.Direction && item.Direction && desc.permission == true && this.canDrop == true && item.name.Equals("Right") && Tutorialstep == 18)
                                {
                                    AlgothimDevelopment.addCellArray(item.name, parent.name, 1);
                                    PlaceItem(item);
                                    Dialogue.text = "Dirígeme hacia dónde debo nadar para alcanzar el cristal.";
                                    Tutorialstep++;
                                }
                                //2
                                else if (this.Quantiy && item.Quantiy && desc.permission == true && this.canDrop == true && item.name.Equals("2") && Tutorialstep == 19)
                                {
                                    AlgothimDevelopment.addCellArray(item.name, parent.name, 2);
                                    PlaceItem(item);
                                    Dialogue.text = "Ayudame a recoger la botella de cristal.";
                                    Tutorialstep++;
                                }
                                //PickUp
                                else if (this.Action && item.Action && desc.permission == true && Tutorialstep == 20 && item.name.Equals("PickUp"))
                                {
                                    AlgothimDevelopment.addCellArray(item.name, parent.name, 0);
                                    PlaceItem(item);
                                    Dialogue.text = "Ayúdame a llevar la botella sobre el contenedor de reciclaje de cristal.";
                                    Tutorialstep++;
                                    DisableDropOtherblocks(1);
                                }
                                //Jump
                                else if (this.Action && item.Action && desc.permission == true && item.name.Equals("Jump") && Tutorialstep == 21)
                                {
                                    AlgothimDevelopment.addCellArray(item.name, parent.name, 0);
                                    PlaceItem(item);
                                    Dialogue.text = "Ayúdame a llevar la botella sobre el contenedor de reciclaje de cristal.";
                                    Tutorialstep++;
                                    DisableDropOtherblocks(2);
                                }
                                //Down
                                else if (this.Direction && item.Direction && desc.permission == true && this.canDrop == true && item.name.Equals("Down") && Tutorialstep == 22)
                                {
                                    AlgothimDevelopment.addCellArray(item.name, parent.name, 1);
                                    PlaceItem(item);
                                    Dialogue.text = "Ayúdame a saber que hago con la botella de cristal en el contendor.";
                                    Tutorialstep++;
                                }
                                //Drop
                                else if (this.Action && item.Action && desc.permission == true && Tutorialstep == 23 && item.name.Equals("Drop"))
                                {
                                    AlgothimDevelopment.addCellArray(item.name, parent.name, 0);
                                    PlaceItem(item);
                                    Dialogue.text = "Presiona el botón de play para correr el algoritmo.";
                                    Tutorialstep++;
                                    DisableDropOtherblocks(1);
                                }
                            } //ends tutorial ---------------------------------------------------------------------------------------------
                            break;
                        default:
                            break;
                    }
                }
            }
            if (item != null)
            {
                if (item.GetComponentInParent<DragAndDropCell>() == null)   // If item have no cell after drop
                {
                    Destroy(item.gameObject);                               // Destroy it
                }
            }
			UpdateMyItem();
			UpdateBackgroundState();
			sourceCell.UpdateMyItem();
			sourceCell.UpdateBackgroundState();
        }
    }

	/// <summary>
	/// Put item into this cell.
	/// </summary>
	/// <param name="item">Item.</param>
	private void PlaceItem(DragAndDropItem item)
	{
		if (item != null)
		{
			DestroyItem();                                            	// Remove current item from this cell
			myDadItem = null;
			DragAndDropCell cell = item.GetComponentInParent<DragAndDropCell>();
			if (cell != null)
			{
				if (cell.unlimitedSource == true)
				{
					string itemName = item.name;
					item = Instantiate(item);                               // Clone item from source cell
					item.name = itemName;
				}
			}
			item.transform.SetParent(transform, false);
			item.transform.localPosition = Vector3.zero;
			item.MakeRaycast(true);
			myDadItem = item;
		}
		UpdateBackgroundState();
	}

    /// <summary>
    /// Destroy item in this cell
    /// </summary>
    public void DestroyItem()
    {
		UpdateMyItem();
		if (myDadItem != null)
        {
            DropEventDescriptor desc = new DropEventDescriptor();
            // Fill event descriptor
            desc.triggerType = TriggerType.ItemWillBeDestroyed;
			desc.item = myDadItem;
            desc.sourceCell = this;
            desc.destinationCell = this;
            SendNotification(desc);                                         // Notify application about item destruction
			if (myDadItem != null)
			{
				Destroy(myDadItem.gameObject);
			}
        }
		myDadItem = null;
		UpdateBackgroundState();
        EnableDropOtherblocks();

    }

    /// <summary>
    /// Send drag and drop information to application
    /// </summary>
    /// <param name="desc"> drag and drop event descriptor </param>
    private void SendNotification(DropEventDescriptor desc)
    {
        if (desc != null)
        {
            // Send message with DragAndDrop info to parents GameObjects
            gameObject.SendMessageUpwards("OnSimpleDragAndDropEvent", desc, SendMessageOptions.DontRequireReceiver);
        }
    }

    /// <summary>
    /// Send drag and drop request to application
    /// </summary>
    /// <param name="desc"> drag and drop event descriptor </param>
    /// <returns> result from desc.permission </returns>
    private bool SendRequest(DropEventDescriptor desc)
    {
        bool result = false;
        if (desc != null)
        {
            desc.triggerType = TriggerType.DropRequest;
           // desc.permission = true;
            SendNotification(desc);
            result = desc.permission;
        }
        return result;
    }

    /// <summary>
    /// Wait for event end and send notification to application
    /// </summary>
    /// <param name="desc"> drag and drop event descriptor </param>
    /// <returns></returns>
    private IEnumerator NotifyOnDragEnd(DropEventDescriptor desc)
    {
        // Wait end of drag operation
        while (DragAndDropItem.draggedItem != null)
        {
            yield return new WaitForEndOfFrame();
        }
        desc.triggerType = TriggerType.DropEventEnd;
        SendNotification(desc);
    }

	/// <summary>
	/// Change cell's sprite color on item put/remove.
	/// </summary>
	/// <param name="condition"> true - filled, false - empty </param>
	public void UpdateBackgroundState()
	{
		Image bg = GetComponent<Image>();
		if (bg != null)
		{
			bg.color = myDadItem != null ? full : empty;
		}
	}

	/// <summary>
	/// Updates my item
	/// </summary>
	public void UpdateMyItem()
	{
		myDadItem = GetComponentInChildren<DragAndDropItem>();
	}

	/// <summary>
	/// Get item from this cell
	/// </summary>
	/// <returns> Item </returns>
	public DragAndDropItem GetItem()
	{
		return myDadItem;
	}

    /// <summary>
    /// Manualy add item into this cell
    /// </summary>
    /// <param name="newItem"> New item </param>
    public void AddItem(DragAndDropItem newItem)
    {
        if (newItem != null)
        {
			PlaceItem(newItem);
            DropEventDescriptor desc = new DropEventDescriptor();
            // Fill event descriptor
            desc.triggerType = TriggerType.ItemAdded;
            desc.item = newItem;
            desc.sourceCell = this;
            desc.destinationCell = this;
            SendNotification(desc);
        }
    }

    /// <summary>
    /// Manualy delete item from this cell
    /// </summary>
    public void RemoveItem()
    {
        DestroyItem();
    }

	/// <summary>
	/// Swap items between two cells
	/// </summary>
	/// <param name="firstCell"> Cell </param>
	/// <param name="secondCell"> Cell </param>
	public void SwapItems(DragAndDropCell firstCell, DragAndDropCell secondCell)
	{
		if ((firstCell != null) && (secondCell != null))
		{
			DragAndDropItem firstItem = firstCell.GetItem();                // Get item from first cell
			DragAndDropItem secondItem = secondCell.GetItem();              // Get item from second cell
			// Swap items
			if (firstItem != null)
			{
				firstItem.transform.SetParent(secondCell.transform, false);
				firstItem.transform.localPosition = Vector3.zero;
				firstItem.MakeRaycast(true);
			}
			if (secondItem != null)
			{
				secondItem.transform.SetParent(firstCell.transform, false);
				secondItem.transform.localPosition = Vector3.zero;
				secondItem.MakeRaycast(true);
			}
			// Update states
			firstCell.UpdateMyItem();
			secondCell.UpdateMyItem();
			firstCell.UpdateBackgroundState();
			secondCell.UpdateBackgroundState();
		}
	}
    // Depeding of the type of action blocks, the player will be able to place a direction and quantity block.
    //it will also destroy any previously deposity item.
    void DisableDropOtherblocks(int x) {
        GameObject parent = this.transform.parent.gameObject;
        parent.transform.Find("Direction Cell").GetComponent<DragAndDropCell>().canDrop = true;
        parent.transform.Find("Quantity Cell").GetComponent<DragAndDropCell>().canDrop = true;
 
        if (x==1) // Pickup or Drop Block
        { //Dirrection
            parent.transform.Find("Direction Cell").GetComponent<DragAndDropCell>().canDrop = false;
            parent.transform.Find("Direction Cell").GetComponent<DragAndDropCell>().DestroyItem();
            AlgothimDevelopment.deleteCellArray(parent.name, 1);
        //Quantity
            parent.transform.Find("Quantity Cell").GetComponent<DragAndDropCell>().canDrop = false;
            parent.transform.Find("Quantity Cell").GetComponent<DragAndDropCell>().DestroyItem();
            AlgothimDevelopment.deleteCellArray(parent.name, 2);         
        }
        else if (x==2) { // Jump Block
            parent.transform.Find("Quantity Cell").GetComponent<DragAndDropCell>().canDrop = false;
            parent.transform.Find("Quantity Cell").GetComponent<DragAndDropCell>().DestroyItem();
            AlgothimDevelopment.deleteCellArray(parent.name, 2);
        }
    }

    void EnableDropOtherblocks()
    {
        GameObject parent = this.transform.parent.gameObject;
        parent.transform.Find("Direction Cell").GetComponent<DragAndDropCell>().canDrop = true;
        parent.transform.Find("Quantity Cell").GetComponent<DragAndDropCell>().canDrop = true;
       
    }

}
