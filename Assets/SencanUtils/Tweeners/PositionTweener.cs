using UnityEngine;

namespace Tweeners
{
  /// <summary>
  /// Position Tweener to animate UI transforms
  /// </summary>
  public class PositionTweener : TransformTweener
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
      cachedTransform.localPosition = initialPosition + ((finalPosition - initialPosition) * curveValue);
    }
  }
}