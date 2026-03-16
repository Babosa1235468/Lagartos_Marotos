using UnityEngine;
using TMPro;

public class KeyRebinder : MonoBehaviour
{
    public int playerIndex; // 1 = P1, 2 = P2
    public ControlType keyToRebind;
    public enum ControlType
    {
        Jump,
        Left,
        Down,
        Right,
        Shoot,
        Reload
    }
    public TextMeshProUGUI buttonText;
    //foi o calude que deu, um dicionario para ficar em portugues
    private static readonly System.Collections.Generic.Dictionary<KeyCode, string> nomeTeclas =
        new System.Collections.Generic.Dictionary<KeyCode, string>()
    {
    { KeyCode.UpArrow, "↑" },
    { KeyCode.DownArrow, "↓" },
    { KeyCode.LeftArrow, "←" },
    { KeyCode.RightArrow, "→" },
    { KeyCode.Space, "Espaço" },
    { KeyCode.Return, "Enter" },
    { KeyCode.LeftShift, "Shift Esq" },
    { KeyCode.RightShift, "Shift Dir" },
    { KeyCode.LeftControl, "Ctrl Esq" },
    { KeyCode.RightControl, "Ctrl Dir" },
    { KeyCode.LeftAlt, "Alt Esq" },
    { KeyCode.RightAlt, "Alt Dir" },
    { KeyCode.Backspace, "Apagar" },
    { KeyCode.Tab, "Tab" },
    { KeyCode.CapsLock, "Caps Lock" },
    { KeyCode.Mouse0, "Clique Esq" },
    { KeyCode.Mouse1, "Clique Dir" },
    { KeyCode.Mouse2, "Clique Meio" },
    };

    string ObterNomeTecla(KeyCode tecla)
    {
        if (nomeTeclas.TryGetValue(tecla, out string nome))
            return nome;
        return tecla.ToString(); // teclas normais como A, B, C...
    }
    private bool waitingForKey = false;

    public void StartRebind()
    {
        waitingForKey = true;
        buttonText.text = "...";
    }

    void Update()
    {
        if (!waitingForKey) return;

        foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(key))
            {
                if (key == KeyCode.Escape)
                {
                    waitingForKey = false;
                    buttonText.text = "Essa Nao";
                    return;
                }
                AssignKey(key);
                waitingForKey = false;
                return;
            }
        }
    }

    void AssignKey(KeyCode key)
    {
        KeyCode[] movement;
        KeyCode[] shooting;

        if (playerIndex == 1)
        {
            movement = DataManager.instance.P1MovementControls;
            shooting = DataManager.instance.P1ShootingControls;
        }
        else
        {
            movement = DataManager.instance.P2MovementControls;
            shooting = DataManager.instance.P2ShootingControls;
        }
        switch (keyToRebind)
        {
            case ControlType.Jump:
                movement[0] = key;
                break;

            case ControlType.Left:
                movement[1] = key;
                break;

            case ControlType.Down:
                movement[2] = key;
                break;

            case ControlType.Right:
                movement[3] = key;
                break;

            case ControlType.Shoot:
                shooting[0] = key;
                break;

            case ControlType.Reload:
                shooting[1] = key;
                break;
        }

        buttonText.text = ObterNomeTecla(key);
    }
}