using UnityEngine;
using UnityEngine.EventSystems;

namespace Aurore.DialogSystem
{
    public class SkipOnClickBehavior : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler

    {
        public void OnPointerClick(PointerEventData eventData)
        {
            DialogueUiManager.Instance.OnSkipDialog();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            DialogueUiManager.Instance.OnSkipHovered(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            DialogueUiManager.Instance.OnSkipHovered(false);
        }
    }
}
