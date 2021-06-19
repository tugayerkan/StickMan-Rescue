using UnityEngine;
using UnityEngine.UI;

namespace Tweeners
{
  /// <summary>
  /// Color Tweener to animate color elements
  /// </summary>
  public class ColorAlfaTweener : Tweener
  {

    /**********************************************************************************************/
    /*  Members                                                                                   */
    /**********************************************************************************************/

    [SerializeField]
    private float initialAlpha = default;

    [SerializeField]
    private float finalAlpha = default;

    private MaskableGraphic colorComponent;


    /**********************************************************************************************/
    /*  MonoBehaviour Methods                                                                     */
    /**********************************************************************************************/

    void Awake()
    {
      colorComponent = GetComponent<MaskableGraphic>();
    }

    /**********************************************************************************************/
    /*  Protected Methods                                                                         */
    /**********************************************************************************************/

    protected override void UpdateTransform(float curveValue)
    {
      Color c = colorComponent.color;
      c.a = Mathf.Lerp(initialAlpha, finalAlpha, curveValue);
      colorComponent.color = c;
    }

  }
}