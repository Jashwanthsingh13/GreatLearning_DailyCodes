Console.WriteLine("Hello, World!");

// add two numbers

Console.WriteLine("Enter first number:");
int num1 = Convert.ToInt32(Console.ReadLine());
Console.WriteLine("Enter second number:");
int num2 = Convert.ToInt32(Console.ReadLine());
int sum = num1 + num2;
Console.WriteLine("The sum is: " + sum);


// take input from user and print it
Console.WriteLine("Enter your name:");
string name = Console.ReadLine();
Console.WriteLine("Hello, " + name + "!");


// check if number is even or odd
Console.WriteLine("Enter a number:");
int number = Convert.ToInt32(Console.ReadLine());
if (number % 2 == 0)
{

    Console.WriteLine("The number is even.");
}
else
{
    Console.WriteLine("The number is odd.");
}

// find the largest of three numbers
Console.WriteLine("Enter first number:");
int a = Convert.ToInt32(Console.ReadLine());
Console.WriteLine("Enter second number:");
int b = Convert.ToInt32(Console.ReadLine());
Console.WriteLine("Enter third number:");
int c = Convert.ToInt32(Console.ReadLine());
int largest = a;
if (b > largest)
{
    largest = b;
}
if (c > largest)
{
    largest = c;
}
Console.WriteLine("The largest number is: " + largest);

