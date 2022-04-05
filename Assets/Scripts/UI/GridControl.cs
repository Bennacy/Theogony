using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Theogony{
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

        public void NextRow(){
            if(gameObject.activeSelf){
                currRow ++;
                if(currRow >= totalRows){
                    currRow = 0;
                }
            }
        }

        public void PrevRow(){
            if(gameObject.activeSelf){
                currRow --;
                if(currRow < 0){
                    currRow = totalRows - 1;
                }
            }
        }

        public void NextCol(){
            if(gameObject.activeSelf){
                currCol ++;
                if(currCol >= totalCols){
                    currCol = 0;
                }
            }
        }

        public void PrevCol(){
            if(gameObject.activeSelf){
                currCol --;
                if(currCol < 0){
                    currCol = totalCols - 1;
                }
            }
        }
    }
}