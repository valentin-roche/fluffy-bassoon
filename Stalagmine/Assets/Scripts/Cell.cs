using UnityEngine;

namespace Grids {
    enum Status
    {
        Empty,
        Void,
        Full
    }
    public class Cell {
        private Status status; 
        private GameObject content;

        public Cell() { status = Status.Empty; }
        public Cell(GameObject Content) { SetContent(Content); }

        public void SetContent(GameObject Content) {
            if (content != null) {
                status = Status.Full;
                content = Content;
            }
        }
    }
}