namespace CourseAuditor.Views
{
    public interface IFrame
    {

    }
    public  interface IView
    {
        object DataContext { get; set; }
        IFrame CurrentFrame { get; set; }
        void Show();
        void Close();
    }
}