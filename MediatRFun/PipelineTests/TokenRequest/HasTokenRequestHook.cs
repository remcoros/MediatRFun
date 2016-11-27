using MediatR.Pipeline;

namespace MediatRFun.PipelineTests.TokenRequest
{
    public class HasTokenRequestHook :
        RequestHook<IToken, object>
    {
        public static int PreHandleCount;
        public static int PostHandleCount;
        public static int ProcessResponseCount;

        protected override void OnPreRequest(IToken request)
        {
            PreHandleCount++;
        }

        protected override void OnPostRequest(IToken request, object response)
        {
            PostHandleCount++;
        }

        protected override void ProcessResponse(object response)
        {
            ProcessResponseCount++;
        }

        public static void Reset()
        {
            PreHandleCount = 0;
            PostHandleCount = 0;
            ProcessResponseCount = 0;
        }
    }

    public class HasTokenWithTestResponseRequestHook :
        RequestHook<IToken, TestResponse>
    {
        public static int PreHandleCount;
        public static int PostHandleCount;
        public static int ProcessResponseCount;

        protected override void OnPreRequest(IToken request)
        {
            PreHandleCount++;
        }

        protected override void OnPostRequest(IToken request, TestResponse response)
        {
            PostHandleCount++;
        }

        protected override void ProcessResponse(TestResponse response)
        {
            ProcessResponseCount++;
        }

        public static void Reset()
        {
            PreHandleCount = 0;
            PostHandleCount = 0;
            ProcessResponseCount = 0;
        }
    }

    public class HasTokenWithResponseStringRequestHook :
        RequestHook<IToken, string>
    {
        public static int PreHandleCount;
        public static int PostHandleCount;
        public static int ProcessResponseCount;

        protected override void OnPreRequest(IToken request)
        {
            PreHandleCount++;
        }

        protected override void OnPostRequest(IToken request, string response)
        {
            PostHandleCount++;
        }

        protected override void ProcessResponse(string response)
        {
            ProcessResponseCount++;
        }

        public static void Reset()
        {
            PreHandleCount = 0;
            PostHandleCount = 0;
            ProcessResponseCount = 0;
        }
    }
}