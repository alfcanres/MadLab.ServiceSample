namespace MadLab.ServiceSample.BLL.Dto.Todo
{
    public class TodoUpdateDTO
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
    }
}
