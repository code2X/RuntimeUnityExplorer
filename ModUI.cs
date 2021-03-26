using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ExplorerSpace
{
    class ScrollView
    {
        public float x;
        public float y;
        public float width;
        public float height;
        public float mainButtomHeight = 25;
        public float classInfoHeight = 20;
        public float classTitleHeight = 20;

        float curHeight = 0;
        float totoalHeight = 1000;

        Vector2 scrollPosition;

        public ScrollView(float x = 10, float y = 50, float width = 200, float height = 700)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public void Begin()
        {
            curHeight = 0;
            scrollPosition = GUI.BeginScrollView(new Rect(x, y, width, height), scrollPosition, new Rect(0, 0, 1000, totoalHeight));
        }

        public void End()
        {
            totoalHeight = curHeight;
            GUI.EndScrollView();
        }

        public bool MainTitle(bool value, string txt)
        {
            curHeight += 3;
            bool res = GUI.Toggle(new Rect(0, curHeight, width - 20, mainButtomHeight), value, txt);
            curHeight += mainButtomHeight;
            return res;
        }

        public bool Button(string txt)
        {
            curHeight += 2;
            bool res = GUI.Button(new Rect(10, curHeight, width - 10, mainButtomHeight), txt);
            curHeight += 20;
            return res;
        }

        public void Lable(string txt)
        {
            curHeight += 2;
            GUI.Label(new Rect(10, curHeight, width - 10, mainButtomHeight), txt);
            curHeight += 20;
        }

        public bool ClassTitle(bool value, string txt)
        {
            curHeight += 2;
            if (curHeight > scrollPosition.y - 700 && curHeight < scrollPosition.y + 700)
            {
                bool res = GUI.Toggle(new Rect(15, curHeight, width - 15, classTitleHeight), value, txt);
                curHeight += classTitleHeight + 3;
                return res;
            }
            else
            {
                curHeight += classTitleHeight + 3;
                return value;
            }
        }

        public bool ClassInfo(string txt)
        {
            curHeight += 2;
            bool res = GUI.Button(new Rect(25, curHeight, width - 45, classInfoHeight), txt);
            curHeight += classInfoHeight;
            return res;
        }
    }
}
