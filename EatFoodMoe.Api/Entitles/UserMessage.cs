using System;

namespace EatFoodMoe.Api.Entitles
{
    public class UserMessage
    {
        public Guid Id { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public DateTimeOffset LastEditTime { get; set; }

        public string Content { get; set; }

        public bool IsReply { get; set; }

        public Guid ReplyMessageId { get; set; }
    }
}