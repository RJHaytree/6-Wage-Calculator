using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Softsite_Software_Solutions
{
    class Program
    {
        //Variables defined above main method due to them being across the entire program
        static private MySqlConnection connection;
        static private string server;
        static private string database;
        static private string uid;
        static private string password;
        static private double overtimeTotal;

        static void Main(string[] args)
        {
            //Set console title 
            Console.Title = "Wage Calculator";

            initialisation();

            //Initiating login on startup - ensures method is ran first to protect data and prevent bypass
            login();

            //Menu located in it's own menu so we can call it anywhere in the program
            main_menu();

            //Create method 
            create_payslip();

            //View a single or multiple payslips method
            view_payslip();

            //Edit a single payslip method
            edit_payslip();

            //delete and single or multiple payslips method
            delete_payslip();

            //Help method
            help();
        }
        static void initialisation()
        {
            //Declare database credentials.

            server = "localhost";
            database = "wage_calculator";
            uid = "root";
            password = "";

            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            //Create connection which can be called when data needs to be saved to the database.

            connection = new MySqlConnection(connectionString);
        }
        static void login()
        {
            int attempt_num = 0;

            string storedUsername1 = "Admin123";
            string storedUsername2 = "root";
            string storedPassword1 = "SecurePassword123.";
            string storedPassword2 = "";

            bool login = false;

            while (login == false && attempt_num < 4)
            {
                //Header
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("===================[(");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(" LOGIN ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(")]===================");

                //Username and password input with no regex to increase the possibility of someone getting it wrong
                //In other words, the input must be an exact match and regex decreases the range in which
                //a username can be entered wrong.

                Console.Write("\n\nENTER ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("USERNAME;\n");
                Console.ForegroundColor = ConsoleColor.Gray;
                string enteredUsername = Console.ReadLine();
                Console.Clear();

                //Console is cleared following the input of both so username and password is not displayed on the same
                //screen. Decreases the chance of an overlooker seeing the correct details being entered.

                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("===================[(");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(" LOGIN ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(")]===================");

                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("\n\nENTER ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("PASSWORD;\n");
                Console.ForegroundColor = ConsoleColor.Gray;
                string enteredPassword = Console.ReadLine();

                //Check whether input corresponds with stored *correct* details and perform successful login.
                if (enteredUsername == storedUsername1 && enteredPassword == storedPassword1 || enteredUsername == storedUsername2 && enteredPassword == storedPassword2)
                {
                    Console.Clear();

                    //Header
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("===================[(");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(" LOGIN ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(")]===================");

                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("\n\nLOGIN COMPLETED");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("\nWait 3 seconds for login to complete");
                    Thread.Sleep(3000);
                    //Break login loop so main menu method can be ran.
                    login = true;
                }

                //Check whether they have exceeded the maximum number of unsuccessful login attempts, which is 3. 
                //Checking whether they have attempted to login unsuccessfully 2 times is due to the counter starting at 0 rather than 1.
                else if (attempt_num == 2)
                {
                    Console.Clear();

                    //Header

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("===================[(");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(" LOGIN ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(")]===================");

                    //Close when reaching 3 unsuccessful login attempts.
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n\nYOU HAVE EXCEEDED YOUR MAX ATTEMPTS");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine("\nApplication closing...");
                    Thread.Sleep(3000);
                    Environment.Exit(0);
                }

                else
                {
                    //Display unsuccessul login attempt message and increase counter to be used later.
                    Console.Clear();

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("===================[(");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(" LOGIN ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(")]===================");

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n\nLOGIN UNSUCCESSFUL");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine("\nTry again...");
                    Thread.Sleep(2000);
                    attempt_num++; //Adds +1 to login attempt counter.
                    Console.Clear();
                }
            }
        }
        static void main_menu()
        {
            bool main = true;

            while (main == true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("===================[(");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(" MENU ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(")]===================");

                Console.WriteLine("\n\nChoose an option;");

                //Create Shortcut.
                Console.Write("\n [");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("1");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("] Create Payslip");

                //View Shortcut.
                Console.Write("\n [");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("2");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("] View Payslip");

                //Edit Shortcut.
                Console.Write("\n [");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("3");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("] Edit Payslip");

                //Delete Shortcut.
                Console.Write("\n [");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("4");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("] Delete Payslip");

                //Help Shortcut.
                Console.Write("\n [");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("5");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("] Help");

                //Exit Shortcut for exiting the program.
                Console.Write("\n [");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("6");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("] Exit\n\n");

                Console.ForegroundColor = ConsoleColor.Gray;

                //Use int.TryParse incase an interger isn't entered by the user, preventing the program from erroring and eventually crashing.

                int choice;
                bool parsed = int.TryParse(Console.ReadLine(), out choice);
                if (choice.Equals(1))
                {
                    create_payslip();
                }

                else if (choice.Equals(2))
                {
                    view_payslip();
                }

                else if (choice.Equals(3))
                {
                    edit_payslip();
                }

                else if (choice.Equals(4))
                {
                    delete_payslip();
                }

                else if (choice.Equals(5))
                {
                    help();
                }

                else if (choice.Equals(6))
                {
                    exit();
                }

                else
                {
                    //Display error message if input is not valid, and then re-displaying the menu after 2 seconds.
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("===================[(");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(" MENU ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(")]===================");

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n\nERROR: Enter the number attatched\nto the action you want");
                    Console.ForegroundColor = ConsoleColor.White;
                    Thread.Sleep(2000);
                }
            }
        }
        static void create_payslip()
        {
            bool create = true;
            //Regex rule which declares that only uppercase letters, lowercase letters, spaces and hyphens can be used 
            //where this is checked.
            Regex regex = new Regex(@"^[a-zA-Z -]*$");

            while (create.Equals(true))
            {

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("==================[(");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(" CREATE ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(")]==================");

                Console.WriteLine("\n\nChoose an option;");

                Console.Write("\n [");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("1");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("] New payslip");

                Console.Write("\n [");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("2");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("] Main menu\n\n");

                int choice;
                //TryParsed used to ensure any invalid entry does not stall the program when attempting to to parsed.
                //Boolean is declared false or true depending on outcome, but in this instance does not need to be check since
                //an *else* statement is already in use.
                bool parsed = int.TryParse(Console.ReadLine(), out choice);

                if (choice.Equals(1))
                {
                    //Variables declared here so calc functions can get values when 
                    //they're inside other if statements.
                    string forename = "";
                    string surname = "";
                    string title = "";

                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("==================[(");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(" CREATE ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(")]==================");

                    //Collect forename in a loop to make question re-appear until a valid entry is saved.
                    while (true)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\n\nENTER EMPLOYEE FORENAME;\n");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        forename = Console.ReadLine();

                        //Check whether input is null or empty before regex is checked due to regex supporting
                        //spaces due to double-barrel names.
                        if (string.IsNullOrEmpty(forename))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("ERROR: Characters and spaces only");
                        }

                        //Check whether input follows regex and break loop if valid.
                        else if (regex.IsMatch(forename))
                        {
                            //Break loop to move on to the next section
                            break;
                        }

                        //Regex is not being followed, input invalid.
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\nERROR: Characters and spaces only");
                        }
                    }

                    //Collect surname within a loop to ensure a valid input is entered before moving on to 
                    //collecting the NI number.
                    while (true)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\nENTER EMPLOYEE SURNAME;\n");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        surname = Console.ReadLine();

                        //Check whether string is empty (pressed enter without inputting appropriate data.
                        if (string.IsNullOrEmpty(surname))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("ERROR: Characters and spaces only");
                        }

                        //Check whether input follows regex and break loop if invalid.
                        else if (regex.IsMatch(surname))
                        {
                            //Break loop to move on to the next section
                            break;
                        }

                        //Regex is not being followed, input invalid.
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\nERROR: Characters and spaces only");
                        }
                    }

                    //Collect title within a loop to ensure a valid entry is saved before moving on.
                    while (true)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\nENTER EMPLOYEE TITLE\n");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        title = Console.ReadLine();
                        title = title.ToUpper();

                        //Check input, and break loop if valid.
                        if (title.Equals("MR") || title.Equals("MISS") || title.Equals("DR") || title.Equals("MS") || title.Equals("MRS"))
                        {
                            //Break loop to move on to the next section.
                            break;
                        }

                        //Invalid is invalid.
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\nERROR: Choose 'Mr, Miss, Dr, Ms or Mrs'");
                        }
                    }

                    //Collect national insurance number in a loop to make question continue until a valid entry is entered.

                    string NI = "";
                    int maxLength = 9;

                    //Regex rule decalred which only supports uppercase letters and numbers.

                    Regex regex2 = new Regex(@"^[A-Z0-9]*$");

                    while (true)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\nENTER EMPLOYEE NI (NO SPACES);\n");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        NI = Console.ReadLine();

                        //Checking whether entry is null or empty before regex.
                        if (string.IsNullOrEmpty(NI))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\nERROR: Uppercase characters and numbers only");
                        }

                        //Check whether national insurance number is the correct length
                        else if (NI.Length != maxLength)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\nERROR: National insurance numbers can only be 9 characters long");
                        }

                        //Checking whether input matches regex, and then breaking the loop to move onto the next section.
                        else if (regex2.IsMatch(NI))
                        {
                            //Break loop
                            break;
                        }

                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\nERROR: Uppercase characters and numbers only");
                        }
                    }

                    //Collect hours worked within a loop incase of invalid entry

                    double hours = 0;
                    while (true)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\nENTER EMPLOYEE HOURS WORKED;\n");
                        Console.ForegroundColor = ConsoleColor.Gray;

                        //TryParse used so an invalid entry which cannot be parsed to an int doesn't stall the program
                        //in the processes. Returns true or false depending on successful, which is then checked.
                        bool parsed2 = double.TryParse(Console.ReadLine(), out hours);

                        //Input is valid
                        if (parsed2.Equals(true))
                        {
                            break; //Break loop so we can move on to rate
                        }

                        //Input is invalid
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("ERROR: Enter the amount of hours worked");
                            Console.ForegroundColor = ConsoleColor.White;
                        }

                    }

                    //Rate Options

                    bool rateCollect = true;
                    double rate = 0;
                    while (rateCollect == true)
                    {
                        //Collect Rate from a list per specification.

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\nENTER EMPLOYEE RATE;");

                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("\n [");
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.Write("1");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("] Programmers: £12.03/h");

                        Console.Write("\n [");
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.Write("2");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("] Administrators/Clerks: £8.07/h");

                        Console.Write("\n [");
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.Write("3");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("] Project Leaders/Manager: £22.54/h");

                        Console.Write("\n [");
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.Write("4");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("] Miscellaneous: Enter Manually\n\n");

                        int rateChoice;

                        //TryParse used to ensure input can be parsed, and does not stall the program is it cannot.
                        //Boolean is not checked due to else statement already being used if choice does not match.

                        bool parsed3 = int.TryParse(Console.ReadLine(), out rateChoice);

                        //Programmer rate.
                        if (rateChoice.Equals(1))
                        {
                            rate = 12.03;
                            break; //Break loop.
                        }

                        //Admin/clerk rate.
                        else if (rateChoice.Equals(2))
                        {
                            rate = 8.07;
                            break; //Break loop.
                        }

                        //Manager/leader rate
                        else if (rateChoice.Equals(3))
                        {
                            rate = 22.54;
                            break; //break loop.
                        }

                        //Manual input.

                        else if (rateChoice.Equals(4))
                        {
                            bool rateManual = true;

                            while (rateManual == true)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("\nENTER EMPLOYEE RATE;\n");
                                Console.ForegroundColor = ConsoleColor.Gray;

                                //TryParse used and boolean checked due to no other else statement being used here.

                                bool parsed5 = double.TryParse(Console.ReadLine(), out rate);

                                if (parsed5.Equals(true))
                                {
                                    rateManual = false;
                                    rateCollect = false;

                                    //break all loops so section can move on.
                                }

                                else
                                {
                                    //Error if input cannot be parsed.

                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("\nERROR: Enter a number");
                                }
                            }
                        }

                        else
                        {
                            //Error if input cannot be parsed, and does not match one of those checked.
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\n\nERROR: Enter the rate you want");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                    }

                    //Calculate Gross using function.
                    double gross = grosscalc(hours, rate);

                    //Calculate NIcharge (Gross * 0.11) in its own function.
                    double NIcharge = NItax(gross);

                    //Calculate Net in it's own function.
                    double tax = taxCalc(gross);
                    double net;

                    if (gross > tax)
                    {
                        net = netcalc(gross, tax, NIcharge);
                    }

                    else
                    {
                        net = gross - NIcharge; //Since no tax has been taken. value remains same.
                    }

                    string query = "INSERT INTO payslip VALUES(NULL, '" + forename + "','" + surname + "','" + title + "','" + NI + "','" + hours + "','" + rate + "','" + gross + "','" + net + "','" + NIcharge + "','" + overtimeTotal + "','" + tax + "', NOW())";

                    while (true)
                    {
                        //Display formatted wageslip before giving the user to ability to save it to the database.
                        //Allows the user to check whether the payslip is correct.

                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("==================[(");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(" PAYSLIP ");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(")]==================");

                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("\n\n NAME: ");
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write(forename);
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(", ");
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write(surname);

                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("\n TITLE: ");
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write(title);

                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("\n NI: ");
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write(NI);

                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("\n\n HOURS: ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(hours);

                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("              RATE: ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("{0:C}", rate);

                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("\n OVERTIME:");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("{0:C}", overtimeTotal );

                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("\n GROSS: ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("{0:C}", gross);

                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("\n TAX: ");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("{0:C}", tax);

                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("\n NI TAX: ");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("{0:C}", NIcharge);

                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("\n NET: ");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("{0:C}", net);

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\n\nWOULD YOU LIKE TO SAVE THIS PAYSLIP?");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("\n [");
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.Write("1");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("] Yes");

                        Console.Write("\n [");
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.Write("2");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("] No\n\n");

                        //Use TryParse to catch any input from user which cannot be parsed to an int. 
                        //Will display error message instead of stalling/crashing the program.

                        int saveChoice;
                        bool parsed4 = int.TryParse(Console.ReadLine(), out saveChoice);

                        //Selected to save to database.

                        if (saveChoice.Equals(1))
                        {
                            try
                            {
                                //Attempt to send query to database in a try loop. Ensures any exception can be caught
                                //and have an error message displayed.

                                connection.Open();
                                MySqlCommand cmd = new MySqlCommand(query, connection);
                                cmd.ExecuteNonQuery();
                                Console.ForegroundColor = ConsoleColor.DarkGreen;
                                Console.WriteLine("\n ---> RECORD ADDED");
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                Console.WriteLine("\nPRESS ENTER TO CONTINUE...");
                                Console.ForegroundColor = ConsoleColor.White;
                                connection.Close();
                                Console.ReadLine();
                                break;
                            }

                            catch
                            {
                                //Error occured when inputting the database. Will close loop and send back to create
                                //wageslip menu.

                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("\nERROR: An exception occured when inputting to database. Report to a programmer");
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                Console.WriteLine("\nPRESS ENTER TO CONTINUE...");
                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            }
                        }

                        //Does not wish to save to database.

                        else if (saveChoice.Equals(2))
                        {
                            break;
                        }

                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\nERROR: Enter a number");
                        }
                    }
                }

                //Redirect Back to the menu.

                else if (choice.Equals(2))
                {
                    main_menu();
                }

                //Invalid Input message.

                else
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("==================[(");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(" CREATE ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(")]==================");

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n\nERROR: Enter the number attatched\nto the action you want");
                    Console.ForegroundColor = ConsoleColor.White;
                    Thread.Sleep(2000);
                }
            }
        }
        static void view_payslip()
        {
            //Placed in loop incase of inavlid input
            while (true)
            {
                //Menu within view method to allow for multiple viewing options
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("===================[(");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(" VIEW ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(")]===================");

                Console.WriteLine("\n\nChoose an option;");

                Console.Write("\n [");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("1");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("] View single record");

                Console.Write("\n [");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("2");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("] View all records");

                Console.Write("\n [");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("3");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("] Main Menu\n\n");
                Console.ForegroundColor = ConsoleColor.Gray;

                int choice;
                //TryParsed used to ensure output can be parsed and does not stall program if cannot.
                bool parsed = int.TryParse(Console.ReadLine(), out choice);

                if (choice.Equals(1))
                { 
                    while (true)
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("===================[(");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(" VIEW ");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(")]===================");

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\n\nENTER ID OF RECORD: \n");
                        Console.ForegroundColor = ConsoleColor.Gray;

                        int ID = 0;

                        //TryParsed used to ensure output can be parsed and does not stall program if cannot.
                        //Bool checked to ensure it has parsed for use in query, along with an else statement 
                        //if input cannot be parsed.

                        bool parsed3 = int.TryParse(Console.ReadLine(), out ID);

                        if (parsed3 == true)
                        {
                            //Try and catch used to ensure data has been successfully input to database, and give an error 
                            //data has not.

                            try
                            {
                                string query = "SELECT * FROM payslip WHERE ID ='" + ID + "'";
                                Console.WriteLine();
                                connection.Open();
                                MySqlCommand cmd = new MySqlCommand(query, connection);
                                MySqlDataReader dataReader = cmd.ExecuteReader();

                                while (dataReader.Read())
                                {
                                    Console.Clear();
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.Write("===================[(");
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.Write(" VIEW ");
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.Write(")]===================");

                                    Console.Write("\n\n ID: ");
                                    Console.ForegroundColor = ConsoleColor.Cyan;
                                    Console.Write(dataReader["ID"]);

                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.Write("\n NAME: ");
                                    Console.ForegroundColor = ConsoleColor.Cyan;
                                    Console.Write(dataReader["Forename"]);
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.Write(", ");
                                    Console.ForegroundColor = ConsoleColor.Cyan;
                                    Console.Write(dataReader["Surname"]);

                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.Write("\n TITLE: ");
                                    Console.ForegroundColor = ConsoleColor.Cyan;
                                    Console.Write(dataReader["Title"]);

                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.Write("\n NI: ");
                                    Console.ForegroundColor = ConsoleColor.Cyan;
                                    Console.Write(dataReader["NI"]);

                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.Write("\n\n HOURS: ");
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.Write(dataReader["Hours"]);

                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.Write("              RATE: ");
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.Write("{0:C} ", dataReader["Rate"]);

                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.Write("\n OVERTIME:");
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.Write("{0:C} ", dataReader["Overtime"]);

                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.Write("\n GROSS: ");
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.Write("{0:C}", dataReader["Gross"]);

                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.Write("\n TAX: ");
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.Write("{0:C}", dataReader["Tax"]);

                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.Write("\n NI TAX: ");
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.Write("{0:C} ", dataReader["NI_Tax"]);

                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.Write("\n NET: ");
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.Write("{0:C}", dataReader["Net"]);

                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.Write("\n DATE: £");
                                    Console.ForegroundColor = ConsoleColor.Gray;
                                    Console.Write(String.Format("{0:dd/MM/yyyy}", dataReader["Date"]));

                                    //Set footer for payslip so it remains constant with the view-all task.

                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.Write("\n\n================================================");
                                }
                                dataReader.Close();
                                connection.Close();
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                Console.WriteLine("\n\nPRESS ENTER TO CONTINUE...");
                                Console.ReadLine();
                                //Break loop so it client goes back to the view-payslip menu
                                break;
                            }

                            catch
                            {
                                //This was added to give an error if an issue arises with the sql database, 
                                //Rather than simply stalling the program

                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("ERROR: An exception occured when inputting to database. Report to a programmer");
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                Console.WriteLine("\nPRESS ENTER TO CONTINUE...");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.ReadLine();
                                break;
                            }
                        }

                        else
                        {
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write("===================[(");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(" VIEW ");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write(")]===================");

                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\n\nERROR: Enter the number attatched\nto the action you want");
                            Console.ForegroundColor = ConsoleColor.White;
                            Thread.Sleep(2000);
                        }
                    }
                }

                else if (choice.Equals(2))
                {
                    bool viewAll = true;

                    while (viewAll.Equals(true))
                    {
                        //define regex to allow alphabet characters(uppercase and lowercase)
                        //alongwith spaces and hyphens

                        bool surnameCollect = true;

                        while (surnameCollect.Equals(true))
                        {
                            Regex regex = new Regex(@"^[a-zA-Z -]*$");

                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write("===================[(");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(" VIEW ");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write(")]===================");

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("\n\nENTER SURNAME OR PART OF SURNAME;\n");
                            Console.ForegroundColor = ConsoleColor.Gray;

                            string surname = Console.ReadLine();

                            if (string.IsNullOrEmpty(surname))
                            {
                                //Checks whether anything was entered as a surname
                                //and if surname is null, program will loop back to 
                                //collect a surname

                                Console.Clear();
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write("===================[(");
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write(" VIEW ");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write(")]===================");

                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("\n\nERROR: Surname can only have letters, hyphens and spaces");
                                Console.ForegroundColor = ConsoleColor.White;
                                Thread.Sleep(2000);
                            }

                            else if (regex.IsMatch(surname))
                            {
                                try
                                {
                                    string query = "SELECT * FROM payslip WHERE surname LIKE '" + surname + "%'";
                                    Console.WriteLine();
                                    connection.Open();
                                    MySqlCommand cmd = new MySqlCommand(query, connection);
                                    MySqlDataReader dataReader = cmd.ExecuteReader();

                                    //Set header outside of while loop to ensure it isn't displayed
                                    //Multiple times

                                    Console.Clear();
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.Write("===================[(");
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.Write(" VIEW ");
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.Write(")]===================");

                                    //Set payslip number to help define the end and start of a paylsip

                                    int payslipNum = 1;

                                    while (dataReader.Read())
                                    {
                                        //This is identical to the 'view single payslip' except
                                        //a payslip number has been introduced to help divide them
                                        //on screen

                                        Console.Write("\n\n PAYSLIP NUM: ");
                                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                                        Console.Write(payslipNum);

                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.Write("\n\n ID: ");
                                        Console.ForegroundColor = ConsoleColor.Cyan;
                                        Console.Write(dataReader["ID"]);

                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.Write("\n NAME: ");
                                        Console.ForegroundColor = ConsoleColor.Cyan;
                                        Console.Write(dataReader["Forename"]);
                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.Write(", ");
                                        Console.ForegroundColor = ConsoleColor.Cyan;
                                        Console.Write(dataReader["Surname"]);

                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.Write("\n TITLE: ");
                                        Console.ForegroundColor = ConsoleColor.Cyan;
                                        Console.Write(dataReader["Title"]);

                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.Write("\n NI: ");
                                        Console.ForegroundColor = ConsoleColor.Cyan;
                                        Console.Write(dataReader["NI"]);

                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.Write("\n\n HOURS: ");
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        Console.Write(dataReader["Hours"]);

                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.Write("         RATE: ");
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        Console.Write("{0:C}", dataReader["Rate"]);

                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.Write("\n OVERTIME: ");
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        Console.Write("{0:C}", dataReader["Overtime"]);

                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.Write("\n GROSS: ");
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        Console.Write("{0:C}", dataReader["Gross"]);

                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.Write("\n TAX: ");
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.Write("{0:C}", dataReader["Tax"]);

                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.Write("\n NI TAX: ");
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.Write("{0:C}", dataReader["NI_Tax"]);

                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.Write("\n NET: ");
                                        Console.ForegroundColor = ConsoleColor.Yellow;
                                        Console.Write("{0:C}", dataReader["Net"]);

                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.Write("\n DATE: ");
                                        Console.ForegroundColor = ConsoleColor.Gray;
                                        Console.Write(String.Format("{0:dd/MM/yyyy}", dataReader["Date"]));

                                        //Set bottom of payslip to ensure payslips are easily dividible

                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.Write("\n\n================================================");
                                        payslipNum++;
                                    }
                                    dataReader.Close();
                                    connection.Close();
                                    Console.ForegroundColor = ConsoleColor.Magenta;
                                    Console.WriteLine("\n\nPRESS ENTER TO CONTINUE...");
                                    Console.ReadLine();

                                    //Break loop and take client back to view menu

                                    surnameCollect = false;
                                    viewAll = false;
                                }

                                catch
                                {
                                    //This was added to give an error if an issue arises with the sql database, 
                                    //Rather than simply stalling the program

                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("ERROR: An exception occured when inputting to database. Report to a programmer");
                                    Console.ForegroundColor = ConsoleColor.Magenta;
                                    Console.WriteLine("\nPRESS ENTER TO CONTINUE...");
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.ReadLine();

                                    //Break loop and take client back to view menu

                                    surnameCollect = false;
                                    viewAll = false;
                                }
                            }

                            else
                            {
                                //Will state whether regex conditions were not met,
                                //and run the client back to the surname input after 
                                //2 seconds

                                Console.Clear();
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write("===================[(");
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write(" VIEW ");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write(")]===================");

                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("\n\nERROR: Surname can only have letters, hyphens and spaces");
                                Console.ForegroundColor = ConsoleColor.White;
                                Thread.Sleep(2000);
                            }
                        }
                    }
                }

                else if (choice.Equals(3))
                {
                    //Send user back to main menu
                    main_menu();
                }

                else
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("===================[(");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(" VIEW ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(")]===================");

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n\nERROR: Enter the number attatched\nto the action you want");
                    Console.ForegroundColor = ConsoleColor.White;
                    Thread.Sleep(2000);
                }
            }
        }
        static void edit_payslip()
        {
            bool edit = true;

            while (edit.Equals(true))
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("===================[(");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(" EDIT ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(")]===================");

                Console.WriteLine("\n\nChoose an option;");

                Console.Write("\n [");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("1");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("] Edit payslip");

                Console.Write("\n [");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("2");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("] Main menu\n\n");
                Console.ForegroundColor = ConsoleColor.Gray;

                int choice;
                bool parsed = int.TryParse(Console.ReadLine(), out choice);

                if (choice.Equals(1))
                {
                    bool editTask = true;

                    while (editTask == true)
                    {
                        int ID = 0;
                        string forename = "";
                        string surname = "";
                        string title = "";
                        Regex regex = new Regex(@"^[a-zA-Z -]*$");

                        //Collect ID for WHERE statement to ensure question is asked until a valid input is received.

                        while (true)
                        {
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write("===================[(");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(" EDIT ");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write(")]===================");

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("\n\nENTER ID OF RECORD TO EDIT;\n");
                            Console.ForegroundColor = ConsoleColor.Gray;

                            //Checking whether string is properly parsed to an INT using TryParse,
                            //setting the boolean to true if successful and storing the INT in the
                            //ID variable, and false if unsuccessful.

                            bool parsed2 = int.TryParse(Console.ReadLine(), out ID);

                            if (parsed2.Equals(true))
                            {
                                //Loop broken due to successful parsing
                                break;
                            }

                            else
                            {
                                //Display error if unsuccessful
                                Console.Clear();
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write("===================[(");
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write(" EDIT ");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write(")]===================");

                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("\n\nERROR: Integers only");
                                Thread.Sleep(2000);
                            }
                        }

                        //Collect new forename whilst ensuring it abides by the regex rule
                        //stated above by looping it in a loop and breaking the loop ONLY when
                        //a valid entry is accepted

                        while (true)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("\nENTER NEW EMPLOYEE FORENAME;\n");
                            Console.ForegroundColor = ConsoleColor.Gray;
                            forename = Console.ReadLine();

                            //Check null entries before regex due to the regex rule accepted them.

                            if (string.IsNullOrEmpty(forename))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("\nERROR: There must be something saved for forename");
                            }

                            //Check regex rule and break loop if accepted.

                            else if (regex.IsMatch(forename))
                            {
                                //Loop broken due to successful entry
                                break;
                            }

                            //Regex is not being followed, and the loop continues.

                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("\nERROR: Characters and spaces only");
                            }
                        }

                        //Collect new forename whilst ensuring it abides by the regex rule
                        //stated above by looping it in a loop and breaking the loop ONLY when
                        //a valid entry is accepted.

                        while (true)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("\nENTER NEW EMPLOYEE SURNAME;\n");
                            Console.ForegroundColor = ConsoleColor.Gray;
                            surname = Console.ReadLine();

                            //Check for null entries before applying regex due to the rule accepting then.

                            if (string.IsNullOrEmpty(surname))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("\nERROR: There must be something saved for surname");
                            }

                            //Check input against regex rule.

                            else if (regex.IsMatch(surname))
                            {
                                //Break loop due to regex matching.
                                break;
                            }

                            //Input is invalid.

                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("\nERROR: Characters and spaces only");
                            }
                        }

                        //Collect new title and ensuring it's a valid entry.

                        while (true)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("\nENTER NEW EMPLOYEE TITLE\n");
                            Console.ForegroundColor = ConsoleColor.Gray;
                            title = Console.ReadLine();
                            title = title.ToUpper();

                            //Check input, and break loop if valid.

                            if (title.Equals("MR") || title.Equals("MISS") || title.Equals("DR") || title.Equals("MS") || title.Equals("MRS"))
                            {
                                break;
                            }

                            //Invalid is invalid.

                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("\nERROR: Choose 'Mr, Miss, Dr, Ms or Mrs'");
                            }
                        }

                        //Collect National Insurnace number whilst ensuring some validation occurs.

                        string NI = "";
                        int maxLength = 9;
                        Regex regex2 = new Regex(@"^[A-Z0-9]*$"); //Rule only allows for letters and numbers.

                        while (true)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("\nENTER NEW EMPLOYEE NI (NO SPACES);\n");
                            Console.ForegroundColor = ConsoleColor.Gray;
                            NI = Console.ReadLine();

                            //Check whether the input was null or empty (most likely due to pressing ENTER too quickly).

                            if (string.IsNullOrEmpty(NI))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("\nERROR: Uppercase characters and numbers only");
                            }

                            //Check whether the input is greater than the max length of NI numbers.

                            else if (NI.Length != maxLength)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("\nERROR: National insurance numbers can only be 9 characters long");
                            }

                            //Check whether input only allows for numbers and uppercase letters.

                            else if (regex2.IsMatch(NI))
                            {
                                break;
                            }

                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("\nERROR: Uppercase characters and numbers only");
                            }
                        }

                        //Collect hours worked within a loop incase of invalid entry.

                        double hours = 0;

                        while (true)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("\nENTER EMPLOYEE HOURS WORKED;\n");
                            Console.ForegroundColor = ConsoleColor.Gray;

                            bool parsed2 = double.TryParse(Console.ReadLine(), out hours);

                            //Input is valid.

                            if (parsed2.Equals(true))
                            {
                                break; //Break loop so the process can move on to rate
                            }

                            //Input is invalid.

                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("\nERROR: Enter the amount of hours worked");
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                        }

                        //Rate Options.

                        bool rateCollect = true;
                        double rate = 0;
                        while (rateCollect == true)
                        {
                            //Collect Rate.

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("\nENTER EMPLOYEE RATE;");

                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write("\n [");
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.Write("1");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write("] Programmers: £12.03/h");

                            Console.Write("\n [");
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.Write("2");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write("] Administrators/Clerks: £8.07/h");

                            Console.Write("\n [");
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.Write("3");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write("] Project Leaders/Manager: £22.54/h");

                            Console.Write("\n [");
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.Write("4");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write("] Miscellaneous: Enter Manually\n\n");

                            int rateChoice;
                            bool parsed3 = int.TryParse(Console.ReadLine(), out rateChoice);

                            //Programmer rate.

                            if (rateChoice.Equals(1))
                            {
                                rate = 12.03;
                                rateCollect = false;
                            }

                            //Admin/clerk rate.

                            else if (rateChoice.Equals(2))
                            {
                                rate = 8.07;
                                rateCollect = false;
                            }

                            //Manager/leader rate.

                            else if (rateChoice.Equals(3))
                            {
                                rate = 22.54;
                                rateCollect = false;
                            }

                            //Manual input.

                            else if (rateChoice.Equals(4))
                            {
                                bool rateMisc = true;

                                while (rateMisc.Equals(true))
                                {
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine("\nENTER EMPLOYEE RATE;\n");
                                    Console.ForegroundColor = ConsoleColor.Gray;

                                    bool parsed5 = double.TryParse(Console.ReadLine(), out rate);

                                    if (parsed5.Equals(true))
                                    {
                                        rateMisc = false;
                                        rateCollect = false;
                                    }

                                    else
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("\nERROR: Enter a number");
                                    }
                                }
                            }

                            //Invalid input.

                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("\n\nERROR: Enter the number attatched\nto the action you want");
                                Console.ForegroundColor = ConsoleColor.White;
                                Thread.Sleep(2000);
                            }
                        }

                        //Calculate Gross.

                        double gross = grosscalc(hours, rate);

                        //Calculate NIcharge (Gross * 0.11) in its own function.

                        double NIcharge = NItax(gross);

                        //Calculate Net in it's own function.

                        double tax = taxCalc(gross);
                        double net;

                        if (gross > tax)
                        {
                            net = netcalc(gross, tax, NIcharge);
                        }

                        else
                        {
                            net = gross - NIcharge; //Since no tax has been taken. value remains same.
                        }

                        string query = "UPDATE payslip SET Forename='" + forename + "', Surname='" + surname + "', Title='" + title + "', NI='" + NI + "', Hours='" + hours + "', Rate='" + rate + "', Gross='" + gross + "', Net='" + net + "', NI_Tax='" + NIcharge + "', Overtime='" + overtimeTotal + "', Tax='" + tax + "', Date=NOW() WHERE ID='" + ID + "'";

                        try
                        {
                            //Attempt to send query to database in a try statement to ensure an error message is displayed if unsuccessful.

                            connection.Open();
                            MySqlCommand cmd = new MySqlCommand();
                            cmd.CommandText = query;
                            cmd.Connection = connection;
                            cmd.ExecuteNonQuery();
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.WriteLine("\n ---> RECORD WITH ID {0} UPDATED", ID);
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.WriteLine("\nPRESS ENTER TO CONTINUE...");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.ReadLine();
                            connection.Close();
                            break;
                        }

                        catch
                        {
                            //This was added to give an error if an issue arises with the sql database, 
                            //Rather than simply stalling the program.

                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\nERROR: An exception occured when inputting to database. Report to a programmer");
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.WriteLine("\nPRESS ENTER TO CONTINUE...");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.ReadLine();

                            //Break loop back to edit menu.

                            break;
                        }
                    }
                }

                else if (choice.Equals(2))
                {
                    //Back to main menu.

                    main_menu();
                }

                else
                {
                    //Display error if input cannot be parsed or inputted number does not match one in the menu.

                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("===================[(");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(" EDIT ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(")]===================");

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n\nERROR: Enter the number attatched\nto the action you want");
                    Console.ForegroundColor = ConsoleColor.White;
                    Thread.Sleep(2000);
                }
            }
        }
        static void delete_payslip()
        {
            bool delete = true;

            while (delete.Equals(true))
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("==================[(");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(" DELETE ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(")]==================");

                Console.WriteLine("\n\nChoose an option;");

                Console.Write("\n [");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("1");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("] Delete a payslip");

                Console.Write("\n [");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("2");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("] Delete all payslips");

                Console.Write("\n [");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("3");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("] Main menu\n\n");

                int choice;
                bool parsed = int.TryParse(Console.ReadLine(), out choice);

                if (parsed == true)
                {
                    if (choice.Equals(1))
                    {
                        //Create a loop to ensure question is asked until a valid entry is saved, and delete task is 
                        //executed correctly.

                        while (true)
                        {
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write("==================[(");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(" DELETE ");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write(")]==================");

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("\n\nENTER ID OF PAYSLIP TO DELETE;\n");
                            Console.ForegroundColor = ConsoleColor.Gray;

                            int ID;

                            //Using TryParse allows us to check whether the input was successfully parsed *without*
                            //having the program stall. Returns a true or false which we can then check.

                            bool parsed2 = int.TryParse(Console.ReadLine(), out ID);

                            if (parsed2 == true)
                            {
                                string query = "DELETE FROM payslip WHERE ID='" + ID + "'";

                                try
                                {
                                    connection.Open();
                                    MySqlCommand cmd = new MySqlCommand(query, connection);
                                    cmd.ExecuteNonQuery();
                                    connection.Close();
                                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                                    Console.WriteLine("\n\n ---> PLAYSLIP WITH ID {0} DELETED", ID);
                                    Console.ForegroundColor = ConsoleColor.Magenta;
                                    Console.WriteLine("\nPRESS ENTER TO CONTINUE...");
                                    Console.ReadLine();
                                    break;
                                }

                                catch
                                {
                                    //This was added to give an error if an issue arises with the sql database, 
                                    //Rather than simply stalling the program.

                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("\nERROR: An exception occured when inputting to database. Report to a programmer");
                                    Console.ForegroundColor = ConsoleColor.Magenta;
                                    Console.WriteLine("\nPRESS ENTER TO CONTINUE...");
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.ReadLine();
                                    break;
                                }
                            }

                            //ID did not get parsed correctly, so input was not an integer. Will display this message
                            //and re-ask the question until input is valid.

                            else
                            {
                                Console.Clear();
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write("===================[(");
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write(" VIEW ");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write(")]===================");

                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("\n\nERROR: Enter the ID integer of the payslip");
                                Console.ForegroundColor = ConsoleColor.White;
                                Thread.Sleep(2000);
                            }
                        }
                    }

                    else if (choice.Equals(2))
                    {
                        while (true)
                        {
                            //Regex rule only allows for lower and uppercase letters, as well as spaces and hyphens to 
                            //support double-barrel names.

                            Regex regex = new Regex(@"^[a-zA-Z -]*$");

                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write("==================[(");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(" DELETE ");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write(")]==================");

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("\n\nENTER SURNAME OR PART OF SURNAME\n");
                            Console.ForegroundColor = ConsoleColor.Gray;

                            string surname = Console.ReadLine();

                            //Check whether input was null before regex due to the regex rule allowing for spaces.

                            if (string.IsNullOrEmpty(surname))
                            {
                                //Input was null or empty, so loop error is displayed and the user is asked to enter
                                //a username again.

                                Console.Clear();
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write("===================[(");
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write(" DELETE ");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write(")]===================");

                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("\n\nERROR: You must enter a string. Enter the surname associated with records");
                                Console.ForegroundColor = ConsoleColor.White;
                                Thread.Sleep(2000);
                            }

                            //Check whether user input matched the regex rule defined.

                            else if (regex.IsMatch(surname))
                            {
                                //Input did abide by the regex defined.

                                string query = "DELETE FROM payslip WHERE surname LIKE '" + surname + "%'";

                                //Attempt to add payslip to database in a try statement to catch any error which may occur. 
                                //if entry is added successfully, user can press enter to go back to the menu.

                                try
                                {
                                    connection.Open();
                                    MySqlCommand cmd = new MySqlCommand(query, connection);
                                    cmd.ExecuteNonQuery();
                                    connection.Close();
                                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                                    Console.WriteLine("\n ---> PAYSLIPS WITH SURNAME LIKE {0} DELETED", surname);
                                    Console.ForegroundColor = ConsoleColor.Magenta;
                                    Console.WriteLine("\nPRESS ENTER TO CONTINUE...");
                                    Console.ReadLine();
                                    //Sent back to menu as data was sent successfully, and user can then start another task.
                                    break;
                                }

                                //An error occured when inputting data to database. Could be due to a connection not closing
                                //in a previous method after data was sent.

                                catch
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("\nERROR: An exception occured when inputting to database. Report to a programmer");
                                    Console.ForegroundColor = ConsoleColor.Magenta;
                                    Console.WriteLine("\nPRESS ENTER TO CONTINUE...");
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.ReadLine();
                                    //sent back to menu to prevent loop when inputting data to mysql.
                                    break;
                                }

                            }
                            else
                            {
                                //User did not enter a string which can be stored in the variable due to it containing numbers or special symbols
                                //Which do not correspond with the regex rule applied above.

                                Console.Clear();
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write("===================[(");
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write(" DELETE ");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write(")]===================");

                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("\n\nERROR: Enter surname associated with records");
                                Console.ForegroundColor = ConsoleColor.White;
                                Thread.Sleep(2000);
                            }
                        }
                    }

                    else if (choice.Equals(3))
                    {
                        //Link back to main menu for easy navigation.

                        main_menu();
                    }

                    else
                    {
                        //Input was not valid/did not match checked fields, and will eventually lead back to asking 
                        //The question again until the input macthes.

                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("===================[(");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(" DELETE ");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(")]===================");

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n\nERROR: Enter the number attatched\nto the action you want");
                        Console.ForegroundColor = ConsoleColor.White;
                        Thread.Sleep(2000);
                    }
                }

                else
                {
                    //No valid input occured, displaying this message for 2 seconds then resetting the loop
                    //whre the question is asked again and input is entered.

                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("===================[(");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(" VIEW ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(")]===================");

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n\nERROR: Enter the number attatched\nto the action you want");
                    Console.ForegroundColor = ConsoleColor.White;
                    Thread.Sleep(2000);
                }
            }
        }
        static void help()
        {
            while (true)
            {
                //Here, each option on the main menu of the application has
                //a dedicated help section, allowing for easy navigation to
                //the area help is needed.

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("===================[(");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(" HELP ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(")]===================");

                Console.WriteLine("\n\nChoose an option;");

                Console.Write("\n [");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("1");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("] Creating a payslip");

                Console.Write("\n [");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("2");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("] Viewing payslips");

                Console.Write("\n [");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("3");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("] Editing a payslip");

                Console.Write("\n [");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("4");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("] Deleting a payslip");

                Console.Write("\n [");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("5");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("] Main menu\n\n");

                int choice;
                bool parsed = int.TryParse(Console.ReadLine(), out choice);

                //Check whether input can be arsed - is an integer.

                if (parsed == true)
                {
                    //Was parsed correctly - was an integer.

                    if (choice.Equals(1))
                    {
                        //Create a payslip help. Displays an overview of how to create a payslip
                        //and the requirements needed prior to do so such as employee name, hours
                        //worked and rate. Can simply escape by pressing enter which sends you back
                        //to the help menu.

                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("===================[(");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(" HELP ");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(")]===================");

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\n\nCREATING A PAYSLIP\n");

                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine(" First, you must have the employee's:");
                        Console.WriteLine("  - Forename\n  - Surname\n  - Title\n  - National Insurance number\n  - Hours worked\n  - Rate\n");
                        Console.WriteLine(" The program will then calculate the gross pay\n and net pay using the rate and hours entered.");
                        Console.WriteLine("\n The payslip will then be saved to the database.\n You can then view, delete and edit the payslip\n in this program.");

                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("\nPRESS ENTER TO GO BACK...");
                        Console.ReadLine();

                    }

                    else if (choice.Equals(2))
                    {
                        //Viewing a payslip or viewing multiple playslips for a specific user help.
                        //Same format as others, except it gives a brief overview of both methods.

                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("===================[(");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(" HELP ");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(")]===================");

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\n\nVIEWING PAYSLIPS\n");

                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine(" First, you must choose which task to perform;");
                        Console.WriteLine("  - View an individual record\n  - View all records for a specific employee\n");
                        Console.WriteLine(" To view a specific playslip, you must have\n the payslip ID.\n");
                        Console.WriteLine(" To view all payslips for a specific employee,\n simply choose option 2 and enter the surname.");

                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("\nPRESS ENTER TO GO BACK...");
                        Console.ReadLine();
                    }

                    else if (choice.Equals(3))
                    {
                        //Editing a payslip help. Gives a brief overview of how to use this application
                        //to edit an existing payslip.

                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("===================[(");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(" HELP ");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(")]===================");

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\n\nEDITING A PAYSLIP\n");

                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine(" In order to edit a payslip, you must have\n the payslip ID\n");
                        Console.WriteLine(" From there, you will be prompted to enter:\n  - New forename\n  - New surname\n  - New title\n  - New national insurnance number\n -  New hours worked\n  - New hourly rate\n");
                        Console.WriteLine(" The net pay and pay gross will be calculated,\n and then saved to the database in place of\n the old payslip.");

                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("\nPRESS ENTER TO GO BACK...");
                        Console.ReadLine();
                    }

                    else if (choice.Equals(4))
                    {
                        //Deleting a payslip help. Same format as others except a brief guide on how to delete entries from the database
                        //Through this application.

                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("===================[(");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(" HELP ");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(")]===================");

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\n\nDELETING A PAYSLIP\n");

                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine(" First, you will be prompted to enter the\n ID of the payslip you wish to delete\n");
                        Console.WriteLine(" This will then search the database for\n the payslip with that ID and\n remove it.");

                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("\nPRESS ENTER TO GO BACK...");
                        Console.ReadLine();
                    }

                    else if (choice.Equals(5))
                    {
                        main_menu();
                    }

                    //Send error if user input was invalid - did not correspond to an option but could be parsed.

                    else
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("===================[(");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(" HELP ");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(")]===================");

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n\nERROR: Enter the number attatched\nto the action you want");
                        Console.ForegroundColor = ConsoleColor.White;
                        Thread.Sleep(2000);
                    }
                }
                //Send error is user input could not be parsed.

                else
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("===================[(");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(" HELP ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(")]===================");

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n\nERROR: Enter the number attatched\nto the action you want");
                    Console.ForegroundColor = ConsoleColor.White;
                    Thread.Sleep(2000);
                }
            }
        }
        static void exit()
        {
            //I have decided to make a closing message incase users think the program
            //has crashed (may have put the wrong input).

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nCLOSING PROGRAM\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Closing in 3 seconds");
            Thread.Sleep(3000);
            Environment.Exit(0);
        }
        static public double grosscalc(double hours, double rate)
        {
            double gross = 0;
            double difference = 0;
            double regwage = 0;
            double overtime = 0;
            double overtime2 = 0;

            //Check if hours is greater than 40.

            if (hours >= 40)
            {
                //Hours is greater than 40.
                //Now check if hours is greater than fifty.

                if (hours >= 50)
                {
                    //Hours is greater than 50.

                    difference = hours - 50;
                    regwage = 40 * rate;
                    overtime = 10 * (rate * 1.5);
                    overtime2 = difference * (rate * 2);
                    gross = regwage + overtime + overtime2;
                    overtimeTotal = overtime + overtime2;
                }
                else
                {
                    //Hours is not greater than 50.

                    difference = hours - 40;
                    regwage = 40 * rate;
                    overtime = difference * (rate * 1.5);
                    gross = regwage + overtime;
                    overtimeTotal = overtime;
                }
            }
            else
            {
                //Hours is not greater than 40, so no overtime is calculated.

                gross = hours * rate;
                overtimeTotal = 0;
            }

            return gross;
        }
        static public double netcalc(double gross, double tax, double NIcharge)
        {
            double net;

            //Remove tax amount from overall gross to give current gross.

            net = gross - (tax + NIcharge);

            return net;
        }
        static public double NItax(double gross)
        {
            double NIcharge;

            //Calculate NI tax which is 11% of overall earnings.

            NIcharge = gross * 0.11;

            return NIcharge;
        }
        static public double taxCalc(double gross)
        {
            double tax;
            double taxCode;

            //Store amount which is untaxed ready for the calculation.

            taxCode = 143;

            //Calculating amount which can be taxed by taking untaxe amount away from gross.

            tax = gross - taxCode;

            //Applying the 25% tax above the allowance.

            tax = tax * 0.25;

            return tax;
        }
    }
}