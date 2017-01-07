using System;
using System.Threading.Tasks;
using MediatR.Pipeline;

namespace MediatRFun.PipelineTests.TokenRequest
{
    public class HasTokenRequestHook :
        IRequestPreProcessor<IToken>,
        IRequestPostProcessor<IToken, object>
    {
        public static int PreHandleCount;
        public static int PostHandleCount;

        public static void Reset()
        {
            PreHandleCount = 0;
            PostHandleCount = 0;
        }

        public Task Process(IToken request)
        {
            PreHandleCount++;
            return Task.CompletedTask;
        }

        public Task Process(IToken request, object response)
        {
            PostHandleCount++;
            return Task.CompletedTask;
        }
    }

    public class HasTokenWithTestResponseRequestHook :
        IRequestPostProcessor<IToken, TestResponse>
    {
        public static int PostHandleCount;

        public static void Reset()
        {
            PostHandleCount = 0;
        }

        public Task Process(IToken request, TestResponse response)
        {
            PostHandleCount++;
            return Task.CompletedTask;
        }
    }

    public class HasTokenWithResponseStringRequestHook :
        IRequestPostProcessor<IToken, string>
    {
        public static int PostHandleCount;

        public static void Reset()
        {
            PostHandleCount = 0;
        }

        public Task Process(IToken request, string response)
        {
            PostHandleCount++;
            return Task.CompletedTask;
        }
    }
}