using System;

namespace JCore
{
    public class JRandom
    {
        int m_num1;
        int m_num2;
        string m_operate;
        string[] m_operateList = new string[] { "+", "×" };
        const string c_equal = "=";

        public string Content {
            get {
                return m_num1 + m_operate + m_num2 + c_equal;
            }
        }

        public int Next() {
            Random random = new Random();
            m_num1 = random.Next(0, 10);
            m_num2 = random.Next(0, 10);
            var index = random.Next(0, m_operateList.Length);
            m_operate = m_operateList[index];

            int result = 0;
            switch (index) {
                case 1:
                    result = m_num1 * m_num2;
                    break;
                case 0:
                default:
                    result = m_num1 + m_num2;
                    break;
            }

            return result;
        }

    }
}