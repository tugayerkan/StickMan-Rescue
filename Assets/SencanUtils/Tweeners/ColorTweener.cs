using UnityEngine;
using UnityEngine.UI;

namespace Tweeners
{
  /// <summary>
  /// Color Tweener to animate color elements
  /// </summary>
  public class ColorTweener : Tweener
  {

    /**********************************************************************************************/
    /*  Members                                                                                   */
    /**********************************************************************************************/

    [SerializeField]
    private Color initialColor = default;

    [SerializeField]
    private Color finalColor = default;

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
      colorComponent.color = Color.Lerp(initialColor, finalColor, curveValue);
    }

  }
}