using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;

public class ShootController : OnScreenControl, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private RectTransform centerArea = null;
    [SerializeField] private RectTransform handle = null;
    [SerializeField] private RectTransform circle;
    [InputControl(layout = "Vector2")]
    [SerializeField] private string dPadControlPath;
    [SerializeField] private float movementRange = 10f;
    [SerializeField] private float moveThreshold = 0f;
    [SerializeField] private float uiMovementRange = 10f;
    [SerializeField] private bool forceIntValue = true;
    private Vector3 startPos;

    [SerializeField] private Transform environment;
    [SerializeField] private Transform prefabToSpawn; // Префаб для створення
    [SerializeField] private Transform ball; // Місце для створення
    [SerializeField] private float maxBullet; // Максимальний розмір префабу
    [SerializeField] private float minBullet; // Мінімальний розмір префабу
    [SerializeField] private float scaleSpeed; // Швидкість збільшення розміру
    private PlayerController playerController;

    private bool isShooting = false;
    private Coroutine shootingCoroutine;
    private float shootTime = 0f;
    private Transform bullet;
    private Vector3 startBallScale;
    private Vector3 initialBallScale = new Vector3(15, 15, 15);
    private Vector3 startEnvScale;
    private float criticaltBallScale = 1f;
    private float envScaleFactor;
    private float playerStratSpeed;

    protected override string controlPathInternal
    {
        get => dPadControlPath;
        set => dPadControlPath = value;
    }

    private void Awake()
    {
        if (centerArea == null)
            centerArea = GetComponent<RectTransform>();

        Vector2 center = new Vector2(0.5f, 0.5f);
        centerArea.pivot = center;
        handle.anchorMin = center;
        handle.anchorMax = center;
        handle.pivot = center;
        handle.anchoredPosition = Vector2.zero;
    }

    private void Start()
    {
        playerController = ball.GetComponent<PlayerController>();
        playerStratSpeed = playerController.playerSpeed;

        startEnvScale = environment.localScale;
        startBallScale = ball.localScale;
        startPos = handle.anchoredPosition;
    }

    void Update()
    {
        if (isShooting && ball.localScale.x > criticaltBallScale)
        {
            if (shootingCoroutine == null)
            {
                shootingCoroutine = StartCoroutine(ShootingCoroutine());
            }
        }
    }

    private IEnumerator ShootingCoroutine()
    {
        while (isShooting && ball.localScale.x > criticaltBallScale)
        {
            shootTime += Time.deltaTime;

            if (bullet != null && bullet.localScale.x < maxBullet)
            {
                float scaleFactor = Mathf.Clamp(shootTime * scaleSpeed, minBullet, maxBullet);
                Vector3 newScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
                Vector3 _newScale = Vector3.Lerp(bullet.localScale, newScale, Time.deltaTime * scaleSpeed);

                bullet.localScale = _newScale;
                ball.localScale = startBallScale - bullet.localScale/ scaleSpeed;

                envScaleFactor = 1 - ball.localScale.x / initialBallScale.x;
                environment.localScale = startEnvScale - startEnvScale * envScaleFactor;

                circle.localScale = new Vector2(scaleFactor / scaleSpeed, scaleFactor / scaleSpeed);
            }
            yield return null;
        }
        shootingCoroutine = null;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isShooting = true;
        playerController.playerSpeed = 0;
        shootTime = 0f;

        if (ball.localScale.x > criticaltBallScale)
        {        
            bullet = Instantiate(prefabToSpawn, ball.position + new Vector3(0, ball.position.y, ball.localScale.z), Quaternion.identity);
        }

        if (eventData == null)
            throw new System.ArgumentNullException(nameof(eventData));

        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isShooting = false;

        playerController.playerSpeed = playerStratSpeed - playerStratSpeed * envScaleFactor/3;
        playerController.isAtack = true;

        if (bullet != null)
        {
            bullet.GetComponent<ProjectileController>().moveSpeed = 10;
        }

        startBallScale = ball.localScale;
        circle.localScale = new Vector2(0.5f, 0.5f);

        handle.anchoredPosition = startPos;
        SendValueToControl(Vector2.zero);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData == null)
            throw new System.ArgumentNullException(nameof(eventData));

        RectTransformUtility.ScreenPointToLocalPointInRectangle(handle.parent.GetComponentInParent<RectTransform>(), eventData.position, eventData.pressEventCamera, out var position);
        Vector2 delta = position;

        Vector2 buttonDelta = Vector2.ClampMagnitude(delta, uiMovementRange);
        handle.anchoredPosition = startPos + (Vector3)buttonDelta;

        Vector2 newPos = SanitizePosition(delta);
        SendValueToControl(newPos);
    }


    private Vector2 SanitizePosition(Vector2 pos)
    {
        pos = Vector2.ClampMagnitude(pos, movementRange);

        float minMovementRange = this.moveThreshold > movementRange ? movementRange : this.moveThreshold;
        if (pos.x < minMovementRange && pos.x > (minMovementRange * -1)) pos.x = 0;
        if (pos.y < minMovementRange && pos.y > (minMovementRange * -1)) pos.y = 0;

        pos = new Vector2(pos.x / movementRange, pos.y / movementRange);

        if (forceIntValue)
        {
            if (pos.x < 0) pos.x = -1;
            else if (pos.x > 0) pos.x = 1;

            if (pos.y < 0) pos.y = -1;
            else if (pos.y > 0) pos.y = 1;
        }

        return pos;
    }
}

