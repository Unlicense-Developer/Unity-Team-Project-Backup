using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GrillingMeatGame
{
    public class MouseCursor : MonoBehaviour
    {
        [SerializeField]
        Texture2D cursorIcon;//커서이미지
        void CursorChange()
        {
            Cursor.lockState = CursorLockMode.Confined; //커서 화면밖으로 안나가게 하기 

            Cursor.SetCursor(cursorIcon, new Vector2(cursorIcon.width / 6, 0), CursorMode.ForceSoftware);
            //Cursor.SetCursor(cursorIcon, new Vector2(cursorIcon.width / 5, 0), (cursorIcon.height / 0, -3), CursorMode.ForceSoftware);
        }

        void Update()
        {
            CursorChange();

        }
    }

}