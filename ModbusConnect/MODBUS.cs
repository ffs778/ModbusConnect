using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCConnect
{
    public class MODBUS
    {
        #region 读写string
        /// <summary>
        /// 赋值string
        /// </summary>
        /// <param name="num">字符串变量总长度</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static ushort[] SetString(ushort num, string value)
        {
            ushort[] fullData = new ushort[num];   // 定义指定长度，目的是通过覆盖，解决长字符串干扰短字符串的问题。
            byte[] bytesTemp = Encoding.UTF8.GetBytes(value);
            ushort[] realData = Bytes2Ushorts(bytesTemp, reverse: false);
            realData.CopyTo(fullData, 0);
            return fullData;
        }

        /// <summary>
        /// 获取string
        /// </summary>
        /// <param name="src"></param>
        /// <param name="start"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string GetString(ushort[] src)
        {
            byte[] bytesTemp = Ushorts2Bytes(src, reverse: false);
            string res = Encoding.UTF8.GetString(bytesTemp).Trim(new char[] { '\0' });
            return res;
        }
        /// <summary>
        /// 获取PLC中wstring类型的数据，wstring可以包含中文字符
        /// </summary>
        /// <param name="src"></param>
        /// <param name="start"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string GetWString(ushort[] src)
        {
            byte[] bytesTemp = Ushorts2Bytes(src, reverse: true);
            string res = Encoding.BigEndianUnicode.GetString(bytesTemp).Trim(new char[] { '\0' });
            return res;
        }
        /// <summary>
        /// 写如wstring类型。
        /// </summary>
        /// <param name="num"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ushort[] SetWString(ushort num, string value)
        {
            ushort[] fullData = new ushort[num];
            byte[] bytesTemp = Encoding.BigEndianUnicode.GetBytes(value);
            ushort[] realData = Bytes2Ushorts(bytesTemp, reverse: true); // 存在byte数组顺序的问题导致乱序时，颠倒一下数组
            realData.CopyTo(fullData, 0);
            return fullData;
        }
        #endregion

        #region 读写Real
        /// <summary>
        /// 赋值Real类型数据
        /// </summary>
        /// <param name="src"></param>
        /// <param name="start"></param>
        /// <param name="value"></param>
        public static ushort[] SetReal(float value)
        {

            byte[] bytes = BitConverter.GetBytes(value);

            ushort[] dest = Bytes2Ushorts(bytes);

            return dest;
        }
        public static ushort[] SetReals(float[] values)
        {
            ushort[] temp = Array.Empty<ushort>();
            for (int i = 0; i < values.Length; i++)
            {
                temp = temp.Concat(SetReal(values[i])).ToArray();
            }
            return temp;
        }
       
        /// <summary>
        /// 获取float类型数据
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static float GetReal(ushort[] src)
        {
            byte[] bytesTemp = Ushorts2Bytes(src);
            float res = BitConverter.ToSingle(bytesTemp, 0);
            return res;
        }
        public static float[] GetReals(ushort[] src)
        {
            List<float> floatList = new List<float>(src.Length / 2);
            for (int i = 0; i < src.Length; i += 2)
            {
                var currentUshort = src.Skip(i).Take(2).ToArray();
                floatList.Add(GetReal(currentUshort));
            }
            return floatList.ToArray();
        }
        #endregion

        #region 读写short
        /// <summary>
        /// 赋值Short类型数据
        /// </summary>
        /// <param name="src"></param>
        /// <param name="start"></param>
        /// <param name="value"></param>
        public static ushort[] SetShort(short value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            ushort[] dest = Bytes2Ushorts(bytes);
            return dest;
        }
        public static ushort[] SetShorts(short[] values)
        {
            ushort[] temp = Array.Empty<ushort>();
            for (int i = 0; i < values.Length; i++)
            {
                temp = temp.Concat(SetShort(values[i])).ToArray();
            }
            return temp;
        }

        /// <summary>
        /// 获取short类型数据
        /// </summary>
        /// <param name="src"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        public static short GetShort(params ushort[] src)
        {
            byte[] bytesTemp = Ushorts2Bytes(src);
            short res = BitConverter.ToInt16(bytesTemp, 0);
            return res;
        }
        public static short[] GetShorts(ushort[] src)
        {
            List<short> shortList = new List<short>(src.Length);
            for (int i = 0; i < src.Length; i++)
            {
                shortList.Add(GetShort(src[i]));
            }
            return shortList.ToArray();
        }
        #endregion

        #region 读写bool

        public static ushort[] SetBool(bool value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            ushort[] dest = Bytes2Ushorts(bytes);
            return dest;
        }
        public static ushort[] SetBools(bool[] values)
        {
            ushort[] temp = Array.Empty<ushort>();
            for (int i = 0; i < values.Length; i++)
            {
                temp = temp.Concat(SetBool(values[i])).ToArray();
            }
            return temp;
        }

        public static bool GetBool(params ushort[] src)
        {
            byte[] bytes = Ushorts2Bytes(src);  // 从16位到8位，后8位的数据是无效的。
            List<bool> data = new List<bool>(src.Length);
            bool[] res = Byte2Bool(bytes);
            for (int i = 0; i < src.Length; i++)
            {
                if (i % 2 == 0) data.Add(res[i]);
            }
            return data.ToArray()[0];
        }
        public static bool[] GetBools(ushort[] src)
        {
            List<bool> boolList = new List<bool>(src.Length);
            for (int i = 0; i < src.Length; i++)
            {
                boolList.Add(GetBool(src[i]));
            }
            return boolList.ToArray();
        }
        #endregion

        #region byte 和 bool互转
        private static bool[] Byte2Bool(byte[] b)
        {
            bool[] array = new bool[b.Length];
            for (int i = 0; i < b.Length; i++)
            {
                array[i] = BitConverter.ToBoolean(b, i);
            }
            return array;
        }

        private static bool[] Bytes2Bools(byte[] b)
        {
            bool[] array = new bool[8 * b.Length];

            for (int i = 0; i < b.Length; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    array[i * 8 + j] = (b[i] & 1) == 1;//判定byte的最后一位是否为1，若为1，则是true；否则是false
                    b[i] = (byte)(b[i] >> 1);//将byte右移一位
                }
            }
            return array;
        }

        private static byte Bools2Byte(bool[] array)
        {
            if (array != null && array.Length > 0)
            {
                byte b = 0;
                for (int i = 0; i < 8; i++)
                {
                    if (array[i])
                    {
                        byte nn = (byte)(1 << i);//左移一位，相当于×2
                        b += nn;
                    }
                }
                return b;
            }
            return 0;
        }
        #endregion

        #region byte 和 ushort 互转
        private static ushort[] Bytes2Ushorts(byte[] src, bool reverse = false)
        {
            int len = src.Length;

            byte[] srcPlus = new byte[len + 1];
            src.CopyTo(srcPlus, 0);
            int count = len >> 1;

            if (len % 2 != 0)
            {
                count += 1;
            }

            ushort[] dest = new ushort[count];
            if (reverse)
            {
                for (int i = 0; i < count; i++)
                {
                    dest[i] = (ushort)(srcPlus[i * 2] << 8 | srcPlus[2 * i + 1] & 0xff);
                }
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    dest[i] = (ushort)(srcPlus[i * 2] & 0xff | srcPlus[2 * i + 1] << 8);
                }
            }

            return dest;
        }

        private static byte[] Ushorts2Bytes(ushort[] src, bool reverse = false)
        {

            int count = src.Length;
            byte[] dest = new byte[count << 1];
            if (reverse)
            {
                for (int i = 0; i < count; i++)
                {
                    dest[i * 2] = (byte)(src[i] >> 8);
                    dest[i * 2 + 1] = (byte)(src[i] >> 0);
                }
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    dest[i * 2] = (byte)(src[i] >> 0);
                    dest[i * 2 + 1] = (byte)(src[i] >> 8);
                }
            }
            return dest;
        }
        #endregion
    }
}
