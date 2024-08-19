using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TestBase : MonoBehaviour
{
    PlayerInputAction action;

    PlayerBase player;

    public string fileName = "health"; // Resources 폴더 내의 파일 이름 (확장자 제외)
    private Texture2D texture;

    void Awake()
    {
        // 필요한 컴포넌트 초기화
        action = new PlayerInputAction();
    }

    private void OnEnable()
    {
        action.Player.Enable();
        action.Player.Test1.performed += Test1_performed;
        action.Player.Test2.performed += Test2_performed;
        action.Player.Test3.performed += Test3_performed;
        action.Player.Test4.performed += Test4_performed;
        action.Player.Test5.performed += Test5_performed;

    }

    private void OnDisable()
    {
        action.Player.Test5.canceled -= Test5_performed;
        action.Player.Test4.canceled -= Test4_performed;
        action.Player.Test3.canceled -= Test3_performed;
        action.Player.Test2.canceled -= Test2_performed;
        action.Player.Test1.canceled -= Test1_performed;
        action.Player.Disable();
    }

    private void Start()
    {
        player = GameManager.Instance.Player;
    }

    private void Test1_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Factory.Instance.GetSkeleton(new Vector3(10, 5));
    }

    private void Test2_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Vector3 test = Quaternion.AngleAxis(90, Vector3.forward) * new Vector3(1, 2, 0);
        Debug.Log(test);
    }

    private void Test3_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        GameManager.Instance.Wave = 8;
    }
   
    private void Test4_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        player.Test_LevelUp();
    }

    private void Test5_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Factory.Instance.GetEnemyBullet(Vector3.zero);
    }


}
