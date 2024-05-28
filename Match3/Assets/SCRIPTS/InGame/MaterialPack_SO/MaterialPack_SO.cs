using UnityEngine;

namespace Match3
{
    [CreateAssetMenu(fileName = "MP_", menuName = "Extras/Create MaterialPack")]
    public sealed class MaterialPack_SO : ScriptableObject
    {
        [SerializeField] private Material[] _materials;

        public bool TryGetMaterial(int index, out Material material)
        {
            if (index < 0 || index >= _materials.Length)
            {
                material = null;
                return false;
            }

            material = _materials[index];
            return true;
        }
    }
}
