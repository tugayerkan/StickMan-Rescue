using UnityEngine;

namespace Tweeners
{
  /// <summary>
  /// Scale Tweener
  /// </summary>
  public class LocalScaleTweener : TransformTweener
  {

    /**********************************************************************************************/
    /*  Members                                                                                   */
    /**********************************************************************************************/

    [SerializeField]
    private Vector3 initialLocalScale = default;

    [SerializeField]
    private Vector3 finalLocalScale = default;

    /**********************************************************************************************/
    /*  Protected Methods                                                                         */
    /**********************************************************************************************/

    protected override void UpdateTransform(float curveValue)
    {
      cachedTransform.localScale = initialLocalScale + ((finalLocalScale - initialLocalScale) * curveValue);
    }
  }
}