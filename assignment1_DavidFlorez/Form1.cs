/* Form1.cs
*  Title: Form1
*  Name of Project: Assignment1 - Venue Booking System
*  Purpose: Develop a simple booking system to manage the seating for a venue
* 
*  Revision History
*   David Florez, 2023.02.10: Created
*/

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
            string customerName = CustomerNameValidated();

            // Using out parameter to return multiple string values from the Method
            SelectedRowColumnText(out selectedRowText, out selectedRowIndex, out selectedColumnText, out selectedColumnIndex);

            // Concatenated button that represents a seat (e.g. btnA1)
            string btnTarget = $"btn{selectedRowText}{selectedColumnText}";

            // Calling BookSeat Method from Class Reservation
            // If true: reserve Seat, reduce Capacity, change Seat (button) color & display appropiate text.
            // If False: Add to waiting list & display appropiate text.
            if (Reservation.BookSeat(selectedRowIndex, selectedColumnIndex, customerName))
            {
                // Outputs message to show that a new customer has booked a seat
                lblOutput.Text += $"{customerName} was booked in seat {selectedRowText}{selectedColumnText}";

                // Changes Seat Color to represent that it has been taken
                ButtonReservedSeat(btnTarget, selectedRowIndex, selectedColumnIndex); 
            }
            else
            {
                // Outputs message to show that a new customer has been added to the Waiting List
                lblOutput.Text += $"{customerName} was added to the waiting list";
            }

            // Output Capacity
            lblCapacity.Text = CapacityIndicator();

            // TODO: Remove / Comment this code when done
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

            // TODO: Remove / Comment this code when done
            // Prints values inside Reservation.WaitingList (Array)
            Console.WriteLine("Waiting List: ");
            for (int i = 0; i < Reservation.WaitingList.Count; i++) // For a List T, instead of .Length() use .Count() to get # elements for the Loop
            {
                Console.WriteLine(Reservation.WaitingList[i]);
            }

        }

        // Cancel
        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        // Add to Watchlist
        private void btnWatchlist_Click(object sender, EventArgs e)
        {
            // Variable Declaration (used for out parameters)         
            string selectedRowText;
            string selectedColumnText;
            int selectedRowIndex; // If returns -1 == no row selected
            int selectedColumnIndex; // If returns -1 == no column selected
            string customerName = CustomerNameValidated();

            // Using out parameter to return multiple string values from the Method
            SelectedRowColumnText(out selectedRowText, out selectedRowIndex, out selectedColumnText, out selectedColumnIndex);

            // Calling AddToWaitingList Method from Class Reservation
            // If true: Displays "Seats are available" text.
            // If False: Add to waiting list & display appropiate text.
            if (Reservation.AddToWaitingList(selectedRowIndex, selectedColumnIndex, customerName))
            {
                lblOutput.Text += $"Seats are available.";
            }
            else
            {
                // Outputs message to show that a new customer has been added to the Waiting List
                lblOutput.Text += $"{customerName} was added to the waiting list";
            }            

        }

        // Fill All Seats
        private void btnFillAll_Click(object sender, EventArgs e)
        {

        }

        // Cancel All Bookings
        private void btnCancelAll_Click(object sender, EventArgs e)
        {

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
        public void SelectedRowColumnText(out string selectedRowText, out int selectedRowIndex, out string selectedColumnText, out int selectedColumnIndex)
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
            }
            else
            {
                selectedRowText = lsbRows.GetItemText(lsbRows.SelectedItem);
                selectedColumnText = lsbColumns.GetItemText(lsbColumns.SelectedItem);
                selectedRowIndex = lsbRows.SelectedIndex;
                selectedColumnIndex = lsbColumns.SelectedIndex;
                lblOutput.Text = "";
            }
        }

        // Validates that customer name Textbox is not empty
        public string CustomerNameValidated()
        {
            return string.IsNullOrEmpty(txtCustomerName.Text) ? "Customer name cannot be empty. " : txtCustomerName.Text;
        }

        // Checks for button within GroupBox Venue to change color & add ToolTip text to represent a "Reserved Seat"
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