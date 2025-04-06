using UnityEngine;
using System.Collections.Generic;

namespace Grids
{
    public enum Status
    {
        Empty, 
        Void,   
        Full    
    }

    //public Dictionary<Status, Mesh> StatusMesh = new Dictionary<Status, Mesh>
    //{
    //    { Status.Empty, Refs.EmptyMesh },
    //    { Status.Void, Refs.VoidMesh },
    //    { Status.Full, Refs.EmptyMesh }
    //};

    public class Cell: MonoBehaviour
    {
        public Status Status { get; private set; }
        public GameObject Content { get; private set; }
        public Vector2 Position { get; private set; }

        public Cell(Vector2 position, Status status = Status.Empty, GameObject content = null)
        {
            Position = position;
            Status = status;
            Content = content;
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

        public void MakeVoid()
        {
            if (Status == Status.Full)
            {
                Destroy(Content.gameObject);
            }
            Status = Status.Void;
        }

        public void Clear()
        {
            if (Content != null)
                Object.Destroy(Content);
            Status = Status.Empty;
        }

        //internal Mesh getMesh()
        //{

        //}
    }
}