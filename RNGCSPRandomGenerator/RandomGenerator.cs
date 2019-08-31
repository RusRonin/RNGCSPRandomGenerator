using System;
using System.Security.Cryptography;

//RNGCSPRandomGenerator by Ronin
//wrapper for RNGCryptoServiceProvider class from System.Security.Cryptography
//created for .Net Core 2.2
//surely works on .Net Core 2.1, other versions are untested


namespace RNGCSPRandomGenerator
{
    public class RandomGenerator : IRandomGenerator
    {
        private readonly RNGCryptoServiceProvider rngcsp;

        private bool disposed = false;

        public RandomGenerator()
        {
            rngcsp = new RNGCryptoServiceProvider();

        }

        public byte RandomByte(byte lowerLimit, byte upperLimit)
        {
            //if lower limit equals upper limit, the only number that fit this condition is this lower/upper limit
            //so we can just return it
            if (lowerLimit == upperLimit)
            {
                return lowerLimit;
            }

            //if lower limit is more than upper one, we swap it and return random result between those limits
            if (lowerLimit > upperLimit)
            {
                byte tmpSwapper = lowerLimit;
                lowerLimit = upperLimit;
                upperLimit = tmpSwapper;
            }

            byte[] randomBytes = new byte[1];
            byte randomNumber;
            do
            {
                rngcsp.GetBytes(randomBytes);
                randomNumber = randomBytes[0];
            }
            while (!IsCorrectlyRandomized(randomNumber, (byte) (upperLimit - lowerLimit + 1), byte.MaxValue));


            return (byte)((randomNumber % (upperLimit - lowerLimit + 1)) + lowerLimit);
        }

        public sbyte RandomSByte(sbyte lowerLimit, sbyte upperLimit)
        {
            //MinValue is -128, so -MinValue gives us +128
            byte unsignedResult = RandomByte((byte) (lowerLimit - sbyte.MinValue), (byte) (upperLimit - sbyte.MinValue));
            //and now we add -128 to make result signed (f.e. if unsignedResult is 254 (byte.MaxValue - 1) we get 126 (sbyte.MaxValue - 1))
            return (sbyte)(unsignedResult + sbyte.MinValue);
        }

        public ushort RandomUShort(ushort lowerLimit, ushort upperLimit)
        {
            //if lower limit equals upper limit, the only number that fit this condition is this lower/upper limit
            //so we can just return it
            if (lowerLimit == upperLimit)
            {
                return lowerLimit;
            }

            //if lower limit is more than upper one, we swap it and return random result between those limits
            if (lowerLimit > upperLimit)
            {
                ushort tmpSwapper = lowerLimit;
                lowerLimit = upperLimit;
                upperLimit = tmpSwapper;
            }

            byte[] randomBytes = new byte[2];
            uint randomNumber;
            do
            {
                rngcsp.GetBytes(randomBytes);
                randomNumber = (uint)(randomBytes[0] + (randomBytes[1] * Math.Pow(2, 8)));
            }
            while (!IsCorrectlyRandomized(randomNumber, (ushort) (upperLimit - lowerLimit + 1), uint.MaxValue));


            return (ushort)((randomNumber % (upperLimit - lowerLimit + 1)) + lowerLimit);
        }

        public short RandomShort(short lowerLimit, short upperLimit)
        {
            //MinValue is -32768, so -MinValue gives us +32768
            ushort unsignedResult = RandomUShort((ushort)(lowerLimit - short.MinValue), (ushort)(upperLimit - short.MinValue));
            //and now we add -32768 to make result signed (f.e. if unsignedResult is 65534 (ushort.MaxValue - 1) we get 32766 (short.MaxValue - 1))
            return (short)(unsignedResult + short.MinValue);
        }

