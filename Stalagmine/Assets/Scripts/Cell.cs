using UnityEngine;

namespace Grids
{
    public enum Status
    {
        Empty, 
        Void,   
        Full    
    }

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

        
    }
}