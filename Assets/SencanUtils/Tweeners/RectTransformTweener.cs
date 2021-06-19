using UnityEngine;

namespace Tweeners
{
  /// <summary>
  /// Transform Tweener for Rect Transform animations
  /// </summary>
  public abstract class RectTransformTweener : Tweener
  {

    /**********************************************************************************************/
    /*  Members                                                                                   */
    /**********************************************************************************************/
    protected RectTransform cachedRectTransform;

    /**********************************************************************************************/
    /*  MonoBehaviour Methods                                                                     */
    /**********************************************************************************************/
    void Awake()
    {
      cachedRectTransform = transform as RectTransform;
    }

  }
}