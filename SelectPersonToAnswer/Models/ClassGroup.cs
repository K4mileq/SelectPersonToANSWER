using System.Collections.ObjectModel;

public class ClassGroup
{
    public string Name { get; set; }
    public ObservableCollection<Student> Students { get; set; }

    public ClassGroup(string name)
    {
        Name = name;
        Students = new ObservableCollection<Student>();
    }

    public void ResetSelection()
    {
        foreach (var student in Students)
        {
            student.IsSelected = false;
        }
    }
}
