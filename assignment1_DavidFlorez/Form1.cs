/* Form1.cs
*  Title: Form1
*  Name of Project: Assignment1 - Venue Booking System
*  Purpose: Develop a simple booking system to manage the seating for a venue
* 
*  Revision History
*   David Florez, 2023.02.10: Created
*/

using System.Windows.Forms;

namespace assignment1_DavidFlorez
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //================
        // Button Methods
        //================
        // Book
        private void btnBook_Click(object sender, EventArgs e)
        {
            // Variable Declaration (used for out parameters)         
            string selectedRowText;
            string selectedColumnText;
            int selectedRowIndex; // If returns -1 == no row selected
            int selectedColumnIndex; // If returns -1 == no column selected
            string customerName;

            // Evaluates if user has selected Row & Columns from ListBox
            // Using out parameter to return multiple string values from the Method                   
            if (SelectedRowColumnText(out selectedRowText, out selectedRowIndex, out selectedColumnText, out selectedColumnIndex))
            {

                // Validates Customer Name is not null or blank
                if (CustomerNameValidated(out customerName))
                {
                    // Concatenated button that represents a seat (e.g. btnA1)
                    string btnTarget = $"btn{selectedRowText}{selectedColumnText}";

                    // Calling BookSeat Method from Class Reservation
                    // If true: reserve Seat, reduce Capacity, change Seat (button) color & display appropiate text.
                    // If False: Add to waiting list & display appropiate text.
                    if (Reservation.BookSeat(selectedRowIndex, selectedColumnIndex, customerName))
                    {
                        // Outputs message to show that a new customer has booked a seat
                        lblOutput.Text = $"{customerName} was booked in seat {selectedRowText}{selectedColumnText}";

                        // Changes Seat Color to represent that it has been taken
                        ButtonReservedSeat(btnTarget, selectedRowIndex, selectedColumnIndex);
                    }
                    else
                    {
                        // Outputs message to show that a new customer has been added to the Waiting List
                        lblOutput.Text = $"{customerName} was added to the waiting list";
                    }

                    // Output Capacity
                    lblCapacity.Text = CapacityIndicator();
                }
                else
                {
                    lblOutput.Text = "Customer name cannot be empty. ";
                }
            }
            else
            {
                lblOutput.Text = "Make sure you have selected a row and column. ";
            }

            /*
            // TODO: Remove / Comment this code when done - Used for testing
            // Prints values inside Reservation.Seats (2D Array)
            Console.WriteLine("Seats: ");
            for (int i = 0; i < Reservation.Seats.GetLength(0); i++) // For 2D arrays - Remember to use .GetLength(0) in the outer loop
            {
                for (int j = 0; j < Reservation.Seats.GetLength(1); j++) // For 2D arrays - Remember to use .GetLength(1) in the nested loop
                {
                    Console.Write(Reservation.Seats[i, j]);
                    Console.Write(" ");
                }
                Console.Write("\n");
                Console.WriteLine("***");
            }

            // TODO: Remove / Comment this code when done - Used for testing
            // Prints values inside Reservation.WaitingList (Array)
            Console.WriteLine("Waiting List: ");
            for (int i = 0; i < Reservation.WaitingList.Count; i++) // For a List T, instead of .Length() use .Count() to get # elements for the Loop
            {
                Console.WriteLine(Reservation.WaitingList[i]);
            }
            */

        }

        // Cancel
        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Variable Declaration (used for out parameters)         
            string selectedRowText;
            string selectedColumnText;
            int selectedRowIndex; // If returns -1 == no row selected
            int selectedColumnIndex; // If returns -1 == no column selected
            string customerName; // used for out parameter

            // Evaluates if user has selected Row & Columns from ListBox
            // Using out parameter to return multiple string values from the Method
            if (SelectedRowColumnText(out selectedRowText, out selectedRowIndex, out selectedColumnText, out selectedColumnIndex))
            {
                // Concatenated button that represents a seat (e.g. btnA1)
                string btnTarget = $"btn{selectedRowText}{selectedColumnText}";

                // Calling CancelSeat Method from Class Reservation
                // If true: reserve Seat, reduce Capacity, change Seat (button) color & display appropiate text.
                // If False: Add to waiting list & display appropiate text.
                switch (Reservation.CancelSeat(selectedRowIndex, selectedColumnIndex, out customerName))
                {
                    // Seat taken & customers in the Waiting List
                    case 0:
                        lblOutput.Text = $"Customer {customerName} was booked from the Waiting List. ";

                        // Changes Seat Color to represent that it has been taken
                        ButtonReservedSeat(btnTarget, selectedRowIndex, selectedColumnIndex);

                        // Output Capacity
                        lblCapacity.Text = CapacityIndicator();
                        break;

                    // Seat taken & no customers in the Waiting List
                    case 1:
                        // Changes Seat Color to represent that it is now empty
                        ButtonEmptySeat(btnTarget, selectedRowIndex, selectedColumnIndex);

                        // Output
                        lblCapacity.Text = CapacityIndicator();
                        lblOutput.Text = $"The booking for {customerName} was successfully canceled. ";

                        break;

                    // Seat is empty
                    case 2:
                        lblOutput.Text = $"The seat at {selectedRowText}{selectedColumnText} was not booked. Nothing to cancel. ";
                        break;
                }
            }
            else
            {
                lblOutput.Text = "Make sure you have selected a row and column. ";
            }
        }

        // Add to Watchlist
        private void btnWatchlist_Click(object sender, EventArgs e)
        {
            // Variable Declaration (used for out parameters)
            string customerName;

            // Validates Customer Name is not null or blank
            if (CustomerNameValidated(out customerName))
            {
                // Calling AddToWaitingList Method from Class Reservation
                // If true: Displays "Seats are available" text.
                // If False: Add to waiting list & display appropiate text.
                if (Reservation.AddToWaitingList(customerName))
                {
                    lblOutput.Text = $"Seats are available.";
                }
                else
                {
                    // Outputs message to show that a new customer has been added to the Waiting List
                    lblOutput.Text = $"{customerName} was added to the waiting list. ";
                }

                // Output Capacity
                lblCapacity.Text = CapacityIndicator();
            }
            else
            {
                lblOutput.Text = "Customer name cannot be empty. ";
            }
        }

        // Fill All Seats
        private void btnFillAll_Click(object sender, EventArgs e)
        {
            // Variable Declaration (used for out parameters)
            string customerName;

            // Validates Customer Name is not null or blank
            if (CustomerNameValidated(out customerName))
            {
                // Calls FillAllSeats() Method
                Reservation.FillAllSeats(customerName);

                // Changes Seat Color to represent that it has been taken
                ButtonReservedAllSeats();

                // Output
                lblOutput.Text = $"{customerName} booked all the empty seats. ";
                lblCapacity.Text = CapacityIndicator();
            }
            else
            {
                lblOutput.Text = "Customer name cannot be empty. ";
            }
        }

        // Cancel All Bookings
        private void btnCancelAll_Click(object sender, EventArgs e)
        {
            // Calls Reservation.CancelAllBookings() Method
            Reservation.CancelAllBookings();

            // Changes Seat Color to represent that it is now empty
            ButtonCenceledAllSeats();

            // Output
            lblOutput.Text = $"All bookings have been canceled and waiting list emptied. ";
            lblCapacity.Text = CapacityIndicator();

        }

        //================
        // Initializations
        //================
        // Form Load
        private void Form1_Load(object sender, EventArgs e)
        {
            // Calling instance of Reservation on Form Load ** IMPORTANT STEP TO REMEMBER
            ReservationInstance();

            // Initial Capacity Indicator
            lblCapacity.Text = CapacityIndicator();

            // Initializes all GroupBox Venue buttons with default tooltip text
            foreach (Control control in grbVenue.Controls)
            {
                tipSeats.SetToolTip(control, "empty");
            }
        }

        // Instantiating Reservation (object)
        public Reservation ReservationInstance()
        {
            // Constants
            const int ROWS = 3;
            const int COLUMNS = 4;

            // Initializing instance
            Reservation reservation = new Reservation(ROWS, COLUMNS);
            return reservation;
        }


        //================
        // Helper Methods
        //================
        // Validates that the user has selected a Row & a Column from the List Boxes. If selection is empty a message will be displayed.
        public bool SelectedRowColumnText(out string selectedRowText, out int selectedRowIndex, out string selectedColumnText, out int selectedColumnIndex)
        {
            // Initial Declarations
            selectedRowText = null;
            selectedColumnText = null;
            selectedRowIndex = -1; // Using -1 to represent "null"
            selectedColumnIndex = -1; // Using -1 to represent "null"

            // Validates that a row has been selected by the user
            if (lsbRows.SelectedIndex == -1 || lsbColumns.SelectedIndex == -1)
            {
                lblOutput.Text = "Make sure you have selected a row and column. ";
                return false;
            }
            else
            {
                selectedRowText = lsbRows.GetItemText(lsbRows.SelectedItem);
                selectedColumnText = lsbColumns.GetItemText(lsbColumns.SelectedItem);
                selectedRowIndex = lsbRows.SelectedIndex;
                selectedColumnIndex = lsbColumns.SelectedIndex;
                lblOutput.Text = "";
                return true;
            }
        }

        // Validates that customer name Textbox is not empty       
        public bool CustomerNameValidated(out string customerName)
        {
            // Initial Declarations
            customerName = "";

            // Validates that Customer Name is not Null or Empty
            if (string.IsNullOrEmpty(txtCustomerName.Text))
            {
                // lblOutput.Text = "Customer name cannot be empty. ";
                return false;
            }
            else
            {
                customerName = txtCustomerName.Text;
                lblOutput.Text = "";
                return true;
            }
        }

        // Sets color & tooltip text when calling Reservation.BookSeat()        
        public void ButtonReservedSeat(string btnTarget, int selectedRowIndex, int selectedColumnText)
        {
            // Initial Declarations
            string bookedCustomerName = Reservation.Seats[selectedRowIndex, selectedColumnText];
            string tooltipText = $"Booked by: {bookedCustomerName}";

            // Iterates over GroupBox Venue to manipulate the buttons (controls)
            foreach (Control control in grbVenue.Controls)
            {
                if (control.Name == btnTarget)
                {
                    control.BackColor = Color.Red;
                    control.ForeColor = Color.White;
                    // Calls tooltip.SetToolTip(control, "text") Method to dynamically generate the text in the tooltip on button_hover
                    tipSeats.SetToolTip(control, tooltipText);
                }
            }
        }

        // Sets color & tooltip text when calling Reservation.CancelSeat()
        public void ButtonEmptySeat(string btnTarget, int selectedRowIndex, int selectedColumnText)
        {
            // Iterates over GroupBox Venue to manipulate the buttons (controls)
            foreach (Control control in grbVenue.Controls)
            {
                if (control.Name == btnTarget)
                {
                    control.BackColor = Color.Green;
                    control.ForeColor = Color.White;
                    // Calls tooltip.SetToolTip(control, "text") Method to dynamically generate the text in the tooltip on button_hover
                    tipSeats.SetToolTip(control, "empty");
                }
            }
        }

        // Sets color & tooltip text when calling Reservation.FillAllSeats()
        public void ButtonReservedAllSeats()
        {
            // Initial Declarations            
            const int LENGTH_OF_ROW = 4;
            List<string> customerNamesList = new List<string>();

            // 
            for (int i = 0; i < Reservation.Seats.GetLength(0); i++)
            {
                for (int j = 0; j < Reservation.Seats.GetLength(1); j++)
                {
                    // Formula to convert 2D Array Indices to 1D Indices
                    // (row * lengthOfRow)+column = 1d index; // Will be used to define the index to insert the name into the List T
                    customerNamesList.Insert((i * LENGTH_OF_ROW) + j, Reservation.Seats[i, j]); // List T .Insert(index, item to be inserted)
                }
            }

            // Run For Loop backwards to change all the buttons, because the collection starts iterating at the last buttonb C4 -> A1
            // This tweak will assure that the correct information is set on the tooltip
            int namesListCounter = 0;
            for (int i = grbVenue.Controls.Count - 1; i >= 0; i--) // ALWAYS REMEMBER to -1 the Length/Count when runing the For Loop in reverse            
            {
                grbVenue.Controls[i].BackColor = Color.Red;
                grbVenue.Controls[i].ForeColor = Color.White;
                // Calls tooltip.SetToolTip(control, "text") Method to dynamically generate the text in the tooltip on button_hover
                tipSeats.SetToolTip(grbVenue.Controls[i], customerNamesList[namesListCounter]);
                namesListCounter++; // increase counter since customerNamesList is going from 0->11, while i is going from 11 -> 0
            }
        }

        // Sets color & tooltip text when calling Reservation.CancelAllBookings()
        public void ButtonCenceledAllSeats()
        {
            // Run For Loop to change all the buttons back to represent "EMPTY" seats
            for (int i = 0; i < grbVenue.Controls.Count; i++)
            {
                grbVenue.Controls[i].BackColor = Color.Green;
                grbVenue.Controls[i].ForeColor = Color.White;
                // Calls tooltip.SetToolTip(control, "text") Method to dynamically generate the text in the tooltip on button_hover
                tipSeats.SetToolTip(grbVenue.Controls[i], "empty");
            }
        }

        // Returns text to be displayed in lblCapacity with the information of Total Capacity. Seats Available (with % capacity). Waiting List Count
        public string CapacityIndicator()
        {
            // Declarations & Calculations
            const int FULL_PERCENT = 100;
            int seatsAvailable = Reservation.TotalCapacity - Reservation.ReservedSeats;
            double capacityPercentage = (Convert.ToDouble(Reservation.ReservedSeats) / Convert.ToDouble(Reservation.TotalCapacity)) * Convert.ToDouble(FULL_PERCENT);
            capacityPercentage = Math.Round(capacityPercentage, 1);
            string seatsAvailableText = $"{seatsAvailable.ToString()} (i.e. at {capacityPercentage.ToString()}%)";

            return $"Total capacity: {Reservation.TotalCapacity}. Seats available: {seatsAvailableText}. Waiting List: {Reservation.WaitingList.Count}";
        }



        // lblOutput -> Error messages or confirmations
        // lblCapacity -> shows # of seats available, % capacity & # people in watchlist

        /*
            try
            {
            }
            catch (Exception ex)
            {
                lblOutput.Text = ex.Message;
            } 
         */

    }
}