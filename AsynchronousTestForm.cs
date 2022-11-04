using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FibonacciAsynchronous
{
    public partial class AsynchronousTestForm : Form
    {
        List<Task> tasks = new List<Task>();

        public AsynchronousTestForm()
        {
            InitializeComponent();
        }

        // Start asynchronous calls to Fibonacci
        private async void startButton_Click(object sender, EventArgs e)
        {
            outputTextBox.Text = "Starting Task to calculate Fibonacci(46)\r\n";

            // Create Task to perform Fibonacci(46) calculation in thread
            Task<TimeData> task1 = Task.Run(() => StartFibonacci(46));
            tasks.Add(task1);

            outputTextBox.AppendText("Starting Task to calculate Fibonacci(45)\r\n");

            // Create Task to perform Fibonacci(45) calculation in thread
            Task<TimeData> task2 = Task.Run(() => StartFibonacci(45));
            tasks.Add(task2);

            //await Task.WhenAll(task1, task2);  // Wait for both to complete
            await Task.WhenAll(tasks);  // Wait for both to complete

            // Determine time that first thread started
            DateTime startTime = (task1.Result.StartTime < task2.Result.StartTime) ? task1.Result.StartTime : task2.Result.StartTime;

            // Determine time that last thread ended
            DateTime endTime = (task1.Result.EndTime < task2.Result.EndTime) ? task1.Result.EndTime : task2.Result.EndTime;

            // Display total time for calculations 
            double totalMinutes = (endTime - startTime).TotalMinutes;
            outputTextBox.AppendText($"Total calculation time = {totalMinutes:F6} minutes\r\n");
        }

        // Start a call to Fibonacci and capture start/end times
        TimeData StartFibonacci(int n)
        {
            // Create a TimeData object to store start/end times
            var result = new TimeData();

            AppendText($"Calculating Fibonacci({n})");
            result.StartTime = DateTime.Now;
            long fibonacciValue = Fibonacci(n);
            result.EndTime = DateTime.Now;

            AppendText($"Fibonacci({n}) = {fibonacciValue}");
            double minutes = (result.EndTime - result.StartTime).TotalMinutes;
            AppendText($"Calculation of time = {minutes:F6} minutes\r\n");

            AppendText($"Total tasks in List<Task> = {tasks.Count()}\r\n");
            return result;
        }

        // Recursively calculates Fibonacci numbers
        public long Fibonacci(long n)
        {
            if (n == 0 || n == 1)
            {
                return n;
            }
            else
            {
                return Fibonacci(n - 1) + Fibonacci(n - 2);
            }
        }

        // Append text to outputTextBox in UI thread
        public void AppendText(String text)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => AppendText(text)));
            }
            else
            {
                outputTextBox.AppendText(text + "\r\n");
            }
        }
    }
}
