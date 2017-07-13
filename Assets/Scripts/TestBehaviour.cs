using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class TestBehaviour : MonoBehaviour
{
    [SerializeField]
    private InputField _paddingSize = null;

    [SerializeField]
    private InputField _source = null;

    [SerializeField]
    private Text _destination = null;

    [SerializeField]
    private Text _result = null;

    public void Awake()
    {
        _paddingSize.text = "1";
        _source.text = "1234.5678";
    }

    public unsafe void Do()
    {
        var paddingSize = int.Parse(_paddingSize.text);

        double value = double.Parse(_source.text);

        Debug.LogFormat("padding:{0}\nvalue:{1}", paddingSize, value);

        var len = paddingSize + sizeof(double);
        var buf = new byte[len];
        fixed (byte* head = buf)
        {
            var p = head + paddingSize;
            *((double*)p) = value;
        }

        Debug.Log("Serialize Done");

        var sb = new StringBuilder();
        for (int i = 0; i < 0x10; i++)
        {
            sb.Append(string.Format(" {0:x2}", i));
        }
        sb.Append("\n");
        for (int i = 0; i < len; i++)
        {
            sb.Append(string.Format(" {0:x2}", buf[i]));
            if (i == 0x10) sb.Append("\n");
        }
        Debug.Log(sb.ToString());

        double destination;
        fixed (byte* head = buf)
        {
            var p = head + paddingSize;
            destination = *((double*)p);
        }
        _destination.text = destination.ToString();

        Debug.Log("Deserialize Done");
        Debug.LogFormat("result:{0}", destination);

        var succeeded = value == destination;
        _result.text = succeeded ? "Success" : "Fail";
    }
}
