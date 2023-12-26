/*
 * Created by: Selim
 * Date created: 15.02.2023
 * Last modified: 
 * Modified by: 
 */
using MimeKit;

namespace TUSO.Domain.Dto
{
    public class EmailModelDto
    {
        public string ToMail { get; set; }

        public string Subject { get; set; }

        public List<string> Bcc { get; set; } = new List<string>();

        public MimeEntity Body { get; set; }
    }
}