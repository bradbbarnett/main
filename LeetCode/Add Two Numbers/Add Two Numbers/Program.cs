using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Add_Two_Numbers
{
    public class ListNode
    {
        public int val;
        public ListNode next;
        public ListNode(int val = 0, ListNode next = null)
        {
            this.val = val;
            this.next = next;
        }
    }
    public class Program
    {
        static void Main()
        {
            Solution s = new Solution();

            ListNode l1 = new ListNode(2);
            l1 = new ListNode(4, l1);
            l1 = new ListNode(3, l1);

            ListNode l2 = new ListNode(5);
            l2 = new ListNode(6, l2);
            l2 = new ListNode(4, l2);

            ListNode result = new ListNode();
            result = s.AddTwoNumbers(l1, l2);
        }
    }

    public class Solution
    {
        ListNode list;
        public ListNode AddTwoNumbers(ListNode l1, ListNode l2)
        {
            int carry = 0;

            while (l1 != null || l2 != null)
            {
                if (l1 == null) l1 = new ListNode(0, l1);
                if (l2 == null) l2 = new ListNode(0, l2);

                list = new ListNode((l1.val + l2.val + carry) % 10, list);

                carry = (l1.val + l2.val + carry < 10) ? 0 : 1;

                l1 = l1.next;
                l2 = l2.next;
            }

            if (carry == 1) list = new ListNode(1, list);

            return ReverseListNode(list);
        }

        ListNode reversedList;
        public ListNode ReverseListNode(ListNode l1)
        {
            //base case
            if (l1 == null) return l1;

            //recursive case
            reversedList = new ListNode(l1.val, reversedList);
            ReverseListNode(l1.next);
            return reversedList;
        }
    }
}


//ListNode node;
//public ListNode DecreaseBy1()
//{
//    for (int i = 1; i < 5; i++)
//    {
//        node = new ListNode(i, node);
//    }

//    return node;
//}