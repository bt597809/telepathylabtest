using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelepathyLabsTest
{
    class MathExpression : ICommand
    {
        public static MathExpression Instance { get; } = new MathExpression();

        private MathExpression()
        {
        }
        static nptr newNode(string c)
        {
            nptr n = new nptr();
            n.data = c;
            n.left = n.right = null;
            return n;
        }

        static nptr build(String s)
        {
            try
            {
                Stack<nptr> stN = new Stack<nptr>();

                Stack<char> stC = new Stack<char>();
                nptr t, t1, t2 = null;

                int[] p = new int[123];
                p['+'] = p['-'] = 1;
                p['/'] = p['*'] = 2;
                p[')'] = 0;

                string num = string.Empty;
                for (int i = 0; i < s.Length; i++)
                {
                    //if(char.IsDigit(s[i]))
                    //{
                    //    num += s[i].ToString();
                    //    if(i + 1 < s.Length && char.IsDigit(s[i + 1]))
                    //    {
                    //        continue;
                    //    }
                    //}


                    if (s[i] == '(')
                    {
                        stC.Push(s[i]);
                    }

                    else if (char.IsDigit(s[i]))
                    {
                        t = newNode(s[i].ToString());
                        stN.Push(t);
                        num = string.Empty;
                    }
                    else if (p[s[i]] > 0)
                    {

                        while (stC.Count != 0 && stC.Peek() != '('
                            && ((p[stC.Peek()] >= p[s[i]])
                                || (p[stC.Peek()] > p[s[i]])))
                        {

                            t = newNode(stC.Peek().ToString());
                            stC.Pop();

                            t1 = stN.Peek();
                            stN.Pop();

                            if(stN.Count > 0)
                            {
                                t2 = stN.Peek();
                                stN.Pop();
                            }
                            
                            t.left = t2;
                            t.right = t1;

                            stN.Push(t);
                        }

                        stC.Push(s[i]);
                    }
                    else if (s[i] == ')')
                    {
                        while (stC.Count != 0 && stC.Peek() != '(')
                        {
                            t = newNode(stC.Peek().ToString());
                            stC.Pop();
                            t1 = stN.Peek();
                            stN.Pop();
                            t2 = stN.Peek();
                            stN.Pop();
                            t.left = t2;
                            t.right = t1;
                            stN.Push(t);
                        }
                        stC.Pop();
                    }
                }
                t = stN.Peek();
                return t;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        static void postorder(nptr root)
        {
            if (root != null)
            {
                postorder(root.left);
                postorder(root.right);
                Console.Write(root.data);
            }
        }

        public void ProcessCommand(string command)
        {
            if (command == GlobalConstants.EXIT)
            {
                Environment.Exit(0);
            }
            else
            {
                var math = MathExpression.Instance;
                var inputs = command.Split(' ');
                var cmd = inputs[0];
                var expression = inputs.Length > 1 ? inputs[1] : "((5/(7-(1+1)))*3)-(2+(1+1))";
                switch (cmd)
                {
                    case "run":
                        math.run(expression);
                        break;
                    default:
                        Console.WriteLine("Invalid command");
                        break;
                }
            }
        }

        private void run(string expression)
        {
            expression = "(" + expression + ")";
            nptr root = build(expression);

            // Function call
            postorder(root);
            BTreePrinter.Print(root);
            Console.WriteLine(evalTree(root));
        }

        public int evalTree(nptr root)
        {

            // Empty tree
            if (root == null)
                return 0;

            // Leaf node i.e, an integer
            if (root.left == null && root.right == null)
                return Convert.ToInt32(root.data);

            // Evaluate left subtree
            int leftEval = evalTree(root.left);

            // Evaluate right subtree
            int rightEval = evalTree(root.right);

            // Check which operator to apply
            if (root.data == "+")
                return leftEval + rightEval;

            if (root.data == "-")
                return leftEval - rightEval;

            if (root.data == "*")
                return leftEval * rightEval;

            return leftEval / rightEval;
        }
    }

    public class nptr
    {
        public string data;
        public nptr left, right;
    };
}
