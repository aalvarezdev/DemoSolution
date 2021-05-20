using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleApp3
{
    class Product
    {
        public Product(string productName, decimal productValue)
        {
            ProductName = productName;
            ProductValue = productValue;
        }
        /// <summary>
        /// We could use [Key] or fluent  HasKey()  to assign the key to this product. (If using Data Annotations).
        /// </summary>
        public string ProductName { get; set; }

        public decimal ProductValue { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {

            int giftcardBalance;
            string fileName;
            int ammount;


            Console.WriteLine("Set the  Full file path   :  ");
            fileName = Console.ReadLine();

           
           
          
            if (!File.Exists(fileName))
            {
                Console.WriteLine("File does not exists. Please try again.");
                Console.ReadLine();
                return;
            }
            Console.WriteLine("Set the giftcard balance (Must be an integer number");
            var strGiftCard = Console.ReadLine();
            if (!Int32.TryParse(strGiftCard, out giftcardBalance))
            {
                Console.WriteLine("Invalid ammount. Please use an integer value between Int.Max()*-1  and Int.Max()");
            }

            
            var fileContent = File.ReadAllLines(fileName);


            try
            {
                Product[] products = fileContent.Select(x => new Product(x.Split(' ')[0], Convert.ToDecimal(x.Split(' ')[1]))).ToArray();

                decimal ammoutToSpend = 1000;
                int arrayLenght = products.Length;
                var ammounts = products.Select(x => Convert.ToInt32(x.ProductValue)).ToArray();

                GetClosestPair(products, arrayLenght, ammoutToSpend);


            }
            catch (FormatException ex)
            {
                Console.WriteLine("Unable to parse the file. Make sure you have valid integer prices. ");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to process the product list. ");
                Console.ReadLine();
            }



        }


        static void GetClosestPair(Product[] products, int productLength, decimal desiredammount)
        {


            //We pair every number in the array, from the 0 position to the productLength position.
            int leftPosition = 0;
            int rightPosition = productLength - 1;


            // To store indexes of result pair
            int current_most_left_position = 0;
            int current_most_right_position = 0;


            decimal currentDifference = products.Select(x => x.ProductValue).Max() + 1;


            while (rightPosition > leftPosition)
            {

                //Printing values. 
                var firstValue = Math.Abs(products[leftPosition].ProductValue);
                var secondValue = products[rightPosition].ProductValue;
                Console.WriteLine($"CHecking for {firstValue} and {secondValue}");


                if (Math.Abs(firstValue + secondValue - desiredammount) < currentDifference)
                {
                    current_most_left_position = leftPosition;
                    current_most_right_position = rightPosition;
                    currentDifference = Math.Abs(products[leftPosition].ProductValue + products[rightPosition].ProductValue - desiredammount);
                }

                //Pair is larger than current desired ammount. We pick the next number from right to left ,[right position - 1 ]
                if (products[leftPosition].ProductValue + products[rightPosition].ProductValue > desiredammount)
                    rightPosition--;
                //Pair is shorter to current desired ammount. We pick the next number from left to right ,[left position + 1 ]
                else
                    leftPosition++;
            }

            if (products[current_most_left_position].ProductValue + products[current_most_right_position].ProductValue > desiredammount)
            {
                Console.WriteLine("No pair was found");
                return;
            }

            Console.Write(
                $"Pair is [{{ {products[current_most_left_position].ProductName } : {products[current_most_left_position].ProductValue  }}} , " +
               $"{{ {products[current_most_right_position].ProductName } : {products[current_most_right_position].ProductValue  }}}]");

            Console.Read();
        }



    }
}
