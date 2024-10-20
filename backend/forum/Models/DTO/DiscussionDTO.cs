public class DiscussionDto
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime Date { get; set; }
    public string? LastMessage { get; set; }
    public int CreatorId { get; set; }
    public string? CreatorIdentifiant { get; set; }
    public int CommentCount { get; set; }
}
