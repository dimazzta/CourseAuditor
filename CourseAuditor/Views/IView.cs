namespace CourseAuditor.Views
{
    public interface IView
    {
        object DataContext { get; set; }
        IPage CurrentFrame { get; set; }
        void Show();
        void Close();
        bool? ShowDialog();
    }
}