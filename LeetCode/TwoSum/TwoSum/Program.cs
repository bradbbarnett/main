using System;
using System.Collections.Generic;

namespace TwoSum
{
    internal class Program
    {
        /*
        Given an array of integers nums and an integer target,
        return indices of the two numbers such that they add up to target.

        You may assume that each input would have exactly one solution,
        and you may not use the same element twice.

        You can return the answer in any order.

        Example 1:
        Input: nums = [2,7,11,15], target = 9
        Output: [0,1]
        Explanation: Because nums[0] + nums[1] == 9, we return [0, 1].

        Example 2:
        Input: nums = [3,2,4], target = 6
        Output: [1,2]

        Example 3:
        Input: nums = [3,3], target = 6
        Output: [0,1]

        Constraints:
        2 <= nums.length <= 10^4
        -10^9 <= nums[i] <= 10^9
        -10^9 <= target <= 10^9
        Only one valid answer exists.
        */

        static void Main(string[] args)
        {
            int[] nums = { 2, 7, 11, 15 };
            int target = 26;

            //int[] nums = { 3, 2, 4 };
            //int target = 6;

            //int[] nums = { 3, 3 };
            //int target = 6;


            int[] numIndex = TwoSum(nums, target);

            if (numIndex != null)
            {
                Console.WriteLine("[" + numIndex[0] + "," + numIndex[1] + "]");
                Console.ReadLine();
            }
        }

        //best runtime / O(n)
        public static int[] TwoSum(int[] nums, int target)
        {
            Dictionary<int, int> dict = new Dictionary<int, int>();
            for (int i = 0; i < nums.Length; i++)
            {
                int complement = target - nums[i];

                //key = value of index
                //value = index
                if (dict.ContainsKey(complement))
                {
                    return new[] { dict[complement] , i };
                }
                if (!dict.ContainsKey(nums[i]))
                {
                    dict.Add(nums[i], i);
                }
            }
            return null;
        }

        //best memory usage / O(n^2)
        //public static int[] TwoSum(int[] nums, int target)
        //{
        //    for (int i = 0; i < nums.Length; i++)
        //    {
        //        for (int j = i+1; j < nums.Length; j++)
        //        {
        //            if (nums[i] + nums[j] == target)
        //            {
        //                return new[] { i, j };
        //            }
        //        }
        //    }
        //    return null;
        //}
    }
}
