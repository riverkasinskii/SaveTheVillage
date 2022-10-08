using UnityEngine;
using UnityEngine.UI;

public class ChangeSprite : MonoBehaviour
{
    [SerializeField] private Sprite newSprite; 
    [SerializeField] private Sprite oldSprite;
    private bool isCheckedChanged;

    public void OnClick()
    {
        if (isCheckedChanged == true)
        {
            GetComponent<Image>().sprite = oldSprite;
            isCheckedChanged = false;
        }
        else
        {
            GetComponent<Image>().sprite = newSprite;
            isCheckedChanged = true;
        }
    }
}
