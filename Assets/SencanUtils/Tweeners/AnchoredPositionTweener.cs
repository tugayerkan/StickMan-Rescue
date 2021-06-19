using UnityEngine;

namespace Tweeners
{
  /// <summary>
  /// Position Tweener to animate UI transforms
  /// </summary>
  public class AnchoredPositionTweener : RectTransformTweener
  {

    /**********************************************************************************************/
    /*  Members                                                                                   */
    /**********************************************************************************************/

    [SerializeField]
    private Vector3 initialPosition = default;

    [SerializeField]
    private Vector3 finalPosition = default;

    /**********************************************************************************************/
    /*  Protected Methods                                                                         */
    /**********************************************************************************************/

    protected override void UpdateTransform(float curveValue)
    {
      cachedRectTransform.anchoredPosition = initialPosition + ((finalPosition - initialPosition) * curveValue);
    }

  }
}