using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palindrome_Number
{
    internal class Program
    {
        /*
        Given an integer x, return true if x is a 
        palindrome, and false otherwise.

        Example 1:
        Input: x = 121
        Output: true
        Explanation: 121 reads as 121 from left to right and from right to left.

        Example 2:
        Input: x = -121
        Output: false
        Explanation: From left to right, it reads -121. From right to left, it becomes 121-. Therefore it is not a palindrome.
        
        Example 3:
        Input: x = 10
        Output: false
        Explanation: Reads 01 from right to left. Therefore it is not a palindrome.

        Constraints:
        -231 <= x <= 231 - 1
        */

        static void Main(string[] args)
        {
            int a = 121;


            bool z = IsPalindrome(a);
        }

        public static bool IsPalindrome(int x)
        {
            string strOrig = x.ToString();

            for (int i = 0; i < strOrig.Length / 2; i++)
            {
                if (strOrig[i] != strOrig[strOrig.Length - 1 - i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
