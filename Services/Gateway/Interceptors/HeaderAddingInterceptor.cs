using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Gateway.Interceptors;

public class HeaderAddingInterceptor: Interceptor
{
    private readonly string _host;
    private readonly string _headerName;
    private readonly string _headerValue;

    public HeaderAddingInterceptor(string host, string headerName, string headerValue)
    {
        _headerName = headerName;
        _headerValue = headerValue;
        _host = host;
    }
    
    public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
        TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        Metadata headers = context.Options.Headers ?? [];

        headers.Add(_headerName, _headerValue);
        
        CallOptions newOptions = context.Options.WithHeaders(headers);
        
        return continuation(request, new ClientInterceptorContext<TRequest, TResponse>(context.Method, _host, newOptions));
    }

    /// <summary>
    /// Intercepte les appels de streaming du client (requêtes multiples du client, réponse unique du serveur).
    /// </summary>
    public override AsyncClientStreamingCall<TRequest, TResponse> AsyncClientStreamingCall<TRequest, TResponse>(
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncClientStreamingCallContinuation<TRequest, TResponse> continuation)
    {
        Metadata headers = context.Options.Headers ?? [];
        headers.Add(_headerName, _headerValue);
        CallOptions newOptions = context.Options.WithHeaders(headers);
        
        // La continuation pour le streaming du client prend directement le nouveau contexte.
        return continuation(new ClientInterceptorContext<TRequest, TResponse>(context.Method, _host, newOptions));
    }

    /// <summary>
    /// Intercepte les appels de streaming du serveur (requête unique du client, réponses multiples du serveur).
    /// </summary>
    public override AsyncServerStreamingCall<TResponse> AsyncServerStreamingCall<TRequest, TResponse>(
        TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncServerStreamingCallContinuation<TRequest, TResponse> continuation)
    {
        Metadata headers = context.Options.Headers ?? [];
        headers.Add(_headerName, _headerValue);
        CallOptions newOptions = context.Options.WithHeaders(headers);
        
        // La continuation pour le streaming du serveur prend la requête et le nouveau contexte.
        return continuation(request, new ClientInterceptorContext<TRequest, TResponse>(context.Method, _host, newOptions));
    }

    /// <summary>
    /// Intercepte les appels de streaming duplex (requêtes et réponses multiples dans les deux sens).
    /// </summary>
    public override AsyncDuplexStreamingCall<TRequest, TResponse> AsyncDuplexStreamingCall<TRequest, TResponse>(
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncDuplexStreamingCallContinuation<TRequest, TResponse> continuation)
    {
        Metadata headers = context.Options.Headers ?? [];
        headers.Add(_headerName, _headerValue);
        CallOptions newOptions = context.Options.WithHeaders(headers);
        
        // La continuation pour le streaming duplex prend directement le nouveau contexte.
        return continuation(new ClientInterceptorContext<TRequest, TResponse>(context.Method, _host, newOptions));
    }
}