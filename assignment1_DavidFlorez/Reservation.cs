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

        //================
        // Static Methods
        //================
        // BookSeat(): takes in multiple parameters to search the exact position of an item inside the 2D Array Reservation.Seats
        // If the seat is empty, then the customer name is inserted into the 2D Array & Increaseas the count of Reservation.ReservedSeats
        // If the seat is not empty, the customer will be added to the Waiting List
        // Return values true & false are used to display conditional messages to the output & capacity labels
        public static bool BookSeat(int rowIndex, int columnIndex, string customerName)
        {
            // Evaluates if seat is available
            if (Reservation.Seats[rowIndex, columnIndex] == "EMPTY")
            {
                // Inserting value into Seats Array                
                Reservation.Seats[rowIndex, columnIndex] = customerName;
                // Reservation.Seats.SetValue(customerName, rowIndex, columnIndex); // Another way of inserting a value into an array using SetValue() Method

                // Increase Reservation.ReservedSeats
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

        // CancelSeat(): takes in multiple parameters to search the exact position of an item inside the 2D Array Reservation.Seats
        // Has 1 out parameter. The Method returns ints 0, 1 & 2 according to the conditions described below.
        // Cancelation works as follows:
        // return 0 - If the seat is NOT empty and there are customers in the waiting list -> Replaces customer from Reservation.Seats
        // at RowColumn indices with 1st customer from the waiting list
        // return 1 - If the seat is NOT empty and there are NO customers in the waiting list -> Removes customer from Reservation.Seats
        // and changes the value to "EMPTY" to represent an empty chair inside the 2D Array
        // return 2 - If the seat is empty -> No actions only displays corresponding message
        public static int CancelSeat(int rowIndex, int columnIndex, out string customerName)
        {
            // Initial Declarations
            customerName = "";

            // Evaluates if seat is taken
            if (Reservation.Seats[rowIndex, columnIndex] != "EMPTY")
            {
                // Evaluates if there are customers in the Waiting List
                if (Reservation.WaitingList.Count != 0)
                {
                    // 0 == "Seat taken & Customers in the Waiting List"

                    // Initial Declarations
                    string firstCustomerInWaitingList = Reservation.WaitingList[0];
                    customerName = firstCustomerInWaitingList; // out parameter. Customer will be the 1st in the waiting list

                    // Inserts first customer into the 2D Array 
                    Reservation.Seats[rowIndex, columnIndex] = firstCustomerInWaitingList;
                    Reservation.WaitingList.RemoveAt(0);

                    return 0;
                }
                else
                {
                    // 1 == "Seat taken &  No Customers in the Waiting List"

                    // Initial Declarations
                    customerName = Reservation.Seats[rowIndex, columnIndex]; // Current customer holding the seat

                    // Cancel Booking: Replace current value of Reservation.Seats[rowIndex, columnIndex] with "EMPTY" to make seat available again
                    Reservation.Seats[rowIndex, columnIndex] = "EMPTY";

                    // Decrease Reservation.ReservedSeats
                    Reservation.ReservedSeats--;

                    return 1;
                }
            }
            else
            {
                // 2 == "Seat is Empty"
                return 2;
            }
        }

        // AddToWaitingList(): takes in one parameter to add the customer name into the Waiting List
        // The variable "availableSeats" keeps track of empty seats in the 2D Array Reservation.Seats
        // A Foreach Loop is run to verify each seat & see if it is empty/taken
        // Then an If evaluates if "availableSeats" != 0, If true -> returns true, Else -> adds customer to the Waiting List and returns false
        // Return values true & false are used to display conditional messages to the output & capacity labels
        public static bool AddToWaitingList(string customerName)
        {
            // Initial Declarations
            int availableSeats = 0;

            // Using a Foreach Loop to traverse 2D Arrat Reservation.Seats and look if there are available seats
            foreach (string seat in Reservation.Seats)
            {
                if (seat.Equals("EMPTY"))
                {
                    availableSeats++;
                }
            }

            // If availableSeats != 0 True: returns true (will be used to display message "Seats are available")
            // Else: returns false (add customer to the Waiting List & display appropiate message)
            if (availableSeats != 0)
            {
                return true;
            }
            else
            {
                Reservation.WaitingList.Add(customerName);
                return false;
            }
        }

        // FillAllSeats(): Receives 1 parameter customerName
        // Books all "EMPTY" chairs to the passed customerName parameter
        public static void FillAllSeats(string customerName)
        {
            // Inserts customerName in "EMPTY" seat by traversing the 2D Array
            for (int i = 0; i < Reservation.Seats.GetLength(0); i++)
            {
                for (int j = 0; j < Reservation.Seats.GetLength(1); j++)
                {

                    if (Reservation.Seats[i, j] == "EMPTY")
                    {
                        Reservation.Seats[i, j] = customerName;
                    }

                }
            }

            // Updates Reserved Seats
            Reservation.ReservedSeats = Reservation.TotalCapacity;

        }

        // CancelAllBookings():
        // Cancels all bookings & clears waiting list
        public static void CancelAllBookings()
        {
            // Changes all seats to "EMPTY"
            for (int i = 0; i < Reservation.Seats.GetLength(0); i++)
            {
                for (int j = 0; j < Reservation.Seats.GetLength(1); j++)
                {
                    Reservation.Seats[i, j] = "EMPTY";
                }
            }

            // Clears Waiting List
            Reservation.WaitingList.Clear();

            // Resets Reserved Seats
            Reservation.ReservedSeats = 0;
        }


    }


}
