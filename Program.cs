﻿using static System.Console;

class BankAccountMngmtSystem
// main method and BankAccountMngmtSystem class
{
    static void Main()
    {
        ProcessUserIn processUserIn = new ProcessUserIn();
        WriteLine("Hello and welcome to your new Bloom Banking account!");
        processUserIn.PrintUserOptions();
        processUserIn.GetUserIn();
    }
}

class ProcessUserIn
{
    public void PrintUserOptions()
    // method used to show the user what commands are availible
    {
        // array storing the user commands
        string[] menuOptions = {
            "1. Deposit",
            "2. Withdraw",
            "3. View transaction history",
            "4. View account details",
            "5. Save history and quit"};

        WriteLine($"\nYour current Bloom bank account balance is {Actions.Balance.ToString("C")}\nHow may we assist you today?");
        // loops through and indents each command
        foreach (var option in menuOptions)
        {
            WriteLine("\t" + option);
        }
        Write(">> ");
    }

    public void GetUserIn()
    {
        Actions actions = new Actions();
        int userIn;
        //while userIn is not 6, the program will run
        do
        {
            string sUserIn = ReadLine();
            //if userIn is a whole number 1-5, the program will proceed
            if (int.TryParse(sUserIn, out userIn) && userIn >= 1 && userIn <= 5)
            {
                //each case will call the cooresponding method from Actions
                switch (userIn)
                {
                    case 1:
                        actions.Deposit();
                        break;
                    case 2:
                        // if the Balance is 0 then withdrawal will be blocked
                        if (Actions.Balance > 0.0)
                        {
                            actions.Withdraw();
                        }
                        else
                        {
                            WriteLine("\nYour balance must be more than $0.00 to make a withdrawal.");
                        }
                        break;
                    case 3:
                        // Account history is recorded throughout the program so the variable with the saved info only needs to be called
                        WriteLine("\n" + Actions.AccHistory);
                        break;
                    case 4:
                        WriteLine("\nYour account number is " + Actions.AccNum + ".");
                        WriteLine("Your routing number is " + Actions.RoutNum + ".");
                        break;
                    case 5:
                        // save and quit
                        actions.SaveAndQuit();
                        break;
                }
            }
            // error message for any input not 1-5
            else
            {
                WriteLine("Input must be a whole number 1-5.");
            }

            // this continues the program
            if (userIn != 5)
            {
                PrintUserOptions();
            }
        } while (userIn != 5);
    }
}

class Actions
{
    public static double Balance;
    public static string AccHistory;
    public static int AccNum;
    public static string RoutNum;
    DateTime now = DateTime.Now;

    public Actions()
    // constructor that instaniates the above variables
    {
        string accCreationTime = now.ToString("yyyy/MM/dd HH:mm:ss");
        Random rand = new Random();

        Balance = 0.0;
        // randomly generates the user's account and routing numbers each time the program is started
        AccNum = rand.Next(100000000, 1000000000);
        // 10 digits is too much for an int, so I had to combine two smaller ints into a string
        RoutNum = rand.Next(100000, 1000000).ToString() + rand.Next(1000, 10000).ToString();
        // AccHistory must be assigned last since its info relies on previous variables
        AccHistory = "Bloom Bank Account History:" +
                     $"\n\nTime - {accCreationTime}" +
                     $"\n\tBalance - {Balance.ToString("C")}" +
                     $"\n\tAction - None";
    }

