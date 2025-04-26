using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharactorCard : MonoBehaviour
{
    [SerializeField] private Image CardImage;
    [SerializeField] private Sprite WaitCardImage;
    [SerializeField] private Sprite SelectedCardImage;
    [SerializeField] private Button SelectButton;
    [SerializeField] private Button CancelButton;
    [SerializeField] private TextMeshProUGUI playerName;

    public void EnableCard_Cancel()
    {
        CardImage.sprite = SelectedCardImage;
        SelectButton.gameObject.SetActive(false);
        CancelButton.gameObject.SetActive(true);
    }

    public void EnableCard_Select()
    {
        CardImage.sprite = WaitCardImage;
        SelectButton.gameObject.SetActive(true);
        CancelButton.gameObject.SetActive(false);
    }

    public void DisableCard()
    {
        CardImage.sprite = SelectedCardImage;
        SelectButton.gameObject.SetActive(false);
        CancelButton.gameObject.SetActive(false);
    }


}
