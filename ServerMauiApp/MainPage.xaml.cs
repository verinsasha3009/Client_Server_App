using System.Net.Sockets;
using System.Reflection.PortableExecutable;
using System.Text.Encodings.Web;
using static System.Net.Mime.MediaTypeNames;


namespace ServerMauiApp
{
    public partial class MainPage : ContentPage
    {
        //Label stackLabel;
        //Entry entry;
        //bool loaded = false;
        
        StreamReader? Reader = null;
        StreamWriter? Writer = null;
       
        int? count = TextMessanger.Trech;
        
        string textname="";
        
        public MainPage()
        {
            InitializeComponent();
        }      
        // Переход вперед на Page2
        private void ClickNameSave(object? sender, EventArgs e)
        {
            ClickName();
        }
        public string ClickName()
        {
            textname = EntryName.Text;
            if (textname.Length<4) 
            {
                EntryName.Text = "Error";
               return textname = "";
            }
            return textname;
        }
        private async void GoToForward(object? sender, EventArgs e)
        {            
            await Navigation.PushAsync(new CommonPage());

            string host = "127.0.0.1";
            int port = 8888;
            using TcpClient client = new TcpClient();

            try
            {
                client.Connect(host, port); //подключение клиента
                Reader = new StreamReader(client.GetStream());
                Writer = new StreamWriter(client.GetStream());
                if (Writer is null || Reader is null) return;
                // запускаем новый поток для получения данных
                await Writer.WriteLineAsync(textname);
                await Writer.FlushAsync();
                //await SengAsync();

            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
            }
            Writer?.Close();
            Reader?.Close();
        }
        async Task SengAsync() 
        {
                while (true)
                {
                    Thread.Sleep(1000);
                    if(count == new int())
                    {
                        await ChatMessageAsync(); 
                    }
                }
        }
        async Task SendMessageAsync(StreamWriter writer, string userName)
        {
            // сначала отправляем имя
            //await writer.WriteLineAsync(userName);
            //await writer.FlushAsync();
            while (count == new int ())
            {
                string messages = TextMessanger.message;
                await writer.WriteLineAsync(messages);
                await writer.FlushAsync();
            }

            
        }
        
        // получение сообщений
        async Task ReceiveMessageAsync(StreamReader reader)
        {
            while (true)
            {
                try
                {
                    // считываем ответ в виде строки
                    string? message = await reader.ReadLineAsync();
                    // если пустой ответ, ничего не выводим на консоль
                    if (string.IsNullOrEmpty(message)) continue;
                    Print(message);//вывод сообщения
                }
                catch
                {
                    break;
                }
            }
        }
        // чтобы полученное сообщение не накладывалось на ввод нового сообщения
        void Print(string message)
        {
            if (OperatingSystem.IsWindows())    // если ОС Windows
            {
                var position = Console.GetCursorPosition(); // получаем текущую позицию курсора
                int left = position.Left;   // смещение в символах относительно левого края
                int top = position.Top;     // смещение в строках относительно верха
                                            // копируем ранее введенные символы в строке на следующую строку
                Console.MoveBufferArea(0, top, left, 1, 0, top + 1);
                // устанавливаем курсор в начало текущей строки
                Console.SetCursorPosition(0, top);
                // в текущей строке выводит полученное сообщение
                //Console.WriteLine(message);
                // переносим курсор на следующую строку
                // и пользователь продолжает ввод уже на следующей строке
                Console.SetCursorPosition(left, top + 1);
            }
            /*else;*/ //Console.WriteLine(message);
        }
        public async Task ChatMessageAsync()
        {
            await Task.Run(() => ReceiveMessageAsync(Reader));
            // запускаем ввод сообщений
            await SendMessageAsync(Writer, textname);
        }

    }
    
}
