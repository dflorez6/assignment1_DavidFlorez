/* Reservation.cs
*  Title: Reservation
*  Name of Project: Assignment1 - Venue Booking System
*  Purpose: Develop a simple booking system to manage the seating for a venue.
* 
*  Revision History
*   David Florez, 2023.02.10: Created
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment1_DavidFlorez
{

    public class Reservation
    {
        //================
        // Properties
        //================
        public static string[,] Seats { get; set; } // setters & getters
        public static List<string> WaitingList { get; set; } // using a List T instead of an Array because I don't know how many entries will there be in the Waiting List
        public static int TotalCapacity { get; set; }
        public static int ReservedSeats { get; set; }

        //================
        // Constructors
        //================
        // Default
        public Reservation() { }

        // Non-default        
        public Reservation(int rows, int columns)
        {
            // Initializing Capcity
            Reservation.TotalCapacity = rows * columns;
            Reservation.ReservedSeats = 0;

            // Initializing WaitingList Array
            Reservation.WaitingList = new List<string>();

            // Initializing Seats (2D Array)
            Reservation.Seats = new string[rows, columns];

            // Populating the 2D Array
            // Prefilling Array with "EMPTY" to represent an empty seat. Easier for comparisons & finding/counting empty seats
            for (int i = 0; i < Reservation.Seats.GetLength(0); i++)
            {
                for (int j = 0; j < Reservation.Seats.GetLength(1); j++)
                {
                    Reservation.Seats[i, j] = "EMPTY";
                }
            }

        }


        /*
		
		// Displaying Values inside array		
		    for (int i = 0; i < seatsArray.GetLength(0); i++) // For 2D arrays - Remember to use .GetLength(0) in the outer loop
		    {
			    for (int j = 0; j < seatsArray.GetLength(1); j++) // For 2D arrays - Remember to use .GetLength(1) in the nested loop
			    {
				    Console.Write(seatsArray[i,j]);				
			    }
			    Console.Write("\n");
			    Console.WriteLine("***");
		    }
        */

        //================
        // Methods
        //================


        //================
        // Static Methods
        //================
        // TODO: Perhaps pass selectedRowText & selectedColumnText to change color of btnRowColumn to show if seat is available/taken
        public static bool BookSeat(int rowIndex, int columnIndex, string customerName)
        {
            // Evaluates if seat is available
            if (Reservation.Seats[rowIndex, columnIndex] == "EMPTY")
            {
                Console.WriteLine("Seat is Empty");

                // Inserting value into Seats Array                
                Reservation.Seats[rowIndex, columnIndex] = customerName;
                // Reservation.Seats.SetValue(customerName, rowIndex, columnIndex); // Another way of inserting a value into an array using SetValue() Method

                // Decrease Reservation.ReservedSeats
                Reservation.ReservedSeats++;

                return true;
            }
            else
            {
                // Insert into Waiting List
                // Use .Add() Method to insert values into the List T
                Reservation.WaitingList.Add(customerName);

                return false;
            }
        }

        //




    }

    /*
    Example array (seats withouth a name == empty seats)
    // To store info -> create a 2d array 3 rows X 4 columns to store data
    seatsArray = [
                    ["david", A2, A3, "bailey"],
                    [B1, B2, "kanut", B4],
                    [C1, "lina", C3, C4]
                 ]


    */


}
