using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(RectTransform), typeof(CanvasGroup))]
public class EntityUi : MonoBehaviour
{
    [SerializeField] public Vector3 offset = new Vector3(0, 1);
    const float MAX_DISTANCE_FROM_CAMERA = 100;
    protected GameObject _gameObject;
    protected Camera _camera;
    protected Canvas _canvas;
    private RectTransform canvasRect;
    private RectTransform rect;
    protected CanvasGroup canvasGroup;
    bool isVisible;
    bool subscribed;
    public virtual void Awake()
    {
        rect = this.GetComponent<RectTransform>();
        canvasGroup = this.GetComponent<CanvasGroup>();
        Hide();
    }
    public virtual void Subscribe(GameObject go, Camera camera, Canvas canvas)
    {
        _gameObject = go;
        _camera = camera;
        _canvas = canvas;
        rect = this.GetComponent<RectTransform>();
        subscribed = true;
        Show();
    }
    public virtual void Unsubscribe()
    {
        Hide();
        Reset();
    }

    protected virtual void Update()
    {
        if (!subscribed) return;
        isVisible = InRange();
        if (isVisible) { Show(); }
        else { Hide(); }
        UpdatePosition();
    }
    protected virtual void Hide()
    {
        canvasGroup.alpha = 0;
    }
    protected virtual void Show()
    {
        canvasGroup.alpha = 1;
    }
    //TODO so that it doesn go outside of screen
    protected virtual void UpdatePosition()
    {
        if (!isVisible) { return; }
        if (gameObject != null) { rect.position = _camera.WorldToScreenPoint(_gameObject.transform.position + offset); }
    }
    protected bool InRange()
    {
        if (!_camera || !_gameObject) { return false; }
        return (Mathf.Abs((_camera.transform.position - _gameObject.transform.position).magnitude) < MAX_DISTANCE_FROM_CAMERA);
    }
    private void Reset()
    {
        _gameObject = null;
        _camera = null;
        _canvas = null;
        isVisible = false;
        subscribed = false;
    }
}
