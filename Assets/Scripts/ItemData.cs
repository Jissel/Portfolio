using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class ItemData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler {

    public Item item;
    public int amount;
    public int slot;
    
    private Inventory inv;
    private Tooltip tooltip;
    private Vector2 offset;

    public GameObject whitePiece;
    public GameObject blackPiece;
    
    private Piece piece;

    public void Start() {
        inv = GameObject.Find("Inventory").GetComponent<Inventory>();
        tooltip = inv.GetComponent<Tooltip>();
    }

    //se selecciona
    public void OnBeginDrag(PointerEventData eventData) {
        if (item != null) {
            offset = eventData.position - new Vector2(this.transform.position.x, this.transform.position.y);
            this.transform.SetParent(this.transform.parent.parent);
            this.transform.position = eventData.position - offset;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    //Arrastrando
    public void OnDrag(PointerEventData eventData) {
        if (item != null) {
            this.transform.position = eventData.position - offset;
        }
    }

    // se suelta el item
    public void GeneratePieceA() {
        
        //Creacion de objeto en el tablero dependiendo del item seleccionado
        if (item.Title == "Steel Globes") {
            //whitePiece = Resources.Load<GameObject>("Sprites/Objects/White");
            GameObject go = Instantiate(whitePiece, new Vector3(2, 0, 3), Quaternion.identity);
            go.transform.SetParent(transform);
        }
        else if (item.Title == "The Great Stick") {
            blackPiece = Resources.Load<GameObject>("Sprites/Objects/Black");
            GameObject go = Instantiate(blackPiece, new Vector3(1, 0, 5), Quaternion.identity);
            go.transform.SetParent(transform);
        }
        
    }

    public void OnEndDrag(PointerEventData eventData) {
        this.transform.SetParent(inv.slots[slot].transform);
        this.transform.position = inv.slots[slot].transform.position;
        GeneratePieceA();
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        tooltip.Activate(item);
    }

    public void OnPointerExit(PointerEventData eventData) {
        tooltip.Deactivate();
    }

    
}
