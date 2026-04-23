//Console.WriteLine("Hello, World!");

//C#Features in 7 and 8 versions

//1) Tuples and Out variables:

//2) Pattern Matching:

//3) Local Functions:

//4) Async Streams:

//5) Nullable Reference Types:

//C#Features in 9 and 10 versions

//1) Records:

//2) Init-only properties:

//3) Top-level statements:

//4) Target-typed new expressions:

//5) Pattern Matching Enhancements:




// Console.WriteLine("Enter a value:");

// string input = Console.ReadLine();

// switch (input)

// {

//     // 

//     case string s:

//         Console.WriteLine($"The input is a string: {s}");

//         break;

//     default:

//         Console.WriteLine("The input is of an unknown type.");

//         break;

// }




// user define exceptions handling example:
// try
// {
//     Console.WriteLine("Enter a number:");
//     int number = int.Parse(Console.ReadLine());
//     if (number < 0)
//     {
//         throw new NegativeNumberException("Negative numbers are not allowed.");
//     }
//     Console.WriteLine($"You entered: {number}");
// }
// catch (NegativeNumberException ex)       
// {
//     Console.WriteLine($"Error: {ex.Message}");
// }

// public class NegativeNumberException : Exception
// {
//     public NegativeNumberException(string message) : base(message)
//     {
//     }
// }    

