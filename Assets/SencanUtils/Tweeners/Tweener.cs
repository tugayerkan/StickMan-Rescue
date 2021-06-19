using System.Collections;
using UnityEngine;

namespace Tweeners
{
  /// <summary>
  /// Tweener Utility to animate UI elements
  /// </summary>
  public abstract class Tweener : MonoBehaviour
  {

    /**********************************************************************************************/
    /*  Members                                                                                   */
    /**********************************************************************************************/

    [SerializeField]
    private bool playOnStart = default;
    [SerializeField]
    private bool disableOnFinished = default;

    [SerializeField]
    private AnimationCurve tweenerCurve = default;

    [SerializeField]
    private TweenerType tweenerType = default;

    [SerializeField]
    private float startDelay = default;

    [SerializeField]
    private float duration = default;

    private float currentTime;

    private System.Action onFinishedCallback = delegate { };

    private enum TweenerType
    {
      Once,
      Loop
    }

    /**********************************************************************************************/
    /*  MonoBehaviour Methods                                                                     */
    /**********************************************************************************************/
    void OnEnable()
    {
      if (playOnStart)
      {
        PlayTweener();
      }
    }


    /**********************************************************************************************/
    /*  Public Methods                                                                            */
    /**********************************************************************************************/

    public void AddOnFinishedCallback(System.Action callback)
    {
      onFinishedCallback = callback;
    }

    public void RemoveOnFinishedCallback()
    {
      onFinishedCallback = null;
    }

    public void PlayTweener()
    {
      StartCoroutine(EvaluateTweener());
    }

    public void ResetTweener()
    {
      StopCoroutine(EvaluateTweener());
      UpdateTransform(0);
    }


    /**********************************************************************************************/
    /*  Protected Methods                                                                         */
    /**********************************************************************************************/

    protected abstract void UpdateTransform(float curveValue);


    /**********************************************************************************************/
    /*  Private Methods                                                                           */
    /**********************************************************************************************/

    private IEnumerator EvaluateTweener()
    {
      do
      {
        UpdateTransform(tweenerCurve.Evaluate(0));
        if(startDelay > 0)
        {
          yield return new WaitForSeconds(startDelay);
        }
        currentTime = 0;
        while (currentTime < duration)
        {
          UpdateTransform(tweenerCurve.Evaluate(currentTime / duration));
          currentTime += Time.deltaTime;
          yield return null;
        }
        UpdateTransform(tweenerCurve.Evaluate(1));

        onFinishedCallback();
      } while (tweenerType == TweenerType.Loop);
      if(disableOnFinished)
      {
        gameObject.SetActive(false);
      }
    }

  }
}