namespace SolderingStation.Hardware.Implementation.Devices.SelfBuiltDeviceV1.Commands;

public struct Q15
{
    private const int FractionalBits = 15;
    private const int One = 1 << FractionalBits;
    private readonly int _rawValue;

    public Q15(int rawValue)
    {
        _rawValue = rawValue;
    }

    public static Q15 FromInt(int value)
    {
        return new Q15(value << FractionalBits);
    }

    public int ToInt()
    {
        return _rawValue >> FractionalBits;
    }
    
    public static Q15 FromFloat(float value)
    {
        return new Q15((int)(value * One));
    }

    public float ToFloat()
    {
        return (float)_rawValue / One;
    }

    public static Q15 operator +(Q15 a, Q15 b)
    {
        return new Q15(a._rawValue + b._rawValue);
    }

    public static Q15 operator -(Q15 a, Q15 b)
    {
        return new Q15(a._rawValue - b._rawValue);
    }

    public static Q15 operator *(Q15 a, Q15 b)
    {
        long result = (long)a._rawValue * b._rawValue;
        return new Q15((int)(result >> FractionalBits));
    }

    public static Q15 operator /(Q15 a, Q15 b)
    {
        long numerator = ((long)a._rawValue << FractionalBits);
        return new Q15((int)(numerator / b._rawValue));
    }

    public override string ToString()
    {
        return ((double)_rawValue / One).ToString("F4");
    }
}