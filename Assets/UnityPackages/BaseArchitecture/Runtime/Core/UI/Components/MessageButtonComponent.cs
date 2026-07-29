using UnityEngine.UI;
using Zenject;

namespace BaseArchitecture.Core
{
    /// <summary>
    /// Button that publishes a message on the bus when clicked. Subclass per concrete message type.
    /// </summary>
    public abstract class MessageButtonComponent<TMessage> : Button where TMessage : IMessageObject, new()
    {
        [Inject] private readonly IMessageBus _messageBus;

        protected override void Awake()
        {
            base.Awake();
            onClick.AddListener(RaiseClicked);
        }

        private void RaiseClicked()
        {
            _messageBus.Publish(new TMessage());
        }
    }
}
