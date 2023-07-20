
using System.Collections.ObjectModel;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace ServerMauiApp;

public partial class CommonPage : ContentPage
{
    ObservableCollection<PersonMessage> People ;
    int count=0;
    public CommonPage()
    {
        InitializeComponent();

        People = new ObservableCollection<PersonMessage>
        {
            new PersonMessage{ Name="ddddd" ,Text= "ssssss"},
            new PersonMessage{ Name="fffff",Text="rtttrfft"}
        };
        peopleList.ItemsSource = People;
        var Pha =new  List<Person>{ new Person { Name = "Tom" }, new Person { Name = "Bob" },new Person { Name = "Sam" }, new Person { Name = "Alice" } };
        peopleListt.ItemsSource = Pha;
    }
    public void LabelTextMessangerClick(object? sender, EventArgs e)
    {
       TextMessanger.message = EnterEntry.Text;
        count++;
        TextMessanger.Trech = count;
    }
}
public class PersonMessage
{
    public string Name { get; set; } 
    public string Text { get; set; } 

}
public class Person
{
    public string Name { get; set; }
}

public static class TextMessanger 
{
    
    public static string? message
    {
        get;
        set
       ;
    }
    public static int? Trech
    {
        get ;
        set
        ;
    }

}
