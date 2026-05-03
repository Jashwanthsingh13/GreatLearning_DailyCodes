


//Problem statement based on use of delgates in C#:

//1. Create a delegate type called "MathOperation" that takes two integers as parameters and

//returns an integer as output.

//2. Implement three methods that match the signature of the "MathOperation" delegate:

//   a. Add: This method should return the sum of the two integers.

//   b. Subtract: This method should return the difference of the two integers.

//   c. Multiply: This method should return the product of the two integers.

//3. Create an instance of the "MathOperation" delegate and assign it the "Add" method.

//4. Invoke the delegate with two integers and display the result.

//5. Change the delegate assignment to the "Subtract" method and invoke it again with the same integers, displaying the result.

// using System;
// // Step 1: Create a delegate type called "MathOperation"
// public delegate int MathOperation(int a, int b);

// class Program
// {
//     // Step 2: Implement three methods that match the signature of the "MathOperation" delegate
//     public static int Add(int a, int b)
//     {
//         return a + b;
//     }

//     public static int Subtract(int a, int b)
//     {
//         return a - b;
//     }

//     public static int Multiply(int a, int b)
//     {
//         return a * b;
//     }

//     static void Main(string[] args)
//     {
//         // Step 3: Create an instance of the "MathOperation" delegate and assign it the "Add" method
//         MathOperation operation = Add;

//         // Step 4: Invoke the delegate with two integers and display the result
//         int result = operation(5, 3);
//         Console.WriteLine("Addition Result: " + result);

//         // Step 5: Change the delegate assignment to the "Subtract" method and invoke it again with the same integers, displaying the result
//         operation = Subtract;
//         result = operation(5, 3);
//         Console.WriteLine("Subtraction Result: " + result);
//     }
// }




// //Creating an instance of the "MathOperation" delegate and assigning it the "Add" method.

// MathOperation operation = Add;

// Console.WriteLine("Delegate ref is created and currently it is pointing to Add()");

// //invoking the delegate with two integers and displaying the result.

// int result = operation(10, 5);

// Console.WriteLine("Since Delegate is pointing to Add() so the result of Addition is " + result);



// //Changing the delegate assignment to the "Subtract" method and invoking it again with the same integers, displaying the result.

// operation = Subtract;

// Console.WriteLine("Now the delegate ref is changed and currently it is pointing to Subtract()");

// result = operation(10, 5);

// Console.WriteLine("Since Delegate is pointing to Subtract() so the result of Subtraction is " + result);






// delegate int MathOperation(int a, int b);

// //define a delegate type called "MathOperation" that takes two integers as parameters and returns an integer as output.

// //outside the main method of the class and before the main method of the class

// public class delgateDemo

// {

//     //here we will implemnt methods that match the signature of the delegate

//     //Here we are defining methods as static because we are going to call these methods without creating an instance of the class.







//     public static int Add(int a, int b)

//     {

//         return a + b;

//     }

//     public static int Subtract(int a, int b)

//     {

//         return a - b;

//     }

//     public static int Multiply(int a, int b)

//     {

//         return a * b;

//     }




using DelegateDemo;

MathOperation operation = delgateDemo.Add;
Console.WriteLine("Delegate ref is created and currently it is pointing to Add()");
//invoking the delegate with two integers and displaying the result.
int result = operation(10, 5);
Console.WriteLine("Since Delegate is pointing to Add() so the result of Addition is " + result);



//Changing the delegate assignment to the "Subtract" method and invoking it again with the same integers, displaying the result.
operation = delgateDemo.Subtract;
Console.WriteLine("Now the delegate ref is changed and currently it is pointing to Subtract()");
result = operation(10, 5);
Console.WriteLine("Since Delegate is pointing to Subtract() so the result of Subtraction is " + result);


//Changing the delegate assignment to the "Multiply" method and invoking it again with the same integers, displaying the result.
operation = delgateDemo.Multiply;
Console.WriteLine("Now the delegate ref is changed and currently it is pointing to Multiply()");
result = operation(10, 5);
Console.WriteLine("Since Delegate is pointing to Multiply() so the result of Multiplication is " + result);