        public uint RandomUInt(uint lowerLimit, uint upperLimit)
        {
            //if lower limit equals upper limit, the only number that fit this condition is this lower/upper limit
            //so we can just return it
            if (lowerLimit == upperLimit)
            {
                return lowerLimit;
            }

            //if lower limit is more than upper one, we swap it and return random result between those limits
            if (lowerLimit > upperLimit)
            {
                uint tmpSwapper = lowerLimit;
                lowerLimit = upperLimit;
                upperLimit = tmpSwapper;
            }

            byte[] randomBytes = new byte[4];
            uint randomNumber;
            do
            {
                rngcsp.GetBytes(randomBytes);
                randomNumber = (uint)(randomBytes[0] + (randomBytes[1] * Math.Pow(2, 8)) +
                (randomBytes[2] * Math.Pow(2, 16)) + (randomBytes[3] * Math.Pow(2, 24)));
            }
            while (!IsCorrectlyRandomized(randomNumber, upperLimit - lowerLimit + 1, uint.MaxValue));


            return (uint) ((randomNumber % (upperLimit - lowerLimit + 1)) + lowerLimit);
        }

        public int RandomInt(int lowerLimit, int upperLimit)
        {
            //MinValue is -2147483648, so -MinValue gives us +2147483648
            uint unsignedResult = RandomUInt((uint)(lowerLimit - int.MinValue), (uint)(upperLimit - int.MinValue));
            //and now we add -2147483648 to make result signed 
            //f.e. if unsignedResult is 4294967294 (uint.MaxValue - 1) we get 2147483646 (int.MaxValue - 1)
            return (int) (unsignedResult + int.MinValue);
        }

        public ulong RandomULong(ulong lowerLimit, ulong upperLimit)
        {
            //if lower limit equals upper limit, the only number that fit this condition is this lower/upper limit
            //so we can just return it
            if (lowerLimit == upperLimit)
            {
                return lowerLimit;
            }

            //if lower limit is more than upper one, we swap it and return random result between those limits
            if (lowerLimit > upperLimit)
            {
                ulong tmpSwapper = lowerLimit;
                lowerLimit = upperLimit;
                upperLimit = tmpSwapper;
            }

            byte[] randomBytes = new byte[8];
            ulong randomNumber;
            do
            {
                rngcsp.GetBytes(randomBytes);
                randomNumber = (uint)(randomBytes[0] + (randomBytes[1] * Math.Pow(2, 8)) +
                (randomBytes[2] * Math.Pow(2, 16)) + (randomBytes[3] * Math.Pow(2, 24)) +
                (randomBytes[4] * Math.Pow(2, 32)) + (randomBytes[5] * Math.Pow(2, 40)) + 
                (randomBytes[6] * Math.Pow(2, 48)) + (randomBytes[7] * Math.Pow(2, 48)));
            }
            while (!IsCorrectlyRandomized(randomNumber, upperLimit - lowerLimit + 1, ulong.MaxValue));


            return (ulong)((randomNumber % (upperLimit - lowerLimit + 1)) + lowerLimit);
        }

        public long RandomLong(long lowerLimit, long upperLimit)
        {
            long MakeSigned(ulong result)
            {
                //if result > long.MaxValue we can simply substract MaxValue from result and get positive number,
                //which is necessary because calculation is performed in ulong
                if (result > long.MaxValue)
                {
                    return (long) (result - long.MaxValue);
                }
                //and if result < long.MaxValue, we need to substract result from MaxValue, 
                //and then make the result negative by multiplying on -1
                else
                {
                    return ((long)(long.MaxValue - result) * -1);
                }
            }

            //same as in other cases, but we need to make it signed in a different way (described in MakeSigned)
            ulong unsignedResult = RandomULong((ulong)(lowerLimit - long.MinValue), (ulong)(upperLimit - long.MinValue));
            
            return MakeSigned(unsignedResult);
        }

        private bool IsCorrectlyRandomized(ulong randomized, ulong dividedBy, ulong proceedingTypeMaxValue)
        {
            //method based on Microsoft Documentation example

            // There are maxValue / dividedBy full sets of numbers that can come up in a single variable
            ulong fullSetsOfValues = (ulong) (proceedingTypeMaxValue / dividedBy);

            // If the randomized result is within this range of correct values, then we let it continue
            return randomized < dividedBy * fullSetsOfValues;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    rngcsp.Dispose();
                }
                disposed = true;
            }
        }

        ~RandomGenerator()
        {
            Dispose(false);
        }
    }
}
