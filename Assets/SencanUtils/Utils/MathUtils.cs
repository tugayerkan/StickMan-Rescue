using UnityEngine;

namespace SencanUtils
{
    public static class MathUtils
    {
        public static Vector2 RandomPointOnCircle(float radius)
        {
            return Random.insideUnitCircle.normalized * radius;
        }

        public static Vector3 RandomPointOnSphere(float radius)
        {
            return Random.onUnitSphere * radius;
        }
        
        public static Vector3 QuadraticLerp(Vector3 posA, Vector3 posB, Vector3 posC, float interpolateAmount)
        {
            Vector3 ab = Vector3.Lerp(posA, posB, interpolateAmount);
            Vector3 bc = Vector3.Lerp(posB, posC, interpolateAmount);

            return Vector3.Lerp(ab, bc, interpolateAmount);
        }

        public static Vector3 CubicLerp(Vector3 posA, Vector3 posB, Vector3 posC, Vector3 posD, float interpolateAmount)
        {
            Vector3 ab_bc = QuadraticLerp(posA, posB, posC, interpolateAmount);
            Vector3 bc_cd = QuadraticLerp(posB, posC, posD, interpolateAmount);
            return Vector3.Lerp(ab_bc, bc_cd, interpolateAmount);
        }
    }
}
