using UnityEngine;
using System.Collections.Generic;

namespace Grids
{
    public enum Status
    {
        Empty, 
        Void,   
        Full, 
        Eternal
    }

    public class Cell: MonoBehaviour
    {
        [SerializeField]
        private GameObject renderObject;

        public Status Status { get; set; }
        public GameObject Content { get; private set; }
        public Vector2 Position { get; set; }

        public Cell(Vector2 position, Status status = Status.Empty, GameObject content = null)
        {
            Position = position;
            Status = status;
            Content = content;

            if(status == Status.Void)
            {
                renderObject.SetActive(false);
            }
        }

        public void SetContent(GameObject newContent)
        {
            if (Status == Status.Void)
            {
                Destroy(newContent);
            }
            Content = newContent;
            Status = (newContent != null) ? Status.Full : Status.Empty;
        }

        public bool MakeVoid()
        {
            if (Status != Status.Eternal)
            {
                if (Status == Status.Full)
                {
                    Destroy(Content.gameObject);
                }
                Status = Status.Void;
                renderObject.SetActive(true);
                return true;
            }
            return false;
        }

        public void Clear()
        {
            if (Content != null)
                Object.Destroy(Content);
            if (Status != Status.Eternal)
            {
                Status = Status.Empty;
            }

        }
    }
}