using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.Text;
using Newtonsoft.Json;


class Program
{
    class Data
    {
        public Kiosk[] last5Kiosk { get; set; }
        public string last5Kiosk_title { get; set; }
        public string result { get; set; }
        public Kiosk[] topKiosk { get; set; }
        public string topKiosk_title { get; set; }
        public Kiosk[] topDownKiosk { get; set; }
        public string topDownKiosk_title { get; set; }
    }

    class Kiosk
    {
        public string kid { get; set; }
        public string name { get; set; }
        public string last_update { get; set; }
        public string down_since { get; set; }
        public int isActive { get; set; }
    }
        static async Task Main()
    {
        // Create an instance of HttpClient
        using (HttpClient client = new HttpClient())
        {
            try
            {
                Console.WriteLine("started");
                // Make the API call
                HttpResponseMessage response = await client.GetAsync("https://security.muthoot.org/api/v2/downDevice?apikey=12334&uk=aGVtYW50bkBnb2RyZWouY29t&up=8104b15002bb022fc1a58ae8e03603fe&type=company");
                Console.WriteLine(response);
                // Ensure a successful response
                response.EnsureSuccessStatusCode();

                // Read the response content as a string
                string responseBody = await response.Content.ReadAsStringAsync();

                // Process the response
              //  Console.WriteLine(responseBody);
                string base64Response = responseBody;
        // Decode Base64 string
        byte[] bytes = Convert.FromBase64String(base64Response);
                string jsonResponse = Encoding.UTF8.GetString(bytes);
                try
                {
                    // Parse JSON response
                    dynamic data = JsonConvert.DeserializeObject(jsonResponse);
                    // Console.WriteLine(data);
                    /*foreach (var i in data)
                    {
                        Console.WriteLine(i);
                       
                    }*/
                   int count = 1;
                    foreach (var kiosk in data.topDownKiosk)
                    {
                        Console.WriteLine($"Count:{count}");
                        Console.WriteLine($"Kid: {kiosk.kid}");
                        Console.WriteLine($"Name: {kiosk.name}");
                        Console.WriteLine($"Last Update: {kiosk.last_update}");
                        Console.WriteLine($"Down Since: {kiosk.down_since}");
                      //  Console.WriteLine($"IsActive: {kiosk.isActive}");
                        Console.WriteLine();
                        count++;
                    }
                    int c=count - 1;
                    Console.WriteLine($"Today downsite :{count-1}");

                    var smtpClient = new SmtpClient("smtp.gmail.com")
                    {
                        Port = 587,
                        EnableSsl = true,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential("attendanceportal@sentry.co.in", "@Wyse123")
                    };

                    // Email details
                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress("attendanceportal@sentry.co.in"),
                        To = { "nikhilkpawar97@gmail.com" },
                        Subject = "Muthoot daily Downcount",
                        Body = "Today Downsite count is :"+   c  +" Muthoot "
                    };

                    try
                    {
                        // Send the email
                        smtpClient.Send(mailMessage);
                        Console.WriteLine("Email sent successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Failed to send mail: " + ex.Message);
                    }
                    Console.ReadKey();

                    // Access the data
                    // string value = data.propertyName;
                    Console.ReadKey();
                    // Print the value
                   // Console.WriteLine(value);
                    /*foreach (char i in value)
                    {
                        Console.WriteLine(i);
                    }*/
                }
                catch (JsonException ex)
                {
                    Console.WriteLine("Failed to parse JSON: " + ex.Message);
                }
            }
            catch (HttpRequestException ex)
            {
                // Handle any exceptions occurred during the API call
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
