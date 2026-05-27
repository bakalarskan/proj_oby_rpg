using System.Collections.Generic;

namespace lab_game.model
{
    public sealed class LocalModel
    {
        public bool IsInventoryOpen { get; set; }
        public int InventoryIndex { get; set; }
        public Queue<string> UiMessages { get; } = new Queue<string>();

        public void AddMessage(string message, int maxMessages = 5)
        {
            UiMessages.Enqueue(message);
            while (UiMessages.Count > maxMessages)
            {
                UiMessages.Dequeue();
            }
        }

        public void ClearMessages()
        {
            UiMessages.Clear();
        }
    }
}