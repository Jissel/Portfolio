using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour {

    public bool isWhite, isKing;
   
   

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool IsForceToMove(Piece[,] board, int x, int y) {

        return true;
    }

    public bool ValidMove(Piece[,] board, int x2, int y2) {

        return ((board[x2, y2] != null) ? false : true);
      
    }
}
