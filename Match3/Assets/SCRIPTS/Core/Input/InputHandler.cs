using UnityEngine;

namespace Match3
{
    public abstract class InputHandler : MonoBehaviour
    {
        protected GridViewer GridViewer;
        protected TileProvider TileProvider;
        protected IGrid Grid;

        protected (int x, int y) FromIntPosition;
        protected (int x, int y) ToIntPosition;

        protected (int x, int y) GridSize
        {
            get
            {
                var grid = Grid.Get();
                return(grid.GetLength(0), grid.GetLength(1));
            }
        }

        [SerializeField] protected bool IsActive;

        public abstract void Init(GridViewer gridViewer, TileProvider tileProvider, ref IGrid grid);

        public void ActivateInput(bool isActive)
        {
            IsActive = isActive;
        }

        public abstract void AbortSwap();
    }
}
