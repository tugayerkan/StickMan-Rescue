using UnityEngine;

namespace Tweeners
{
  /// <summary>
  /// Transform Tweener for Rect Transform animations
  /// </summary>
  public abstract class TransformTweener : Tweener
  {

    /**********************************************************************************************/
    /*  Members                                                                                   */
    /**********************************************************************************************/
    protected Transform cachedTransform;

    /**********************************************************************************************/
    /*  MonoBehaviour Methods                                                                     */
    /**********************************************************************************************/
    void Awake()
    {
      cachedTransform = transform;
    }

  }
}