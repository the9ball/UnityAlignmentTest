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

        var len = paddingSize + sizeof(double);
        var buf = new byte[len];
        fixed (byte* head = buf)
        {
            var p = head + paddingSize;
            *((double*)p) = value;
        }

        double destination;
        fixed (byte* head = buf)
        {
            var p = head + paddingSize;
            destination = *((double*)p);
        }
        _destination.text = destination.ToString();

        var succeeded = value == destination;
        _result.text = succeeded ? "Success" : "Fail";
    }
}