    public void Deposit()
    // method for depositing money
    {
        // needed for consistant spacing in output
        WriteLine();
        try
        {
            // having variables creatated beforehand is nessasary so each while loop can use them
            string sDepositAmt;
            string sSenderRoutNum;
            string sSenderAccNum;
            double depositAmt;
            int senderRoutNum;
            // must be a long since int is too small for 10 digits
            long senderAccNum;

            // bool is implicitly used in conditionals such as while loops
            while (true)
            {
                Write("How much are you depositing? >> $");
                // the input is first stored as a string so it can be validated
                sDepositAmt = ReadLine();
                
                // deposit amount must be a non-negative number with no more then 2 decimal places
                if (!double.TryParse(sDepositAmt, out depositAmt))
                {
                    WriteLine("The deposit amount be a number.");
                    continue;
                }
                if (depositAmt <= 0)
                {
                    WriteLine("The deposit amount must be greater than zero.");
                    continue;
                }
                if (Math.Round(depositAmt, 2) != depositAmt)
                {
                    WriteLine("The deposit amount must have no more than two decimal places.");
                    continue;
                }
                break;
            }

            while (true)
            {
                Write("Please enter the nine digit routing number of the sender account. >> ");
                sSenderRoutNum = ReadLine();
                
                // sender routing number must be a non-negatve 9 digit int
                if (!int.TryParse(sSenderRoutNum, out senderRoutNum) || sSenderRoutNum.Length != 9 || sSenderRoutNum.Contains("-"))
                {
                    WriteLine("The routing number must be a non-negative nine digit whole number.");
                    continue;
                }
                break;
            }

            while (true)
            {
                Write("Please enter the ten digit account number of the sender account. >> ");
                sSenderAccNum = ReadLine();
                
                //must be a 10 digit long
                if (!long.TryParse(sSenderAccNum, out senderAccNum) || sSenderAccNum.Length != 10 || sSenderAccNum.Contains("-"))
                {
                    WriteLine("The routing number must be a non-negative number with ten digits.");
                    continue;
                }
                break;
            }

            // if the program gets to this point, then the transaction was correct
            WriteLine($"\nYour deposit of {depositAmt.ToString("C")} from sender account *****{senderAccNum % 10000} with routing number ******{senderRoutNum % 10000} is complete.");
            // update Balance
            Balance += depositAmt;
            // update AccHistory
            AccHistory = AccHistory + $"\nTime - {DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}" +
                                      $"\n\tBalance - {Balance.ToString("C")}" +
                                      $"\n\tAction - Deposit";
        }
        
        catch (Exception ex)
        {
            WriteLine("An error has occured:" + ex.Message);
        }
    }

    public void Withdraw()
    // method for withdrawing money
    {
        // needed for consistant spacing in output
        WriteLine();
        try
        {
            string sWithAmt;
            double withAmt;
            string sRoutNum;
            int routNum;
            string sAccNum;
            // must be a long since int is too small for 10 digits
            long accNum;

            while (true)
            {
                Write("How much are you withdrawing? >> $");
                // the input is first stored as a string so it can be validated
                sWithAmt = ReadLine();

                // deposit amount must be a non-negative double with no more then 2 decimal places
                if (!double.TryParse(sWithAmt, out withAmt))
                {
                    WriteLine("The withdrawal amount be a number.");
                    continue;
                }
                if (withAmt > Balance || withAmt < 0)
                {
                    WriteLine("The withdrawal amount must be greater than $0.00 and equal to or less than your current balance of " + Balance.ToString("C"));
                    continue;
                }
                if (Math.Round(withAmt, 2) != withAmt)
                {
                    WriteLine("The withdrawal amount must have no more than two decimal places.");
                    continue;
                }
                break;
            }

            while (true)
            {
                Write("Please enter the nine digit routing number of the recipient account. >> ");
                sRoutNum = ReadLine();

                // recipient routing number must be a non-negatve 9 digit int
                if (!int.TryParse(sRoutNum, out routNum) || sRoutNum.Length != 9 || sRoutNum.Contains("-"))
                {
                    WriteLine("The routing number must be a non-negative nine digit whole number.");
                    continue;
                }
                break;
            }

            while (true)
            {
                Write("Please enter the ten digit account number of the recipient account. >> ");
                sAccNum = ReadLine();

                //must be a 10 digit long
                if (!long.TryParse(sAccNum, out accNum) || sAccNum.Length != 10 || sAccNum.Contains("-"))
                {
                    WriteLine("The routing number must be a non-negative ten digit whole number.");
                    continue;
                }
                break;

            }

            // if the program gets to this point, then the transaction was correct
            WriteLine($"\nYour withdrawal of {withAmt.ToString("C")} to recipient account *****{accNum % 10000} with routing number ******{routNum % 10000} is complete.");
            // update Balance
            Balance -= withAmt;
            // update AccHistory
            AccHistory = AccHistory + $"\nTime - {DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}" +
                                      $"\n\tBalance - {Balance.ToString("C")}" +
                                      $"\n\tAction - Withdrawal";
        }
        catch (Exception ex)
        {
            WriteLine("An error has occured:" + ex.Message);
        }
    }
    public void SaveAndQuit()
    // save and quit used to be two seprate options, but I thought it would be more effiecient to combine them
    {
        // save account details to file
        AccHistory = $"Account number: {AccNum}." +
                             $"\nRouting number: {RoutNum}.\n\n" +
                             AccHistory;

        // the two strings make sure that the file is saved inside this program directory
        string projectRoot = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        string filePath = Path.Combine(projectRoot, "AccountHistory.txt");
        File.WriteAllText(filePath, AccHistory);

        WriteLine("\nYour transaction history was saved in the file named AccountHistory.txt" +
                  "\nWe look forward to your next visit. Have a wonderful day!");
    }
}