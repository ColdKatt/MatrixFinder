using UnityEngine;

namespace MatrixFinder.HelpTools
{
    /// <summary>
    /// Determines additional methods for primitive shapes.
    /// </summary>
    public static class PrimitiveHelper
    {
        public static GameObject CreatePrimitiveFromMatrix(Matrix4x4 matrix, PrimitiveType type = PrimitiveType.Cube, Color objectColor = default)
        {
            var primitive = GameObject.CreatePrimitive(type);

            if (primitive.TryGetComponent(out Renderer renderer))
            {
                renderer.material.color = objectColor;
            }

            primitive.transform.SetPositionAndRotation(matrix.GetPosition(), matrix.rotation);
            primitive.transform.localScale = matrix.lossyScale;

            return primitive;
        }
    }
}