using System.Collections;

namespace GroqNet.ChatCompletions
{
    public class GroqChatHistory : IList<GroqMessage>, IReadOnlyList<GroqMessage>
    {
        private readonly List<GroqMessage> messages;

        public GroqChatHistory()
        {
            messages = new();
        }

        public GroqChatHistory(IEnumerable<GroqMessage> messages)
        {
            ArgumentNullException.ThrowIfNull(messages, nameof(messages));
            this.messages = new(messages);
        }

        public GroqChatHistory(string systemMessage)
        {
            messages = new();
            AddSystemMessage(systemMessage);
        }

        private void AddMessage(GroqChatRole role, string content) => messages.Add(new GroqMessage(role, content));

        public void AddUserMessage(string content) => AddMessage(GroqChatRole.User, content);

        public void AddAssistantMessage(string content) => AddMessage(GroqChatRole.Assistant, content);

        private void AddSystemMessage(string content) => AddMessage(GroqChatRole.System, content);

        public int Count => messages.Count;

        IEnumerator IEnumerable.GetEnumerator() => messages.GetEnumerator();

        GroqMessage IReadOnlyList<GroqMessage>.this[int index] => messages[index];

        public IEnumerator<GroqMessage> GetEnumerator()
        {
            return messages.GetEnumerator();
        }

        GroqMessage IList<GroqMessage>.this[int index]
        {
            get => messages[index];
            set
            {
                ArgumentNullException.ThrowIfNull(value, nameof(value));
                messages[index] = value;
            }
        }

        public bool IsReadOnly => false;

        public void Add(GroqMessage item)
        {
            ArgumentNullException.ThrowIfNull(item, nameof(item));
            messages.Add(item);
        }

        public void AddRange(IEnumerable<GroqMessage> items)
        {
            ArgumentNullException.ThrowIfNull(items, nameof(items));
            messages.AddRange(items);
        }

        public bool Contains(GroqMessage item)
        {
            ArgumentNullException.ThrowIfNull(item, nameof(item));
            return messages.Contains(item);
        }

        public void CopyTo(GroqMessage[] array, int arrayIndex)
        {
            messages.CopyTo(array, arrayIndex);
        }

        public int IndexOf(GroqMessage item)
        {
            ArgumentNullException.ThrowIfNull(item, nameof(item));
            return messages.IndexOf(item);
        }

        public void Insert(int index, GroqMessage item)
        {
            ArgumentNullException.ThrowIfNull(item, nameof(item));
            messages.Insert(index, item);
        }

        public bool Remove(GroqMessage item)
        {
            ArgumentNullException.ThrowIfNull(item, nameof(item));
            return messages.Remove(item);
        }

        public void RemoveAt(int index) => messages.RemoveAt(index);

        public void RemoveRange(int index, int count)
        {
            messages.RemoveRange(index, count);
        }

        public void Clear() => messages.Clear();
    }
}
