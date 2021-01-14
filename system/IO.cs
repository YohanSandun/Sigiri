using System.IO;

namespace system
{
    class IO
    {
        static FileStream Stream = null;
        static string fname = "";

        public static void init(string file, string mode)
        {
            fname = file;
            openFile(mode);
        }

        public static string readAllText()
        {
            if (Stream == null)
                openFile("r");
            byte[] array = new byte[Stream.Length];
            Stream.Read(array, 0, array.Length);
            return System.Text.Encoding.UTF8.GetString(array);
        }

        public static void openFile(string mode)
        {
            if (mode.Equals("r"))
                Stream = File.OpenRead(fname);
        }

        public static byte[] read(int offset, long count)
        {
            if (Stream == null)
                openFile("r");
            byte[] buffer = new byte[count];
            Stream.Read(buffer, offset, System.Convert.ToInt32(count));
            return buffer;
        }

        public static void close()
        {
            if (Stream != null)
                Stream.Close();
        }

        public static long getLength()
        {
            if (Stream == null)
                openFile("r");
            return Stream.Length;
        }
    }
}
