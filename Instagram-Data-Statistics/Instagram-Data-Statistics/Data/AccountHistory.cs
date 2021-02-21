using Instagram_Data_Statistics.DataFromJson;
using System;
using System.Linq;

namespace Instagram_Data_Statistics.Data
{
    public class AccountHistory : BaseJsonData<AccHistoryData>, IBaseData
    {
        public AccountHistory(string basePath) : base(basePath, "\\account_history.json")
        {

        }
        public void DisplayOptions()
        {
            while (true)
            {
                ConsoleHelper.WriteAndColorLine(Delimitator, ConsoleColor.Green);
                Console.WriteLine("\nWhat do you want to do next? " +
                    "\n1.Information about you since you first registered on Instagram " +
                    "\n2.How many times you logged in or logged out " +
                    "\nEsc.To exit");
                var action = Console.ReadKey(true).Key;
                switch (action)
                {
                    case ConsoleKey.D1:
                        Console.WriteLine("\n1.Your username was: {0}", Data.registration_info.registration_username);
                        var success = DateTime.TryParse(Data.registration_info.registration_time, out DateTime dateTime);
                        if (success)
                        {
                            Console.WriteLine("You registered your account on: {0}", dateTime.ToLongDateString());
                        }
                        Console.WriteLine("Your email was: {0}", Data.registration_info.registration_email);
                        Console.WriteLine("Your phone number was: {0}", Data.registration_info.registration_phone_number);
                        Console.WriteLine("Your device name was: {0}", Data.registration_info.device_name);
                        break;
                    case ConsoleKey.D2:
                        Console.WriteLine("\n2.You logged in {0} times and logged out {1} times.", Data.login_history.Count(), Data.logout_history.Count());
                        break;
                    case ConsoleKey.Escape:
                        Environment.Exit(0);
                        return;
                    default:
                        Console.Clear();
                        continue;
                }
                ConsoleHelper.WriteAndColorLine(Delimitator, ConsoleColor.Green);
                var respone = ConsoleHelper.GetChoice("\nDone, Choose what you wanna do next: \n1.I want more on account history \n2.Take me back to main menu \nEsc. Exit application",
                                    new ConsoleKey[] { ConsoleKey.D1, ConsoleKey.D2, ConsoleKey.Escape });
                if (respone == ConsoleKey.D1)
                    continue;
                else if (respone == ConsoleKey.D2)
                    return;
                else
                    Environment.Exit(0);
            }
        }

        public void OrganizeDataFromObject()
        {
            //for now, do nothing the data is good
        }
    }
}
