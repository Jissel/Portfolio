using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckersBoard : MonoBehaviour {

    public Piece[,] pieces = new Piece[8,8];
    private Piece selectedPiece;

    public Casilla[,] casillas = new Casilla[8,8];
    private Casilla selectedCell;

    private List<Piece> forcedPieces;

    public GameObject whitePiecePrefab;
    public GameObject blackPiecePrefab;

    public GameObject ColorCell;
    public Renderer rend; 
   
    public GameObject CasillaPrefab;
    public int ancho=8, alto=8;

    private Vector3 boardOffset = new Vector3(-4.0f, 0, -4f);

    private Vector3 pieceOffset = new Vector3(0.5f, 0, 0.5f);

    private Vector2 mouseOver;
    private Vector2 startDrag;
    private Vector2 endDrag;
    

    // Use this for initialization
    private void Start () {
        ColorCell = GameObject.Find("ColorCell");
        GenerateBoard();        
    }
	
	// Update is called once per frame
	void Update () {
        UpdateMouseOver();
        
        int x = (int)mouseOver.x;
        int y = (int)mouseOver.y;

        if (selectedPiece != null) {
            
            UpdatePieceDrag(selectedPiece);
        }          

        if (Input.GetMouseButtonDown(0)) {
            SelectPiece(x, y);
        }                 

        if (Input.GetMouseButtonUp(0))
            TryMove((int)startDrag.x, (int)startDrag.y, x, y);        
    }

    public void UpdateMouseOver() {
        //por turno,"es mi turno"
        if (!Camera.main) {
            Debug.Log("No se consigue la camara principal");
            return;
        }

        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f, LayerMask.GetMask("Board"))){
            mouseOver.x = (int)(hit.point.x - boardOffset.x);
            mouseOver.y = (int)(hit.point.z - boardOffset.z);
        } else {
            mouseOver.x = -1;
            mouseOver.y = -1;
        }
    }

    public void UpdatePieceDrag(Piece p) {
        if (!Camera.main) {
            Debug.Log("Unable to find main camera");
            return;
        }

        RaycastHit hit;

        //Si el mapa se debe agrandar, el 25.5 debe ser mayor, para que el Raycast aumente su alcance
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f, LayerMask.GetMask("Board"))) {

            p.transform.position = hit.point + Vector3.up/2;
            //El hit.point permite modificar el punto donde se movera la pieza
            //Eliminarlo ocasiona que la pieza se coloque en el centro (lugar de inicio del prefabs original)
            //Vector.up eleva la pieza en 1 en Y, es decir Vector3.up==Vector(0,1,0)
        }
    }

    public void SelectPiece(int x, int y) {

        if (x < 0 || x >= 8 || y < 0 || y >= 8)
            return;

        Piece p = pieces[x, y];
        if (p != null) {
            selectedPiece = p;
            startDrag = mouseOver;
        }

    }

    public void TryMove(int x1, int y1, int x2, int y2) {

        startDrag = new Vector2(x1, y1);
        endDrag = new Vector2(x2, y2);
        
        selectedPiece = pieces[x1, y1];

        
        // Out of bounds

        if (x2 < 0 || x2 >= 8 || y2 < 0 || y2 >= 8) {

            if (selectedPiece != null)
                MovePiece(selectedPiece, x1, y1);

            startDrag = Vector2.zero;
            selectedPiece = null;

            return;
        }

        if (selectedPiece != null) {
            // If it has not moved   
            

            if (endDrag == startDrag) {
                MovePiece(selectedPiece, x1, y1);
                startDrag = Vector2.zero;
                selectedPiece = null;
                return;
            }           

            // Check if its a valid move
            if (selectedPiece.ValidMove(pieces, x2, y2)) {
                MovePiece(selectedPiece, x2, y2);

                pieces[x2, y2] = selectedPiece;

                pieces[x1, y1] = null;
                Occupied(x2, y2);
                Occupied(x1, y1);

                selectedPiece = null;
            } else {
                MovePiece(selectedPiece, x1, y1);
                startDrag = Vector2.zero;
                selectedPiece = null;
            }

            //ColorCell.SetActive(false);
            return;
        }
    }
    
    //con ayuda de GeneratePiece, genera las piezas en todo el tablero
    public void GenerateBoard() {
        int cont = 0;
        //Generar las casillas vacias
        for (int i = 0; i < ancho; i++) {
            for (int j = 0; j < alto; j++) {
                GenerateCell(i, j, cont);
                cont++;
            }
        }

        //GeneratePiece(1, 3);

    }

    //Llena la matriz de las celdas en color verde (original)
    public void GenerateCell(int x, int y, int cont) {
        GameObject pr = Instantiate(CasillaPrefab, new Vector3(x, 0, y), Quaternion.identity);
        Casilla c = pr.GetComponent<Casilla>();

        c.GetComponent<Casilla>().numC = cont;

        casillas[x, y] = c;
        c.transform.position = (Vector3.right * x) + (Vector3.forward * y) + boardOffset + pieceOffset;

        //Desactivas el renderer de cada casilla
        //c.GetComponent<Renderer>().enabled = false;

    }


    //Los valores de Y indica si la peiza es negra o blanca, aquellas que sean Y>3 son negras
    public void GeneratePiece(int x, int y) {

        //pregunta si la pieza en negra con respeto al valor de la variable Y 
        bool isnegga = (y > 3) ? true : false;

        GameObject go = Instantiate((isnegga) ? blackPiecePrefab : whitePiecePrefab) as GameObject;
        go.transform.SetParent(transform);
        Piece p = go.GetComponent<Piece>();
        pieces[x, y] = p;
        MovePiece(p,x,y);// Permite mover la pieza dentro del tablero, en este caso se usa para colocar las piezas dispersas en el tablero
    }

    public void MovePiece(Piece p, int x, int y) {
        //Debug.Log("x: " + x);
        //Debug.Log("y: " + y);
        p.transform.position = (Vector3.right * x) + (Vector3.forward * y) + boardOffset + pieceOffset;       
    }

    public void Occupied(int x, int y) {

        selectedCell = casillas[x, y];
        selectedCell.ChangeColor(pieces[x, y] == null, casillas, x, y);
         //False ocupado, true desocupado
    }

}
