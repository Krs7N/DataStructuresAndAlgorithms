using System;
using System.Collections.Generic;
using System.Linq;

namespace Exam.Discord
{
    public class Discord : IDiscord
    {
        private Dictionary<string, Message> messagesById = new Dictionary<string, Message>();
        private Dictionary<string, List<Message>> messagesByChannel = new Dictionary<string, List<Message>>();

        public int Count => messagesById.Count;

        public bool Contains(Message message) => messagesById.ContainsKey(message.Id);

        public void DeleteMessage(string messageId)
        {
            if (!messagesById.ContainsKey(messageId))
            {
                throw new ArgumentException();
            }

            var message = messagesById[messageId];
            messagesById.Remove(messageId);
            messagesByChannel[message.Channel].Remove(message);
        }

        public IEnumerable<Message> GetAllMessagesOrderedByCountOfReactionsThenByTimestampThenByLengthOfContent() =>
            messagesById.Values.OrderByDescending(m => m.Reactions.Count).ThenBy(m => m.Timestamp)
                .ThenBy(m => m.Content.Length);

        public IEnumerable<Message> GetChannelMessages(string channel)
        {
            var channelMessages = messagesById.Values.Where(m => m.Channel == channel).ToList();

            if (channelMessages.Count == 0)
            {
                throw new ArgumentException();
            }

            return channelMessages;
        }

        public Message GetMessage(string messageId)
        {
            if (!messagesById.ContainsKey(messageId))
            {
                throw new ArgumentException();
            }

            return messagesById[messageId];
        }

        public IEnumerable<Message> GetMessageInTimeRange(int lowerBound, int upperBound) =>
            messagesById.Values.Where(m => m.Timestamp >= lowerBound && m.Timestamp <= upperBound).OrderByDescending(m => messagesByChannel[m.Channel].Count);

        public IEnumerable<Message> GetMessagesByReactions(List<string> reactions) => messagesById.Values.Where(m => reactions.All(r => m.Reactions.Contains(r))).OrderByDescending(m => m.Reactions.Count).ThenBy(m => m.Timestamp);


        public IEnumerable<Message> GetTop3MostReactedMessages() =>
            messagesById.Values.OrderByDescending(m => m.Reactions.Count).Take(3);

        public void ReactToMessage(string messageId, string reaction)
        {
            if (!messagesById.ContainsKey(messageId))
            {
                throw new ArgumentException();
            }

            messagesById[messageId].Reactions.Add(reaction);
        }

        public void SendMessage(Message message)
        {
            if (!messagesByChannel.ContainsKey(message.Channel))
            {
                messagesByChannel.Add(message.Channel, new List<Message>());
            }

            messagesByChannel[message.Channel].Add(message);
            messagesById.Add(message.Id, message);
        }
    }
}
