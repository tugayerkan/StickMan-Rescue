using UnityEngine;

namespace Tweeners
{
  /// <summary>
  /// Rotation Tweener to animate UI transforms
  /// </summary>
  public class RotationTweener : TransformTweener
  {

    /**********************************************************************************************/
    /*  Members                                                                                   */
    /**********************************************************************************************/
    [SerializeField]
    private float initialRotation = default;

    [SerializeField]
    private float finalRotation = default;

    private static readonly Vector3 ZComponentModifier = new Vector3(0, 0, 1);


    /**********************************************************************************************/
    /*  Protected Methods                                                                         */
    /**********************************************************************************************/
    protected override void UpdateTransform(float curveValue)
    {
      cachedTransform.localEulerAngles = ZComponentModifier * Mathf.Lerp(initialRotation, finalRotation, curveValue);
    }




  }
}