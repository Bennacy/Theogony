using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GridControl : MonoBehaviour
{
    [Serializable]
    public class GridRow{
        public Image[] img;
    }
    public GridRow[] grid;
    public PauseScreen pauseScreen;
    public Color unselectedC;
    public Color selectedC;
    public int totalCols;
    public int totalRows;
    public int currCol;
    public int currRow;

    void Start()
    {
        unselectedC = new Color(1, 1, 1, .75f);
        selectedC = new Color(.75f, .75f, .75f, .75f);
        grid = new GridRow[totalCols];
        for(int i = 0; i < totalCols; i++){
            grid[i] = new GridRow();
            grid[i].img = new Image[totalRows];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextRow(InputAction.CallbackContext context){
        if(context.performed){
            currRow ++;
            if(currRow >= totalRows){
                currRow = 0;
                NextCol(context);
            }
        }
    }

    public void PrevRow(InputAction.CallbackContext context){
        if(context.performed){
            currRow --;
            if(currRow < 0){
                currRow = totalRows - 1;
                PrevCol(context);
            }
        }
    }

    public void NextCol(InputAction.CallbackContext context){
        if(context.performed){
            currCol ++;
            if(currCol >= totalCols){
                currCol = 0;
                NextRow(context);
            }
        }
    }

    public void PrevCol(InputAction.CallbackContext context){
        if(context.performed){
            currCol --;
            if(currCol < 0){
                currCol = totalCols - 1;
                PrevRow(context);
            }
        }
    }
}
