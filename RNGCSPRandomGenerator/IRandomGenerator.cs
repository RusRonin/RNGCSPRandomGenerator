using System;

namespace RandomGenerator
{
    public interface IRandomGenerator : IDisposable
    {
        byte RandomByte(byte lowerLimit, byte upperLimit);
        sbyte RandomSByte(sbyte lowerLimit, sbyte upperLimit);
        ushort RandomUShort(ushort lowerLimit, ushort upperLimit);
        short RandomShort(short lowerLimit, short upperLimit);
        uint RandomUInt(uint lowerLimit, uint upperLimit);
        int RandomInt(int lowerLimit, int upperLimit);
        ulong RandomULong(ulong lowerLimit, ulong upperLimit);
        long RandomLong(long lowerLimit, long upperLimit);
    }
}
