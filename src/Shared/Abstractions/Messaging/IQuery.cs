using MediatR;

namespace Shared.Abstractions.Messaging;

/// <summary>
/// Represents a query that returns a response of type <typeparamref name="TResponse"/>.
/// </summary>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public interface IQuery<out TResponse> : IRequest<TResponse>
{
}