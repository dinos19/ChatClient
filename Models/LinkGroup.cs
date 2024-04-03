using static ChatClient.Layout.NavMenu;

namespace ChatClient.Models
{
    public class LinkGroup
    {
        public string Name { get; set; } = string.Empty;
        public bool IsExpanded { get; set; } = true;
        public List<LinkItem> Links { get; set; } = new List<LinkItem>();
    }
}