﻿namespace Sigiri
{
    class Util
    {
        public static object[] ListToArray(Values.Value list)
        {
            try
            {
                int count = list.GetElementCount();
                object[] array = new object[count];
                for (int i = 0; i < count; i++)
                {
                    array[i] = list.GetElementAt(i).Value.Data;
                }
                return array;
            }
            catch { }
            return null;
        }

        public static bool isPremitiveType(Values.ValueType type)
        {
            switch (type)
            {
                case Values.ValueType.INTEGER:
                case Values.ValueType.FLOAT:
                case Values.ValueType.STRING:
                case Values.ValueType.LIST:
                case Values.ValueType.DICTIONARY:
                case Values.ValueType.BIGINTEGER:
                case Values.ValueType.INT64:
                case Values.ValueType.COMPLEX:
                    return true;
            }
            return false;
        }

        public static System.Numerics.BigInteger BigIntPow(long b, long exp)
        {
            System.Numerics.BigInteger result = 1;
            if (exp > 0)
            {
                while (exp > 0)
                {
                    result *= b;
                    --exp;
                }
                return result;
            }
            else
            {
                while (exp < 0)
                {
                    result *= b;
                    ++exp;
                }
                return 1 / result;
            }
        }
        public static Values.Value BigPow(System.Numerics.BigInteger b, long exp)
        {
            System.Numerics.BigInteger result = 1;
            if (exp > 0)
            {
                while (exp > 0)
                {
                    result *= b;
                    --exp;
                }
                return new Values.BigInt(result);
            }
            else
            {
                while (exp < 0)
                {
                    result *= b;
                    ++exp;
                }
                return new Values.FloatValue(1.0 / System.Convert.ToDouble(result.ToString()));
            }
        }
        public static Values.Value BigPow(System.Numerics.BigInteger b, System.Numerics.BigInteger exp)
        {
            System.Numerics.BigInteger result = 1;
            if (exp > 0)
            {
                while (exp > 0)
                {
                    result *= b;
                    --exp;
                }
                return new Values.BigInt(result);
            }
            else
            {
                while (exp < 0)
                {
                    result *= b;
                    ++exp;
                }
                return new Values.FloatValue(1.0 / System.Convert.ToDouble(result.ToString()));
            }
        }

        public static Values.Value BigPow(System.Numerics.BigInteger b, double exp)
        {
            return new Values.FloatValue(System.Math.Pow(System.Convert.ToDouble(b), exp));
        }
        public static string Capitalize(string str)
        {
            if (str.Length <= 0)
                return str;
            return str.Length == 1 ? str.ToUpper() : str[0].ToString().ToUpper() + str.Substring(1);
        }

        public static int CountSubstring(this string text, string value, int start, int _count)
        {
            int count = 0, minIndex = text.IndexOf(value, start, _count);
            while (minIndex != -1)
            {
                minIndex = text.IndexOf(value, minIndex + value.Length, _count - (minIndex + value.Length));
                count++;
            }
            return count;
        }
    }
}
