using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    PlayerInputAction action;

    // 플레이어 진행 방향
    Vector2 direction;

    // 플레이어 속도
    public float speed = 5.0f;

    // Start is called before the first frame update
    void Awake()
    {
        action = new PlayerInputAction();
    }

    private void OnEnable()
    {
        action.Player.Enable();
        action.Player.Move.performed += Move_performed;
        action.Player.Move.canceled += Move_canceled;
    }

    private void OnDisable()
    {
        action.Player.Move.canceled -= Move_canceled;
        action.Player.Move.performed -= Move_performed;
        action.Player.Disable();
    }

    /// <summary>
    /// 키보드 버튼을 뗀 순간 방향은 0 이 되는 함수 
    /// </summary>
    /// <param name="context"></param>
    private void Move_canceled(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        direction = Vector2.zero;
    }

    /// <summary>
    /// 키보드로 방향을 Vector2 형식으로 받을 수 있고 나중에 추가적인 행동을 설정할 수 있도록 virtual 로 설정
    /// </summary>
    /// <param name="context"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    protected virtual void Move_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>().normalized;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Time.deltaTime * speed * direction);
    }
}
