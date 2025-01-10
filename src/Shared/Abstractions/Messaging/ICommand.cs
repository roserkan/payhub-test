using MediatR;

namespace Shared.Abstractions.Messaging;

/// <summary>
/// Represents a command that returns a response of type <typeparamref name="TResponse"/>.
/// </summary>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public interface ICommand<out TResponse> : IRequest<TResponse>
{
}