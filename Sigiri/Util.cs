namespace Sigiri
{
    class Util
    {
        public static object[] ListToArray(Values.Value list) {
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

        public static bool isPremitiveType(Values.ValueType type) {
            switch (type) {
                case Values.ValueType.INTEGER:
                case Values.ValueType.FLOAT:
                case Values.ValueType.STRING:
                case Values.ValueType.LIST:
                case Values.ValueType.DICTIONARY:
                    return true;
            }
            return false;
        }
    }
}
