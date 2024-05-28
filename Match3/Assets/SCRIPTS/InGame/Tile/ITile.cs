using UnityEngine;

namespace Match3
{
    public interface ITile
    {
        public void Init();

        public void SetID(int id);

        public int GetID();

        public Transform GetTransform();

        public (int x, int y) GetIntPosition();

        public void SetPosition((int x, int y) position, 
            (int x, int y) gridSize);

        public void UpdateIntPosition();

        public void Highlight(bool highlight);
    }
}
