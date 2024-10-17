using UnityEngine;
using UnityEngine.UIElements;

public abstract class Panel : MonoBehaviour
{
    public string panelName;
    public abstract void Hide();
    public abstract void Show();
}
