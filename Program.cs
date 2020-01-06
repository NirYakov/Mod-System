using System;
using System.Collections.Generic;

namespace Mod_console_V2
{
    public class Program
    {
        public static void Main()
        {
            Func<int, int, int> func = (num1, num2) => num1 % num2;
            Action act = () => Console.Clear();
            Dictionary<string, Action> dict = new Dictionary<string, Action>(){
                    {"c" , act},
                    {"cls" , act},
                    {"clear" , act}
            };


            (new ScreenActiveConsole()
            {
                InitMessage =
@"  Num -Space- Num2 -Enter- => Result.
c || cls || clear , to clear
",
                NameOperation = "%",
                Format = "[ X % Y ] >> ",
                Operation = func,
                ServiceOpertion = dict,
                Logger = new Logger(),
                FormatResult = (n1, n2, res) => $" {n1} % {n2} = {res}",
            }).Run();

        }
    }


    public class ScreenActiveConsole
    {
        public ILogger Logger { get; set; }

        public string NameOperation { get; set; } = "N/a";

        public string Format { get; set; }

        public Func<int, int, int, string> FormatResult { get; set; }

        public string InitMessage { get; set; }

        public Func<int, int, int> Operation { get; set; } // Execution / Executie

        // private List<string> r_LogMemory;
        public Dictionary<string, Action> ServiceOpertion { get; set; } // may be need and interface 

        public virtual void Run()
        {
            bool quit = false;

            Logger.WriteMessage(InitMessage);
            // Logger.WriteMessage(Format, false);

            // Logger.ReadLine();

            while (!quit)
            {
                Logger.WriteMessage(Format, false);
                quit = CheckingInputAndExecuteAction(Logger.ReadLine().Split(' '));
                // inputs[0].ToLower()

                //   Logger.WriteMessage();

            }


            Console.WriteLine();
            Console.ReadLine();
        }

        protected virtual bool CheckingInputAndExecuteAction(string[] i_Input)
        {

            if (i_Input.Length == 1 && ServiceOpertion.ContainsKey(i_Input[0].ToLower()))
            {
                ServiceOpertion[i_Input[0]].Invoke();
            }
            else if (i_Input.Length > 1 && int.TryParse(i_Input[0], out int num1) && int.TryParse(i_Input[1], out int num2))
            {
                int result = Operation(num1, num2);
                Logger.WriteMessage(FormatResult(num1, num2, result));

            }
            else
            {
                // BM : TODO
            }

            return false;
        }
    }



    public class Logger : ILogger
    {
        private string newLineStr = Environment.NewLine;

        public string ReadLine()
        {
            return Console.ReadLine();
        }

        public void WriteMessage(string i_Msg, bool i_HaveNewLine)
        {
            if (i_HaveNewLine)
            {
                i_Msg = string.Format($"{i_Msg}{newLineStr}");
            }

            Console.Write(i_Msg);
        }

        public void Clear()
        {
            Console.Clear();
        }
    }

    public interface ILogger
    {
        void WriteMessage(string i_Msg, bool i_HaveNewLine = true);
        string ReadLine();
        void Clear();
    }
}
