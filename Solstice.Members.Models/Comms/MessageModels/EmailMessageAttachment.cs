
namespace Solstice.Comms.MessageModels
{
    public class EmailMessageAttachment
    {
        public string Name { get; set; }
        public byte[] Content { get; set; }
        public string MimeTypeName { get; set; }
    }
}
